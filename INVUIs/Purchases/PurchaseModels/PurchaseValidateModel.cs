using System.ComponentModel.DataAnnotations;

namespace INVUIs.Purchases.PurchaseModels;

public class PurchaseValidateModel
{
    [Required(ErrorMessage = "يلزم ادخال البيانات ب:")]
    public string Data { get; set; }

    [Required(ErrorMessage = "يلزم ادخال البيانات في:")]
    public string Data2 { get; set; }
}