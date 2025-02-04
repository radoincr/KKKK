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

        private const string insertSupplier =
            @"Insert into Supplier (ID,RC,NIS,RIB,SupplierName,CompanyName,AccountName,Address,Phone,Email,ART,NIF,BankAgency)
            VALUES (@ID,@RC,@NIS,@RIB,@SupplierName,@CompanyName,@AccountName,@address,@Phone,@Email,@ART,@NIF,@BankAgency)";

        private static Supplier getDataFromDataRow(DataRow row)
        {
            return new Supplier
            {
                ID = (Guid)row["ID"],
                RC = row["RC"].ToString(),
                NIS = (int)row["RC"],
                RIB = (int)row["RIB"],
                SupplierName=row["SupplierName"].ToString(),
                CompanyName = row["CompanyName"].ToString(),
                AccountName = row[""].ToString(),
                
            };
        }
       
        public async Task<int> InsertSupplier(Supplier supplier) {

            try
            {
                using var sqlconnection = new SqlConnection(_connectionString);
                var cmd = new SqlCommand(insertSupplier, sqlconnection);
                await sqlconnection.OpenAsync();
                cmd.Parameters.AddWithValue("@ID", supplier.ID);
                cmd.Parameters.AddWithValue("@RC", supplier.RC);
                cmd.Parameters.AddWithValue("@NIS", supplier.NIS);
                cmd.Parameters.AddWithValue("@RIB", supplier.RIB);
                cmd.Parameters.AddWithValue("@SupplierName", supplier.SupplierName);
                cmd.Parameters.AddWithValue("@CompanyName", supplier.CompanyName);
                cmd.Parameters.AddWithValue("@AccountName", supplier.AccountName);
                cmd.Parameters.AddWithValue("@address", supplier.Address);
                cmd.Parameters.AddWithValue("@Phone", supplier.Phone);
                cmd.Parameters.AddWithValue("@Email", supplier.Email);
                cmd.Parameters.AddWithValue("@ART", supplier.ART);
                cmd.Parameters.AddWithValue("@NIF", supplier.NIF);
                cmd.Parameters.AddWithValue("@BankAgency", supplier.BankAgency);
             
                return  cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw;
            }
            
        }

    }
}