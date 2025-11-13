using Microsoft.Maui.Controls;
using CustomBuyMaui.Models;
using System.Collections.Generic; // Asegúrate de tener este 'using' si no lo tienes

namespace CustomBuyMaui
{
    public partial class ProductSelectionPage : ContentPage
    {
        private LinkedListNode<ProductItem>? currentNode;
        private readonly Image productImage;
        private readonly Label titleLabel;
        private readonly Label descriptionLabel;
        private readonly Button mugButton; // 👈 1. Nueva variable para el botón de taza

        public ProductSelectionPage()
        {
            InitializeComponent();
            BackgroundColor = Color.FromArgb("#fff");
            
            // 🔹 Crear lista doblemente enlazada
            var linkedList = new LinkedList<ProductItem>();
            linkedList.AddLast(new ProductItem
            {
                Title = "Stickers",
                Description = "Muy pronto",
                ImageSource = "stickers.png",
                IsAvailable = true
            });
            linkedList.AddLast(new ProductItem
            {
                Title = "Taza",
                Description = "Personaliza la tuya",
                ImageSource = "taza.png",
                IsAvailable = true
            });
            linkedList.AddLast(new ProductItem
            {
                Title = "Diseño en vinil",
                Description = "Muy pronto",
                ImageSource = "vinil.png",
                IsAvailable = true
            });

            currentNode = linkedList.First;

            // 🔹 Crear elementos visuales (omitiendo el código de Labels/Image por brevedad)
            productImage = new Image
            {
                Aspect = Aspect.AspectFit,
                HeightRequest = 250, 
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(20, 10)
            };

            titleLabel = new Label
            {
                FontSize = 30, 
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Colors.DarkGreen 
            };

            descriptionLabel = new Label
            {
                FontSize = 18, 
                TextColor = Color.FromArgb("#FF006400"),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };
            
            // 🔹 Botón de Personalizar Taza
            mugButton = new Button 
            {
                Text = "¡Personalizar Taza!",
                BackgroundColor = Colors.DarkGreen,
                TextColor = Colors.White,
                FontSize = 20,
                CornerRadius = 10,
                HeightRequest = 50,
                WidthRequest = 250,
                Margin = new Thickness(0, 20, 0, 0),
                IsVisible = false // 👈 Inicialmente oculto
            };
            mugButton.Clicked += async (s, e) => await GoToMugCustomizationPage(); // 👈 Evento Click

            // 🔹 Botones de Navegación
            var nextButton = new Button
            {
                Text = ">",
                BackgroundColor = Color.FromArgb("#135404ff"),
                TextColor = Colors.DarkGreen,
                FontSize = 18,
                CornerRadius = 20,
                Padding = new Thickness(15, 8)
            };
            nextButton.Clicked += async (s, e) => await ShowNextAsync(linkedList);

            var prevButton = new Button
            {
                Text = "<",
                BackgroundColor = Color.FromArgb("#FFFFFFFF"),
                TextColor = Colors.DarkGreen,
                FontSize = 18,
                CornerRadius = 20,
                Padding = new Thickness(15, 8)
            };
            prevButton.Clicked += async (s, e) => await ShowPreviousAsync(linkedList);

            Content = new Grid
            {
                Children =
                {
                    // 🔹 Imagen de fondo
                    new Image
                    {
                        Source = "fondo.png", 
                        Aspect = Aspect.AspectFill,
                        Opacity = 5
                    },

                    // 🔹 Armar interfaz
                    new StackLayout
                    {
                        Padding = new Thickness(20, 40),
                        Spacing = 25,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                        {
                            productImage,
                            titleLabel,
                            descriptionLabel,
                            mugButton, // 👈 2. Se agrega el botón al StackLayout
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                HorizontalOptions = LayoutOptions.Center,
                                Spacing = 25,
                                Children = { prevButton, nextButton }
                            }
                        }
                    }
                }
            };

            // 🔹 Mostrar primer producto
            UpdateUI(currentNode!.Value);
        }

        // 🔹 Actualiza los datos en pantalla y la visibilidad del botón
        private void UpdateUI(ProductItem product)
        {
            productImage.Source = product.ImageSource;
            titleLabel.Text = product.Title;
            descriptionLabel.Text = product.Description;
            
            // 👈 3. Lógica condicional para mostrar el botón
            mugButton.IsVisible = product.Title == "Taza";
        }

        // 🔹 Método para navegar a la siguiente interfaz
        private async Task GoToMugCustomizationPage()
        {
            // Nota: Debes crear la clase MugCustomizationPage en tu proyecto.
            await Navigation.PushAsync(new MugCustomizationPage()); 
        }

        // (Otros métodos ShowNextAsync, ShowPreviousAsync, AnimateTransitionAsync permanecen igual)
        private async Task AnimateTransitionAsync(ProductItem newItem, bool forward)
        {
            double direction = forward ? 1 : -1;
            uint duration = 250;

            // Animación de salida
            await Task.WhenAll(
                productImage.TranslateTo(-100 * direction, 0, duration, Easing.SinIn),
                productImage.FadeTo(0, duration)
            );

            // Actualizar contenido
            UpdateUI(newItem);

            // Resetear posición
            productImage.TranslationX = 100 * direction;
            productImage.Opacity = 0;

            // Animación de entrada
            await Task.WhenAll(
                productImage.TranslateTo(0, 0, duration, Easing.SinOut),
                productImage.FadeTo(1, duration)
            );
        }

        // 🔹 Mostrar siguiente producto
        private async Task ShowNextAsync(LinkedList<ProductItem> list)
        {
            currentNode = currentNode?.Next ?? list.First;
            if (currentNode != null)
                await AnimateTransitionAsync(currentNode.Value, forward: true);
        }

        // 🔹 Mostrar producto anterior
        private async Task ShowPreviousAsync(LinkedList<ProductItem> list)
        {
            currentNode = currentNode?.Previous ?? list.Last;
            if (currentNode != null)
                await AnimateTransitionAsync(currentNode.Value, forward: false);
        }
    }
}