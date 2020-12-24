using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Dominoes
{
    public class PlayerUI
    {
        protected readonly Player player;

        protected readonly StackPanel deck;

        private readonly TableUI tableUI;

        public PlayerUI(Player player, StackPanel deck, TableUI tableUI)
        {
            this.player = player;
            this.deck = deck;
            this.tableUI = tableUI;
            Update();
        }

        public void Update()
        {
            deck.Children.RemoveRange(0, deck.Children.Count);
            foreach (var domino in player.Deck)
            {
               deck.Children.Add(CreateImageDominoButton(domino));
            }
        }

        protected virtual ImageDomino CreateImageDominoButton(Domino domino)
        {
            var image = new ImageDomino(domino);
            image.InitializeComponent();

            image.MouseEnter += ImageDominoMouseEnter;
            image.MouseLeave += ImageDominoMouseLeave;

            image.MouseLeftButtonUp += ImageDominoMouseLeftButtonUp;

            return image;
        }

        public void GetButtonClick(object sender, RoutedEventArgs e)
        {
            if (player.TakeDomino() != null)
                Update();
        }

        protected void ImageDominoMouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Image image)
            {
                Animation.Unselect(image);
            }
        }

        protected void ImageDominoMouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Image image)
            {
                Animation.Select(image);
            }
        }

        private void ImageDominoMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is ImageDomino imageDomino)
            {
                tableUI.AddHelpDomino(imageDomino.Domino, (direction) =>
                {
                    Animation.Delete(imageDomino, (s, _e) => 
                    {
                        player.MakeMove(imageDomino.Domino, direction);
                        Update();
                    });
                });
            }
        }
    }
}
