
using QRCoder;
using System.IO;
namespace CustomBuyMaui;
#pragma warning disable CA1416

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        GenerarQR();
    }

    private void GenerarQR()
    {
        // 🔹 IP local del kiosco (puedes cambiarla por la tuya)
        string ip = "192.168.1.10";
        string url = $"http://{ip}:5000/upload";

        // 🔹 Generar el código QR
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        QRCode qrCode = new QRCode(qrCodeData);

        var qrImage = qrCode.GetGraphic(20);

        // 🔹 Mostrar el QR en la interfaz
        imgQR.Source = ImageSource.FromStream(() =>
        {
            var ms = new MemoryStream();
            qrImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            return ms;
        });
    }
}
#pragma warning restore CA1416
