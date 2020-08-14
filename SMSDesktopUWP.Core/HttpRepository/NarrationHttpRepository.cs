using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SMSDesktopUWP.Core.Models;

namespace SMSDesktopUWP.Core.HttpRepository
{
    public class NarrationHttpRepository : INarrationHttpRepository
    {
        private readonly HttpClient _client;
        private const string Controller = "narrations";

        public NarrationHttpRepository(HttpClient client)
        {
             _client = client;
        }

        public async Task AddNarrationAsync(NarrationCreation newNarration)
        {
            string url = $"{ HttpRepositorySettings.BaseApiUrl }/{ Controller }";

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            
            var content = JsonSerializer.Serialize<NarrationCreation>(newNarration, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
       
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateNarrationAsync(int narrationId, NarrationUpdate narrationUpdate)
        {
            string url = $"{ HttpRepositorySettings.BaseApiUrl }/{ Controller }/{ narrationId }";

            var request = new HttpRequestMessage(HttpMethod.Put, url);

            var content = JsonSerializer.Serialize<NarrationUpdate>(narrationUpdate, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteNarrationAsync(int narrationId)
        {
            string uri = $"{ HttpRepositorySettings.BaseApiUrl }/{ Controller }/{ narrationId }";

            var request = new HttpRequestMessage(HttpMethod.Delete, uri);

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Narration>> GetOrphanNarrations(int orphanId)
        {
            string url = $"{ HttpRepositorySettings.BaseApiUrl }/{ Controller }/{ "orphan" }/{ orphanId }";
    

            System.Console.WriteLine(url);
            
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var narrations = JsonSerializer.Deserialize<List<Narration>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return narrations;
        }

        public async Task<List<Narration>> GetGuardianNarrations(int guradianId)
        {
            string url = $"{ HttpRepositorySettings.BaseApiUrl }/{ Controller }/{ "guardian" }/{ guradianId }";

            return null;
        }
    }
}