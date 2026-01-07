using InventoryService.Application.Products.Commands.CreateProduct.DTOs;
using InventoryService.Domain.Storage;
using InventoryService.Domain.Types;
using Shared.Application.Infrastructure.Cqs;
using Shared.Application.Infrastructure.Results;
using Shared.Domain.Types;
using IResult = Shared.Application.Infrastructure.Results.Abstractions.IResult;

namespace InventoryService.Application.Products.Commands.CreateProduct;

public class CreateProductCommand(CreateProductDto dto) : Command
{
    public CreateProductDto Dto { get; set; } = dto;
}

public class CreateProductCommandHandler(IProductRepository repository) : CommandHandler<CreateProductCommand>
{
    protected override async Task<IResult> HandleAsync(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Dto.Name,
            InStockQuantity = request.Dto.InStockQuantity,
            Price = Money.FromRub(request.Dto.PriceRub)
        };

        await repository.AddAsync(product, cancellationToken);
        await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}