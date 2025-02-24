using INV.Domain.Shared;

namespace INV.Domain.Entities.Products;

public class ProductError
{
    public static Error DesignationExsist => Error.Conflict("ProductConflict.DesignationExsist",
        "This Designation is Exsist by other Product");
}