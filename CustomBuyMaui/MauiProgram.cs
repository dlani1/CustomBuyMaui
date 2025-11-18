using Microsoft.Maui.Hosting;
using CommunityToolkit.Maui; 
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Extensions.Logging; // <--- Recomendado para Debug

namespace CustomBuyMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit(); 

            builder.ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
            });

            // Registrar Servicios
            builder.Services.AddSingleton<IImageUploadService, ImageUploadService>();
            
            // Registrar Páginas
            builder.Services.AddTransient<MugCustomizationPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}