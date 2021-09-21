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

        [HttpGet("{id}")]
        public async Task<ActionResult<Students>> GetStudent(int id)
        {
            try
            {
                _logger.LogInformation("GetStudent : Begin.");
                var student = await _db.Students.FindAsync(id);

                if (student == null)
                {
                    _logger.LogInformation("GetStudent : Student not found.");
                    return NotFound(new { message = "Student not found." });
                }
                _logger.LogInformation("GetStudent Result : {0}", JsonConvert.SerializeObject(student).ToString());
                _logger.LogInformation("GetStudent : End.");
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetStudent Exception : {0}", ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, new { errorCode = "500", errorMessage = "Some Error occured" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Students>> PostStudent(Students student)
        {
            try
            {
                _logger.LogInformation("PostStudent : Begin.");
                _db.Students.Add(student);
               
                await _db.SaveChangesAsync();

                _logger.LogInformation("PostStudent : End.");
                return Ok(new { message = "Student Created Successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError("PostStudent Exception : {0}", ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, new { errorCode = "500", errorMessage = "Some Error occured" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Students>> PutStudent(int id, Students student)
        {
            try
            {
                _logger.LogInformation("PutStudent : Begin.");
                if (id != student.StudentId)
                {
                    _logger.LogInformation("PutStudent : Bad Request.");
                    return BadRequest(new { message = "Invalid data" });
                }
                if(!_db.Students.Any(e => e.StudentId == id))
                {
                    _logger.LogInformation("PutStudent : Student not found.");
                    return NotFound(new { message = "Student you are trying to update does not exists." });
                }
                _db.Entry(student).State=EntityState.Modified;

                await _db.SaveChangesAsync();

                _logger.LogInformation("PutStudent : End.");
                return Ok(new { message = "Student Updated Successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError("PutStudent Exception : {0}", ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, new { errorCode = "500", errorMessage = "Some Error occured" });
            }
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Students>> DeleteStudent(int id)
        {
            try
            {
                _logger.LogInformation("DeleteStudent : Begin.");
                var student = await _db.Students.FindAsync(id);
                if (student == null)
                {
                    _logger.LogInformation("DeleteStudent : Student not found.");
                    return NotFound(new { message = "Student not found." });
                }
                _logger.LogInformation("DeleteStudent Result : {0}", JsonConvert.SerializeObject(student).ToString());
                _logger.LogInformation("DeleteStudent : End.");
                _db.Students.Remove(student);
                await _db.SaveChangesAsync();

                return Ok(new { message = "Student Deleted." });
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteStudent Exception : {0}", ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, new { errorCode = "500", errorMessage = "Some Error occured" });
            }
        }
    }
}
