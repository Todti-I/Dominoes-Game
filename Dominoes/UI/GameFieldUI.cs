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
        public readonly GameField gameField = new GameField(2);

        public readonly GameFieldView view;

        private readonly PlayerUI playerUI;

        private readonly BotUI botUI;

        private readonly TableUI tableUI;

        public GameFieldUI(GameFieldView view)
        {
            this.view = view;

            tableUI = new TableUI(gameField, view.Table);

            playerUI = new PlayerUI(gameField.Players[0], view.PlayerDeck, tableUI);
            gameField.EventStartTurn += () =>
            {
                if (gameField.CurrentPlayer == gameField.Players[0])
                    view.PlayerDeck.IsEnabled = true;
                else view.PlayerDeck.IsEnabled = false;
            };

            botUI = new BotUI((Bot)gameField.Players[1], view.BotDeck, gameField);
            gameField.EventStartTurn += () =>
            {
                if (gameField.CurrentPlayer == gameField.Players[1])
                    botUI.MakeMove();
            };

            gameField.EventGameOver += (state) => EndGame(state);
            gameField.EventGetDomino += () => view.GetButton.Content = $"Get({gameField.Pool.Count})";

            view.GetButton.Content = $"Get({gameField.Pool.Count})";
            view.GetButton.Click += playerUI.GetButtonClick;

            view.SkipButton.Click += (s, e) =>
            {
                if (gameField.CheckPlayerForNoMoves(gameField.Players[0]) && !gameField.CanTakeDomino(gameField.Players[0]))
                    gameField.Players[0].EndMove();
            };

            gameField.StartGame();
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
