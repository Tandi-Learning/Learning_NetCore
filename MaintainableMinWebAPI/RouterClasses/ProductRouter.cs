using MaintainableMinWebAPI.Components;
using MaintainableMinWebAPI.Models;
using System.Linq;

namespace MaintainableMinWebAPI.RouterClasses;

public class ProductRouter : RouterBase
{
    public ProductRouter()
    {
        urlFragment = "product";
    }

    protected virtual IEnumerable<Product> GetAll()
    {
        return new List<Product>
        {
            new Product
            {
                ProductId = 706,
                Name = "HL Road Frame - Red, 58",
                Color = "Red",
                ListPrice = 1500.0000m
            },
            new Product
            {
                ProductId = 707,
                Name = "Sport-100 Helmet, Red",
                Color = "Red",
                ListPrice = 34.9900m
            },
            new Product
            {
                ProductId = 708,
                Name = "Sport-100 Helmet, Black",
                Color = "Black",
                ListPrice = 34.9900m
            },
            new Product
            {
                ProductId = 709,
                Name = "Mountain Bike Socks, M",
                Color = "White",
                ListPrice = 9.5000m
            },
            new Product
            {
                ProductId = 710,
                Name = "Mountain Bike Socks, L",
                Color = "White",
                ListPrice = 9.5000m
            }
        };
    }
    
    protected virtual IResult Get()
    {
        return Results.Ok(GetAll());
    }

    protected virtual IResult Get(int id)
    {
        var product = GetAll().ToList<Product>().Find(product => product.ProductId == id);
        if (product is null)
            return Results.NotFound();
        else
            return Results.Ok(product);
    }

    protected virtual IResult Post(Product entity)
    {
        entity.ProductId = GetAll().Max(p => p.ProductId) + 1;

        return Results.Created($"{urlFragment}/{entity.ProductId}", entity);
    }

    public override void AddRoutes(WebApplication app)
    {
        app.MapGet($"/{urlFragment}", () => Get());
        app.MapGet($"/{urlFragment}/{{id:int}}", (int id) => Get(id));
    }
}