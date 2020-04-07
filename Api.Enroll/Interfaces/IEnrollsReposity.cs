
using System.Threading.Tasks;
using Api.Enroll.Helpers;


namespace Api.Enroll.Interfaces
{
    /*
     * --- Obtener usuarios registrados en un curso ( enrolleTypeID = 1 or 2, courseID ) -> [... Student or Teacher]
     * --- Obtener cursos por enrrolled (tech or studen)  (  enrolledID ) -> [...Courses ]
     * --- Obtener enrolled por courseId  (  courseId ) -> [...Enrolled ]
     * --- Guardar enrolled (enrolleTypeID, courseID, enrolledID) -> { isSuccess, enrolled }
     * --- Remover enrolled (courseID, enrolledID) -> { isSuccess, enrolled }
     */
    public interface IEnrollsRepository
    {
        Task<Response> AddEnrolledAsync(int EnrolledId, int CourseId, int EnrollTypeId);

        Task<Response> GetCoursesByEnrolledIdAsync(int EnrolledId);

        Task<Response> GetEnrolledByIdAsync(int Id);

        Task<Response> GetEnrolledByTypeIdAndCourseIdAsync(int EnrollTypeId, int CourseId);

        Task<Response> RemoveEnrolledAsync(int EnrolledId, int CourseId);

    }
}
