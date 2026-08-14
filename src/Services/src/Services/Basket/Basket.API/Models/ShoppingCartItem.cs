namespace Basket.API.Models;

public class ShoppingCartItem
{
    public int Quntity { get; set; } = default!;

    public string Color { get; set; } = default!;

    public string Price { get; set; } = default!;

    public Guid ProductId { get; set; } = default!;

    public string ProductName { get; set; } = default!;
}