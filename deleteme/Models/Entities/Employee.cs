using System.ComponentModel.DataAnnotations;

namespace deleteme.Models.Entities
{
    public class Employee
    { 
        [Key]
            public int EmployeeId { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string Sex { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        public double Salary { get; set; }
        public string Dni { get; set; }
        public List<Invoice> Invoices { get; set; }


    }
}
