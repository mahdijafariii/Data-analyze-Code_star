using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.FileUploadedRepository;

public interface IFileUploadedRepository
{
    Task<IEnumerable<UploadData>> GetFileUploadedInDb(int page, int limit);
}