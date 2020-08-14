using SMSDesktopUWP.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SMSDesktopUWP.Core.HttpRepository
{
    public class PictureHttpRepository : IPictureHttpRepository
    {
        private readonly HttpClient _client;

        private const string Controller = "pictures";

        public PictureHttpRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> UploadImageAsync(PictureCreation picCreation, Stream fileStream)
        {            
            string url = $"{ HttpRepositorySettings.BaseApiUrl }/{ Controller }";

            // Create the content
            var content = new MultipartFormDataContent();
            content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");

            var jsonData = JsonSerializer.Serialize<PictureCreation>(picCreation, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            content.Add(new StringContent(jsonData, Encoding.UTF8, "application/json"));

            content.Add(new StreamContent(fileStream, (int)fileStream.Length), "image", picCreation.PictureFileName);


            var response = await _client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }
    }
}
