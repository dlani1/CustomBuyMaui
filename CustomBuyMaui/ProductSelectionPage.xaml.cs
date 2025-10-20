using Microsoft.Maui.Controls;
using CustomBuyMaui.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CustomBuyMaui
{
    public partial class ProductSelectionPage : ContentPage
    {
        public ObservableCollection<ProductItem> Products { get; set; } = new();
        private int _currentIndex = 0;

        public ICommand OnProductTapped { get; }

        public ProductSelectionPage()
        {
            InitializeComponent();

            // 🔹 Inicializar productos
            Products.Add(new ProductItem
            {
                Title = "PC Gamer de Entrada",
                Description = "Equipo básico para juegos populares y tareas diarias.",
                ImageSource = "imagen1.png",
                IsAvailable = true
            });
            Products.Add(new ProductItem
            {
                Title = "Laptop Ultraligera",
                Description = "Perfecta para trabajo, clases y movilidad.",
                ImageSource = "custom_buy_logo.png",
                IsAvailable = false
            });
            Products.Add(new ProductItem
            {
                Title = "Estación de Trabajo",
                Description = "Ideal para diseño, programación y edición.",
                ImageSource = "dotnet_bot.png",
                IsAvailable = false
            });

            // 🔹 Comando para clic en producto
            OnProductTapped = new Command<ProductItem>(async (item) => await HandleProductTapped(item));

            BindingContext = this;
        }

        private async Task HandleProductTapped(ProductItem item)
        {
            if (item == null)
                return;

            if (item.IsAvailable)
            {
                await Navigation.PushAsync(new ProductDetailPage(item));
            }
            else
            {
                await DisplayAlert("Producto no disponible", $"{item.Title} estará disponible próximamente.", "OK");
            }
        }

        private void OnNextProductClicked(object sender, EventArgs e)
        {
            if (Products.Count == 0) return;

            _currentIndex = (_currentIndex + 1) % Products.Count;
            MyCarousel.ScrollTo(_currentIndex, position: ScrollToPosition.Center, animate: true);
        }

        private void OnPreviousProductClicked(object sender, EventArgs e)
        {
            if (Products.Count == 0) return;

            _currentIndex = (_currentIndex - 1 + Products.Count) % Products.Count;
            MyCarousel.ScrollTo(_currentIndex, position: ScrollToPosition.Center, animate: true);
        }
    }
}
