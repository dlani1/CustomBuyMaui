namespace CustomBuyMaui.Models
{
    public class ProductItem
    {
        public string Title { get; set; } = string.Empty; // 👈 SOLUCIONADO
        public string Description { get; set; } = string.Empty; // 👈 SOLUCIONADO
        public string ImageSource { get; set; } = string.Empty; // 👈 SOLUCIONADO
        public bool IsAvailable { get; set; }
    }
}