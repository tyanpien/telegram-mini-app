namespace TubeAppBackend.Models
{
    public class Stock
    {
        public string IDStock { get; set; } = string.Empty;
        public string StockCity { get; set; } = string.Empty;
        public string StockName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;
        public string IDDivision { get; set; } = string.Empty;
        public bool CashPayment { get; set; }
        public bool CardPayment { get; set; }
        public string FIASId { get; set; } = string.Empty;
        public string OwnerInn { get; set; } = string.Empty;
        public string OwnerKpp { get; set; } = string.Empty;
        public string OwnerFullName { get; set; } = string.Empty;
        public string OwnerShortName { get; set; } = string.Empty;
        public string RailwayStation { get; set; } = string.Empty;
        public string ConsigneeCode { get; set; } = string.Empty;
    }
}
