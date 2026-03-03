namespace VatTraders.Domain.Entities;

public class Invoice
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public DateTime IssuedAtUtc { get; set; } = DateTime.UtcNow;
    public decimal SubTotal { get; set; }
    public decimal GstTotal { get; set; }
    public decimal Cgst { get; set; }
    public decimal Sgst { get; set; }
    public decimal GrandTotal { get; set; }
    public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
}
