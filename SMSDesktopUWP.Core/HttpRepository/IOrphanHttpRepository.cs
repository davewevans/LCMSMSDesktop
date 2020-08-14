using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SMSDesktopUWP.Core.Models;

namespace SMSDesktopUWP.Core.HttpRepository
{
    public interface IOrphanHttpRepository
    {
        Task<OrphansResponse> GetOrphansAsync(OrphanParametes parameters);

        Task<OrphanDetailsModel> GetOrphanDetailsAsync(int orphanId);

        Task AddOrphanAsync(OrphanCreation newOrphan);

        Task UpdateOrphanAsync(int orphanId, OrphanEdit orphanEdit);

        Task DeleteOrphanAsync(int orphanId);
    }
}
