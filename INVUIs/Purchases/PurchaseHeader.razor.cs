using System.ComponentModel.DataAnnotations;
using INV.App.Budgets;
using INV.Domain.Entities.Budget;
using INVUIs.Purchases.PurchaseModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace INVUIs.Purchases;

public partial class PurchaseHeader : ComponentBase
{
    [CascadingParameter] public PurchaseModel purchaseModel { get; set; } = new();
    [Parameter] public EventCallback<PurchaseModel> OnPurchaseOrder { get; set; }
    [Inject] public IBudgetService budgetService { get; set; }

    private int _selectedChapterCode;
    private int _selelctedArticleCode;
    public List<Article> articles = new();

    public List<Chapter> chapters = new();
    private int selectedChapterCode;
    private EditForm form;


    public int SelectedArticleCode
    {
        get => _selelctedArticleCode;
        set
        {
            if (_selelctedArticleCode != value)
            {
                _selelctedArticleCode = value;
                purchaseModel.selectedArticle= value.ToString(); 
                LoadArticleTitle();
            }
        }
    }

   

    public int SelectedChapterCode
    {
        get => _selectedChapterCode;
        set
        {
            if (_selectedChapterCode != value)
            {
                _selectedChapterCode = value;
                purchaseModel.selectedChapter = value.ToString(); 
                LoadChapterTitle();
                LoadArticlesBycodeChapter();
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {
        chapters = await budgetService.GetAllChapitres();
    }

    private async void LoadChapterTitle()
    {
        var chapter = await budgetService.GetChapterByCode(SelectedChapterCode);
        purchaseModel.title_chapter = chapter.Name;
        StateHasChanged();
    }

    private async void LoadArticlesBycodeChapter()
    {
        articles = await budgetService.GetArticlesByCodeChapter(SelectedChapterCode);

        StateHasChanged();
    }

    private async void LoadArticleTitle()
    {
        var article = await budgetService.GetArticlesByCodeArticle(SelectedArticleCode);
        purchaseModel.description_article = article.Name;
        StateHasChanged();
    }

    public async Task Save()
    {
        await OnPurchaseOrder.InvokeAsync(purchaseModel);
    }

    public async Task SubmitForm()
    {
        if (form is not null)
        {
            form.EditContext?.Validate();
            if (form.EditContext?.Validate() == true)
            {
                await Save();
            }
        }
    }
}