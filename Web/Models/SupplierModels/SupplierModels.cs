using System.ComponentModel.DataAnnotations;

namespace Web.Models.SupplierModels
{
    public class SupplierModel
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Name of Supplier is required.",AllowEmptyStrings = false)]
        public string NameSupplier { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        public string NameCompany { get; set; }

        [Required(ErrorMessage = "Account Name is required.")]
        public string NameAccount { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public int Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "RC is required.")]
        public string RC { get; set; }

        [Required(ErrorMessage = "ART is required."), ]
        public int ART { get; set; }

        [Required(ErrorMessage = "NIF is required.")]
        public int NIF { get; set; }

        [Required(ErrorMessage = "NIS is required.")]
        public int NIS { get; set; }

        [Required(ErrorMessage = "RIB is required.")]
        public int RIB { get; set; }

        [Required(ErrorMessage = "Bank Agency is required.")]
        public string BankAgency { get; set; }
    }
}