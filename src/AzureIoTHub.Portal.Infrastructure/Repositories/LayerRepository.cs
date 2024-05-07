// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AzureIoTHub.Portal.Infrastructure.Repositories
{
    using AzureIoTHub.Portal.Domain.Repositories;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class LayerRepository : GenericRepository<Layer>, ILayerRepository
    {
        public LayerRepository(PortalDbContext context) : base(context)
        {
        }
        public async Task<Layer?> GetByNameAsync(string layerId)
        {
            return await this.context.Set<Layer>()
                             .FirstOrDefaultAsync(layer => layer.Id == layerId);
        }
    }
}
