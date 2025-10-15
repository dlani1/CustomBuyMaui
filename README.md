# 🛍️ CustomBuyMaui

¡Bienvenido al repositorio de **CustomBuyMaui**! Esta es una aplicación móvil multiplataforma desarrollada con .NET MAUI, diseñada para gestionar pedidos personalizados.

## ✨ Características Principales

* **Catálogo de Productos:** Navegación y visualización del catálogo.
* **Personalización:** Interfaz para crear pedidos con opciones específicas (Ej: colores, tamaños).
* **Gestión de Pedidos:** Sección para ver el estado y el historial de pedidos.

## 🚀 Cómo Ejecutar la Aplicación

Sigue estos pasos para clonar el repositorio y ejecutar la aplicación en tu máquina local (Windows, Android, etc.).

### Requisitos

1.  **Visual Studio 2022** (Versión 17.8 o superior).
2.  **SDK de .NET 9.0** (o la versión que estés usando).
3.  Carga de trabajo de **.NET MAUI** instalada en Visual Studio.

### Clonar y Compilar

1.  **Clonar el Repositorio:**
    ```bash
    git clone [https://github.com/dlani1/CustomBuyMaui.git](https://github.com/dlani1/CustomBuyMaui.git)
    ```
2.  **Navegar al Directorio:**
    ```bash
    cd CustomBuy
    ```
3.  **Ejecutar en Windows:**

    Asegúrate de estar en la carpeta **`CustomBuyMaui`** y usa el Target Framework Moniker (TFM) correcto:
    ```bash
    cd CustomBuyMaui
    dotnet run -f net9.0-windows10.0.19041.0
    ```
    *(Alternativamente, abre `CustomBuy.sln` en Visual Studio 2022 y presiona F5).*

## 📁 Estructura del Proyecto

Los archivos de tu aplicación se encuentran principalmente dentro de la carpeta `CustomBuyMaui/`:

| Archivo/Carpeta | Descripción |
| :--- | :--- |
| **`AppShell.xaml`** | Define la estructura de navegación principal (Shell). |
| **`CustomBuyMaui.csproj`** | El archivo de configuración principal del proyecto. |
| **`MainPage.xaml`** | La página inicial de demostración. |
| **`InicioPage.xaml`** | La página principal con botones de Catálogo, Personalizar, etc. |
| **`Resources/`** | Iconos, fuentes, imágenes y el *splash screen*. |

## 🌟 ¡Gracias por tu Visita!

¡Felicidades por llegar hasta aquí! El desarrollo multiplataforma tiene sus retos (¡como ya lo experimentamos con la caché!), pero has superado todos los obstáculos y ahora tu proyecto **CustomBuyMaui** está listo para crecer.

Siéntete libre de clonar, probar y contribuir. ¡Feliz codificación y a seguir creando cosas geniales! ✨
