using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Api.Enroll.Helpers;
using Api.Enroll.Interfaces;
using Api.Enroll.Models;

namespace Api.Enroll.Services
{
    public class EnrollService : IEnrollsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EnrollService> _logger;

        private readonly ICoursesService _coursesService;
        private readonly IUsersService _usersService;

        private readonly IEnrollsRepository _enrollsRepository;

        public EnrollService(IHttpClientFactory httpClientFactory, ILogger<EnrollService> logger, ICoursesService coursesService,
            IUsersService usersService, IEnrollsRepository enrollsRepository )
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;

            _coursesService = coursesService;
            _usersService = usersService;

            _enrollsRepository = enrollsRepository;
        }

        // Guardar enrolled(enrolleTypeID, courseID, enrolledID) -> { isSuccess, enrolled
        public Task<Response> AddEnrolledAsync(int EnrolledId, int CourseId, int EnrollTypeId) 
                => _enrollsRepository.AddEnrolledAsync(EnrolledId, CourseId, EnrollTypeId);

        // Obtener cursos por enrrolled (tech or studen)  (  enrolledID ) -> [...Courses ]
        public async Task<Response> GetCoursesByEnrolledIdAsync(int EnrolledId)
        {
            try
            {
                // Getting list of EnrolledId records
                var CoursesId =  await _enrollsRepository.GetCoursesByEnrolledIdAsync(EnrolledId);

                if (!CoursesId.IsSuccess) return CoursesId; 

                List<CourseModel> result = new  List<CourseModel>();
                foreach (var id in CoursesId.Data)
                {
                    // Fetch Course by Id
                    var course = await _coursesService.GetCourseById(id);
                    if (course.IsSuccess)
                    {
                        var responseData = JsonConvert.DeserializeObject<CourseModel>(Convert.ToString(course.Data) );
                        result.Add(responseData);
                    }
                }

                var IsSuccess = result.ToArray().Length > 0;
                return new Response() { 
                    IsSuccess = IsSuccess,
                    Data = IsSuccess ? result : null,
                    ErrorMessage = IsSuccess ? string.Empty : "Courses was not found"
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (new Response()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        // Obtener enrolled por courseId  (  courseId ) -> [...Id ]
        public Task<Response> GetEnrolledByIdAsync(int Id)
                => _enrollsRepository.GetEnrolledByIdAsync(Id);

        // Obtener usuarios registrados en un curso ( enrolleTypeID = 1 or 2, courseID ) -> [... Student or Teacher]
        public async Task<Response> GetEnrolledByTypeIdAndCourseIdAsync(int EnrollTypeId, int CourseId)
        {
            try
            {
                // Getting Enrolled records
                var enrolledId = await _enrollsRepository.GetEnrolledByTypeIdAndCourseIdAsync(EnrollTypeId, CourseId);

                if (!enrolledId.IsSuccess) return enrolledId;

                List<UserModel> users = new  List<UserModel>();
                foreach (var id in enrolledId.Data)
                {
                    var resultResponse = await _usersService.GetUserById(id);
                    if (resultResponse.IsSuccess)
                    {
                        var responseData = JsonConvert.DeserializeObject<UserModel>(Convert.ToString(resultResponse.Data));
                        users.Add(responseData);
                    }
                }

                var isSuccess = users.ToArray().Length > 0;
                return (new Response() { 
                    IsSuccess = isSuccess,
                    Data = isSuccess ? users : null,
                    ErrorMessage = isSuccess ? string.Empty : "User was not found"
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (new Response()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                });
            }

        }

        // Remover enrolled (courseID, enrolledID) -> { isSuccess, enrolled }
        public Task<Response> RemoveEnrolledAsync(int EnrolledId, int CourseId)
                => _enrollsRepository.RemoveEnrolledAsync(EnrolledId, CourseId);

    }
}


