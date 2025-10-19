// En LoadingPage.xaml.cs

namespace CustomBuyMaui; 
public partial class LoadingPage : ContentPage
{
    public LoadingPage()
    {
        InitializeComponent();
    }

    // Este método se ejecuta cuando la página aparece en la pantalla
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // DEBE HABER UN RETRASO SUFICIENTE PARA QUE VEAS LA PANTALLA
        await Task.Delay(3000); // Esto debe mantener la pantalla visible por 3 segundos

        await Shell.Current.GoToAsync("..//ProductSelectionPage"); 
    }
} 