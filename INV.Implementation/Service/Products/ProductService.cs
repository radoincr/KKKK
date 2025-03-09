using INV.App.Products;
using INV.Domain.Entities.Products;
using INV.Domain.Shared;
using INV.Infrastructure.Storage.Products;

namespace INV.Implementation.Service.Products;

public class ProductService : IProductService
{
    private readonly IProductStorage productStorage;

    public ProductService(IProductStorage productStorage)
    {
        this.productStorage = productStorage;
    }

    public async Task<Result> CreateProduct(Product product)
    {
        var errorList = validateProductCreate(product);
        if (errorList.Any())
            return Result.Failure(errorList.First());

        var designationExists = await productStorage.ProductExistsByaDesignation(product.Designation);
        if (designationExists)
            errorList.Add(ProductError.DesignationExsist);

        if (errorList.Any())
            return Result.Failure(errorList.First());

        await productStorage.InsertProduct(product);
        return Result.Success();
    }

    public async Task<int> SetProducts(Product product)
    {
        return await productStorage.UpdateProduct(product);
    }

    public async Task<int> RemoveProduct(Guid id)
    {
        return await productStorage.DeleteProduct(id);
    }

    public async Task<List<Product>> GetProducts()
    {
        return await productStorage.SelectProducts();
    }

    private List<Error> validateProductCreate(Product product)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(product.Designation))
            errors.Add(ProductError.DesignationExsist);

        return errors;
    }
}