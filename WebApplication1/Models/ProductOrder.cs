namespace WebApplication1.Models;

public class ProductOrder
{
    public int IdProduct { get; set; }
    public Product Product { get; set; } = null!;

    public int IdOrder { get; set; }
    public Order Order { get; set; } = null!;

    public int Amount { get; set; }
}