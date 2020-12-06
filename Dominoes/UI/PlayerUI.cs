using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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

            image.MouseEnter += Image_MouseEnter;
            image.MouseLeave += Image_MouseLeave;

            image.MouseLeftButtonUp += Image_MouseLeftButtonUp;

            return image;
        }

        protected void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Image image)
            {
                image.Opacity = 1;
            }
        }


        protected void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Image image)
            {
                image.Opacity = 0.5;
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is ImageDomino imageDomino)
            {
                tableUI.AddHelpDomino(imageDomino.Domino, (direction) =>
                {
                    player.MakeMove(imageDomino.Domino, direction);
                    Update();
                });
            }
        }
    }
}
