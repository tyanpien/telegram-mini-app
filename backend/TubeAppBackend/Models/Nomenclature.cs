namespace TubeAppBackend.Models
{
    public class Nomenclature
    {
        public string ID { get; set; } = string.Empty;
        public string IDCat { get; set; } = string.Empty;
        public string IDType { get; set; } = string.Empty;
        public string IDTypeNew { get; set; } = string.Empty;
        public string ProductionType { get; set; } = string.Empty;
        public string IDFunctionType { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Gost { get; set; } = string.Empty;
        public string FormOfLength { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string SteelGrade { get; set; } = string.Empty;
        public double Diameter { get; set; }
        public double ProfileSize2 { get; set; }
        public double PipeWallThickness { get; set; }
        public int Status { get; set; }
        public double Koef { get; set; }
    }
}
