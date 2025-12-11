namespace MyMicroservice.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController(IOrderRepository orderRepository) : ControllerBase
{
    [HttpGet]
    public List<Order> GetList()
    {
        return orderRepository.GetAllOrders();
    }

    [HttpGet("{id}")]
    public ActionResult<Order> GetById(int id)
    {
        var order = orderRepository.GetOrderById(id);
        if (order == null)
        {
            return NotFound();
        }
        return order;
    }

    [HttpPost]
    public ActionResult Add(Order order)
    {
        order.Id = 0;
        orderRepository.SaveOrder(order);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    public ActionResult<Order> Edit(int id, Order order)
    {
        if (id < 1)
        {
            return NotFound();
        }
        order.Id = id;
        orderRepository.SaveOrder(order);
        return order;
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        orderRepository.DeleteOrder(id);
        return Ok();
    }
}
