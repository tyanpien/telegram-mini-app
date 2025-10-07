using TubeAppBackend.Models;

namespace TubeAppBackend.Services.Interfaces
{
    public interface ITypeService
    {
        Task<List<ProductType>> GetAllTypesAsync();
        Task<ProductType?> GetTypeByIdAsync(string id);
        Task<List<ProductType>> GetTypesByParentAsync(string parentId);
    }
}
