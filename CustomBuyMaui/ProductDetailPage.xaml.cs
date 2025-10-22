using CustomBuyMaui.Models;

namespace CustomBuyMaui
{
    public partial class ProductDetailPage : ContentPage
    {
        public ProductDetailPage(ProductItem product)
        {
            InitializeComponent();
            BindingContext = product;
        }
    }
}
