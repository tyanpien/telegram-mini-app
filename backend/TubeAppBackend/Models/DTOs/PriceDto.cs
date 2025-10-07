namespace TubeAppBackend.Models
{
    public class PriceDTO
    {
        public string NomenclatureId { get; set; } = string.Empty;
        public string StockId { get; set; } = string.Empty;
        public string StockName { get; set; } = string.Empty;
        public string StockCity { get; set; } = string.Empty;
        public decimal PriceValue { get; set; }
        public string Currency { get; set; } = "RUB";
        public string Unit { get; set; } = "ton";
        public decimal NDS { get; set; }
        public decimal PriceWithNDS { get; set; }
        public string PriceType { get; set; } = "base";
        public decimal VolumeLimit { get; set; }
    }
}
