using System.ComponentModel.DataAnnotations;

namespace deleteme.Models.Entities
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<Invoice> Invoices { get; set; } = new List<Invoice>();

    }
}
