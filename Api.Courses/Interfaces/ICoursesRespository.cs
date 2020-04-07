using Api.Courses.Models;
using System.Threading.Tasks;
using Api.Courses.Helpers;


namespace Api.Courses.Interfaces
{
    public interface ICoursesRespository
    {
        Task<Response> AddAsync(CourseModel course);
       
        Task<Response> GetAllAsync(int page, int pageSize);

        Task<Response> GetByIdAsync(int courseId);

        Task<Response> SearchAsync(string search);

        Task<Response> UpdateAsync(int id, CourseModel course);

        Task<Response> DeleteAsync(int courseId);

    }
}
