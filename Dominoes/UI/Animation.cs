using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Dominoes
{
    public static class Animation
    {
        public static void Add(Image image)
        {
            var marginAnimation = new ThicknessAnimation
            {
                From = new Thickness(-5),
                To = new Thickness(0),
                Duration = TimeSpan.FromSeconds(0.5)
            };

            var opacityAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            image.BeginAnimation(FrameworkElement.MarginProperty, marginAnimation);
            image.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
        }

        public static void Select(Image image)
        {
            var widthAnimation = new DoubleAnimation
            {
                From = image.ActualWidth,
                To = 45,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            var heightAnimation = new DoubleAnimation
            {
                From = image.ActualHeight,
                To = 90,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            var marginAnimation = new ThicknessAnimation
            {
                From = image.Margin,
                To = new Thickness(1.25, -15, 1.25, 0),
                Duration = TimeSpan.FromSeconds(0.1)
            };

            image.BeginAnimation(FrameworkElement.WidthProperty, widthAnimation);
            image.BeginAnimation(FrameworkElement.HeightProperty, heightAnimation);
            image.BeginAnimation(FrameworkElement.MarginProperty, marginAnimation);
        }

        public static void Unselect(Image image)
        {
            var widthAnimation = new DoubleAnimation
            {
                From = image.ActualWidth,
                To = 37.5,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            var heightAnimation = new DoubleAnimation
            {
                From = image.ActualHeight,
                To = 75,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            var marginAnimation = new ThicknessAnimation
            {
                From = image.Margin,
                To = new Thickness(5, 0, 5, 0),
                Duration = TimeSpan.FromSeconds(0.1)
            };

            image.BeginAnimation(FrameworkElement.WidthProperty, widthAnimation);
            image.BeginAnimation(FrameworkElement.HeightProperty, heightAnimation);
            image.BeginAnimation(FrameworkElement.MarginProperty, marginAnimation);
        }

        public static void Delete(Image image, EventHandler nextEvent, bool reverse = false)
        {
            var marginAnimation = new ThicknessAnimation
            {
                From = image.Margin,
                To = reverse ? new Thickness(5, 0, 5, -100) : new Thickness(5, -100, 5, 0),
                Duration = TimeSpan.FromSeconds(1)
            };

            var opacityAnimation = new DoubleAnimation
            {
                From = image.Opacity,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            opacityAnimation.Completed += nextEvent;

            image.BeginAnimation(FrameworkElement.MarginProperty, marginAnimation);
            image.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
        }
    }
}
