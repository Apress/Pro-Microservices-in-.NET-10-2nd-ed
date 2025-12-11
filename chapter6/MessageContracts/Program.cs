namespace MessageContracts;
public class InvoiceCreated
{
	public int InvoiceNumber { get; set; }
	public InvoiceToCreate? InvoiceData { get; set; }
}
public class InvoiceToCreate
{
	public int CustomerNumber { get; set; }
	public List<InvoiceItems>? InvoiceItems { get; set; }
}
public class InvoiceItems
{
	public required string Description { get; set; }
	public double Price { get; set; }
	public double ActualMileage { get; set; }
	public double BaseRate { get; set; }
	public bool IsOversized { get; set; }
	public bool IsRefrigerated { get; set; }
	public bool IsHazardousMaterial { get; set; }
}
