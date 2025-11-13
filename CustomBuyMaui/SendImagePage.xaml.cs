using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage; // <--- CORRECCIÓN CLAVE PARA FileSystem

namespace CustomBuyMaui
{
    public partial class SendImagePage : ContentPage
    {
        public SendImagePage()
        {
            InitializeComponent();
            
            // Iniciamos un proceso simulado de BLE
            SimulateBluetoothTransfer();
        }

        private async void SimulateBluetoothTransfer()
        {
            statusLabel.Text = "Esperando conexión móvil (BLE)...";
            activityIndicator.IsRunning = true;

            // --- SIMULACIÓN DE TRANSFERENCIA EXITOSA (Reemplazar con lógica real de Plugin.BLE) ---
            
            // Simular que el móvil se conecta y envía una imagen
            await Task.Delay(5000); 

            // Crear una ruta de imagen temporal simulada (Esto es solo para probar la navegación)
            string simulatedImagePath = Path.Combine(FileSystem.CacheDirectory, "temp_image_ble.jpg");

            // Si necesitas que la app de escritorio tenga una imagen para la prueba,
            // debes crear una imagen en esa ruta simulada.
            // Para el propósito de la compilación, solo necesitamos la ruta.
            
            // --- FIN DE SIMULACIÓN ---
            
            statusLabel.Text = "¡Imagen recibida por BLE! Procesando...";
            activityIndicator.IsRunning = false;

            // Navegar a la página de ajuste, tal como lo querías
            await Shell.Current.GoToAsync($"///AjustarImagenPage?ImagePath={Uri.EscapeDataString(simulatedImagePath)}");
        }
        
        protected override void OnDisappearing()
        {
             // Si implementas BLE real, aquí detendrías la publicidad y desconectarías.
            base.OnDisappearing();
        }
    }
}