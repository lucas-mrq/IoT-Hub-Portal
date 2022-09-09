// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AzureIoTHub.Portal.Infrastructure
{
    using AzureIoTHub.Portal.Domain;
    using AzureIoTHub.Portal.Domain.Entities;
    using AzureIoTHub.Portal.Infrastructure.Seeds;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public class PortalDbContext : DbContext
    {
        public DbSet<DeviceModelProperty> DeviceModelProperties { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public PortalDbContext(DbContextOptions<PortalDbContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            try
            {
                var config = this.Database.GetService<ConfigHandler>();

                _ = modelBuilder
                    .MigrateDeviceModelProperties(config);
            }
            catch (InvalidOperationException)
            {

            }
        }
    }
}