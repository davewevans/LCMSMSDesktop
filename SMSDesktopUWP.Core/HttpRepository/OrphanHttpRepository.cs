using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SMSDesktopUWP.Core.Models;

namespace SMSDesktopUWP.Core.HttpRepository
{
    public class OrphanHttpRepository: IOrphanHttpRepository
    {
        private readonly HttpClient _client;
        private const string Controller = "orphans";

        public OrphanHttpRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<OrphansResponse> GetOrphansAsync(OrphanParametes parameters)
        {
            string queryString = $"?pageSize={parameters.PageSize}&pageNumber={parameters.PageNumber}";
            string url = $"{ HttpRepositorySettings.BaseApiUrl }/{ Controller }{ queryString} ";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            // TODO Add JWT token to header
            // request.Headers 

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            IEnumerable<string> xPagination;
            response.Headers.TryGetValues("X-Pagination", out xPagination);

            ResponseMetaData metaData = null;
            if (xPagination != null)
            {
                string xPaginationData = xPagination.FirstOrDefault();  
                metaData = JsonSerializer.Deserialize<ResponseMetaData>(xPaginationData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }            
            
            var content = await response.Content.ReadAsStringAsync();
            var orphans = JsonSerializer.Deserialize<List<Orphan>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return new OrphansResponse {Orphans = orphans, MetaData = metaData};
        }
        
        public async Task<OrphanDetailsModel> GetOrphanDetailsAsync(int orphanId)
        {
            string url = $"{ HttpRepositorySettings.BaseApiUrl }/{ Controller }/{ orphanId }";
            
            Console.WriteLine($"URI: { url }");


            var request = new HttpRequestMessage(HttpMethod.Get, url);

            // TODO Add JWT token to header
            // request.Headers 

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var orphanDetails = JsonSerializer.Deserialize<OrphanDetailsModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return orphanDetails;
        }

        public async Task AddOrphanAsync(OrphanCreation newOrphan)
        {
            string url = $"{ HttpRepositorySettings.BaseApiUrl }/{ Controller }";

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            // string content = new StringContent(newOrphan, Encoding.UTF8, "application/json");
            
            var content = JsonSerializer.Serialize<OrphanCreation>(newOrphan, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            


            //var payload = JsonConvert.SerializeObject(newOrphan);

            // TODO Add JWT token to header
            // request.Headers 

            //request.Content = new StringContent(payload, Encoding.UTF8, "application/json");



            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();



            //
            // TODO
            // Update UpdateDb entity
            //
        }

        public async Task UpdateOrphanAsync(int orphanId, OrphanEdit orphanEdit)
        {
            string url = $"{ HttpRepositorySettings.BaseApiUrl }/{ Controller }/{orphanId}";

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            //var payload = JsonConvert.SerializeObject(orphanUpdate);

            var content = JsonSerializer.Serialize<OrphanEdit>(orphanEdit, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            // TODO Add JWT token to header
            // request.Headers 

            //request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteOrphanAsync(int orphanId)
        {
            string uri = $"{ HttpRepositorySettings.BaseApiUrl }/{ Controller }/{ orphanId }";

            var request = new HttpRequestMessage(HttpMethod.Delete, uri);

            //TODO Add JWT token to header
            // request.Headers

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        //
        // TODO
        // Update UpdateDb entity
        //
    }
}
