using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Settings;

namespace UserManagement.Services
{    
    public interface IHCaptchaTokenService
    {        
        bool IsCaptchaValid(string token);
    }

    public class HCaptchaTokenService : IHCaptchaTokenService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IOptions<HCaptchaSettings> hCaptchaSettings;

        public HCaptchaTokenService(
            IHttpClientFactory httpClientFactory,
            IOptions<HCaptchaSettings> hCaptchaSettings
            )
        {
            this.httpClientFactory = httpClientFactory;
            this.hCaptchaSettings = hCaptchaSettings;
        }

        public bool IsCaptchaValid(string token)
        {
            var validationTask = ValidateCaptcha(token);
            validationTask.Wait(500);
            return validationTask.Result.IsSuccessStatusCode;
        }
        private async Task<HttpResponseMessage> ValidateCaptcha(string token)
        {
            HttpClient client = httpClientFactory.CreateClient("hCaptcha");

            // create post data
            List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("secret", this.hCaptchaSettings.Value.Secret),
                new KeyValuePair<string, string>("response", token)
            };
            
            return await client.PostAsync(
                "/siteverify", new FormUrlEncodedContent(postData));
        }
    }
}
