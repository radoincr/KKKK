using INV.App.WareHouses;
using INV.Domain.Entities.WareHouse;
using INV.Domain.Shared;
using INVUIs.WareHouses.Models;
using Microsoft.AspNetCore.Components;

namespace INVUIs.WareHouses;

public partial class WareHouseForm : ComponentBase
{
    private WareHouseModel newWareHouse = new();
    private Result result;
    private string success = string.Empty;
    private bool visibility = false;
    [Inject] public IWareHouseService wareHouseService { set; get; }

    public async Task CreateWareHouse()
    {
        var wareHouse = new WareHouse
        {
            Id = Guid.NewGuid(),
            Name = newWareHouse.WareHouseName
        };
        result = await wareHouseService.CreateWareHouse(wareHouse);
        if (result.IsSuccess) success = "warehouse has been create ";

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