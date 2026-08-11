using BuildingBlocks.CQRS;
using CatalogAPI.Models;
using MediatR;

namespace CatalogAPI.Products.GreatProduct;
public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;
public record CreateProductResult(Guid ID);
internal class CreateProductCommandHandler 
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        //create Product entity from command object
        //save to database
        //return CreateProductResult result

        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        //save to database
        // return result
        return new CreateProductResult(Guid.NewGuid());
    }

}

