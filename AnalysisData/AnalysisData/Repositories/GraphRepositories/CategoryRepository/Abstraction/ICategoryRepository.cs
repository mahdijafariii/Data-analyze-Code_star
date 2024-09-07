using AnalysisData.Models.GraphModel.Category;

namespace AnalysisData.Repositories.GraphRepositories.CategoryRepository.Abstraction;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category> GetByNameAsync(string name);
    Task<Category> GetByIdAsync(int id);
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
}