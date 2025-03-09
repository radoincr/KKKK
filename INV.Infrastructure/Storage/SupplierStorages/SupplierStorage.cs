using INV.Domain.Entities.Suppliers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace INV.Infrastructure.Storage.SupplierStorages;

public class SupplierStorage : ISupplierStorage
{
    private const string insertSupplierCommand = @"
             INSERT INTO [dbo].[SUPPLIERS] (Id,CompanyName,ManagerName,Address,Phone,Email, RC, NIS, ART,NIF, RIB,  BankAgency)
             VALUES (@aId, @aCompanyName, @aManagerName, @aAddress, @aPhone, @aEmail, @aRC, @aNIS, @aART, @aNIF, @aRIB, @aBankAgency)";

    private const string selectAllSuppliersQuery = "SELECT * FROM [dbo].[SUPPLIERS]";
    private const string selectSuppliersByIdQuery = "SELECT * FROM [dbo].[SUPPLIERS] where Id=@aId";

    private const string updateSupplierCommand = @"
             UPDATE [dbo].[SUPPLIERS] 
             SET CompanyName = @aCompanyName,ManagerName = @aManagerName,Address = @aAddress, Phone = @aPhone,Email = @aEmail,
             RC = @aRC, NIS = @aNIS, ART = @aART,NIF = @aNIF,RIB = @aRIB,NIF= @aNIF,BankAgency = @aBankAgency WHERE Id = @aId";

    private const string selectSupplierCountByRcQuery = "select count(*) from [dbo].[SUPPLIERS]  WHERE RC = @aRC";
    private const string selectSupplierCountByNISQuery = "select count(*) from [dbo].[SUPPLIERS]  WHERE NIS = @aNIS";
    private const string selectSupplierCountByRIBQuery = "select count(*) from [dbo].[SUPPLIERS]  WHERE RIB = @aRIB";
    private readonly string _connectionString;

    public SupplierStorage(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("INV");
    }

    public async Task<int> InsertSupplier(Supplier supplier)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(insertSupplierCommand, sqlConnection);

        cmd.Parameters.AddWithValue("@aId", supplier.Id);
        cmd.Parameters.AddWithValue("@aCompanyName", supplier.CompanyName);
        cmd.Parameters.AddWithValue("@aManagerName", supplier.ManagerName);
        cmd.Parameters.AddWithValue("@aAddress", supplier.Address);
        cmd.Parameters.AddWithValue("@aPhone", supplier.Phone);
        cmd.Parameters.AddWithValue("@aEmail", supplier.Email);
        cmd.Parameters.AddWithValue("@aRC", supplier.RC);
        cmd.Parameters.AddWithValue("@aNIS", supplier.NIS);
        cmd.Parameters.AddWithValue("@aART", supplier.ART);
        cmd.Parameters.AddWithValue("@aNIF", supplier.NIF);
        cmd.Parameters.AddWithValue("@aRIB", supplier.RIB);
        cmd.Parameters.AddWithValue("@aBankAgency", supplier.BankAgency);
        await sqlConnection.OpenAsync();
        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<Supplier>> SelectAllSupplier()
    {
        var suppliers = new List<Supplier>();

        using var sqlConnection = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(selectAllSuppliersQuery, sqlConnection);

        await sqlConnection.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync()) suppliers.Add(getSupplierData(reader));

        return suppliers;
    }

    public async Task<Supplier?> SelectSupplierByID(Guid id)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(selectSuppliersByIdQuery, sqlConnection);
        cmd.Parameters.AddWithValue("@aId", id);

        await sqlConnection.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        return await reader.ReadAsync() ? getSupplierData(reader) : null;
    }

    public async Task<int> UpdateSupplier(Supplier supplier)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(updateSupplierCommand, sqlConnection);

        cmd.Parameters.AddWithValue("@aId", supplier.Id);
        cmd.Parameters.AddWithValue("@aCompanyName", supplier.CompanyName);
        cmd.Parameters.AddWithValue("@aManagerName", supplier.ManagerName);
        cmd.Parameters.AddWithValue("@aPhone", supplier.Phone);
        cmd.Parameters.AddWithValue("@aEmail", supplier.Email);
        cmd.Parameters.AddWithValue("@aAddress", supplier.Address);
        cmd.Parameters.AddWithValue("@aRC", supplier.RC);
        cmd.Parameters.AddWithValue("@aNIS", supplier.NIS);
        cmd.Parameters.AddWithValue("@aART", supplier.ART);
        cmd.Parameters.AddWithValue("@aNIF", supplier.NIF);
        cmd.Parameters.AddWithValue("@aRIB", supplier.RIB);
        cmd.Parameters.AddWithValue("@aBankAgency", supplier.BankAgency);

        await sqlConnection.OpenAsync();
        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<bool> SupplierExistsByRC(string rc)
    {
        await using var connection = new SqlConnection(_connectionString);

        var command = new SqlCommand(selectSupplierCountByRcQuery, connection);
        command.Parameters.AddWithValue("@aRC", rc);
        connection.Open();

        var count = (int)(await command.ExecuteScalarAsync() ?? 0);

        return count > 0;
    }

    public async Task<bool> SupplierExistsByNIS(string nis)
    {
        await using var connection = new SqlConnection(_connectionString);

        var command = new SqlCommand(selectSupplierCountByNISQuery, connection);
        command.Parameters.AddWithValue("@aNIS", nis);
        connection.Open();

        var count = (int)(await command.ExecuteScalarAsync() ?? 0);

        return count > 0;
    }

    public async Task<bool> SupplierExistsByRIB(string rib)
    {
        await using var connection = new SqlConnection(_connectionString);

        var command = new SqlCommand(selectSupplierCountByRIBQuery, connection);
        command.Parameters.AddWithValue("@aRIB", rib);
        connection.Open();

        var count = (int)(await command.ExecuteScalarAsync() ?? 0);

        return count > 0;
    }

    private static Supplier getSupplierData(SqlDataReader reader)
    {
        var r = new Supplier
        {
            Id = (Guid)reader["Id"],
            CompanyName = (string)reader["CompanyName"],
            ManagerName = (string)reader["ManagerName"],
            Address = (string)reader["Address"],
            Phone = (string)reader["Phone"],
            Email = (string)reader["Email"],
            RC = (string)reader["RC"],
            NIS = (string)reader["NIS"],
            ART = (string)reader["ART"],
            RIB = (string)reader["RIB"],
            NIF = (string)reader["NIF"],
            BankAgency = (string)reader["BankAgency"],
            State = (SupplierState)reader["Status"]
        };
        return r;
    }
}