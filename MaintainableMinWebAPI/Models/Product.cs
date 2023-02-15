namespace MaintainableMinWebAPI.Models;

public partial class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public decimal ListPrice { get; set; }
}
