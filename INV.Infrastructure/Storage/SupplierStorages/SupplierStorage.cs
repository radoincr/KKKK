using System.Data;
using Entity.SupplierEntity;
using Interface.SupplierStorages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Storage.SupplierStorages
{
    public class SupplierStorage : ISupplierStorage
    {
        private readonly string _connectionString;

        public SupplierStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("INV");
        }

        private const string InsertSupplierQuery = @"
            INSERT INTO Supplier (ID, RC, NIS, RIB, SupplierName, CompanyName, AccountName, Address, Phone, Email, ART, NIF, BankAgency)
            VALUES (@ID, @RC, @NIS, @RIB, @SupplierName, @CompanyName, @AccountName, @Address, @Phone, @Email, @ART, @NIF, @BankAgency)";

        private const string SelectAllSuppliersQuery = "SELECT * FROM Supplier";

        private static Supplier getAllSupplier(SqlDataReader reader)
        {
            return new Supplier
            {
                ID = (Guid)reader["ID"],
                RC = reader["RC"].ToString(),
                NIS = (int)reader["NIS"],
                RIB = reader["RIB"].ToString(),
                SupplierName = reader["SupplierName"].ToString(),
                CompanyName = reader["CompanyName"].ToString(),
                AccountName = reader["AccountName"].ToString(),
                Address = reader["Address"].ToString(),
                Phone = reader["Phone"].ToString(),  
                Email = reader["Email"].ToString(),
                ART =(long)reader["ART"],
                NIF = (long)reader["NIF"],
                BankAgency = reader["BankAgency"].ToString()
            };
        }

        public async Task<int> InsertSupplier(Supplier supplier)
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(InsertSupplierQuery, sqlConnection);
            
            cmd.Parameters.AddWithValue("@ID", supplier.ID);
            cmd.Parameters.AddWithValue("@RC", supplier.RC);
            cmd.Parameters.AddWithValue("@NIS", supplier.NIS);
            cmd.Parameters.AddWithValue("@RIB", supplier.RIB);
            cmd.Parameters.AddWithValue("@SupplierName", supplier.SupplierName);
            cmd.Parameters.AddWithValue("@CompanyName", supplier.CompanyName);
            cmd.Parameters.AddWithValue("@AccountName", supplier.AccountName);
            cmd.Parameters.AddWithValue("@Address", supplier.Address);
            cmd.Parameters.AddWithValue("@Phone", supplier.Phone);
            cmd.Parameters.AddWithValue("@Email", supplier.Email);
            cmd.Parameters.AddWithValue("@ART", supplier.ART);
            cmd.Parameters.AddWithValue("@NIF", supplier.NIF);
            cmd.Parameters.AddWithValue("@BankAgency", supplier.BankAgency);

            await sqlConnection.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<Supplier>> SelectAllSupplier()
        {
            var suppliers = new List<Supplier>();

            using var sqlConnection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(SelectAllSuppliersQuery, sqlConnection);

            await sqlConnection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                suppliers.Add(getAllSupplier(reader));
            }

            return suppliers;
        }
    }
}
