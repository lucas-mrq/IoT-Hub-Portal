// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AzureIoTHub.Portal.Server.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Exceptions;
    using AzureIoTHub.Portal.Shared.Models;
    using Domain.Entities;
    using Domain.Repositories;
    using Domain;
    using Managers;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.Devices.Shared;
    using Helpers;

    public class DeviceModelService<TListItem, TModel> : IDeviceModelService<TListItem, TModel>
        where TListItem : class, IDeviceModel
        where TModel : class, IDeviceModel
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDeviceModelRepository deviceModelRepository;
        private readonly IDeviceModelCommandRepository deviceModelCommandRepository;

        private readonly IDeviceProvisioningServiceManager deviceProvisioningServiceManager;
        private readonly IConfigService configService;
        private readonly IDeviceModelImageManager deviceModelImageManager;
        private readonly IDeviceModelMapper<TListItem, TModel> deviceModelMapper;
        private readonly IExternalDeviceService externalDeviceService;

        public DeviceModelService(IMapper mapper,
            IUnitOfWork unitOfWork,
            IDeviceModelRepository deviceModelRepository,
            IDeviceModelCommandRepository deviceModelCommandRepository,
            IDeviceProvisioningServiceManager deviceProvisioningServiceManager,
            IConfigService configService,
            IDeviceModelImageManager deviceModelImageManager,
            IDeviceModelMapper<TListItem, TModel> deviceModelMapper,
            IExternalDeviceService externalDeviceService)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.deviceModelRepository = deviceModelRepository;
            this.deviceModelCommandRepository = deviceModelCommandRepository;
            this.deviceProvisioningServiceManager = deviceProvisioningServiceManager;
            this.configService = configService;
            this.deviceModelImageManager = deviceModelImageManager;
            this.deviceModelMapper = deviceModelMapper;
            this.externalDeviceService = externalDeviceService;
        }

        public async Task<IEnumerable<TListItem>> GetDeviceModels()
        {
            return await Task.Run(() => this.deviceModelRepository.GetAll()
                .Select(model =>
                {
                    var deviceModelDto = this.mapper.Map<TListItem>(model);
                    deviceModelDto.ImageUrl = this.deviceModelImageManager.ComputeImageUri(deviceModelDto.ModelId);
                    return deviceModelDto;
                })
                .ToList());
        }

        public async Task<TModel> GetDeviceModel(string deviceModelId)
        {
            var deviceModelEntity = await this.deviceModelRepository.GetByIdAsync(deviceModelId);

            if (deviceModelEntity == null)
            {
                throw new ResourceNotFoundException($"The device model {deviceModelId} doesn't exist");
            }

            return this.mapper.Map<TModel>(deviceModelEntity);
        }

        public async Task CreateDeviceModel(TModel deviceModel)
        {
            var deviceModelEntity = this.mapper.Map<DeviceModel>(deviceModel);

            await this.deviceModelRepository.InsertAsync(deviceModelEntity);
            await this.unitOfWork.SaveAsync();

            await CreateDeviceModelConfiguration(deviceModel);
        }

        public async Task UpdateDeviceModel(TModel deviceModel)
        {
            var deviceModelEntity = await this.deviceModelRepository.GetByIdAsync(deviceModel.ModelId);

            if (deviceModelEntity == null)
            {
                throw new ResourceNotFoundException($"The device model {deviceModel.ModelId} doesn't exist");
            }

            _ = this.mapper.Map(deviceModel, deviceModelEntity);

            this.deviceModelRepository.Update(deviceModelEntity);
            await this.unitOfWork.SaveAsync();

            await CreateDeviceModelConfiguration(deviceModel);
        }

        public async Task DeleteDeviceModel(string deviceModelId)
        {
            var deviceModelEntity = await this.deviceModelRepository.GetByIdAsync(deviceModelId);

            if (deviceModelEntity == null)
            {
                return;
            }

            var devices = await this.externalDeviceService.GetAllDevice();

            if (devices.Items.Any(x => DeviceHelper.RetrieveTagValue(x, "modelId") == deviceModelId))
            {
                throw new ResourceAlreadyExistsException(
                    $"The device model {deviceModelId} is already in use by a device and cannot be deleted");
            }

            var deviceModelCommands = this.deviceModelCommandRepository.GetAll().Where(command =>
                    command.DeviceModelId.Equals(deviceModelId, StringComparison.Ordinal)).ToList();

            foreach (var deviceModelCommand in deviceModelCommands)
            {
                this.deviceModelCommandRepository.Delete(deviceModelCommand.Id);
            }

            // Image deletion
            await this.deviceModelImageManager.DeleteDeviceModelImageAsync(deviceModelId);

            this.deviceModelRepository.Delete(deviceModelId);

            await this.unitOfWork.SaveAsync();
        }

        public Task<string> GetDeviceModelAvatar(string deviceModelId)
        {
            return Task.Run(() => this.deviceModelImageManager.ComputeImageUri(deviceModelId).ToString());
        }

        public Task<string> UpdateDeviceModelAvatar(string deviceModelId, IFormFile file)
        {
            return Task.Run(() => this.deviceModelImageManager.ChangeDeviceModelImageAsync(deviceModelId, file?.OpenReadStream()));
        }

        public Task DeleteDeviceModelAvatar(string deviceModelId)
        {
            return this.deviceModelImageManager.DeleteDeviceModelImageAsync(deviceModelId);
        }

        private async Task CreateDeviceModelConfiguration(TModel deviceModel)
        {
            var desiredProperties = this.deviceModelMapper.BuildDeviceModelDesiredProperties(deviceModel);

            var deviceModelTwin = new TwinCollection();

            _ = await this.deviceProvisioningServiceManager.CreateEnrollmentGroupFromModelAsync(deviceModel.ModelId, deviceModel.Name, deviceModelTwin);

            await this.configService.RollOutDeviceModelConfiguration(deviceModel.ModelId, desiredProperties);
        }
    }
}