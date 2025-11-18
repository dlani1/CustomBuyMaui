using EmbedIO;
using EmbedIO.WebApi;
using EmbedIO.Routing;

public class RootController : WebApiController
{
    [Route(HttpVerbs.Get, "/")]
    public string GetRoot()
    {
        // Devolvemos el mismo HTML de antes
        // (Asegúrate de poner la IP correcta en la acción del formulario si es necesario, 
        // aunque /api/upload debería funcionar)
        var html = @"
            <html>
            <head>
                <title>Cargar Imagen</title>
                <meta name='viewport' content='width=device-width, initial-scale=1'>
                <style>
                    body { font-family: Arial, sans-serif; display: grid; place-items: center; min-height: 90vh; background-color: #f4f4f4; }
                    form { background-color: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 2px 5px rgba(0,0,0,0.1); }
                    input[type='file'] { margin-bottom: 10px; }
                    input[type='submit'] { background-color: #007bff; color: white; padding: 10px 15px; border: none; border-radius: 4px; cursor: pointer; }
                </style>
            </head>
            <body>
                <form action='/api/upload' method='post' enctype='multipart/form-data'>
                    <h2>Sube tu diseño para la taza</h2>
                    <input type='file' name='file' accept='image/*' required />
                    <br/>
                    <input type='submit' value='Subir Imagen' />
                </form>
            </body>
            </html>";
        
        // EmbedIO es inteligente y pondrá el Content-Type por nosotros
        return html;
    }
}