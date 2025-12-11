namespace MyMicroservice.Repositories;

public interface IOrderRepository
{
    void DeleteOrder(int id);
    List<Order> GetAllOrders();
    Order? GetOrderById(int id);
    int SaveOrder(Order order);
}

public class OrderRepository(MyDbContext db) : IOrderRepository
{

    public int SaveOrder(Order order)
    {
        if (order.Id < 1)
        {
            // Add
            db.Orders.Add(order);
        }
        else
        {
            // Update
            db.Orders.Attach(order);
            db.Entry(order).State = EntityState.Modified;
        }
        return db.SaveChanges();
    }

    public List<Order> GetAllOrders() => db.Orders.ToList();

    public Order? GetOrderById(int id)
    {
        if (id < 1)
        {
            return null; // no need to ask for nothing
        }
        return (
            from o in db.Orders
            where o.Id == id
            select o
        ).FirstOrDefault();
    }

    public void DeleteOrder(int id)
    {
        Order? order = db.Orders.Find(id);
        if (order == null)
        {
            // TOD: throw?
            // the purpose was to make it not exist, and it doesn't. Is that success?
            return;
        }

        db.Orders.Remove(order);
        db.SaveChanges();
    }

}
