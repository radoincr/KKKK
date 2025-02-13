using INV.Domain.Entity.ChapterEntity;
using INV.Domain.Entity.ProductEntity;
using INV.Infrastructure.Storage.ChapterStorages;
using INV.Infrastructure.Storage.ProductsStorages;
using INV.Infrastructure.Storage.SupplierStorages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INV.App.IChapterServices;

namespace INV.Implementation.Service.ChapterServices
{
    public class ChapterService : IChapterService
    {
        public readonly IChapterStorage ChapterStorage;
        public ChapterService(IChapterStorage _ChapterStorage)
        {
            ChapterStorage = _ChapterStorage;
        }
        public async Task<int> AddChapter(Chapter Chapter)
        {
            return await ChapterStorage.InsertChapter(Chapter);
        }
        public async Task<List<Chapter>> GetAllChapters()
        {
            return await ChapterStorage.SelectAll();
        }
        public async Task<Chapter> GetChaptersByCodeChapter(int CodeChapter)
        {
            return await ChapterStorage.SelectChapterByCode(CodeChapter);
        }
      
    }
}