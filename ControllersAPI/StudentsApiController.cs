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
    public class StudentsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/v1/StudentsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/v1/StudentsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // POST: api/v1/StudentsApi
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // PUT: api/v1/StudentsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest(new { Message = "ID mismatch" });
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Students.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/v1/StudentsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    // ============= VERSION 2 =============
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/StudentsApi")]
    public class StudentsApiV2Controller : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsApiV2Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/v2/StudentsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetStudentsV2()
        {
            // Di V2, kita hanya mengembalikan Id, Name, dan Email
            var students = await _context.Students
                .Select(s => new 
                { 
                    s.Id, 
                    s.Name, 
                    s.Email,
                    Version = "2.0" // Penanda bahwa ini dari API V2
                })
                .ToListAsync();

            return Ok(students);
        }

        // GET: api/v2/StudentsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetStudentV2(int id)
        {
            var student = await _context.Students
                .Where(s => s.Id == id)
                .Select(s => new 
                { 
                    s.Id, 
                    s.Name, 
                    s.Email,
                    Version = "2.0"
                })
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound(new { Message = "Student not found", Version = "2.0" });
            }

            return Ok(student);
        }

        // POST: api/v2/StudentsApi
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudentV2(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudentV2), new { id = student.Id }, new
            {
                student.Id,
                student.Name,
                student.Email,
                Version = "2.0",
                Message = "Student created successfully"
            });
        }

        // PUT: api/v2/StudentsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentV2(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest(new { Message = "ID mismatch", Version = "2.0" });
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Students.Any(e => e.Id == id))
                {
                    return NotFound(new { Message = "Student not found", Version = "2.0" });
                }
                throw;
            }

            return Ok(new { Message = "Student updated successfully", Version = "2.0" });
        }

        // DELETE: api/v2/StudentsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentV2(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound(new { Message = "Student not found", Version = "2.0" });
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Student deleted successfully", Version = "2.0" });
        }
    }
}