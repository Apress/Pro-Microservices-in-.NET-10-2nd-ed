namespace MyMicroservice.Pages.Orders;

public class OrderListModel(IOrderRepository orderRepository) : PageModel
{
    public List<Order> Orders { get; set; } = new();

    public void OnGet()
    {
        Orders = orderRepository.GetAllOrders();
    }
}
