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
    public class DepartmentController : ControllerBase
    {

        private SmartEmployerDbContext _dbContext;
        public DepartmentController(SmartEmployerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<DepartmentController>
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                //var departments = _dbContext.tbldepartment.Skip((paginator.page - 1) * paginator.limit)
                //    .Take(paginator.limit)
                //    .ToList();

                var departments = _dbContext.Departments.ToList();

                if (departments.Count == 0)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "No departments Found" });
                }
                else
                    return Ok(departments);

            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occured: " + e.ToString());

            }
        }

        // GET api/<DepartmentController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var department = _dbContext.Departments.FirstOrDefault(x => x.Id == id);
                if (department == null)
                {
                    return StatusCode(404, "Department not found");
                }
                return Ok(department);
            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occured" + e.ToString());
            }
        }

        // POST api/<DepartmentController>
        [HttpPost]
        public IActionResult Post([FromBody] Department request)
        {
            Department department = new Department();
            department.Name = request.Name.ToLower();
            department.CreatedDate = request.CreatedDate;
            department.LastModifiedDate = request.LastModifiedDate;

            try
            {
                if (_dbContext.Departments.Any(p => p.Name.ToLower() == request.Name))
                {
                    return StatusCode(500, "department with the same name exists.");
                }
                _dbContext.Departments.Add(department);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occurred" + e.ToString());
            }

            //get new added/edited item
            var new_item = _dbContext.Departments.Where(a => a.Name.Equals(request.Name))
                    .FirstOrDefault();
            return Ok(new_item);
        }

        // PUT api/<DepartmentController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Department request)
        {
            try
            {
                var department = _dbContext.Departments.FirstOrDefault(x => x.Id == id);
                if (department == null)
                {
                    return StatusCode(404, "Department not found");
                }
                department.Name = request.Name.ToLower();
                department.LastModifiedDate = request.LastModifiedDate;
                _dbContext.Entry(department).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occured" + e.ToString());
            }

            //get new added/edited item
            var new_item = _dbContext.Departments.Where(a => a.Name.Equals(request.Name))
                    .FirstOrDefault();

            return Ok(new_item);
        }

        // DELETE api/<DepartmentController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var department = _dbContext.Departments.FirstOrDefault(x => x.Id == id);
                if (department == null)
                {
                    return StatusCode(404, "Department not found");
                }

                _dbContext.Entry(department).State = EntityState.Deleted;
                _dbContext.SaveChanges();
                return Ok(department);
            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occured" + e.ToString());
            }
        }
    }
}
