using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;

namespace CustomBuyMaui
{
    // La clase App debe ser pública y parcial
    public partial class App : Application
    {
        // Constructor, requerido para inicializar la aplicación
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell(); // Asegúrate de que esto dirija a tu Shell
        }

        // Método para configurar la ventana en Windows (Pantalla Completa/Sin Bordes)
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
// 1. Permitir que el contenido se extienda (necesario para SetTitleBar)
                        nativeWindow.ExtendsContentIntoTitleBar = true; 
                        
                        // 2. Establecer un control nulo como barra de título (la quita)
                        nativeWindow.SetTitleBar(null); 
                        
                        // 3. Forzar que el área de la barra sea transparente (opcional, pero ayuda)
                        if (Microsoft.UI.Xaml.Window.Current is Microsoft.UI.Xaml.Window w)
                        {
                            var titleBar = w.AppWindow.TitleBar;
                            if (titleBar != null)
                            {
                                titleBar.ButtonBackgroundColor = Microsoft.UI.Colors.Transparent;
                                titleBar.ButtonForegroundColor = Microsoft.UI.Colors.Transparent;
                            }
                        }
                        
                        // 4. Establecer la ventana en pantalla completa
                        appWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.FullScreen);
                    }
                }
            };
            #endif

            return window;
        }
    }
}