namespace ProductManagement_Api.Models
{
    public class ItemModel
    {
        public string ItemCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double SellingPrice { get; set; }
        public double CostPrice { get; set; }
        public string PhotoBase64 { get; set; }
        public DateTime PunchDate { get; set; }
    }
}
