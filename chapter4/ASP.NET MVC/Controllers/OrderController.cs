namespace MyMicroservice.Controllers;

public class OrderController(IOrderRepository orderRepository) : Controller
{
    [HttpGet("order")]
    public IActionResult Index()
    {
        var orders = orderRepository.GetAllOrders();
        return View(orders);
    }

    [HttpGet("order/add")]
    public IActionResult Add()
    {
        Order order = new Order();
        return View();
    }

    [HttpPost("order/add")]
    public IActionResult Add(Order order)
    {
        if (!ModelState.IsValid)
        {
            // Fix your errors first
            return View(order);
        }
        var saved = orderRepository.SaveOrder(order);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("order/edit/{id}")]
    public IActionResult Edit(int id)
    {
        var order = orderRepository.GetOrderById(id);
        if (order == null)
        {
            return View("NotFound");
        }
        return View(order);
    }

    [HttpPost("order/edit/{id}")]
    public IActionResult Edit(int id, Order order)
    {
        if (!ModelState.IsValid || id != order.Id)
        {
            // Fix your errors first
            return View(order);
        }
        var saved = orderRepository.SaveOrder(order);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("order/delete/{id}")]
    public IActionResult Delete(int id)
    {
        var order = orderRepository.GetOrderById(id);
        if (order == null)
        {
            return View("NotFound");
        }
        return View(order);
    }

    [HttpPost("order/delete/{id}")]
    public IActionResult Delete(int id, string? confirm)
    {
        orderRepository.DeleteOrder(id);
        return RedirectToAction(nameof(Index));
    }
}
