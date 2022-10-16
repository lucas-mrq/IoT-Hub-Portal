// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AzureIoTHub.Portal.Server.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AzureIoTHub.Portal.Domain;
    using AzureIoTHub.Portal.Domain.Entities;
    using AzureIoTHub.Portal.Domain.Exceptions;
    using AzureIoTHub.Portal.Domain.Repositories;
    using AzureIoTHub.Portal.Models.v10;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using AzureIoTHub.Portal.Server.Managers;
    using AzureIoTHub.Portal.Shared.Models.v10;

    public class EdgeModelService : IEdgeModelService
    {

        /// <summary>
        /// The Configurations service.
        /// </summary>
        private readonly IConfigService configService;

        ///// <summary>
        ///// The device model mapper.
        ///// </summary>
        ////private readonly IEdgeDeviceModelMapper edgeDeviceModelMapper;

        /// <summary>
        /// The device model image manager.
        /// </summary>
        private readonly IDeviceModelImageManager deviceModelImageManager;

        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IEdgeDeviceModelRepository edgeModelRepository;
        private readonly IEdgeDeviceModelCommandRepository commandRepository;

        public EdgeModelService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IEdgeDeviceModelRepository edgeModelRepository,
            IConfigService configService,
            IDeviceModelImageManager deviceModelImageManager,
            IEdgeDeviceModelCommandRepository commandRepository)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.edgeModelRepository = edgeModelRepository;
            this.configService = configService;
            this.deviceModelImageManager = deviceModelImageManager;
            this.commandRepository = commandRepository;
        }

        /// <summary>
        /// Return the edge model template list.
        /// </summary>
        /// <returns>IEnumerable IoTEdgeModelListItem.</returns>
        public IEnumerable<IoTEdgeModelListItem> GetEdgeModels()
        {
            return this.edgeModelRepository
                .GetAll()
                .Select(model =>
                {
                    var edgeDeviceModelListItem = this.mapper.Map<IoTEdgeModelListItem>(model);
                    edgeDeviceModelListItem.ImageUrl = this.deviceModelImageManager.ComputeImageUri(edgeDeviceModelListItem.ModelId);
                    return edgeDeviceModelListItem;
                })
                .ToList();
        }

        /// <summary>
        /// Create a new edge model template and roll out
        /// the edge model configuration.
        /// </summary>
        /// <param name="edgeModel">the new edge modle object.</param>
        /// <returns>nothing.</returns>
        /// <exception cref="ResourceAlreadyExistsException">If edge model template already exist return ResourceAlreadyExistsException.</exception>
        /// <exception cref="InternalServerErrorException"></exception>
        public async Task CreateEdgeModel(IoTEdgeModel edgeModel)
        {
            if (!string.IsNullOrEmpty(edgeModel?.ModelId))
            {
                try
                {
                    var edgeModelEntity = await this.edgeModelRepository.GetByIdAsync(edgeModel.ModelId);
                    if (edgeModelEntity == null)
                    {
                        edgeModelEntity = this.mapper.Map<EdgeDeviceModel>(edgeModel);
                        await this.edgeModelRepository.InsertAsync(edgeModelEntity);
                        await this.unitOfWork.SaveAsync();
                    }
                    else
                    {
                        throw new ResourceAlreadyExistsException($"The edge model with id {edgeModel?.ModelId} already exists");
                    }
                }
                catch (DbUpdateException e)
                {
                    throw new InternalServerErrorException($"Unable to create the device model with id {edgeModel?.ModelId}", e);
                }

                await SaveModuleCommands(edgeModel);
                await this.configService.RollOutEdgeModelConfiguration(edgeModel);
            }

        }

        /// <summary>
        /// Saves the module commands for a specific model object.
        /// </summary>
        /// <param name="deviceModelObject">The device model object.</param>
        /// <returns></returns>
        /// <exception cref="InternalServerErrorException"></exception>
        public async Task SaveModuleCommands(IoTEdgeModel deviceModelObject)
        {
            IEnumerable<IoTEdgeModuleCommand> moduleCommands = deviceModelObject.EdgeModules
                .SelectMany(x => x.Commands.Select(cmd => new IoTEdgeModuleCommand
                {
                    EdgeDeviceModelId = deviceModelObject.ModelId,
                    CommandId = Guid.NewGuid().ToString(),
                    ModuleName = x.ModuleName,
                    Name = cmd.Name,
                })).ToArray();

            try
            {
                var existingCommands = this.commandRepository.GetAll().Where(x => x.EdgeDeviceModelId == deviceModelObject.ModelId).ToList();
                foreach (var command in existingCommands)
                {
                    this.commandRepository.Delete(command.Id);
                }

                foreach (var cmd in moduleCommands)
                {
                    await this.commandRepository.InsertAsync(this.mapper.Map<EdgeDeviceModelCommand>(cmd));
                }
                await this.unitOfWork.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                throw new InternalServerErrorException("Unable to save commands", e);
            }
        }

        /// <summary>
        /// Get the edge model template and its configuration by the edge model id.
        /// </summary>
        /// <param name="modelId">The model identifier.</param>
        /// <returns>An edge model object.</returns>
        /// <exception cref="ResourceNotFoundException">Resource not found if template does not exist.</exception>
        public async Task<IoTEdgeModel> GetEdgeModel(string modelId)
        {
            var edgeModelEntity = await this.edgeModelRepository.GetByIdAsync(modelId);

            if (edgeModelEntity == null)
            {
                throw new ResourceNotFoundException($"The edge model with id {modelId} doesn't exist");
            }

            var modules = await this.configService.GetConfigModuleList(modelId);
            var routes = await this.configService.GetConfigRouteList(modelId);
            var commands =  this.commandRepository.GetAll().Where(x => x.EdgeDeviceModelId == modelId).ToList();

            //TODO : User a mapper
            //Previously return this.edgeDeviceModelMapper.CreateEdgeDeviceModel(query.Value, modules, routes, commands);
            var result = new IoTEdgeModel
            {
                ModelId = edgeModelEntity.Id,
                ImageUrl = this.deviceModelImageManager.ComputeImageUri(edgeModelEntity.Id),
                Name = edgeModelEntity.Name,
                Description = edgeModelEntity.Description,
                EdgeModules = modules,
                EdgeRoutes = routes
            };

            foreach (var command in commands)
            {
                var module = result.EdgeModules.SingleOrDefault(x => x.ModuleName.Equals(command.ModuleName, StringComparison.Ordinal));
                if (module == null)
                {
                    continue;
                }

                module.Commands.Add(this.mapper.Map<IoTEdgeModuleCommand>(command));
            }

            return result;
        }


        /// <summary>
        /// Update the edge model template and the configuration.
        /// </summary>
        /// <param name="edgeModel">The edge model.</param>
        /// <returns>nothing.</returns>
        /// <exception cref="InternalServerErrorException"></exception>
        public async Task UpdateEdgeModel(IoTEdgeModel edgeModel)
        {
            if (!string.IsNullOrEmpty(edgeModel?.ModelId))
            {
                try
                {
                    var edgeModelEntity = await this.edgeModelRepository.GetByIdAsync(edgeModel.ModelId);
                    if (edgeModelEntity == null)
                    {
                        throw new ResourceNotFoundException($"The edge model with id {edgeModel.ModelId} doesn't exist");
                    }

                    _ = this.mapper.Map(edgeModel, edgeModelEntity);
                    this.edgeModelRepository.Update(edgeModelEntity);

                    await this.unitOfWork.SaveAsync();

                    await SaveModuleCommands(edgeModel);
                    await this.configService.RollOutEdgeModelConfiguration(edgeModel);
                }
                catch (DbUpdateException e)
                {
                    throw new InternalServerErrorException($"Unable to create the device model with id {edgeModel?.ModelId}", e);
                }
            }
        }

        /// <summary>
        /// Delete edge model template and its configuration.
        /// </summary>
        /// <param name="edgeModelId">The edge model indentifier.</param>
        /// <returns></returns>
        /// <exception cref="InternalServerErrorException"></exception>
        public async Task DeleteEdgeModel(string edgeModelId)
        {
            if (!string.IsNullOrEmpty(edgeModelId))
            {
                try
                {
                    this.edgeModelRepository.Delete(edgeModelId);

                    var existingCommands = this.commandRepository.GetAll().Where(x => x.EdgeDeviceModelId == edgeModelId).ToList();
                    foreach (var command in existingCommands)
                    {
                        this.commandRepository.Delete(command.Id);
                    }

                    await this.unitOfWork.SaveAsync();

                    var config = this.configService.GetIoTEdgeConfigurations().Result.FirstOrDefault(x => x.Id.StartsWith(edgeModelId, StringComparison.Ordinal));

                    if (config != null)
                    {
                        await this.configService.DeleteConfiguration(config.Id);
                    }
                }

                catch (DbUpdateException e)
                {
                    throw new InternalServerErrorException($"Unable to delete the edge model with {edgeModelId} model entity.", e);
                }
            }
        }

        /// <summary>
        /// Get the edge model avatar.
        /// </summary>
        /// <param name="edgeModelId">The edge model indentifier.</param>
        /// <returns></returns>
        public Task<string> GetEdgeModelAvatar(string edgeModelId)
        {
            return Task.Run(() => this.deviceModelImageManager.ComputeImageUri(edgeModelId).ToString());
        }

        /// <summary>
        /// Update the edge model avatar.
        /// </summary>
        /// <param name="edgeModelId">The edge model indentifier</param>
        /// <param name="file">The image.</param>
        /// <returns></returns>
        public Task<string> UpdateEdgeModelAvatar(string edgeModelId, IFormFile file)
        {
            return Task.Run(() => this.deviceModelImageManager.ChangeDeviceModelImageAsync(edgeModelId, file?.OpenReadStream()));
        }

        /// <summary>
        /// Delete the edge model avatar.
        /// </summary>
        /// <param name="edgeModelId">The edge model indentifier</param>
        /// <returns></returns>
        public Task DeleteEdgeModelAvatar(string edgeModelId)
        {
            return Task.Run(() => this.deviceModelImageManager.DeleteDeviceModelImageAsync(edgeModelId));
        }
    }
}