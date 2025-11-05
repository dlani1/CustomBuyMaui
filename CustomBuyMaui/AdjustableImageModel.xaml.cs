using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace CustomBuyMaui
{
    // 1. Clase para manejar los parámetros de transformación de la imagen
    public class ImageAdjustment
    {
        public double TranslationX { get; set; } = 0;
        public double TranslationY { get; set; } = 0;
        public double Scale { get; set; } = 1.0;
        public double Rotation { get; set; } = 0; // Rotación en grados
        public string ImageSource { get; set; } = "placeholder_image.png"; // Usar valor predeterminado
    }

    public partial class ImageAdjustmentPage : ContentPage
    {
        // El modelo que contiene la posición, escala y rotación actual de la imagen.
        private readonly ImageAdjustment _currentImage;

        // Variables privadas para rastrear el estado del gesto
        private double _currentScale = 1;
        private double _startScale;
        private double _offsetX;
        private double _offsetY;
        
        public ImageAdjustmentPage()
        {
            InitializeComponent();
            
            // Inicializar el modelo con valores predeterminados
            _currentImage = new ImageAdjustment();
            
            // Establecer el contexto de enlace (BindingContext) para la UI
            BindingContext = _currentImage;

            // Inicializar las variables de seguimiento de gestos
            _currentScale = _currentImage.Scale;
        }

        // --- MANEJO DE GESTOS ---

        // 1. Movimiento (PanGestureRecognizer)
        private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    // Guardar la posición inicial al inicio del arrastre
                    _offsetX = _currentImage.TranslationX;
                    _offsetY = _currentImage.TranslationY;
                    break;

                case GestureStatus.Running:
                    // Calcular nueva posición sumando el desplazamiento total
                    _currentImage.TranslationX = _offsetX + e.TotalX;
                    _currentImage.TranslationY = _offsetY + e.TotalY;
                    
                    // Aplicar las transformaciones en la UI
                    UpdateImageTransform();
                    break;
                    
                case GestureStatus.Completed:
                    // La nueva posición se mantiene para el próximo arrastre
                    break;
            }
        }

        // 2. Zoom (PinchGestureRecognizer)
        private void OnPinchUpdated(object? sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                _startScale = _currentScale;
            }
            if (e.Status == GestureStatus.Running)
            {
                // Calcular la nueva escala
                _currentScale = _startScale * e.Scale;
                // Asegurar que la escala no sea menor a 0.5 (zoom mínimo)
                _currentImage.Scale = Math.Max(0.5, _currentScale); 
                
                // Aplicar las transformaciones en la UI
                UpdateImageTransform();
            }
            if (e.Status == GestureStatus.Completed)
            {
                // La nueva escala se guarda para el próximo gesto de zoom
                _currentScale = _currentImage.Scale;
            }
        }
        
        // --- MANEJO DE CONTROLES MANUALES (SLIDERS) ---

        // Zoom manual (Slider)
        private void OnZoomSliderValueChanged(object? sender, ValueChangedEventArgs e)
        {
            // Actualizar la variable de seguimiento y el modelo
            _currentScale = e.NewValue;
            _currentImage.Scale = _currentScale;
            UpdateImageTransform();
        }

        // Rotación manual (Slider)
        private void OnRotationSliderValueChanged(object? sender, ValueChangedEventArgs e)
        {
            // Actualizar el modelo de rotación
            _currentImage.Rotation = e.NewValue;
            UpdateImageTransform();
        }

        /// <summary>
        /// Aplica las transformaciones del modelo a la vista de la imagen (ImageDisplay).
        /// Se asegura de que se ejecute en el Hilo Principal para actualizar la UI.
        /// </summary>
        private void UpdateImageTransform()
        {
            // Usar MainThread.BeginInvokeOnMainThread por seguridad, aunque a menudo no es estrictamente
            // necesario para eventos iniciados por la UI, es una buena práctica en MAUI.
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Aplicar las transformaciones al control ImageDisplay
                ImageDisplay.TranslationX = _currentImage.TranslationX;
                ImageDisplay.TranslationY = _currentImage.TranslationY;
                ImageDisplay.Scale = _currentImage.Scale;
                ImageDisplay.Rotation = _currentImage.Rotation;

                // Actualizar las etiquetas de información
                ScaleLabel.Text = $"Zoom: {ImageDisplay.Scale:F2}";
                RotationLabel.Text = $"Rotación: {ImageDisplay.Rotation:F0}°";
            });
        }
        
        // --- BOTONES ---

        private async void OnCropClicked(object? sender, EventArgs e)
        {
            // Simulación del recorte. Muestra los parámetros de transformación finales.
            await DisplayAlert("Recortar (Simulación)", 
                               $"La imagen ha sido ajustada y está lista para el recorte final en las coordenadas:\n" +
                               $"Posición: ({_currentImage.TranslationX:F0}, {_currentImage.TranslationY:F0})\n" +
                               $"Escala: {_currentImage.Scale:F2}\n" +
                               $"Rotación: {_currentImage.Rotation:F0}°", "Entendido");
        }

        private async void OnNextClicked(object? sender, EventArgs e)
        {
            // Lógica para avanzar a la pantalla de confirmación/pago
            await DisplayAlert("Continuar", "Imagen guardada. Avanzando a la confirmación de pedido.", "OK");
            // Ejemplo de navegación real: await Navigation.PushAsync(new ConfirmationPage(_currentImage));
        }

        private async void OnBackClicked(object? sender, EventArgs e)
        {
            // Regresar a la pantalla anterior
            await Navigation.PopAsync();
        }
    }
}