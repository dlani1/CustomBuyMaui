using Microsoft.Maui.Controls;
using CustomBuyMaui.Models;
using System.Collections.ObjectModel;

namespace CustomBuyMaui
{
    public partial class ProductSelectionPage : ContentPage
    {
        private LinkedListNode<ProductItem>? currentNode;
        private readonly Image productImage;
        private readonly Label titleLabel;
        private readonly Label descriptionLabel;

        public ProductSelectionPage()
        {
            InitializeComponent();

            // 🔹 Crear lista doblemente enlazada
            var linkedList = new LinkedList<ProductItem>();
            linkedList.AddLast(new ProductItem
            {
                Title = "PC Gamer de Entrada",
                Description = "Equipo básico para juegos populares y tareas diarias.",
                ImageSource = "imagen1.jpeg",
                IsAvailable = true
            });
            linkedList.AddLast(new ProductItem
            {
                Title = "Laptop Ultraligera",
                Description = "Perfecta para trabajo, clases y movilidad.",
                ImageSource = "custom_buy_logo.png",
                IsAvailable = true
            });
            linkedList.AddLast(new ProductItem
            {
                Title = "Estación de Trabajo",
                Description = "Ideal para diseño, programación y edición.",
                ImageSource = "dotnet_bot.png",
                IsAvailable = true
            });

            currentNode = linkedList.First;

            // 🔹 Crear elementos visuales
            productImage = new Image
            {
                Aspect = Aspect.AspectFit,
                HeightRequest = 300,
                Margin = new Thickness(20)
            };

            titleLabel = new Label
            {
                FontSize = 22,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            descriptionLabel = new Label
            {
                FontSize = 16,
                TextColor = Colors.Gray,
                HorizontalOptions = LayoutOptions.Center
            };

            // 🔹 Botones
            var nextButton = new Button
            {
                Text = "Siguiente",
                BackgroundColor = Colors.Transparent,
                TextColor = Colors.Black
            };
            nextButton.Clicked += async (s, e) => await ShowNextAsync(linkedList);

            var prevButton = new Button
            {
                Text = "Anterior",
                BackgroundColor = Colors.Transparent,
                TextColor = Colors.Black
            };
            prevButton.Clicked += async (s, e) => await ShowPreviousAsync(linkedList);

            // 🔹 Armar interfaz
            Content = new StackLayout
            {
                Padding = new Thickness(10, 30),
                Spacing = 20,
                Children =
                {
                    productImage,
                    titleLabel,
                    descriptionLabel,
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.Center,
                        Spacing = 15,
                        Children = { prevButton, nextButton }
                    }
                }
            };

            // 🔹 Mostrar primer producto
            UpdateUI(currentNode!.Value);
        }

        // 🔹 Actualiza los datos en pantalla
        private void UpdateUI(ProductItem product)
        {
            productImage.Source = product.ImageSource;
            titleLabel.Text = product.Title;
            descriptionLabel.Text = product.Description;
        }

        // 🔹 Animación de transición (deslizar lateral + fade)
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
