﻿// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel.DataAnnotations;

namespace AzureIoTHub.Portal.Shared.Models.V10
{
    public class ConfigItem
    {
        /// <summary>
        /// The IoT Edge configuration name.
        /// </summary>
        [Required(ErrorMessage = "The configuration model name is required.")]
        public string Name { get; set; }

        /// <summary>
        /// The IoT Edge configuration creation date.
        /// </summary>
        public DateTime DateCreation { get; set; }

        /// <summary>
        /// The IoT Edge configuration status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigItem"/> class.
        /// </summary>
        public ConfigItem()
        {
            this.DateCreation = new DateTime(1999, 1, 1);
        }
    }
}