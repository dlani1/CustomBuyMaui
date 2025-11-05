using Microsoft.Maui.Controls;
using Plugin.BLE.Abstractions.Contracts;
using System.Diagnostics;

namespace CustomBuyMaui
{
    public partial class SendImagePage : ContentPage
    {
        // Almacena la referencia al dispositivo Bluetooth conectado
        private readonly IDevice _connectedDevice;

        /// <summary>
        /// Constructor que requiere el dispositivo Bluetooth que fue conectado en la página anterior.
        /// </summary>
        /// <param name="connectedDevice">El dispositivo BLE al que estamos conectados.</param>
        public SendImagePage(IDevice connectedDevice)
        {
            InitializeComponent();
            _connectedDevice = connectedDevice;
            // Establecer el título de la página para saber a qué dispositivo se envía.
            Title = $"Enviar a: {_connectedDevice.Name}";
        }

        /// <summary>
        /// Se activa cuando el usuario presiona "Listo para Recibir".
        /// Inicia un proceso de simulación de espera por la transferencia de archivos.
        /// </summary>
        private async void OnReadyToReceiveClicked(object? sender, EventArgs e)
        {
            // 1. Deshabilitar el botón y cambiar la UI para indicar que está esperando
            ReceiveButton.IsEnabled = false;
            ReceiveButton.Text = "Esperando imagen...";
            ReceiveButton.BackgroundColor = Color.FromHex("#A5D6A7"); // Un verde más claro para indicar estado pasivo

            // NOTA IMPORTANTE:
            // La transferencia de archivos (como una foto) por Bluetooth (OBEX/FTP) es manejada por el 
            // sistema operativo del quiosco, no por Plugin.BLE. Aquí SIMULAMOS la espera 
            // de la transferencia del SO antes de mostrar el mensaje de éxito.

            Debug.WriteLine($"Esperando transferencia de imagen desde el móvil a: {_connectedDevice.Name}");
            
            // Simular un tiempo de espera de 10 segundos para que el usuario complete el envío
            await Task.Delay(10000); 

            // 2. Después del tiempo de espera, mostrar la interfaz de éxito
            SimulateImageReceived();
        }

        /// <summary>
        /// Muestra la interfaz de éxito, haciendo visible el Overlay.
        /// </summary>
        private void SimulateImageReceived()
        {
            // La manipulación de la UI siempre debe hacerse en el Hilo Principal.
            MainThread.BeginInvokeOnMainThread(() =>
            {
                SuccessOverlay.IsVisible = true;
                ReceiveButton.IsVisible = false; // Ocultar el botón principal
                Debug.WriteLine("Simulación de imagen recibida exitosamente.");
            });
        }

        /// <summary>
        /// Se activa al presionar "Continuar" en el mensaje de éxito.
        /// Desconecta el dispositivo y navega de regreso al inicio.
        /// </summary>
        private async void OnContinueClicked(object? sender, EventArgs e)
        {
            // 1. Intentar desconectar del dispositivo
            try
            {
                var adapter = Plugin.BLE.CrossBluetoothLE.Current.Adapter;
                if (_connectedDevice.State == Plugin.BLE.Abstractions.DeviceState.Connected)
                {
                    await adapter.DisconnectDeviceAsync(_connectedDevice);
                    Debug.WriteLine($"Dispositivo {_connectedDevice.Name} desconectado antes de continuar.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al intentar desconectar: {ex.Message}");
            }

            // 2. Regresar a la página principal de selección de dispositivo
            await Navigation.PopToRootAsync();
        }
    }
}