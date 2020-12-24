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

            gameField.EventSetDomino += (domino, direction) => AddDomino(domino, direction);
        }

        public void AddDomino(Domino domino, DirectionMove direction)
        {
            if (gameField.Root == null) return;

            var imageDomino = new ImageDomino(domino);
            imageDomino.InitializeComponent(direction, LastImageDomino[direction]);

            if (IsFirstImage)
            {
                LastImageDomino[DirectionMove.Left] = imageDomino;
                LastImageDomino[DirectionMove.Right] = imageDomino;
            }
            else LastImageDomino[direction] = imageDomino;

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

        private void CreateHelpDomino(Domino domino, DirectionMove direction, Action<DirectionMove> continueMove)
        {
            if (!gameField.CheckDominoForCorrectMove(domino, direction)) return;

            var help = CreateHelpImage(domino, direction, continueMove);
            helpDomino[direction].Add(help);
            table.Children.Add(help);
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

        private ImageDomino CreateHelpImage(Domino domino, DirectionMove direction, Action<DirectionMove> continueMove)
        {
            var imageDomino = new ImageDomino(domino);
            imageDomino.InitializeComponent(direction, LastImageDomino[direction]);

            imageDomino.Opacity = 0.5;
            imageDomino.MouseLeftButtonUp += (s, e) => continueMove(direction);
            imageDomino.MouseLeftButtonUp += (s, e) => ClearAllHelpDomino();
            imageDomino.MouseEnter += (s, e) => imageDomino.Opacity = 0.75;
            imageDomino.MouseLeave += (s, e) => imageDomino.Opacity = 0.5;

            return imageDomino;
        }
    }
}
