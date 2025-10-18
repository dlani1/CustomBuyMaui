namespace CustomBuyMaui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Rutas ya existentes
        Routing.RegisterRoute("ProductSelectionPage", typeof(ProductSelectionPage));

        // -----------------------------------------------------------------
        // ¡NUEVA RUTA DE CARGA!
        // -----------------------------------------------------------------
        Routing.RegisterRoute("LoadingPage", typeof(LoadingPage));
    }
}