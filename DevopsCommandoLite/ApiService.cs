using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DevopsCommandoLite.Terminal.Services
{
    public class ApiService
    {
        public async Task<DevopsResponse<T>> Get<T>(string relativeUrl)
        {
            var _client = new HttpClient();
            string tokenBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Settings.Token)));
            _client.DefaultRequestHeaders.Add("Authorization", "Basic " + tokenBase64);

            string fullUrl = ($"{Settings.InstanceUrl}/_apis/{relativeUrl}");
            var response = await _client.GetAsync(fullUrl);
            int statusInt = (int)response.StatusCode;

            // TODO better handling
            if (statusInt < 400)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return new DevopsResponse<T>(true, JsonConvert.DeserializeObject<T>(responseBody));
            }
            else if (statusInt == 401)
            {
                return new DevopsResponse<T>(false, $"Unauthorized - You do not have permission to perform that action.");
            }
            else if (statusInt == 404)
            {
                return new DevopsResponse<T>(false, $"An error occurred - The requested resource was not found.");
            }
            else
            {
                return new DevopsResponse<T>(false, $"An error occurred - Server returned {statusInt}.");
            }
        }


        public async Task<DevopsResponse<T>> Post<T>(string relativeUrl, object data)
        {
            var _client = new HttpClient();
            string tokenBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Settings.Token)));
            _client.DefaultRequestHeaders.Add("Authorization", "Basic " + tokenBase64);

            string fullUrl = ($"{Settings.InstanceUrl}/_apis/{relativeUrl}");

            var stringContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(fullUrl, stringContent);
            int statusInt = (int)response.StatusCode;

            // TODO better handling
            if (statusInt < 400)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return new DevopsResponse<T>(true, JsonConvert.DeserializeObject<T>(responseBody));
            }
            else if (statusInt == 401)
            {
                return new DevopsResponse<T>(false, $"Unauthorized - You do not have permission to perform that action.");
            }
            else if (statusInt == 404)
            {
                return new DevopsResponse<T>(false, $"An error occurred - The requested resource was not found.");
            }
            else
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return new DevopsResponse<T>(false, $"An error occurred - Server returned {statusInt}: " + responseBody);
            }
        }
    }
}
