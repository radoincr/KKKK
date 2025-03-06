using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INV.Domain.Entities.WareHouse;
using INV.Domain.Shared;

namespace INV.App.WareHouses
{
    public interface IWareHouseService
    {
        ValueTask<List<WareHouse>> GetAllReceipts();

        ValueTask<Result> CreateWareHouse(WareHouse wareHouse);
    }
}