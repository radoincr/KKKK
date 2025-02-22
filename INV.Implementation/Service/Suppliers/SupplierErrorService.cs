using INV.Domain.Shared;

namespace INV.Implementation.Service.Suppliers;

public class SupplierErrorService
{
    public static ErrorCode RCSupplierExsist { get; } =
        new ErrorCode("SupplierError.RCSupplierExsist", 
            "This RC is already used by another supplier");

    public static ErrorCode NISSupplierExsist { get; } =
        new ErrorCode("SupplierError.NISSupplierExsist", 
            "This NIS is already used by another supplier");
    
    public static ErrorCode RIBSupplierExsist { get; } =
        new ErrorCode("SupplierError.RIBSupplierExsist",
            "This RIB is already used by another supplier");
}