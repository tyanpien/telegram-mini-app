namespace TubeAppBackend.Models.DTOs
{
    public class CombinedDataDto
    {
        // Nomenclature data
        public string ID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ProductionType { get; set; } = string.Empty;
        public string SteelGrade { get; set; } = string.Empty;
        public double Diameter { get; set; }
        public double PipeWallThickness { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string Gost { get; set; } = string.Empty;
        public string FormOfLength { get; set; } = string.Empty;
        public double Koef { get; set; }

        // Type data
        public string TypeName { get; set; } = string.Empty;

        // Stock and remnant data
        public double TotalStockT { get; set; }
        public double TotalStockM { get; set; }
        public List<StockRemnantDto> StockRemnants { get; set; } = new List<StockRemnantDto>();

        // Price data
        public List<PriceDto> Prices { get; set; } = new List<PriceDto>();
    }

    public class StockRemnantDto
    {
        public string StockId { get; set; } = string.Empty;
        public string StockCity { get; set; } = string.Empty;
        public string StockName { get; set; } = string.Empty;
        public double InStockT { get; set; }
        public double InStockM { get; set; }
        public double AvgTubeLength { get; set; }
        public double AvgTubeWeight { get; set; }
    }

    public class PriceDto
    {
        public string StockId { get; set; } = string.Empty;
        public string StockCity { get; set; } = string.Empty;
        public string StockName { get; set; } = string.Empty;
        public decimal PriceT { get; set; }
        public decimal PriceLimitT1 { get; set; }
        public decimal PriceT1 { get; set; }
        public decimal PriceLimitT2 { get; set; }
        public decimal PriceT2 { get; set; }
        public decimal PriceM { get; set; }
        public decimal PriceLimitM1 { get; set; }
        public decimal PriceM1 { get; set; }
        public decimal PriceLimitM2 { get; set; }
        public decimal PriceM2 { get; set; }
        public decimal NDS { get; set; }
    }
}
