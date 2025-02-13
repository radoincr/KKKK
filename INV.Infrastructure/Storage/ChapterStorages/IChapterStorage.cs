using INV.Domain.Entity.ChapterEntity;
namespace INV.Infrastructure.Storage.ChapterStorages
{
    public interface IChapterStorage
    {
        Task<int> InsertChapter(Chapter Chapter);
        Task<List<Chapter>> SelectAll();
        Task<Chapter> SelectChapterByCode(int CodeChapter);
    }
}