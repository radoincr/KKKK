using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVUIs.Models.Supplier
{
    internal class SupplierModel
    {
        public Guid ID { get; set; }
        public string NameSupplier { get; set; }
        public string NameCompany { get; set; }

        public string NameAccount { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9._%+-]*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Invalid email format. Email must start with a letter.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "RC is required.")]
        public string RC { get; set; }

        [Required(ErrorMessage = "ART is required.")]
        [Range(10000000000, 99999999999, ErrorMessage = "ART must be exactly 11 digits.")]
        public long ART { get; set; }

        [Required(ErrorMessage = "NIF is required.")]
        [Range(100000000000000, 999999999999999, ErrorMessage = "NIF must be exactly 15 digits.")]
        public long NIF { get; set; }

        [Required(ErrorMessage = "NIS is required.")]
        [Range(100000000000000, 999999999999999, ErrorMessage = "NIS must be exactly 15 digits.")]
        public int NIS { get; set; }

        [Required(ErrorMessage = "RIB is required.")]
        [StringLength(20, MinimumLength = 20, ErrorMessage = "RIB must be exactly 20 characters.")]
        public string RIB { get; set; }


        [Required(ErrorMessage = "Bank Agency is required.")]
        public string BankAgency { get; set; }
    }
}
