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

        public Direction Direction { get; private set; }

        public Point CenterPoint = new Point();

        private bool isInverted;

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

        public void InitializeComponent(DirectionMove move, Direction direction = 0, ImageDomino last = null)
        {
            Direction = direction;

            var b = new BitmapImage();
            b.BeginInit();
            b.UriSource = uriImage;
            var rotation = GetCorrectRotation(move, last);
            b.Rotation = GetCorrectRotation(move, last);
            b.EndInit();

            Source = b;
            MaxWidth = 75 / ((int)rotation % 2 + 1);
            MaxHeight = 37.5 * ((int)rotation % 2 + 1);

            if (last != null)
            {
                if (direction == Direction.Center)
                {
                    CenterPoint.X = last.CenterPoint.X - last.MaxWidth / 2 - MaxWidth / 2;
                    CenterPoint.Y = last.CenterPoint.Y
                        - (last.Direction != Direction.Center ? MaxHeight / 2 : 0)
                        + (last.Direction == Direction.Down ? last.MaxHeight / 2 : 0);
                }
                else
                {
                    CenterPoint.X = last.CenterPoint.X
                        - (last.Direction == Direction.Center ? MaxWidth / 2 : 0);
                    CenterPoint.Y = last.CenterPoint.Y
                        + (direction == Direction.Up ? -1 : 1) * (last.MaxHeight / 2 + MaxHeight / 2);
                }
            }

            if (move == DirectionMove.Left)
                Canvas.SetLeft(this, CenterPoint.X - MaxWidth / 2);
            else Canvas.SetRight(this, CenterPoint.X - MaxWidth / 2);
            Canvas.SetTop(this, CenterPoint.Y - MaxHeight / 2);
        }

        private Rotation GetCorrectRotation(DirectionMove move, ImageDomino last)
        {
            var correctRotation = Rotation.Rotate0;

            if (Domino.FirstValue == Domino.SecondValue)
                return Direction == Direction.Center ? Rotation.Rotate90 : correctRotation;

            if (move == DirectionMove.Left)
            {
                if (Domino.SecondValue != (last.isInverted ? last.Domino.SecondValue : last.Domino.FirstValue))
                {
                    isInverted = true;
                    correctRotation = Rotation.Rotate180;
                }
                if (Direction == Direction.Down)
                    correctRotation = (int)correctRotation - 1 < 0 ? Rotation.Rotate270 : correctRotation - 1;
                if (Direction == Direction.Up)
                    correctRotation = (int)correctRotation - 1 > 3 ? Rotation.Rotate0 : correctRotation + 1;
            }
            else
            {
                if (Domino.FirstValue != (last.isInverted ? last.Domino.FirstValue : last.Domino.SecondValue))
                {
                    isInverted = true;
                    correctRotation = Rotation.Rotate180;
                }
                if (Direction == Direction.Up)
                    correctRotation = (int)correctRotation - 1 < 0 ? Rotation.Rotate270 : correctRotation - 1;
                if (Direction == Direction.Down)
                    correctRotation = (int)correctRotation - 1 > 3 ? Rotation.Rotate0 : correctRotation + 1;
            }
            return correctRotation;
        }
    }
}
