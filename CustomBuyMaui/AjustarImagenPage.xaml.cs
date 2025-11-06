using Microsoft.Maui.Controls;
using System;

namespace CustomBuyMaui
{
    public partial class AjustarImagenPage : ContentPage
    {
        // Propiedades para almacenar los valores de ajuste
        private double _imageScale = 1.0;
        private double _imageRotation = 0;
        private double _imageX = 0;
        private double _imageY = 0;

        public AjustarImagenPage()
        {
            InitializeComponent();
            
            // Inicializar los controles con los valores por defecto
            ScaleSlider.Value = _imageScale;
            RotationSlider.Value = _imageRotation;
            HorizontalSlider.Value = _imageX;
            VerticalSlider.Value = _imageY;

            // Aplicar los ajustes iniciales a la imagen
            ApplyImageTransforms();
        }

        // Método para aplicar las transformaciones a la imagen
        private void ApplyImageTransforms()
        {
            AdjustableImage.Scale = _imageScale;
            AdjustableImage.Rotation = _imageRotation;
            AdjustableImage.TranslationX = _imageX;
            AdjustableImage.TranslationY = _imageY;
        }

        // Eventos de los Sliders
        private void OnScaleSliderValueChanged(object? sender, ValueChangedEventArgs e)
        {
            _imageScale = e.NewValue;
            ApplyImageTransforms();
        }

        private void OnRotationSliderValueChanged(object? sender, ValueChangedEventArgs e)
        {
            _imageRotation = e.NewValue;
            ApplyImageTransforms();
        }

        private void OnPositionSliderValueChanged(object? sender, ValueChangedEventArgs e)
        {
            // Ambos sliders (Horizontal y Vertical) usan este método.
            // Identificamos cuál slider lo llamó para actualizar la propiedad correcta.
            if (sender == HorizontalSlider)
            {
                _imageX = e.NewValue;
            }
            else if (sender == VerticalSlider)
            {
                _imageY = e.NewValue;
            }
            ApplyImageTransforms();
        }

        // Botón "Rotar 90°"
        private void OnRotate90Clicked(object? sender, EventArgs e)
        {
            _imageRotation = (_imageRotation + 90) % 360; // Gira 90 grados y se reinicia a 0 después de 360
            RotationSlider.Value = _imageRotation; // Actualiza el slider
            ApplyImageTransforms();
        }

        // Botón "Cancelar"
        private async void OnCancelClicked(object? sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Regresar a la página anterior
        }

        // Botón "Siguiente"
        private async void OnNextClicked(object? sender, EventArgs e)
        {
            await DisplayAlert("Ajustes Aplicados", 
                               $"Los ajustes se han guardado temporalmente. Continúa con el flujo de compra.", 
                               "OK");

            // Aquí deberías navegar a la siguiente etapa de tu flujo de compra
            // Por ahora, solo muestra el mensaje.
        }
    }
}