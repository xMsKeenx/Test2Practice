using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

public class FulfillOrderDto
{
    [Required]
    public string StatusName { get; set; } = null!;
}