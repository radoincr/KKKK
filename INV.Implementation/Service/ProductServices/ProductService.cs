using INV.App.Products;
using INV.Domain.Entities.Products;
using INV.Domain.Shared;
using INV.Infrastructure.Storage.Products;

namespace INV.Implementation.Service.ProductServices
{
    public class ProductService : IProductService
    {
        private IProductStorage productStorage;

        public ProductService(IProductStorage productStorage)
        {
            this.productStorage = productStorage;
        }

        public async Task<Result> CreateProduct(Product product)
        {
            List<Error> errorList = validateProductCreate(product);
            if (errorList.Any())
                return Result.Failure(errorList.First());
    
            bool designationExists = await productStorage.ProductExistsByaDesignation(product.Designation);
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

        public async Task<List<Product>> SelectProducts()
        {
            return await productStorage.SelectProducts();
        }

        private List<Error> validateProductCreate(Product product)
        {
            List<Error> errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(product.Designation))
                errors.Add(ProductError.DesignationExsist);
    
            return errors;
        }
    }
}