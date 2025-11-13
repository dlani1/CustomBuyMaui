namespace CustomBuyMaui
{
    // 1. Necesitas esta propiedad para recibir la ruta de la imagen desde la página anterior
    [QueryProperty(nameof(ImagePath), "ImagePath")]
    public partial class AjustarImagenPage : ContentPage
    {
        private string _imagePath = string.Empty;

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                // Carga la imagen al control una vez que se recibe la ruta
                receivedImage.Source = ImageSource.FromFile(_imagePath);
                OnPropertyChanged();
            }
        }

        public AjustarImagenPage()
        {
            InitializeComponent();
        }

        private void OnConfirmarAjusteClicked(object sender, EventArgs e)
        {
            // Aquí iría la lógica para recortar la imagen a la medida del marco
            // Esto es complejo y requiere un paquete de dibujo (como SkiaSharp) o el CommunityToolkit.
            // Por ahora, simplemente muestra que la acción se ejecutó.
            
            DisplayAlert("Ajuste Confirmado", $"La imagen en {_imagePath} ha sido marcada para ajuste. Implementación de recorte pendiente.", "OK");
        }
    }
}