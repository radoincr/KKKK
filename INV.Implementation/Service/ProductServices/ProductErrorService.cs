using INV.Domain.Shared;

namespace INV.Implementation.Service.ProductServices;

public static class ProductErrorService
{
    public static Error DesignationProductExists { get; } =
        Error.Conflict("ProductError.DesignationProductExists", 
            "This Designation exists for another Product");
}