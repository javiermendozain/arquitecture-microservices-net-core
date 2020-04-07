using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Api.Enroll.Helpers;
using Api.Enroll.Interfaces;

namespace Api.Enroll.Services
{
    public class UsersService : IUsersService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<UsersService> _logger;

        public UsersService(IHttpClientFactory httpClientFactory, ILogger<UsersService> logger)
        {
            this._httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<Response> GetUserById(int Id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("UsersService");
                var response = await client.GetAsync($"v1/api/users/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<Response>(content, options);
                    
                    return result;
                }
                else
                {
                    return new Response()
                    {
                        IsSuccess = false,
                        ErrorMessage = "The user was not found"
                    };
                }
            
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return new Response()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };

            }
        }
      
    }
}
