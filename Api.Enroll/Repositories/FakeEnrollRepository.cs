using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Api.Enroll.Models;
using Api.Enroll.Interfaces;
using Api.Enroll.Helpers;

namespace Api.Enroll.Repositories
{
    public class FakeEnrollRepository : IEnrollsRepository
    {
        private readonly List<EnrollModel> _repo = new List<EnrollModel>();
        private readonly ILogger<FakeEnrollRepository> _logger;

        public FakeEnrollRepository(ILogger<FakeEnrollRepository> logger)
        {
            _logger = logger;
            _repo.Add(new EnrollModel
            {
                Id = 1,
                CourseId = 2,
                EnrolledId = 1,
                EnrollTypeId = 1
            });
            _repo.Add(new EnrollModel
            {
                Id = 2,
                CourseId = 4,
                EnrolledId = 1,
                EnrollTypeId = 1
            });
        }

        public Task<Response> AddEnrolledAsync(int EnrolledId, int CourseId, int EnrollTypeId)
        {
            try
            {

                _logger?.LogInformation($"Enroll EnrolledId: {EnrolledId}, CourseId: {CourseId}, EnrollTypeId: {EnrollTypeId}  ");
                var newEnroll = new EnrollModel()
                {
                    Id = _repo.Max(c => c.Id) + 1,
                    CourseId = CourseId,
                    EnrolledId = EnrolledId,
                    EnrollTypeId = EnrollTypeId
                };

                _repo.Add(newEnroll);

                return Task.FromResult(new Response()
                {
                    IsSuccess = true,
                    Data = newEnroll
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return Task.FromResult(new Response()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        public Task<Response> GetCoursesByEnrolledIdAsync(int EnrolledId)
        {
            try
            {
                _logger?.LogInformation($"Querying courses by EnrolledId: {EnrolledId} ");

                // Getting list of EnrolledId records
                var courseIdList = _repo.Where(enroll => enroll.EnrolledId == EnrolledId)
                                    .Select(e => e.CourseId).ToList();
              
                var IsSuccess = courseIdList.ToArray().Length > 0;
                return Task.FromResult(new Response()
                {
                    IsSuccess = IsSuccess,
                    Data = IsSuccess ? courseIdList : null,
                    ErrorMessage = IsSuccess ? string.Empty : "Courses was not found"
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return Task.FromResult(new Response()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        public Task<Response> GetEnrolledByIdAsync(int EnrolledId)
        {
            try
            {

                _logger?.LogInformation($"Querying Enroll by EnrolledId: {EnrolledId} ");

                // Getting list of EnrolledId records
                var EnrollList = _repo.Where(enroll => enroll.EnrolledId == EnrolledId)
                                        .Select(e => e.CourseId).ToList();

                return Task.FromResult(new Response()
                {
                    IsSuccess = EnrollList != null,
                    Data = EnrollList,
                    ErrorMessage = EnrollList != null ? null : "Enroll Id was not found"
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return Task.FromResult(new Response()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        public Task<Response> GetEnrolledByTypeIdAndCourseIdAsync(int EnrollTypeId, int CourseId)
        {
            try
            {
                _logger?.LogInformation($"Querying Enrolled by EnrollTypeId: {EnrollTypeId}  and CourseId: {CourseId} ");

                // Getting Enrolled records
                var EnrolledIdList = _repo.Where(enroll => enroll.EnrollTypeId == EnrollTypeId && enroll.CourseId == CourseId)
                                        .Select(e => e.EnrolledId).ToList();

                return Task.FromResult(new Response()
                {
                    IsSuccess = EnrolledIdList != null,
                    Data = EnrolledIdList,
                    ErrorMessage = EnrolledIdList != null ? null : "Enrolled was not found"
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return Task.FromResult(new Response()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        public Task<Response> RemoveEnrolledAsync(int EnrolledId, int CourseId)
        {
            try
            {
                _logger?.LogInformation($"Removing Enrolled by EnrolledId: {EnrolledId}  and CourseId: {CourseId} ");

                var Enroll = _repo.FirstOrDefault(enroll => enroll.EnrolledId == EnrolledId 
                                                            && enroll.CourseId == CourseId);
                if (Enroll == null)
                {
                    return Task.FromResult(new Response()
                    {
                        IsSuccess = false,
                        ErrorMessage = "Enrolled was not found"
                    });
                }

                _repo.Remove(Enroll);
                return Task.FromResult(new Response()
                {
                    IsSuccess = true,
                });

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return Task.FromResult(new Response()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
