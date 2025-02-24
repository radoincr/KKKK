namespace INV.Domain.Entities.Suppliers

{
    public interface ISupplier
    {
         Guid Id { set; get; }
         string CompanyName { set; get; }
         string ManagerName { get; set; }
         string Address { set; get; }
         string Phone { set; get; }
         string Email { set; get; }
         string RC { set; get; }
         string NIS { set; get; }
         string ART { set; get; }
         string RIB { set; get; }
         string NIF { set; get; }
         string BankAgency { set; get; }
         SupplierState State { set; get; }
    }
    public class Supplier : ISupplier
    {
        public Guid Id { set; get; }
        public string CompanyName { set; get; }
        public string ManagerName { get; set; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public string RC { set; get; }
        public string NIS { set; get; }
        public string ART { set; get; }
        public string RIB { set; get; }
        public string NIF { set; get; }
        public string BankAgency { set; get; }
        public SupplierState State { set; get; }
     
    }
}