using System.ComponentModel.DataAnnotations;

namespace INVUIs.Purchases.PurchaseModels
{
    public class PurchaseValidateModel
    {
        [Required (ErrorMessage = "يلزم ادخال البيانات ب:")]
        public String Data { get; set; }
        [Required (ErrorMessage = "يلزم ادخال البيانات في:")]
        public String Data2 { get; set; }
    }
}