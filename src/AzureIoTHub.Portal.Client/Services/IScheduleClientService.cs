// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AzureIoTHub.Portal.Client.Services
{
    using System.Threading.Tasks;
    using AzureIoTHub.Portal.Shared.Models.v10;

    public interface IScheduleClientService
    {
        Task<string> CreateSchedule(ScheduleDto schedule);
        Task UpdateSchedule(ScheduleDto schedule);
        Task DeleteSchedule(string modelId);
        Task<ScheduleDto> GetSchedule(string scheduleId);
        Task<List<ScheduleDto>> GetSchedules();
    }
}
