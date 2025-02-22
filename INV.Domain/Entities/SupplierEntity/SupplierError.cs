using INV.Domain.Shared;

namespace INV.Domain.Entities.SupplierEntity;

public static class SupplierError
{
    public static ErrorCode RCExsist { get; } =
        new ErrorCode("SupplierError.RCExsist", "This RC is already used by another supplier");
    
}