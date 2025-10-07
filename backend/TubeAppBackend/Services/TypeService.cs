using System.Text.Json;
using TubeAppBackend.Models;
using TubeAppBackend.Services.Interfaces;

namespace TubeAppBackend.Services
{
    public class TypeService : ITypeService
    {
        private readonly List<ProductType> _typeData;

        public TypeService(IConfiguration configuration)
        {
            var jsonPath = configuration["DataPaths:Types"] ?? "Data/types.json";

            if (!File.Exists(jsonPath))
            {
                _typeData = new List<ProductType>();
                return;
            }

            var jsonData = File.ReadAllText(jsonPath);

            try
            {
                var data = JsonSerializer.Deserialize<JsonElement>(jsonData);

                if (data.ValueKind == JsonValueKind.Array)
                {
                    _typeData = JsonSerializer.Deserialize<List<ProductType>>(data.GetRawText()) ?? new List<ProductType>();
                }
                else if (data.TryGetProperty("ArrayOfTypeEl", out var arrayElement))
                {
                    _typeData = arrayElement.EnumerateArray()
                        .Select(item => JsonSerializer.Deserialize<ProductType>(item.GetRawText()) ?? new ProductType())
                        .ToList();
                }
                else
                {
                    _typeData = new List<ProductType>();
                }
            }
            catch (Exception)
            {
                _typeData = new List<ProductType>();
            }
        }

        public Task<List<ProductType>> GetAllTypesAsync()
        {
            return Task.FromResult(_typeData);
        }

        public Task<ProductType?> GetTypeByIdAsync(string id)
        {
            var type = _typeData.FirstOrDefault(t => t.IDType == id);
            return Task.FromResult(type);
        }

        public Task<List<ProductType>> GetTypesByParentAsync(string parentId)
        {
            var result = _typeData
                .Where(t => t.IDParentType == parentId)
                .ToList();
            return Task.FromResult(result);
        }
    }
}
