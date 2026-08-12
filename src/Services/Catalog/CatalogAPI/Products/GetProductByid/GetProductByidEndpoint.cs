namespace CatalogAPI.Products.GetProductByid;

//public record GetProductByIdRequest();

public record GetProductByidResponse(Product Product);

public class GetProductByidEndpoint : ICarterModule
{

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByidQuery(id));

            var response = result.Adapt<GetProductByidResponse>();

            return Results.Ok(response);
        })

        .WithName("GetProductByid")
        .Produces<GetProductByidResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By Id");
    }
}
