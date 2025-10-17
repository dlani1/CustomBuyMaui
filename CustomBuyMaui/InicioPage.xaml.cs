namespace CustomBuyMaui;

public partial class InicioPage : ContentPage
{
    public InicioPage()
    {
        InitializeComponent();
    }

    private async void OnStartButtonClicked(object sender, EventArgs e)
    {
        // ❌ NO uses "///ProductSelectionPage"
        // ✅ USA SOLAMENTE la ruta registrada:
        await Shell.Current.GoToAsync("ProductSelectionPage");
    }
}