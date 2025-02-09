using System.ComponentModel.DataAnnotations;

namespace Web.Models.PurchaseModel;

public class ProductModel
{
    public Guid ID { get; set; }
    public int Number { get; set; }

    [Required(ErrorMessage = "Data is required.")]
    public string Data { get; set; }

    [Required(ErrorMessage = "Unit Measure is required.")]
    public string UnitOfMeasure { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Unit Price is required.")]
    public decimal UnitPrice { get; set; }

    [Required(ErrorMessage = "TVA is required.")]

    public string TVA { get; set; } = "19";
}