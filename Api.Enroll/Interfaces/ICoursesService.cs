using System.Threading.Tasks;
using Api.Enroll.Helpers;
using System.Collections.Generic;

namespace Api.Enroll.Interfaces
{
    public interface ICoursesService
    {
        Task<Response> GetCourseById(int Id);

    }
}
