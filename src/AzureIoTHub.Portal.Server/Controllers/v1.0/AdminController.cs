// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AzureIoTHub.Portal.Server.Controllers.V10
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using AzureIoTHub.Portal.Server.Managers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/admin")]
    [ApiExplorerSettings(GroupName = "Admin APIs")]
    public class AdminController : ControllerBase
    {
        private readonly IExportManager exportManager;

        public AdminController(IExportManager exportManager)
        {
            this.exportManager = exportManager;
        }

        [HttpPost("devices/_export", Name = "Export devices")]
        public async Task<IActionResult> ExportDeviceList()
        {
            var stream = new MemoryStream();

            await this.exportManager.ExportDeviceList(stream);
            stream.Position = 0;

            return this.File(stream, "text/csv", $"Devices_{DateTime.Now:yyyyMMddHHmm}.csv");
        }
    }
}