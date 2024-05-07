// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AzureIoTHub.Portal.Application.Services
{
    using System.Threading.Tasks;
    using AzureIoTHub.Portal.Domain.Entities;
    using AzureIoTHub.Portal.Shared.Models.v10;

    public interface IScheduleService
    {
        Task<ScheduleDto> CreateSchedule(ScheduleDto schedule);
        Task UpdateSchedule(ScheduleDto schedule);
        Task DeleteSchedule(string scheduleId);
        Task<Schedule> GetSchedule(string scheduleId);
        Task<IEnumerable<ScheduleDto>> GetSchedules();
    }
}
