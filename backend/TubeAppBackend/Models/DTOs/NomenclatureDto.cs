namespace TubeAppBackend.Models.DTOs
{
    public class NomenclatureDto
    {
        public string ID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ProductionType { get; set; } = string.Empty;
        public string SteelGrade { get; set; } = string.Empty;
        public double Diameter { get; set; }
        public double PipeWallThickness { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string Gost { get; set; } = string.Empty;
        public double TotalStockT { get; set; }
        public double TotalStockM { get; set; }
    }
}
