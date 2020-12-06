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
        public BotUI(Bot bot, StackPanel deck) : base(bot, deck, null) { }

        public async void MakeMove()
        {
            await Task.Run(() => Thread.Sleep(500));
            while (((Bot)player).MakeMove() != MoveState.Successful)
            {
                if (player.TakeDomino() == null)
                {
                    player.EndMove();
                    break;
                }
                Update();
                await Task.Run(() => Thread.Sleep(500));
            }
            Update();
        }

        protected override ImageDomino CreateImageDominoButton(Domino domino)
        {
            var image = new ImageDomino(new Domino(0, 0));
            image.InitializeComponent();

            /*image.MouseEnter += Image_MouseEnter;
            image.MouseLeave += Image_MouseLeave;*/

            return image;
        }
    }
}
