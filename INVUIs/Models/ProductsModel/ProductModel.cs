using System.ComponentModel.DataAnnotations;

namespace INVUIs.Models.ProductsModel;

public class ProductModel
{
    public Guid ID { get; set; }
    public int Number { get; set; }
    public Guid IDPurchaseOrder { get; set; }
    [Required (ErrorMessage = "Name Product is required")]
    public string Designation { get; set; }
    
    [Required (ErrorMessage = "UnitMeasure is required")]
    public string UnitMeasure { get; set; }
    
    [Required (ErrorMessage = "Quantity is required")]
    public int Quantity { get; set; }
    
    [Required (ErrorMessage = "UnitPrice is required")]
    public decimal UnitPrice { get; set; }
    
    [Required (ErrorMessage = "TVA is required")]
    public decimal TVA { get; set; }

    public decimal TotalPrice { set; get; }
    public int DefaultTVARate { get; set; }
}