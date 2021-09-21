using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentsCrudApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StudentsCrudApi.Filters;

namespace StudentsCrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly SundayMobilityDbContext _db;
        private readonly ILogger<StudentController> _logger;

        public StudentController(SundayMobilityDbContext db, ILogger<StudentController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [ServiceFilter(typeof(StudentRequestResponseLoggerFilter))]        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Students>>> GetStudents()
        {
            try
            {
                _logger.LogInformation("GetStudents : Begin.");
                var students = await _db.Students.ToListAsync();
                _logger.LogInformation("GetStudents Result : {0}", JsonConvert.SerializeObject(students).ToString());
                _logger.LogInformation("GetStudents : End.");
                return Ok(students);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetStudents Exception : {0}", ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, new { errorCode = "500", errorMessage = "Some Error occured" });
            }
            
        }

        [ServiceFilter(typeof(StudentRequestResponseLoggerFilter))]
        [HttpGet("{id}")]
        public async Task<ActionResult<Students>> GetStudent(int id)
        {
            try
            {
                var student = await _db.Students.FindAsync(id);

                if (student == null)
                {
                    return NotFound(new { message = "Student not found." });
                }
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetStudent Exception : {0}", ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, new { errorCode = "500", errorMessage = "Some Error occured" });
            }
        }

        [ServiceFilter(typeof(StudentRequestResponseLoggerFilter))]
        [HttpPost]
        public async Task<ActionResult<Students>> PostStudent(Students student)
        {
            try
            {
                _db.Students.Add(student);
               
                await _db.SaveChangesAsync();

                return Ok(new { message = "Student Created Successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError("PostStudent Exception : {0}", ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, new { errorCode = "500", errorMessage = "Some Error occured" });
            }
        }

        [ServiceFilter(typeof(StudentRequestResponseLoggerFilter))]
        [HttpPut("{id}")]
        public async Task<ActionResult<Students>> PutStudent(int id, Students student)
        {
            try
            {
                if (id != student.StudentId)
                {
                    return BadRequest(new { message = "Invalid request data" });
                }
                if(!_db.Students.Any(e => e.StudentId == id))
                {
                    return NotFound(new { message = "Student you are trying to update does not exists." });
                }
                _db.Entry(student).State=EntityState.Modified;

                await _db.SaveChangesAsync();

                return Ok(new { message = "Student Updated Successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError("PutStudent Exception : {0}", ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, new { errorCode = "500", errorMessage = "Some Error occured" });
            }
        }

        [ServiceFilter(typeof(StudentRequestResponseLoggerFilter))]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Students>> DeleteStudent(int id)
        {
            try
            {
                var student = await _db.Students.FindAsync(id);
                if (student == null)
                {
                    return NotFound(new { message = "Student not found." });
                }
                _db.Students.Remove(student);
                await _db.SaveChangesAsync();

                return Ok(new { message = "Student Deleted Successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteStudent Exception : {0}", ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, new { errorCode = "500", errorMessage = "Some Error occured" });
            }
        }
    }
}
