
using System.Collections.Generic;
using System.Threading.Tasks;
using SMSDesktopUWP.Core.Models;

namespace SMSDesktopUWP.Core.HttpRepository 
{
    public interface INarrationHttpRepository
    {    
        Task AddNarrationAsync(NarrationCreation newNarration);

        Task UpdateNarrationAsync(int narrationId, NarrationUpdate narrationUpdate);

        Task DeleteNarrationAsync(int narrationId);

        Task<List<Narration>> GetOrphanNarrations(int orphanId);

        Task<List<Narration>> GetGuardianNarrations(int guradianId);
    }
}



