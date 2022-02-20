﻿// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AzureIoTHub.Portal.Shared.Models.V10.Device
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DeviceDetails
    {
        /// <summary>
        /// The device identifier.
        /// </summary>
        [Required(ErrorMessage = "The device should have a unique identifier.")]
        public string DeviceID { get; set; }

        /// <summary>
        /// The name of the device.
        /// </summary>
        [Required(ErrorMessage = "The device should have a name.")]
        public string DeviceName { get; set; }

        /// <summary>
        /// The model identifier.
        /// </summary>
        [Required(ErrorMessage = "The device should use a model.")]
        public string ModelId { get; set; }

        /// <summary>
        /// The device model image Url.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        ///   <c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        ///   <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// The status updated time.
        /// </summary>
        public DateTime StatusUpdatedTime { get; set; }

        /// <summary>
        /// The location code.
        /// </summary>
        public string LocationCode { get; set; }

        /// <summary>
        /// The asset identifier.
        /// </summary>
        public string AssetId { get; set; }
    }
}