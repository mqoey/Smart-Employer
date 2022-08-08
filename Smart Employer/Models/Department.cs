using System.ComponentModel.DataAnnotations;

namespace Smart_Employer.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

    }
}
