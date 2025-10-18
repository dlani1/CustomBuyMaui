namespace CustomBuyMaui;

public partial class InicioPage : ContentPage
{
    public InicioPage()
    {
        InitializeComponent();
    }

   // En IncioPage.xaml.cs

    private async void OnStartButtonClicked(object sender, EventArgs e)
    {
        // 1. Navega a la pantalla de carga inmediatamente.
        await Shell.Current.GoToAsync("LoadingPage"); 
    }
}