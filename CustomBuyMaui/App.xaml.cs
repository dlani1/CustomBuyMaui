using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using EmbedIO;
using EmbedIO.WebApi;
using EmbedIO.Actions; 
using System.Text;
using System.IO; 
using System.Threading.Tasks;
using System.Threading; 
using System; 
using System.Text.Json; 
using Microsoft.Maui.Storage; 
using EmbedIO.Utilities; // <-- ¡ESTA LÍNEA DEBE ESTAR AHÍ!

namespace CustomBuyMaui
{
    public partial class App : Application
    {
        private readonly IImageUploadService _uploadService;

        public App(IImageUploadService uploadService)
        {
            InitializeComponent();
            MainPage = new AppShell();
            _uploadService = uploadService;
            
            // Iniciar el servidor en un hilo de fondo
            Task.Run(StartEmbedIOWebServer);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);
            #if WINDOWS
            window.HandlerChanged += (sender, e) =>
            {
                if (window.Handler?.PlatformView is Microsoft.UI.Xaml.Window nativeWindow)
                {
                    var appWindow = nativeWindow.AppWindow;
                    if (appWindow != null)
                    {
                        nativeWindow.ExtendsContentIntoTitleBar = true;
                        nativeWindow.SetTitleBar(null);
                        appWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.FullScreen);
                    }
                }
            };
            #endif
            return window;
        }

       private void StartEmbedIOWebServer()
{
    // Usamos localhost para evitar permisos de administrador en Windows durante pruebas
    var url = "http://localhost:9090";

    try 
    {
        using var server = new WebServer(o => o
            .WithUrlPrefix(url)
            .WithMode(HttpListenerMode.EmbedIO))
            
            // 1. Módulo API para RootController
            // Se elimina .WithJsonSerializer. La configuración de serialización se hace aquí:
            .WithModule(new WebApiModule("/", m =>
            {
                // 💡 CORRECCIÓN CS1061/CS1593: Configuración de serialización manual.
                // Esto elimina la dependencia del método de extensión fallido.
                m.ResponseSerializer = new ResponseSerializerCallback((IHttpContext context, object data) => 
                    System.Text.Json.JsonSerializer.Serialize(data));
                    
                m.RegisterController<RootController>();
            }))
            
            // 2. Módulo de Subida (API) - CORRECCIÓN CS1503: Usa WithAction.
            .WithAction(HttpVerbs.Post, "/api", HandleUpload);

        // Bloqueamos este hilo (que ya es un Task.Run secundario) para que el servidor viva
        server.RunAsync(CancellationToken.None).Wait();
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"ERROR CRÍTICO SERVIDOR: {ex.Message}");
    }
}
        // --- FUNCIÓN PARA MANEJAR LA SUBIDA ---
        private async Task HandleUpload(IHttpContext ctx)
        {
            try
            {
                var request = ctx.Request;
                
                // Nombre temporal
                string tempFile = Path.Combine(FileSystem.CacheDirectory, $"upload_{DateTime.Now.Ticks}.jpg");

                // Guardar archivo
                using (var fileStream = File.Create(tempFile))
                {
                    await request.InputStream.CopyToAsync(fileStream);
                }

                // Notificar a la UI
                _uploadService.NotifyImageUploaded(tempFile);

                // Responder al navegador
                await ctx.SendStringAsync("¡Imagen recibida! Regresa a la App.", "text/plain", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                ctx.Response.StatusCode = 500;
                await ctx.SendStringAsync("Error: " + ex.Message, "text/plain", Encoding.UTF8); 
            }
        }
    }
}