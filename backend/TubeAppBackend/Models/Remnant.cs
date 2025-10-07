namespace TubeAppBackend.Models
{
    public class Remnant
    {
        public string ID { get; set; } = string.Empty;
        public string IDStock { get; set; } = string.Empty;
        public double InStockT { get; set; }
        public double InStockM { get; set; }
        public double SoonArriveT { get; set; }
        public double SoonArriveM { get; set; }
        public double ReservedT { get; set; }
        public double ReservedM { get; set; }
        public bool UnderTheOrder { get; set; }
        public double AvgTubeLength { get; set; }
        public double AvgTubeWeight { get; set; }
    }
}
