namespace CustomBuyMaui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // El nombre de la ruta debe coincidir EXACTAMENTE con el GoToAsync.
        Routing.RegisterRoute("ProductSelectionPage", typeof(ProductSelectionPage));
    }
}