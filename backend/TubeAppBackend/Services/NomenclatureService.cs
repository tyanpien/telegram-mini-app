using System.Text.Json;
using TubeAppBackend.Models;
using TubeAppBackend.Services.Interfaces;

namespace TubeAppBackend.Services
{
    public class NomenclatureService : INomenclatureService
    {
        private readonly List<Nomenclature> _nomenclatureData;

        public NomenclatureService(IConfiguration configuration)
        {
            var jsonPath = configuration["DataPaths:Nomenclature"] ?? "Data/nomenclature.json";

            if (!File.Exists(jsonPath))
            {
                _nomenclatureData = new List<Nomenclature>();
                return;
            }

            var jsonData = File.ReadAllText(jsonPath);

            try
            {
                var data = JsonSerializer.Deserialize<JsonElement>(jsonData);

                if (data.ValueKind == JsonValueKind.Array)
                {
                    _nomenclatureData = JsonSerializer.Deserialize<List<Nomenclature>>(data.GetRawText()) ?? new List<Nomenclature>();
                }
                else if (data.TryGetProperty("ArrayOfNomenclatureEl", out var arrayElement))
                {
                    _nomenclatureData = arrayElement.EnumerateArray()
                        .Select(item => JsonSerializer.Deserialize<Nomenclature>(item.GetRawText()) ?? new Nomenclature())
                        .ToList();
                }
                else
                {
                    _nomenclatureData = new List<Nomenclature>();
                }
            }
            catch (Exception)
            {
                _nomenclatureData = new List<Nomenclature>();
            }
        }

        public Task<List<Nomenclature>> GetAllNomenclatureAsync()
        {
            return Task.FromResult(_nomenclatureData);
        }

        public Task<Nomenclature?> GetNomenclatureByIdAsync(string id)
        {
            var nomenclature = _nomenclatureData.FirstOrDefault(n => n.ID == id);
            return Task.FromResult(nomenclature);
        }

        public Task<List<Nomenclature>> GetNomenclatureByTypeAsync(string typeId)
        {
            var result = _nomenclatureData
                .Where(n => n.IDType == typeId)
                .ToList();
            return Task.FromResult(result);
        }

        public Task<List<Nomenclature>> SearchNomenclatureAsync(string searchTerm)
        {
            var result = _nomenclatureData
                .Where(n => n.Name?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true ||
                           n.SteelGrade?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true ||
                           n.Manufacturer?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true)
                .ToList();
            return Task.FromResult(result);
        }

        public Task<List<Nomenclature>> GetNomenclatureByDiameterRangeAsync(double minDiameter, double maxDiameter)
        {
            var result = _nomenclatureData
                .Where(n => n.Diameter >= minDiameter && n.Diameter <= maxDiameter)
                .ToList();
            return Task.FromResult(result);
        }
    }
}
