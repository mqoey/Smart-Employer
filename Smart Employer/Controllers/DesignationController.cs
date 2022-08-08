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
    public class DesignationController : ControllerBase
    {

        private SmartEmployerDbContext _dbContext;
        public DesignationController(SmartEmployerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<designationController>
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                var designation = _dbContext.Designations.ToList();

                if (designation.Count == 0)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "No Designations Found" });
                }
                else
                    return Ok(designation);

            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occured: " + e.ToString());

            }
        }

        // GET api/<designationController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var designation = _dbContext.Designations.FirstOrDefault(x => x.Id == id);
                if (designation == null)
                {
                    return StatusCode(404, "designation not found");
                }
                return Ok(designation);
            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occured" + e.ToString());
            }
        }

        // POST api/<designationController>
        [HttpPost]
        public IActionResult Post([FromBody] Designation request)
        {
            Designation designation = new Designation();
            designation.Name = request.Name.ToLower();
            designation.DepartmentId = request.DepartmentId;
            designation.CreatedDate = request.CreatedDate;
            designation.LastModifiedDate = request.LastModifiedDate;

            try
            {
                if (_dbContext.Designations.Any(p => p.Name.ToLower() == request.Name))
                {
                    return StatusCode(500, "designation with the same name exists.");
                }
                _dbContext.Designations.Add(designation);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occurred" + e.ToString());
            }

            //get new added/edited item
            var new_item = _dbContext.Designations.Where(a => a.Name.Equals(request.Name))
                    .FirstOrDefault();
            return Ok(new_item);
        }

        // PUT api/<designationController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Designation request)
        {
            try
            {
                var designation = _dbContext.Designations.FirstOrDefault(x => x.Id == id);
                if (designation == null)
                {
                    return StatusCode(404, "designation not found");
                }
                designation.Name = request.Name.ToLower();
                designation.DepartmentId = request.DepartmentId;
                designation.LastModifiedDate = request.LastModifiedDate;
                _dbContext.Entry(designation).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occured" + e.ToString());
            }

            //get new added/edited item
            var new_item = _dbContext.Designations.Where(a => a.Name.Equals(request.Name))
                    .FirstOrDefault();

            return Ok(new_item);
        }

        // DELETE api/<designationController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var designation = _dbContext.Designations.FirstOrDefault(x => x.Id == id);
                if (designation == null)
                {
                    return StatusCode(404, "designation not found");
                }

                _dbContext.Entry(designation).State = EntityState.Deleted;
                _dbContext.SaveChanges();
                return Ok(designation);
            }
            catch (Exception e)
            {

                return StatusCode(500, "An error has occured" + e.ToString());
            }
        }
    }
}
