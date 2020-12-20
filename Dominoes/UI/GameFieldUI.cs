using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Dominoes
{
    public class GameFieldUI
    {
        public readonly GameField gameField = new GameField();

        public readonly GameFieldView view;

        private readonly PlayerUI playerUI;

        private readonly BotUI botUI;

        private readonly TableUI tableUI;

        public GameFieldUI(GameFieldView view)
        {
            this.view = view;

            tableUI = new TableUI(gameField, view.Table);

            playerUI = new PlayerUI(gameField.Players[0], view.PlayerDeck, tableUI);
            botUI = new BotUI((Bot)gameField.Players[1], view.BotDeck);

            gameField.EventStartTurn += () =>
            {
                if (gameField.CurrentPlayer == gameField.Players[1])
                    botUI.MakeMove();
            };

            gameField.EventGameOver += (state) => EndGame(state);
            gameField.EventGetDomino += () => view.GetButton.Content = $"Get({gameField.Pool.Count})";

            view.GetButton.Content = $"Get({gameField.Pool.Count})";
            view.GetButton.Click += GetButton_Click;

            view.SkipButton.Click += (s, e) =>
            {
                if (gameField.CheckPlayerForNoMoves(gameField.Players[0]) && !gameField.CanTakeDomino(gameField.Players[0]))
                    gameField.Players[0].EndMove();
            };

            /*window.MouseMove += (s, e) =>
            {
                window.Log.Text = $"{gameField.Players[1]}\n{gameField.Players[0]}\n\nCurrent:\n{gameField.CurrentPlayer}";
                if (gameField.Root != null)
                    window.Log.Text += $"\n\n{ gameField.Root.Left}\n{ gameField.Root.Right}";
            };*/
        }

        private void GetButton_Click(object sender, RoutedEventArgs e)
        {
            if (gameField.CanTakeDomino(gameField.Players[0]))
            {
                var state = gameField.Players[0].TakeDomino();
                if (state != null)
                {
                    playerUI.Update();  
                }
            }
        }

        private async void EndGame(GameState gameState)
        {
            view.IsEnabled = false;
            if (gameState == GameState.Win) 
            {
                var winner = gameField.GetWinner();
                if (!(winner is Bot))
                    view.ResultText.Text = "You win!!!";
                else view.ResultText.Text = $"You lost :(";
            }
            else view.ResultText.Text = "Draw!";
            view.ResultPanel.Visibility = Visibility.Visible;
            await Task.Run(() => Thread.Sleep(3000));
            view.ClickBack(null, null);
        }
    }
}
