using System.Text.Json;
using TubeAppBackend.Models;
using TubeAppBackend.Services.Interfaces;

namespace TubeAppBackend.Services
{
    public class RemnantService : IRemnantService
    {
        private readonly List<Remnant> _remnantData;

        public RemnantService(IConfiguration configuration)
        {
            var jsonPath = configuration["DataPaths:Remnants"] ?? "Data/remnants.json";

            if (!File.Exists(jsonPath))
            {
                _remnantData = new List<Remnant>();
                return;
            }

            var jsonData = File.ReadAllText(jsonPath);

            try
            {
                var data = JsonSerializer.Deserialize<JsonElement>(jsonData);

                if (data.ValueKind == JsonValueKind.Array)
                {
                    _remnantData = JsonSerializer.Deserialize<List<Remnant>>(data.GetRawText()) ?? new List<Remnant>();
                }
                else if (data.TryGetProperty("ArrayOfRemnantEl", out var arrayElement))
                {
                    _remnantData = arrayElement.EnumerateArray()
                        .Select(item => JsonSerializer.Deserialize<Remnant>(item.GetRawText()) ?? new Remnant())
                        .ToList();
                }
                else
                {
                    _remnantData = new List<Remnant>();
                }
            }
            catch (Exception)
            {
                _remnantData = new List<Remnant>();
            }
        }

        public Task<List<Remnant>> GetAllRemnantsAsync()
        {
            return Task.FromResult(_remnantData);
        }

        public Task<List<Remnant>> GetRemnantsByNomenclatureIdAsync(string nomenclatureId)
        {
            var result = _remnantData
                .Where(r => r.ID == nomenclatureId)
                .ToList();
            return Task.FromResult(result);
        }

        public Task<List<Remnant>> GetRemnantsByStockIdAsync(string stockId)
        {
            var result = _remnantData
                .Where(r => r.IDStock == stockId)
                .ToList();
            return Task.FromResult(result);
        }

        public Task<List<Remnant>> GetAvailableRemnantsAsync(string nomenclatureId)
        {
            var result = _remnantData
                .Where(r => r.ID == nomenclatureId && r.InStockT > 0)
                .ToList();
            return Task.FromResult(result);
        }

        public Task<(double totalT, double totalM)> GetTotalStockByNomenclatureAsync(string nomenclatureId)
        {
            var remnants = _remnantData.Where(r => r.ID == nomenclatureId).ToList();
            var totalT = remnants.Sum(r => r.InStockT);
            var totalM = remnants.Sum(r => r.InStockM);
            return Task.FromResult((totalT, totalM));
        }
    }
}
