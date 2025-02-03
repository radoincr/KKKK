namespace Entity.SupplierEntity

{
    public class Supplier
    {
        public Guid ID { set; get; }
        public string RC { set; get; }

        public int NIS { set; get; }

        public int RIB { set; get; }

        public string SupplierName { set; get; }
        public string CompanyName { set; get; }

        public string AccountName { set; get; }
        public string Address { set; get; }
        public int Phone { set; get; }

        public string Email { set; get; }
        public int ART { set; get; }
        public int NIF { set; get; }
        
        public string BankAgency { set; get; }
    }
}