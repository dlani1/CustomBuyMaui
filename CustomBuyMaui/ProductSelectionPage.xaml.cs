using System.Collections.ObjectModel; // Necesitas esto

namespace CustomBuyMaui
{
    public partial class ProductSelectionPage : ContentPage
    {
        // 1. Colección de datos
        public ObservableCollection<ProductItem> Products { get; set; }

        public ProductSelectionPage()
        {
            InitializeComponent();
            
            // 2. Inicializar la colección
            Products = new ObservableCollection<ProductItem>
            {
                new ProductItem { Title = "PC Gamer de Entrada", 
                                  ImageSource = "pc_gamer.png", 
                                  Description = "Equipo básico para juegos populares." },
                new ProductItem { Title = "Laptop Ultraligera", 
                                  ImageSource = "laptop_ultra.png", 
                                  Description = "Portátil ideal para trabajo y universidad." },
                new ProductItem { Title = "Monitor 4K Curvo", 
                                  ImageSource = "monitor_curvo.png", 
                                  Description = "Experiencia inmersiva para edición y diseño." }
            };

            // 3. Establecer el contexto de datos para XAML
            BindingContext = this; 
        }
    }
}