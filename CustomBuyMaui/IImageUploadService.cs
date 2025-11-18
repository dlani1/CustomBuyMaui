// IImageUploadService.cs
public interface IImageUploadService
{
    // Evento que se disparará cuando llegue un archivo
    event Action<string> OnImageUploaded;
    
    // Método que llamará el servidor
    void NotifyImageUploaded(string imagePath);
}