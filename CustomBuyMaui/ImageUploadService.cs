namespace CustomBuyMaui
{
    public interface IImageUploadService
    {
        // Agregamos el ? aquí
        event Action<string>? OnImageUploaded; 
        void NotifyImageUploaded(string imagePath);
    }

    public class ImageUploadService : IImageUploadService
    {
        // Agregamos el ? aquí también
        public event Action<string>? OnImageUploaded;

        public void NotifyImageUploaded(string imagePath)
        {
            OnImageUploaded?.Invoke(imagePath);
        }
    }
}