using Microsoft.Maui.Hosting;
using CommunityToolkit.Maui; // Asegúrate que esta línea exista si usas el toolkit

namespace CustomBuyMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit(); // <<-- ¡Asegúrate que este punto y coma esté aquí!

            builder.ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
            });

            return builder.Build();
        }
    }
}