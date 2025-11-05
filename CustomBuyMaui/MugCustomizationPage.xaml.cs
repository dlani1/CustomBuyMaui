using Microsoft.Maui.Controls;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq; 
using Plugin.BLE.Abstractions;

namespace CustomBuyMaui
{
    public partial class MugCustomizationPage : ContentPage
    {
        // Lista para dispositivos encontrados (DataSource para CollectionView)
        public ObservableCollection<IDevice> DispositivosEncontrados { get; } = new ObservableCollection<IDevice>();
        
        // Adaptadores de Bluetooth
        private readonly IBluetoothLE _bluetoothLE;
        private readonly IAdapter _adapter;

        public MugCustomizationPage()
        {
            InitializeComponent();
            
            _bluetoothLE = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;
            
            // Suscripción a eventos de Bluetooth
            _adapter.DeviceDiscovered += OnDeviceDiscovered;
            _adapter.DeviceConnectionLost += OnDeviceConnectionLost;
            // CORRECCIÓN CLAVE: El evento no debe ser async directamente
            _adapter.DeviceConnected += OnDeviceConnected; 
            
            // Establecer el BindingContext
            BindingContext = this; 
        }

        // --- MANEJO DE EVENTOS BLUETOOTH ---
        
        private void OnDeviceDiscovered(object? sender, DeviceEventArgs e)
        {
            if (e.Device == null) return; 

            // Asegurar que la colección se actualice en el Hilo Principal
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!string.IsNullOrEmpty(e.Device.Name) && 
                    !DispositivosEncontrados.Any(d => d.Id == e.Device.Id))
                {
                    DispositivosEncontrados.Add(e.Device);
                }
            });
        }
        
        // Se llama cuando la conexión es exitosa (AQUÍ ESTÁ LA CORRECCIÓN CLAVE)
        private void OnDeviceConnected(object? sender, DeviceEventArgs e)
        {
            if (e.Device is not IDevice connectedDevice) return; 
            
            // Usar MainThread.BeginInvokeOnMainThread para realizar la navegación (UI)
            // de forma segura y evitar el crash.
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    // Detener el escaneo
                    await _adapter.StopScanningForDevicesAsync(); 
                    
                    // Notificar al usuario (opcional)
                    await DisplayAlert("Conexión Exitosa", $"Vinculado a: {connectedDevice.Name}", "OK");

                    // ✅ Navegación segura al hilo principal
                    // Asume que 'SendImagePage' existe y recibe el dispositivo.
                    await Navigation.PushAsync(new SendImagePage(connectedDevice));
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

        // --- ACCIONES DEL USUARIO ---
        
        private async void OnSearchDevicesClicked(object? sender, EventArgs e)
        {
            if (sender is not Button searchButton) return;

            // 1. Verificar si Bluetooth está encendido 
            if (_bluetoothLE.State != BluetoothState.On) 
            {
                await DisplayAlert("Bluetooth Apagado", "Por favor, enciende Bluetooth en tu dispositivo.", "OK");
                return;
            }
            
            // Detener escaneo previo si estaba activo
            if (_adapter.IsScanning)
            {
                await _adapter.StopScanningForDevicesAsync();
            }

            // 2. Limpiar lista e iniciar escaneo
            searchButton.IsEnabled = false; // Deshabilitar
            DispositivosEncontrados.Clear();
            
            await DisplayAlert("Buscando...", "Escaneando dispositivos Bluetooth cercanos. Selecciona tu dispositivo de la lista.", "OK");
            
            await _adapter.StartScanningForDevicesAsync();
            
            // 3. Volver a habilitar el botón después de un retraso
            await Task.Delay(15000); 
            searchButton.IsEnabled = true; 
        }

        // Lógica al seleccionar un dispositivo de la lista (CollectionView)
        public async void OnDeviceSelected(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is not CollectionView collectionView) return;

            if (e.CurrentSelection.FirstOrDefault() is IDevice selectedDevice)
            {
                // Intentar conectar
                await ConnectToDevice(selectedDevice);
                
                // Deseleccionar el elemento inmediatamente
                collectionView.SelectedItem = null; 
            }
        }

        // --- LÓGICA DE CONEXIÓN ---
        
        private async Task ConnectToDevice(IDevice device)
        {
            try
            {
                // Detener el escaneo antes de conectar
                await _adapter.StopScanningForDevicesAsync();
                
                // Intenta la conexión. Si es exitosa, llama a OnDeviceConnected.
                await _adapter.ConnectToDeviceAsync(device);
                
            }
            catch (Exception ex)
            {
                // Manejar error de conexión
                await DisplayAlert("Error de Conexión", $"Fallo al conectar con {device.Name}: {ex.Message}.", "OK");
            }
        }

        // Liberar recursos al salir de la página
        protected override void OnDisappearing()
        {
            _adapter.StopScanningForDevicesAsync(); 
            _adapter.DeviceDiscovered -= OnDeviceDiscovered;
            _adapter.DeviceConnectionLost -= OnDeviceConnectionLost;
            _adapter.DeviceConnected -= OnDeviceConnected;
            base.OnDisappearing();
        }
    }
}