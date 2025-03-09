using INV.Domain.Shared;

namespace INV.Domain.Entities.Suppliers;

public static class SupplierError
{
    public static Error NISExsist =>
        Error.Conflict("SupplierConflict.NISExsist", "This NIS is already used by another supplier");

    public static Error RIBExsist =>
        Error.Conflict("SupplierConflict.RIBExsist", "This RIB is already used by another supplier");

    public static Error RCExsist(string rc)
    {
        return Error.Conflict("SupplierConflict.RCExsist", $"This {rc} is already used by another supplier");
    }
}