using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INV.Domain.Entities.Budget;

namespace INV.App.IChapterServices
{
    public interface IChapterService
    {
        Task<List<Chapter>> GetAllChapters();
        Task<int> AddChapter(Chapter chapter);
        Task<Chapter> GetChaptersByCodeChapter(int codeChapter);
    }
}