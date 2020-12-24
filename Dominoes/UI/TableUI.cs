using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Dominoes
{
    public class TableUI
    {
        private readonly GameField gameField;

        private readonly Canvas table;

        private Direction currentDirection = Direction.None;

        private readonly Dictionary<DirectionMove, ImageDomino> LastImageDomino
            = new Dictionary<DirectionMove, ImageDomino>()
            {
                [DirectionMove.Left] = null,
                [DirectionMove.Right] = null,
            };

        private readonly Dictionary<DirectionMove, List<ImageDomino>> helpDomino
            = new Dictionary<DirectionMove, List<ImageDomino>>()
            {
                [DirectionMove.Left] = new List<ImageDomino>(),
                [DirectionMove.Right] = new List<ImageDomino>(),
            };

        private bool IsFirstImage { get => LastImageDomino[DirectionMove.Left] == null && LastImageDomino[DirectionMove.Right] == null; }

        public TableUI(GameField gameField, Canvas table)
        {
            this.gameField = gameField;
            this.table = table;

            gameField.EventSetDomino += AddDomino;
        }

        public void AddDomino(Domino domino, DirectionMove move)
        {
            if (gameField.Root == null) return;

            var imageDomino = new ImageDomino(domino);

            if (IsFirstImage)
            {
                imageDomino.InitializeComponent(DirectionMove.Left, Direction.Center);
                LastImageDomino[DirectionMove.Left] = imageDomino;
                LastImageDomino[DirectionMove.Right] = imageDomino;
            }
            else
            {
                if (currentDirection == Direction.None)
                {
                    var directions = GetCorrectDirections(domino, move);
                    currentDirection = directions[Generator.Random.Next(directions.Count)];
                }
                imageDomino.InitializeComponent(move, currentDirection, LastImageDomino[move]);
                LastImageDomino[move] = imageDomino;
                currentDirection = Direction.None;
            }

            table.Children.Add(imageDomino);
            Animation.Add(imageDomino);
        }

        public void AddHelpDomino(Domino domino, Action<DirectionMove> continueMove)
        {
            if (gameField.CheckDominoForCorrectMove(domino, DirectionMove.Left)
                || gameField.CheckDominoForCorrectMove(domino, DirectionMove.Right))
            {
                ClearAllHelpDomino();
                CreateHelpDomino(domino, DirectionMove.Left, continueMove);
                if (!IsFirstImage) CreateHelpDomino(domino, DirectionMove.Right, continueMove);
            }
        }

        private void CreateHelpDomino(Domino domino, DirectionMove move, Action<DirectionMove> continueMove)
        {
            if (!gameField.CheckDominoForCorrectMove(domino, move)) return;

            foreach (var e in GetCorrectDirections(domino, move))
            {
                var help = CreateHelpImage(domino, move, e, continueMove);
                helpDomino[move].Add(help);
                table.Children.Add(help);
            }
        }

        private void ClearHelpDomino(DirectionMove direction)
        {
            foreach (var e in helpDomino[direction])
                table.Children.Remove(e);
            helpDomino[direction].Clear();
        }

        private void ClearAllHelpDomino()
        {
            ClearHelpDomino(DirectionMove.Left);
            ClearHelpDomino(DirectionMove.Right);
        }

        private ImageDomino CreateHelpImage(Domino domino, DirectionMove move, Direction direction, Action<DirectionMove> continueMove)
        {
            var imageDomino = new ImageDomino(domino);
            imageDomino.InitializeComponent(move, direction, LastImageDomino[move]);

            imageDomino.Opacity = 0.5;
            imageDomino.MouseLeftButtonUp += (s, e) =>
            {
                currentDirection = direction;
                continueMove(move);
                ClearAllHelpDomino();
            };
            imageDomino.MouseEnter += (s, e) => imageDomino.Opacity = 0.75;
            imageDomino.MouseLeave += (s, e) => imageDomino.Opacity = 0.5;

            return imageDomino;
        }

        private List<Direction> GetCorrectDirections(Domino domino, DirectionMove move)
        {
            var directions = new List<Direction>();
            if (domino.FirstValue == domino.SecondValue 
                || LastImageDomino[move].Domino.FirstValue == LastImageDomino[move].Domino.SecondValue) 
                directions.Add(IsFirstImage ? Direction.Center : LastImageDomino[move].Direction);
            else
            {
                directions.Add(Direction.Center);
                switch (LastImageDomino[move].Direction)
                {
                    case Direction.Center:
                        directions.Add(Direction.Up);
                        directions.Add(Direction.Down);
                        break;
                    case Direction.Up:
                        directions.Add(Direction.Up);
                        break;
                    case Direction.Down:
                        directions.Add(Direction.Down);
                        break;
                }
            }
            return directions;
        }
    }
}
