using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Dominoes
{
    public class ImageDomino : Image
    {
        public Domino Domino { get; }

        private Rotation rotation;

        private readonly Uri uriImage;

        public ImageDomino(Domino domino)
        {
            Domino = domino;
            uriImage = new Uri(Directory.GetCurrentDirectory() + $@"\domino\{Domino.FirstValue}-{Domino.SecondValue}.png");
        }

        public void InitializeComponent()
        {
            var b = new BitmapImage();
            b.BeginInit();
            b.UriSource = uriImage;
            b.Rotation = Rotation.Rotate90;
            b.EndInit();
            Source = b;
            Height = 75;
            Width = 37.5;
            Margin = new Thickness(5, 0, 5, 0);
            
        }

        public void InitializeComponent(DirectionMove direction, ImageDomino last)
        {
            var b = new BitmapImage();
            b.BeginInit();
            b.UriSource = uriImage;
            b.Rotation = rotation = GetCorrectRotation(direction, last);
            b.EndInit();

            Source = b;
            MaxWidth = 75 / ((int)rotation % 2 + 1);
            MaxHeight = 37.5 * ((int)rotation % 2 + 1);

            if (last == null)
            {
                Canvas.SetLeft(this, -MaxWidth / 2);
                Canvas.SetRight(this, 0);
            }
            else
            {
                var point = new Point(Canvas.GetLeft(last), Canvas.GetTop(last));
                if (direction == DirectionMove.Left)
                    Canvas.SetLeft(this, point.X - MaxWidth);
                else Canvas.SetLeft(this, point.X + MaxWidth);
                Canvas.SetTop(this, point.Y);
            }
        }

        private Rotation GetCorrectRotation(DirectionMove direction, ImageDomino last)
        {
            if (direction == DirectionMove.Left)
            {
                if (last == null) return Rotation.Rotate0;
                if (Domino.SecondValue != (last.rotation == Rotation.Rotate180 ? last.Domino.SecondValue : last.Domino.FirstValue))
                    return Rotation.Rotate180;
            }
            else
            {
                if (last == null) return Rotation.Rotate0;
                if (Domino.FirstValue != (last.rotation == Rotation.Rotate180 ? last.Domino.FirstValue : last.Domino.SecondValue))
                    return Rotation.Rotate180;
            }
            return Rotation.Rotate0;
        }
    }
}
