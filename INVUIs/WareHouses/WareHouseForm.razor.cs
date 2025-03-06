using INV.App.WareHouses;
using INV.Domain.Entities.WareHouse;
using INV.Domain.Shared;
using INVUIs.WareHouses.Models;
using Microsoft.AspNetCore.Components;

namespace INVUIs.WareHouses;

public partial class WareHouseForm : ComponentBase
{
    [Inject] public IWareHouseService wareHouseService { set; get; }
    private WareHouseModel newWareHouse = new WareHouseModel();
    private bool visibility = false;
    private Result result;
    private string success = string.Empty;

    public async Task CreateWareHouse()
    {
        var wareHouse = new WareHouse()
        {
            Id = Guid.NewGuid(),
            Name = newWareHouse.WareHouseName
        };
        result = await wareHouseService.CreateWareHouse(wareHouse);
        if (result.IsSuccess)
        {
            success = "warehouse has been create ";
        }

        StateHasChanged();
        await Task.Delay(1500);
        newWareHouse = new WareHouseModel();
        result = null;
        HideModal();
        StateHasChanged();
    }

    public void ShowModal()
    {
        visibility = true;
        StateHasChanged();
    }

    public void HideModal()
    {
        visibility = false;
        StateHasChanged();
    }
}