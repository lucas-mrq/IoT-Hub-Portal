﻿// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AzureIoTHub.Portal.Server.Mappers
{
    using Azure.Data.Tables;
    using AzureIoTHub.Portal.Shared.Models.V10;

    public interface IDeviceModelCommandMapper
    {
        public DeviceModelCommand GetDeviceModelCommand(TableEntity entity);

        public void UpdateTableEntity(TableEntity commandEntity, DeviceModelCommand element);
    }
}