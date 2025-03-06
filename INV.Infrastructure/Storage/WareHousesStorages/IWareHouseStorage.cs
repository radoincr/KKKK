using INV.Domain.Entities.WareHouse;

namespace INV.Infrastructure.Storage.WareHouseStorages;

public interface IWareHouseStorage
{
    ValueTask<List<WareHouse>> SelectAllReceipts();

    ValueTask<int> InsertWareHouse(WareHouse wareHouse);
}