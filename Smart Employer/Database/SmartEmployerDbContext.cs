using Microsoft.EntityFrameworkCore;
using Smart_Employer.Models;

namespace Smart_Employer.Database
{
    public class SmartEmployerDbContext : DbContext
    {
        public SmartEmployerDbContext(DbContextOptions<SmartEmployerDbContext> options) : base(options)
        {

        }

        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Designation> Designations { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Attendance> Attendances { get; set; } = null!;
    }
}
