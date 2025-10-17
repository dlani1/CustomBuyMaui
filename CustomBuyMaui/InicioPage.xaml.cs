namespace CustomBuyMaui;

public partial class InicioPage : ContentPage
{
    public InicioPage()
    {
        InitializeComponent();
    }

    private async void OnStartButtonClicked(object sender, EventArgs e)
    {
        // Cuando se presione el botón, navegaremos a la siguiente página.
        // "ProductSelectionPage" será el nombre de nuestra próxima página.
        await Shell.Current.GoToAsync("///ProductSelectionPage");
    }
}