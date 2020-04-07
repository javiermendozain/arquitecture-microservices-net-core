
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Enroll.Interfaces;

namespace Api.Enroll.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class EnrollsController : ControllerBase
    {
        private readonly IEnrollsService _enrollService;

        public EnrollsController(IEnrollsService enrollsService)
        {
            this._enrollService = enrollsService;
        }

        // Obtener usuarios registrados en un curso ( enrolleTypeID = 1 or 2, courseID ) -> [... Student or Teacher]
        [HttpGet()]
        public async Task<IActionResult> GetEnrolledByTypeIdAndCourseIdAsync(int? enrolledTypeId, int? courseId)
        {
            if (enrolledTypeId == null || courseId == null) return BadRequest();

            var result = await _enrollService.GetEnrolledByTypeIdAndCourseIdAsync((int)enrolledTypeId, (int)courseId);

            if (result.IsSuccess) return Ok(result);

            return NotFound(result);
        }

        // Obtener cursos por enrrolled (tech or studen)  (  enrolledID ) -> [...Courses ]
        [HttpGet("courses")]
        public async Task<IActionResult> GetCoursesByEnrolledIdAsync(int? enrolledId)
        {
            if ( enrolledId == null ) return BadRequest();

            var result = await _enrollService.GetCoursesByEnrolledIdAsync((int) enrolledId);

            if (result.IsSuccess) return Ok(result);
            
            return NotFound(result);
        }
              
        // Obtener enrolled por courseId  (  courseId ) -> [...Id ]
        [HttpGet("count")]
        public async Task<IActionResult> GetEnrolledByIdAsync(int? enrolledId)
        {
            if (enrolledId == null) return BadRequest();

            var result = await _enrollService.GetEnrolledByIdAsync((int)enrolledId);

            if (result.IsSuccess)  return Ok(result);
            
            return NotFound(result);
        }
        
        // Guardar enrolled(enrolleTypeID, courseID, enrolledID) -> { isSuccess, enrolled
        [HttpPost]
        public async Task<IActionResult> AddEnrolledAsync(int? enrolledId, int? courseId, int? enrolledTypeId)
        {
            if (enrolledId == null || courseId == null || enrolledTypeId == null) return BadRequest();

            var result = await _enrollService.AddEnrolledAsync((int)enrolledId, (int)courseId, (int)enrolledTypeId);

            if (result.IsSuccess) return Ok(result);

            return NotFound(result);
        }

        // Remover enrolled (courseID, enrolledID) -> { isSuccess, enrolled }
        [HttpDelete]
        public async Task<IActionResult> RemoveEnrolledAsync(int? enrolledId, int? courseId)
        {
            if ( enrolledId == null || courseId == null ) return BadRequest();

            var result = await _enrollService.RemoveEnrolledAsync((int)enrolledId, (int)courseId);
            if (result.IsSuccess)  return Ok(result);

            return NotFound(result);
        }
    }
}