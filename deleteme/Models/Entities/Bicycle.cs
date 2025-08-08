using System.ComponentModel.DataAnnotations;

namespace deleteme.Models.Entities
{
    public class Bicycle
    {
        [Key]
        public int BicycleId { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string? Model { get; set; }
        [MaxLength(50)]
        public string? Brand { get; set; }
        public int Year { get; set; }
        [MaxLength(50)]
        public string? Color { get; set; }
        [MaxLength(10)]
        public string Size { get; set; }

        public List<Invoice> Invoices { get; set; }

    }
}
