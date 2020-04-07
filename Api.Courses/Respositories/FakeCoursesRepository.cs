using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Api.Courses.Interfaces;
using Api.Courses.Helpers;
using Api.Courses.Models;



namespace Api.Courses.Respositories
{
    public class FakeCoursesRepository: ICoursesRespository
    {
        private readonly List<CourseModel> _repo = new List<CourseModel>();
        private readonly ILogger<FakeCoursesRepository> _logger;

        public FakeCoursesRepository(ILogger<FakeCoursesRepository> logger)
        {
            _logger = logger;

            _repo.Add(new CourseModel()
            {
                Id = 1,
                Name = "Programando en Blazor - ASP.NET Core 3",
                Description = "Crea aplicaciones web interactivas con C#",
                CreatedAt = DateTime.Now
            });
            _repo.Add(new CourseModel()
            {
                Id = 2,
                Name = "Master en JavaScript: Aprender JS, jQuery, Angular 8, NodeJS",
                Description = "Aprende a programar desde cero y desarrollo web con JavaScript, jQuery, JSON, TypeScript, Angular, Node, MEAN, +30 horas",
                CreatedAt = DateTime.Now
            });
            _repo.Add(new CourseModel()
            {
                Id = 3,
                Name = "Diseño Web Desde Cero a Avanzado 45h Curso COMPLETO",
                Description = "Aprende a Diseñar Páginas Web Responsive Design, atractivas, de forma profesional y sin dificultad con HTML5 y CSS3",
                CreatedAt = DateTime.Now
            });
            _repo.Add(new CourseModel()
            {
                Id = 4,
                Name = "Diseño Web Profesional El Curso Completo, Práctico y desde 0",
                Description = "HTML5, CSS3, Responsive Design, Adobe XD, SASS, JavaScript, jQuery, Bootstrap 4, WordPress, Git, GitHub",
                CreatedAt = DateTime.Now
            });
            _repo.Add(new CourseModel()
            {
                Id = 5,
                Name = "Curso de Desarrollo Web Completo 2.0",
                Description = "¡Aprende haciendo! HTML5, CSS3, Javascript, jQuery, Bootstrap 4, WordPress, PHP, MySQL, APIs, apps móviles y Python",
                CreatedAt = DateTime.Now
            });

        }

        public Task<Response> AddAsync(CourseModel course)
        {
            try
            {
                _logger?.LogInformation($"Add course with Id {course.Id}");
                course.Id = _repo.Max(c => c.Id) + 1;
                course.CreatedAt = DateTime.Now;
                _repo.Add(course);
                return Task.FromResult(new Response()
                {
                    IsSuccess = true,
                    Data = course
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

        public Task<Response> GetByIdAsync(int courseId)
        {
            try
            {
                _logger?.LogInformation($"querying course by Id {courseId}");

                var course = _repo.FirstOrDefault(c => c.Id == courseId);
                if (course == null) return Task.FromResult(new Response() {IsSuccess = false , ErrorMessage = "Resource was not found"});

                return Task.FromResult(new Response()
                {
                    IsSuccess = true,
                    Data = course
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

        public Task<Response> GetAllAsync(int page, int pageSize)
        {
            try
            {
                _logger?.LogInformation($"querying courses with page:{page}, size:{pageSize}");

                var result = _repo.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                return Task.FromResult(new Response()
                {
                    IsSuccess = true,
                    Data = result
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

        public Task<Response> SearchAsync(string search)
        {
            try
            {
                _logger?.LogInformation($"querying courses by {search}");

                var result = _repo.Where(c => c.Name.ToLowerInvariant().Contains(search.ToLowerInvariant())).ToList();
                return Task.FromResult(
                     new Response()
                     {
                         IsSuccess = true,
                         Data = result
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

        public Task<Response> UpdateAsync(int id, CourseModel course)
        {
            try
            {
                _logger?.LogInformation($"Update course with Id: {id}");
                var courseToUpdate = _repo.FirstOrDefault(c => c.Id == id);
                if (courseToUpdate != null)
                {
                    courseToUpdate.Name = course.Name;
                    courseToUpdate.Description = course.Description;

                    return Task.FromResult(new Response()
                    {
                        IsSuccess = true,
                        Data = courseToUpdate
                    });
                }
                else
                {
                    _logger?.LogWarning($"The course with Id: {id} not found");
                    return Task.FromResult(new Response()
                    {
                        IsSuccess = false,
                        ErrorMessage = $"The course with Id: {id} not found"
                    });
                }
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

        public async Task<Response> DeleteAsync(int courseId)
        {
            try
            {
                _logger?.LogInformation($"Deleting course with Id: {courseId}");
                var courseToDelete = _repo.FirstOrDefault(c => c.Id == courseId);
                if (courseToDelete == null)
                {
                    _logger?.LogWarning($"Resource not found");
                    return new Response()
                    {
                        IsSuccess = false,
                        ErrorMessage = "Resource not found"
                    };
                }

                _repo.Remove(courseToDelete);
                return new Response()
                {
                    IsSuccess = true,
                };
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
