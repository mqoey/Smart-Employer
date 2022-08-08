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
    public class EmployeeController : ControllerBase
    {
        private SmartEmployerDbContext _dbContext;
        public EmployeeController(SmartEmployerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<employeeController>
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                var employee = _dbContext.Employees.ToList();

                if (employee.Count == 0)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "No Employees Found" });
                }
                else
                    return Ok(employee);

            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occured: " + e.ToString());

            }
        }

        // GET api/<employeeController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var employee = _dbContext.Employees.FirstOrDefault(x => x.Id == id);
                if (employee == null)
                {
                    return StatusCode(404, "employee not found");
                }
                return Ok(employee);
            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occured" + e.ToString());
            }
        }

        // POST api/<employeeController>
        [HttpPost]
        public IActionResult Post([FromBody] Employee request)
        {
            Employee employee = new Employee();
            employee.FirstName = request.FirstName.ToLower();
            employee.LastName = request.LastName.ToLower();
            employee.UserId = request.UserId;
            employee.DesignationId = request.DesignationId.ToLower();
            employee.Gender = request.Gender.ToLower();
            employee.Address = request.Address.ToLower();
            employee.NationalId = request.NationalId.ToLower();
            employee.DateOfBirth = request.DateOfBirth;
            employee.EngagementmentDate = request.EngagementmentDate;
            employee.CreatedDate = request.CreatedDate;
            employee.LastModifiedDate = request.LastModifiedDate;

            try
            {
                if (_dbContext.Employees.Any(p => p.FirstName.ToLower() == request.FirstName))
                {
                    return StatusCode(500, "employee with the same name exists.");
                }
                _dbContext.Employees.Add(employee);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occurred" + e.ToString());
            }

            //get new added/edited item
            var new_item = _dbContext.Employees.Where(a => a.FirstName.Equals(request.FirstName))
                    .FirstOrDefault();
            return Ok(new_item);
        }

        // PUT api/<employeeController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Employee request)
        {
            try
            {
                var employee = _dbContext.Employees.FirstOrDefault(x => x.Id == id);
                if (employee == null)
                {
                    return StatusCode(404, "employee not found");
                }
                employee.FirstName = request.FirstName.ToLower();
                employee.LastName = request.LastName.ToLower();
                employee.UserId = request.UserId;
                employee.DesignationId = request.DesignationId.ToLower();
                employee.Gender = request.Gender.ToLower();
                employee.Address = request.Address.ToLower();
                employee.NationalId = request.NationalId.ToLower();
                employee.DateOfBirth = request.DateOfBirth;
                employee.EngagementmentDate = request.EngagementmentDate;
                employee.LastModifiedDate = request.LastModifiedDate;
                _dbContext.Entry(employee).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occured" + e.ToString());
            }

            //get new added/edited item
            var new_item = _dbContext.Employees.Where(a => a.FirstName.Equals(request.FirstName))
                    .FirstOrDefault();

            return Ok(new_item);
        }

        // DELETE api/<employeeController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var employee = _dbContext.Employees.FirstOrDefault(x => x.Id == id);
                if (employee == null)
                {
                    return StatusCode(404, "employee not found");
                }

                _dbContext.Entry(employee).State = EntityState.Deleted;
                _dbContext.SaveChanges();
                return Ok(employee);
            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occured" + e.ToString());
            }
        }
    }
}
