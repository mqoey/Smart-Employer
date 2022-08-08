using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Employer.Authentication;
using Smart_Employer.Database;
using Smart_Employer.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Smart_Employer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private SmartEmployerDbContext _dbContext;
        public AttendanceController(SmartEmployerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<attendanceController>
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                var attendance = _dbContext.attendances.ToList();

                if (attendance.Count == 0)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "No attendances Found" });
                }
                else
                    return Ok(attendance);

            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occured: " + e.ToString());

            }
        }

        // GET api/<attendanceController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var attendance = _dbContext.attendances.FirstOrDefault(x => x.Id == id);
                if (attendance == null)
                {
                    return StatusCode(404, "attendance not found");
                }
                return Ok(attendance);
            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occured" + e.ToString());
            }
        }

        // POST api/<attendanceController>
        [HttpPost("Check")]
        public IActionResult Post([FromBody] Attendance request)
        {
            Attendance attendance = new Attendance();
            attendance.EmployeeId = request.EmployeeId;
            attendance.CheckIn = request.CheckIn;
            attendance.CreatedDate = request.CreatedDate;
            attendance.LastModifiedDate = request.LastModifiedDate;

            try
            {
                if (_dbContext.attendances.Any(p => p.Name.ToLower() == request.Name))
                {
                    return StatusCode(500, "attendance with the same name exists.");
                }
                _dbContext.attendances.Add(attendance);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occurred" + e.ToString());
            }

            //get new added/edited item
            var new_item = _dbContext.attendances.Where(a => a.Name.Equals(request.Name))
                    .FirstOrDefault();
            return Ok(new_item);
        }

        // PUT api/<attendanceController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] attendance request)
        {
            try
            {
                var attendance = _dbContext.attendances.FirstOrDefault(x => x.Id == id);
                if (attendance == null)
                {
                    return StatusCode(404, "attendance not found");
                }
                attendance.Name = request.Name.ToLower();
                attendance.DepartmentId = request.DepartmentId;
                attendance.LastModifiedDate = request.LastModifiedDate;
                _dbContext.Entry(attendance).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occured" + e.ToString());
            }

            //get new added/edited item
            var new_item = _dbContext.attendances.Where(a => a.Name.Equals(request.Name))
                    .FirstOrDefault();

            return Ok(new_item);
        }

        // DELETE api/<attendanceController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var attendance = _dbContext.attendances.FirstOrDefault(x => x.Id == id);
                if (attendance == null)
                {
                    return StatusCode(404, "attendance not found");
                }

                _dbContext.Entry(attendance).State = EntityState.Deleted;
                _dbContext.SaveChanges();
                return Ok(attendance);
            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occured" + e.ToString());
            }
        }
    }
}
