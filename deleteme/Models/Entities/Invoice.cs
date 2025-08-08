namespace deleteme.Models.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public int BicycleId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalTaxes { get; set; }
        public decimal TotalAmount { get; set; }

        public List<InvoiceDetail> InvoiceDetails { get; set; }
        public Bicycle Bicycle { get; set; }
        public Employee Employee { get; set; }
        public Client Client { get; set; }



    }
}
