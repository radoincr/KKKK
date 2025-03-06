using INV.Domain.Entities.Receipts;
using INV.Domain.Entities.WareHouse;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace INV.Infrastructure.Storage.WareHouseStorages;

public class WareHouseStrorage : IWareHouseStorage
{
    private readonly string _connectionString;

    public WareHouseStrorage(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("INV");
    }

    private const string selectAllQuery = "SELECT * FROM WareHouse";
    private const string insertQuery = "INSERT INTO WareHouse (Id, Name) VALUES (@Id, @Name)";

    private static WareHouse getWareHouseData(SqlDataReader reader)
    {
        return new WareHouse
        {
            Id = reader.GetGuid(reader.GetOrdinal("Id")),
            Name = reader.GetString(reader.GetOrdinal("Name")),
        };
    }

    public async ValueTask<List<WareHouse>> SelectAllReceipts()
    {
        await using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(selectAllQuery, sqlConnection);
        await sqlConnection.OpenAsync();

        var reader = await cmd.ExecuteReaderAsync();
        var wareHouses = new List<WareHouse>();
        while (await reader.ReadAsync())
        {
            wareHouses.Add(getWareHouseData(reader));
        }
        return wareHouses;
    }

    public async ValueTask<int> InsertWareHouse(WareHouse wareHouse)
    {
        await using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(insertQuery, sqlConnection);
        cmd.Parameters.AddWithValue("@Id", wareHouse.Id);
        cmd.Parameters.AddWithValue("@Name", wareHouse.Name);
        await sqlConnection.OpenAsync();
        return await cmd.ExecuteNonQueryAsync();
    }
}