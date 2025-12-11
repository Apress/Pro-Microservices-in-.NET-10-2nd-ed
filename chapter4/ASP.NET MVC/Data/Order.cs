namespace MyMicroservice.Data;

public class Order
{
    [Key]
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal OrderTotal { get; set; }
    // TODO: add other order details
}
