namespace Smart_Employer.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? EngagementmentDate { get; set; }
        public string? NationalId { get; set; }
        public int UserId { get; set; }
        public string DesignationId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }


    }
}
