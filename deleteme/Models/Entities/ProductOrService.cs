

using System.ComponentModel.DataAnnotations;

namespace deleteme.Models.Entities
{
    public class ProductOrService
    {
        [Key]
        public int ProductOrServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ServiceType { get; set; }
        public double PercentTax { get; set; }
        public int Stock { get; set; }
        public List<InvoiceDetail> InvoiceDetails { get; set; }

    }
}
