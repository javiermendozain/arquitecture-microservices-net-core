using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Api.Users.Interfaces;
using Api.Users.Models;
using Api.Users.Helpers;

namespace Api.Users.Services
{
    public class UsersService: IUsersService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<UsersService> _logger;

        public UsersService(IHttpClientFactory httpClientFactory, ILogger<UsersService> logger, IUsersRepository usersRepository)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _usersRepository = usersRepository;
        }

        public Task<Response> AddAsync(UserModel user) => _usersRepository.AddAsync(user);

        public Task<Response> GetAllAsync(int page, int pageSize) => _usersRepository.GetAllAsync(page, pageSize);

        public Task<Response> GetByIdAsync(int id) => _usersRepository.GetByIdAsync(id);

        public Task<Response> UpdateAsync(int id, UserModel user) => _usersRepository.UpdateAsync(id, user);

        public async Task<Response> DeleteAsync(int userId)
        {
            try
            {
                // Chechk if the user isn't enrolled to any courses
                var client = _httpClientFactory.CreateClient("EnrollService");
                var response = await client.GetAsync($"v1/api/enrolls/count?Id={userId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var resultResponse = JsonSerializer.Deserialize<Response>(content, options);
                    var responseData = (Array)JsonSerializer.Deserialize<int[]>(Convert.ToString(resultResponse.Data));

                    // user isn't enrolled to any course
                    if (responseData.Length  == 0 )
                    {
                        var result = await _usersRepository.DeleteAsync(userId);
                        return result;
                    }
                    else
                    {
                        return new Response()
                        {
                            IsSuccess = false,
                            ErrorMessage = "The user can't be delete, because It have course enrolled"
                        };

                    }
                }
                else
                {
                    return new Response()
                    {
                        IsSuccess = false,
                        ErrorMessage = "Enroll status could not be validated"
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
