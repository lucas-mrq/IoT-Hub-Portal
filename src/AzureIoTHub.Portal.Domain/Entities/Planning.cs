// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AzureIoTHub.Portal.Domain.Entities
{
    using AzureIoTHub.Portal.Domain.Base;
    using AzureIoTHub.Portal.Shared.Constants;

    public class Planning : EntityBase
    {
        /// <summary>
        /// The planning friendly name.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Where planning start.
        /// </summary>
        public string Start { get; set; } = default!;

        /// <summary>
        /// Where planning end.
        /// </summary>
        public string End { get; set; } = default!;

        /// <summary>
        /// How much it repeat.
        /// </summary>
        public bool Frequency { get; set; } = default!;

        /// <summary>
        /// When planning is used
        /// </summary>
        public DaysEnumFlag.DaysOfWeek DayOff { get; set; } = default!;

        /// <summary>
        /// Day off command.
        /// </summary>
        public string CommandId { get; set; } = default!;
    }
}
