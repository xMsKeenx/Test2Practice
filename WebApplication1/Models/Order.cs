namespace WebApplication1.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? FulfilledAt { get; set; }

    public int IdClient { get; set; }
    public Client Client { get; set; } = null!;

    public int IdStatus { get; set; }
    public Status Status { get; set; } = null!;

    public ICollection<ProductOrder> ProductOrders { get; set; } = new List<ProductOrder>();
}