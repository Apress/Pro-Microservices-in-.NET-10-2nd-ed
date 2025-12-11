namespace MyMicroservice.Pages.Orders;

public class OrderViewModel(IOrderRepository orderRepository) : PageModel
{
    public int Id { get; set; }
    public Order Order { get; set; } = new();

    public IActionResult OnGet(int? id)
    {
        Id = id ?? 0;
        var order = orderRepository.GetOrderById(Id);
        if (order == null)
        {
            return NotFound();
        }
        Order = order ?? new Order();
        return Page();
    }
}
