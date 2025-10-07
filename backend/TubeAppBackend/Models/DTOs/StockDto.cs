namespace TubeAppBackend.Models
{
    public class StockDTO
    {
        public string StockId { get; set; } = string.Empty;
        public string StockCity { get; set; } = string.Empty;
        public string StockName { get; set; } = string.Empty;
        public double InStockT { get; set; }
        public double InStockM { get; set; }
        public double AvgTubeLength { get; set; }
        public double AvgTubeWeight { get; set; }
    }
}
