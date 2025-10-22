using Microsoft.Maui.Controls;
using CustomBuyMaui.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CustomBuyMaui
{
    public partial class ProductSelectionPage : ContentPage
    {
        public ObservableCollection<ProductItem> Products { get; set; } = new();

        // No es necesario que este sea un campo privado, puede ser una propiedad.
        public ICommand OnProductTapped { get; } 

        public ProductSelectionPage()
        {
            InitializeComponent();

            // 🔹 Inicializar productos
            Products.Add(new ProductItem
            {
                Title = "PC Gamer de Entrada",
                Description = "Equipo básico para juegos populares y tareas diarias.",
                ImageSource = "imagen1.jpeg",
                IsAvailable = true
            });
            Products.Add(new ProductItem
            {
                Title = "Laptop Ultraligera",
                Description = "Perfecta para trabajo, clases y movilidad.",
                ImageSource = "custom_buy_logo.png",
                IsAvailable = true // Lo cambio a true para que se pueda navegar
            });
            Products.Add(new ProductItem
            {
                Title = "Estación de Trabajo",
                Description = "Ideal para diseño, programación y edición.",
                ImageSource = "dotnet_bot.png",
                IsAvailable = false
            });

            // 🔹 Comando para clic en producto
            // Aquí se maneja la lógica de la navegación
            OnProductTapped = new Command<ProductItem>(async (item) => await HandleProductTapped(item));

            BindingContext = this;
        }

        private async Task HandleProductTapped(ProductItem item)
        {
            if (item == null)
                return;

            if (item.IsAvailable)
            {
                // 👈 NAVEGACIÓN A LA PÁGINA DE DETALLES
                await Navigation.PushAsync(new ProductDetailPage(item));
            }
            else
            {
                await DisplayAlert("Producto no disponible", $"{item.Title} estará disponible próximamente.", "OK");
            }
        }

        // 🛑 MÉTODOS DE BOTÓN ELIMINADOS
        // OnNextProductClicked y OnPreviousProductClicked ya no son necesarios
        // porque el CarouselView ahora maneja el deslizamiento automáticamente.
    }
}