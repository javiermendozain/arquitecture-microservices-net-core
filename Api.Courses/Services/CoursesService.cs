using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Api.Courses.Interfaces;
using Api.Courses.Models;
using Api.Courses.Helpers;

namespace Api.Courses.Services
{
    public class CoursesService : ICoursesService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICoursesRespository _coursesRespository;
        private readonly ILogger<CoursesService> _logger;

        public CoursesService(IHttpClientFactory httpClientFactory, ICoursesRespository coursesRespository, ILogger<CoursesService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _coursesRespository = coursesRespository;
            _logger = logger;
        }

        public Task<Response> AddAsync(CourseModel course)
                => _coursesRespository.AddAsync(course);

        public Task<Response> GetByIdAsync(int courseId)
                => _coursesRespository.GetByIdAsync(courseId);

        public Task<Response> GetAllAsync(int page, int pageSize)
                => _coursesRespository.GetAllAsync(page, pageSize);

        public Task<Response> SearchAsync(string search)
                => _coursesRespository.SearchAsync(search);

        public Task<Response> UpdateAsync(int id, CourseModel course)
                => _coursesRespository.UpdateAsync(id, course);

        public async Task<Response> DeleteAsync(int courseId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("EnrollService");
                var response = await client.GetAsync($"v1/api/enrolls/count?courseId={courseId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<Response>(content, options);
                    var responseData = (Array) JsonSerializer.Deserialize<int[]>(Convert.ToString(result.Data));

                    // Don't have teacher or student enrolled
                    if (result .IsSuccess && responseData.Length == 0)
                    {

                        return await _coursesRespository.DeleteAsync(courseId);
                    }
                    else
                    {
                        return new Response()
                        {
                            IsSuccess = false,
                            ErrorMessage = "The course can't be delete, because It have student or teacher enrolled"
                        };

                    }
                }
                else
                {
                    return new Response()
                    {
                        IsSuccess = false,
                        ErrorMessage = "Couldn't be valid enrolls status course"
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
