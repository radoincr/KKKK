using INV.App.WareHouses;
using INV.Domain.Entities.WareHouse;
using INV.Domain.Shared;
using INV.Infrastructure.Storage.WareHouseStorages;

namespace INV.Implementation.Service.WareHouses;

public class WareHouseService : IWareHouseService
{
    public readonly IWareHouseStorage wareHouseStorage;

    public WareHouseService(IWareHouseStorage _wareHouseStorage)
    {
        wareHouseStorage = _wareHouseStorage;
    }

    public async ValueTask<List<WareHouse>> GetAllReceipts()
    {
        try
        {
            return await wareHouseStorage.SelectAllReceipts();
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async ValueTask<Result> CreateWareHouse(WareHouse wareHouse)
    {
        try
        {
            await wareHouseStorage.InsertWareHouse(wareHouse);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Error.Exception(e);
        }
    }
}