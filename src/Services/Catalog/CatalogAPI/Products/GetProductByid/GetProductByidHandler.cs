namespace CatalogAPI.Products.GetProductByid;

public record GetProductByidQuery(Guid Id) : IQuery<GetProductByidResult>;

public record GetProductByidResult(Product Product);



internal class GetProductByidQueryHandler
    (IDocumentSession session, ILogger<GetProductByidQueryHandler> logger)
    : IQueryHandler<GetProductByidQuery, GetProductByidResult>
{
    public async Task<GetProductByidResult> Handle(GetProductByidQuery query, CancellationToken cancellationToken)
    {
        
        logger.LogInformation("GetProductByidQueryHandler.Handle called with {@Query}", query);
        
        var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

        if(product is null)
        {
            throw new ProductNotFoundException();
        }

        return new GetProductByidResult(product);

    }
}
