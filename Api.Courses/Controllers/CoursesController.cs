using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Courses.Interfaces;
using Api.Courses.Models;
using Api.Courses.Helpers;


namespace Api.Courses.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICoursesService _coursesService;

        public CoursesController(ICoursesService coursesService)
        {
            _coursesService = coursesService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Collection of Courses</returns>
        [HttpGet]
        public async Task<IActionResult > GetAllAsync(int page = 1, int pageSize = 10 )
        {
            var results = await _coursesService.GetAllAsync(page, pageSize);
            if (results.IsSuccess)  return Ok(results);

            return NotFound(results);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>One course or Id inputted</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int? id)
        {
            if (id == null) return BadRequest();
            
            var result = await _coursesService.GetByIdAsync((int) id);
            if (result.IsSuccess) return Ok(result);

            return NotFound(result);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Collection of Courses</returns>
        [HttpGet("search/{search}")]
        public async Task<IActionResult> SearchAsync(string search)
        {
            if (search == null) return BadRequest();

            var results = await _coursesService.SearchAsync(search);

            if (results.IsSuccess) return Ok(results);
           
            return NotFound(search);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="course"></param>
        /// <returns>Id of course added</returns>
        [HttpPost]
        public async Task<IActionResult> AddAsync(CourseModel course)
        {
            if (course == null) return BadRequest();
          
            var result = await _coursesService.AddAsync(course);
            if (result.IsSuccess) return Ok(result);
        
            return NotFound(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="course"></param>
        /// <returns>true or false</returns>
        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateAsync(int? id, CourseModel course)
        {
            if (id == null || course == null) return BadRequest();

            var result = await _coursesService.UpdateAsync((int) id, course);
            if (result.IsSuccess) return Ok(result);

            return NotFound(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns>isSuccess</returns>
        /// <returns>Id</returns>
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteAsync(int? courseId )
        {
            if (courseId == null) return BadRequest();

            var result = await _coursesService.DeleteAsync( (int) courseId);
            if (result.IsSuccess) return Ok(result);

            return NotFound(result);
        }

    }
}