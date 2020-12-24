using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Dominoes
{
    public class BotUI : PlayerUI
    {
        private readonly GameField gameField;

        public BotUI(Bot bot, StackPanel deck, GameField gameField) : base(bot, deck, null) 
        {
            this.gameField = gameField;
        }

        public async void MakeMove()
        {
            await Task.Run(() => Thread.Sleep(1000));
            while (gameField.CheckPlayerForNoMoves(player))
            {
                if (player.TakeDomino() == null)
                {
                    player.EndMove();
                    return;
                }
                Update();
                await Task.Run(() => Thread.Sleep(500));
            }

            var randomImage = (Image)deck.Children[Generator.Random.Next(0, deck.Children.Count)];
            Animation.Delete(randomImage, (s, e) =>
            {
                ((Bot)player).MakeMove();
                Update();
            }, 
            true);
        }

        protected override ImageDomino CreateImageDominoButton(Domino domino)
        {
            var image = new ImageDomino(new Domino(0, 0));
            image.InitializeComponent();
            return image;
        }
    }
}
