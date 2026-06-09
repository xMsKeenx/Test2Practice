using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var order = await _context.Orders
            .AsNoTracking()
            .Where(o => o.Id == id)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                CreatedAt = o.CreatedAt,
                FulfilledAt = o.FulfilledAt,
                Status = o.Status.Name,
                Client = new ClientDto
                {
                    FirstName = o.Client.FirstName,
                    LastName = o.Client.LastName
                },
                Products = o.ProductOrders
                    .Select(po => new OrderProductDto
                    {
                        Name = po.Product.Name,
                        Price = po.Product.Price,
                        Amount = po.Amount
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (order == null)
        {
            return NotFound($"Order with id {id} does not exist.");
        }

        return Ok(order);
    }

    [HttpPut("{id:int}/fulfill")]
    public async Task<IActionResult> FulfillOrder(int id, FulfillOrderDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var order = await _context.Orders
            .Include(o => o.Status)
            .Include(o => o.ProductOrders)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound($"Order with id {id} does not exist.");
        }

        if (order.Status.Name == "Completed")
        {
            return BadRequest("Order is already completed.");
        }

        var newStatus = await _context.Statuses
            .FirstOrDefaultAsync(s => s.Name == dto.StatusName);

        if (newStatus == null)
        {
            return NotFound($"Status '{dto.StatusName}' does not exist.");
        }

        order.IdStatus = newStatus.Id;
        order.FulfilledAt = DateTime.Now;

        _context.ProductOrders.RemoveRange(order.ProductOrders);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}