namespace deleteme.Models.Entities
{
    public class InvoiceDetail
    {
        public int InvoiceDetailId { get; set; }
        public int InvoiceId { get; set; }
        public int ProductOrServiceId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } 
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }
        public Invoice Invoice { get; set; }
        public ProductOrService ProductOrService { get; set; }
    }
}
