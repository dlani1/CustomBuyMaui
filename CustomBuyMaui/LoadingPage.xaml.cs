// En LoadingPage.xaml.cs

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

        // ** 1. SIMULACIÓN DE CARGA PESADA O DE DATOS REALES **
        await Task.Delay(3000); // Espera 3 segundos (simula una carga de datos)
        // Aquí debes llamar a tu servicio para cargar productos o recursos.
        // Ejemplo: var productos = await MiServicioDeDatos.ObtenerProductosAsync();


        // ** 2. NAVEGACIÓN A LA PÁGINA FINAL **
        // Navega a ProductSelectionPage y usa '..' para reemplazar LoadingPage en la pila.
        await Shell.Current.GoToAsync("..//ProductSelectionPage"); 
    }
}