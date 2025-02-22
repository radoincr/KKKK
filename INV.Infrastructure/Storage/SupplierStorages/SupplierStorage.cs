using System.Data;
using INV.Domain.Entities.SupplierEntity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace INV.Infrastructure.Storage.SupplierStorages
{
    public class SupplierStorage : ISupplierStorage
    {
        private readonly string _connectionString;

        public SupplierStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("INV");
        }

        private const string insertSupplierQuery = @"
            INSERT INTO Supplier (ID, RC, NIS, RIB, SupplierName, CompanyName, AccountName, Address, Phone, Email, ART, NIF, BankAgency,Status)
            VALUES (@ID, @RC, @NIS, @RIB, @SupplierName, @CompanyName, @AccountName, @Address, @Phone, @Email, @ART, @NIF, @BankAgency,@Status)";

        private const string SelectAllSuppliersQuery = "SELECT * FROM Supplier";
        private const string selectSuppliersByIDQuery = "SELECT * FROM Supplier where ID=@ID";

        private const string updateSupplierQuery = @"
               UPDATE Supplier 
SET [RC] = @RC, 
    [NIS] = @NIS, 
    [RIB] = @RIB, 
    [SupplierName] = @SupplierName, 
    [CompanyName] = @CompanyName, 
    [AccountName] = @AccountName, 
    [Address] = @Address, 
    [Phone] = @Phone, 
    [Email] = @Email, 
    [ART] = @ART, 
    [NIF] = @NIF, 
    [BankAgency] = @BankAgency
WHERE [ID] = @ID";
        private const string selectSupplierCountByIdQuery = "select count(*) from Supplier WHERE RC = @RC";


        private static Supplier getAllSupplier(SqlDataReader reader)
        {
            return new Supplier
            {
                ID = (Guid)reader["ID"],
                RC = reader["RC"].ToString(),
                NIS = (long)reader["NIS"],
                RIB = reader["RIB"].ToString(),
                SupplierName = reader["SupplierName"].ToString(),
                CompanyName = reader["CompanyName"].ToString(),
                AccountName = reader["AccountName"].ToString(),
                Address = reader["Address"].ToString(),
                Phone = reader["Phone"].ToString(),
                Email = reader["Email"].ToString(),
                ART = (long)reader["ART"],
                NIF = (long)reader["NIF"],
                BankAgency = reader["BankAgency"].ToString(),
                State = Enum.TryParse<SupplierState>(reader["Status"].ToString(), out var state)
                    ? state
                    : SupplierState.Deleted
            };
        }

        public async Task<int> InsertSupplier(Supplier supplier)
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(insertSupplierQuery, sqlConnection);

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
            cmd.Parameters.AddWithValue("@Status", 1);
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

        public async Task<Supplier?> SelectSupplierByID(Guid id)
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(selectSuppliersByIDQuery, sqlConnection);
            cmd.Parameters.AddWithValue("@ID", id);

            await sqlConnection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return getAllSupplier(reader);
            }

            return null;
        }

        public async Task<int> UpdateSupplier(Supplier supplier)
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(updateSupplierQuery, sqlConnection);

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
     
        public async Task<bool> SupplierExistsByRC(string rc)
        {
            await using var connection = new SqlConnection(_connectionString);

            SqlCommand command = new SqlCommand(selectSupplierCountByIdQuery, connection);
            command.Parameters.AddWithValue("@RC", rc);
            connection.Open();

            int count = (int)(await command.ExecuteScalarAsync() ?? 0);

            return count > 0;
        }
        
    }
}