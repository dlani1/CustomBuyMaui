using Microsoft.Maui.Controls;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Plugin.BLE.Abstractions;
using QRCoder;
// USING AÑADIDOS PARA IP
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace CustomBuyMaui
{
    public partial class MugCustomizationPage : ContentPage
    {
        // --- PROPIEDADES BLUETOOTH ---
        public ObservableCollection<IDevice> DispositivosEncontrados { get; } = new ObservableCollection<IDevice>();
        private readonly IBluetoothLE _bluetoothLE;
        private readonly IAdapter _adapter;

        // --- PROPIEDAD DEL SERVIDOR (PUENTE) ---
        private readonly IImageUploadService _uploadService;

        // --- CONSTRUCTOR MODIFICADO ---
        public MugCustomizationPage(IImageUploadService uploadService) // REQUIERE INYECCIÓN
        {
            InitializeComponent();

            // Asignar el servicio (puente)
            _uploadService = uploadService;

            // Lógica de Bluetooth existente
            _bluetoothLE = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;

            // Suscripción a eventos de Bluetooth
            _adapter.DeviceDiscovered += OnDeviceDiscovered;
            _adapter.DeviceConnectionLost += OnDeviceConnectionLost;
            _adapter.DeviceConnected += OnDeviceConnected;

            // Establecer el BindingContext
            BindingContext = this;
            
            // Iniciar en modo Bluetooth por defecto
            OnBluetoothModeClicked(this, EventArgs.Empty);
        }

        // --- CICLO DE VIDA (FUSIONADO) ---

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Suscribirse al evento del servidor
            _uploadService.OnImageUploaded += HandleImageUploaded;
        }

        protected override void OnDisappearing()
        {
            // Limpieza de Bluetooth
            _adapter.StopScanningForDevicesAsync();
            _adapter.DeviceDiscovered -= OnDeviceDiscovered;
            _adapter.DeviceConnectionLost -= OnDeviceConnectionLost;
            _adapter.DeviceConnected -= OnDeviceConnected;

            // Limpieza del servidor
            _uploadService.OnImageUploaded -= HandleImageUploaded;

            base.OnDisappearing();
        }

        // --- LÓGICA NUEVA: MANEJO DEL SERVIDOR/QR ---

        private void HandleImageUploaded(string imagePath)
        {
            // ¡El evento llegó desde el servidor!
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Carga la imagen desde el archivo
                UploadedImage.Source = ImageSource.FromFile(imagePath);
                DisplayAlert("¡Éxito!", "La imagen del cliente se ha cargado.", "OK");
                
                // (Opcional) Cambiar a la vista QR si no estaba activa
                OnQrModeClicked(this, EventArgs.Empty);
            });
        }

        private void OnBluetoothModeClicked(object sender, EventArgs e)
        {
            // Mostrar vistas de Bluetooth
            BluetoothFrame.IsVisible = true;
            SearchButton.IsVisible = true;

            // Ocultar vista de QR
            QrViewLayout.IsVisible = false;

            // (Opcional) Cambiar colores de botones
            BluetoothButton.BackgroundColor = Color.FromArgb("#1B5E20");
            BluetoothButton.TextColor = Colors.White;
            QrButton.BackgroundColor = Color.FromArgb("#E0E0E0");
            QrButton.TextColor = Color.FromArgb("#616161");
        }

        private void OnQrModeClicked(object sender, EventArgs e)
        {
            // Ocultar vistas de Bluetooth
            BluetoothFrame.IsVisible = false;
            SearchButton.IsVisible = false;

            // Mostrar vista de QR
            QrViewLayout.IsVisible = true;

            // (Opcional) Cambiar colores de botones
            QrButton.BackgroundColor = Color.FromArgb("#1B5E20");
            QrButton.TextColor = Colors.White;
            BluetoothButton.BackgroundColor = Color.FromArgb("#E0E0E0");
            BluetoothButton.TextColor = Color.FromArgb("#616161");

            // Generar el valor del QR
            try
            {
                string ip = GetLocalIpAddress();
                string url = $"http://{ip}:8080";

                if (ip == "127.0.0.1")
                {
                    DisplayAlert("Aviso", "No se detectó una red Wi-Fi. El QR podría no funcionar.", "OK");
                }

                // 1. Generar el código QR usando QRCoder
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.L);

                // 2. Convertirlo a bytes (PNG)
                PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeBytes = qRCode.GetGraphic(20);

                // 3. Asignarlo a la Imagen normal de MAUI
                QrImage.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error generando QR: {ex.Message}");
                DisplayAlert("Error", "No se pudo generar el código QR", "OK");
            }
        }

        private string GetLocalIpAddress()
        {
            try
            {
                var ipAddresses = NetworkInterface.GetAllNetworkInterfaces()
                    .SelectMany(i => i.GetIPProperties().UnicastAddresses)
                    .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork &&
                                !System.Net.IPAddress.IsLoopback(a.Address))
                    .Select(a => a.Address.ToString())
                    .FirstOrDefault(ip => ip.StartsWith("192.168.") || ip.StartsWith("10.")); // Común en Wi-Fi

                return ipAddresses ?? "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        // --- LÓGICA EXISTENTE: MANEJO DE BLUETOOTH ---

        private void OnDeviceDiscovered(object? sender, DeviceEventArgs e)
        {
            if (e.Device == null) return;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!string.IsNullOrEmpty(e.Device.Name) &&
                    !DispositivosEncontrados.Any(d => d.Id == e.Device.Id))
                {
                    DispositivosEncontrados.Add(e.Device);
                }
            });
        }

        private void OnDeviceConnected(object? sender, DeviceEventArgs e)
        {
            if (e.Device is not IDevice connectedDevice) return;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await _adapter.StopScanningForDevicesAsync();
                    await DisplayAlert("Conexión Exitosa", $"Vinculado a: {connectedDevice.Name}", "OK");
                    await Navigation.PushAsync(new SendImagePage());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error de navegación después de conectar: {ex.Message}");
                    await DisplayAlert("Error de Navegación", "No se pudo cambiar a la página de imagen.", "OK");
                }
            });
        }

        private async void OnDeviceConnectionLost(object? sender, DeviceEventArgs e)
        {
            if (e.Device == null) return;
            await DisplayAlert("Desconectado", $"Se perdió la conexión con: {e.Device.Name}", "OK");
        }

        private async void OnSearchDevicesClicked(object? sender, EventArgs e)
        {
            if (sender is not Button searchButton) return;

            if (_bluetoothLE.State != BluetoothState.On)
            {
                await DisplayAlert("Bluetooth Apagado", "Por favor, enciende Bluetooth en tu dispositivo.", "OK");
                return;
            }

            if (_adapter.IsScanning)
            {
                await _adapter.StopScanningForDevicesAsync();
            }

            searchButton.IsEnabled = false;
            DispositivosEncontrados.Clear();

            await DisplayAlert("Buscando...", "Escaneando dispositivos Bluetooth cercanos. Selecciona tu dispositivo de la lista.", "OK");

            await _adapter.StartScanningForDevicesAsync();

            await Task.Delay(15000);
After:            searchButton.IsEnabled = true;
        }

        public async void OnDeviceSelected(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is not CollectionView collectionView) return;

            if (e.CurrentSelection.FirstOrDefault() is IDevice selectedDevice)
            {
                await ConnectToDevice(selectedDevice);
                collectionView.SelectedItem = null;
            }
        }

        private async Task ConnectToDevice(IDevice device)
        {
            try
            {
                await _adapter.StopScanningForDevicesAsync();
                await _adapter.ConnectToDeviceAsync(device);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error de Conexión", $"Fallo al conectar con {device.Name}: {ex.Message}.", "OK");
            }
        }
    }
}