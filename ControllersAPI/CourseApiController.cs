using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMVC.Data;
using Asp.Versioning;
using WebMVC.Models;


namespace WebAppMVC.Controllers.Api
{
    // ============= VERSION 1 =============
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CoursesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CoursesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/v1/CoursesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        // GET: api/v1/CoursesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // POST: api/v1/CoursesApi
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }

        // PUT: api/v1/CoursesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest(new { Message = "ID mismatch" });
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Courses.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/v1/CoursesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    // ============= VERSION 2 =============
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/CoursesApi")]
    public class CoursesApiV2Controller : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CoursesApiV2Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/v2/CoursesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCoursesV2()
        {
            // Di V2, kita hanya mengembalikan Id, CourseName, CourseCode, dan Instructor
            var courses = await _context.Courses
                .Select(c => new 
                { 
                    c.Id, 
                    c.CourseName, 
                    c.CourseCode,
                    c.Instructor,
                    Version = "2.0" // Penanda bahwa ini dari API V2
                })
                .ToListAsync();

            return Ok(courses);
        }

        // GET: api/v2/CoursesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCourseV2(int id)
        {
            var course = await _context.Courses
                .Where(c => c.Id == id)
                .Select(c => new 
                { 
                    c.Id, 
                    c.CourseName, 
                    c.CourseCode,
                    c.Description,
                    c.Instructor,
                    Version = "2.0"
                })
                .FirstOrDefaultAsync();

            if (course == null)
            {
                return NotFound(new { Message = "Course not found", Version = "2.0" });
            }

            return Ok(course);
        }

        // POST: api/v2/CoursesApi
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourseV2(Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourseV2), new { id = course.Id }, new
            {
                course.Id,
                course.CourseName,
                course.CourseCode,
                course.Instructor,
                Version = "2.0",
                Message = "Course created successfully"
            });
        }

        // PUT: api/v2/CoursesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourseV2(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest(new { Message = "ID mismatch", Version = "2.0" });
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Courses.Any(e => e.Id == id))
                {
                    return NotFound(new { Message = "Course not found", Version = "2.0" });
                }
                throw;
            }

            return Ok(new { Message = "Course updated successfully", Version = "2.0" });
        }

        // DELETE: api/v2/CoursesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourseV2(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound(new { Message = "Course not found", Version = "2.0" });
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Course deleted successfully", Version = "2.0" });
        }
    }
}