using INV.Domain.Entities.Purchases;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Components.Status
{
    public partial class Status
    {
        [Parameter] public PurchaseStatus status { get; set; }
        [Parameter] public bool icon { get; set; }
        [Parameter] public bool text { get; set; }

        private string GetStatusClass()
        {
            return status switch
            {
                PurchaseStatus.Validated => "status-completed",
                PurchaseStatus.Editing => "status-in-progress",
                PurchaseStatus.Cancelled => "status-pending",
                _ => string.Empty
            };
        }

        private string GetStatusIcon()
        {
            return status switch
            {
                PurchaseStatus.Validated => "bi bi-check-circle", // Font Awesome icon for completed
                PurchaseStatus.Editing => "bi bi-clock", // Font Awesome spinning icon
                PurchaseStatus.Cancelled => "bi bi-exclamation-circle", // Font Awesome clock icon
                _ => "fas fa-question-circle"
            };
        }
    }
}