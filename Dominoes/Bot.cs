using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    public class Bot : Player
    {
        public Bot(string name, List<Domino> deck, GameField gameField)
            : base(name, deck, gameField) { }

        public void MakeMove()
        {
            if (!gameField.CanMakeMove(this)) return;
            var idsLeft = gameField.GetIdOfCorrectMoves(this, DirectionMove.Left);
            var idsRight = gameField.GetIdOfCorrectMoves(this, DirectionMove.Right);
            if (idsLeft.Length == 0 && idsRight.Length == 0)
                return;

            if (idsLeft.Length == 0 || Generator.Random.Next(0, 100) >= 50 && idsRight.Length != 0)
            {
                var randomId = idsRight[Generator.Random.Next(0, idsRight.Length)];
                if (gameField.CheckDominoForCorrectMove(Deck[randomId], DirectionMove.Right))
                    MakeMove(Deck[randomId], DirectionMove.Right);
                else throw new Exception("Something went wrong!");
            }
            else
            {
                var randomId = idsLeft[Generator.Random.Next(0, idsLeft.Length)];
                if (gameField.CheckDominoForCorrectMove(Deck[randomId], DirectionMove.Left))
                    MakeMove(Deck[randomId], DirectionMove.Left);
                else throw new Exception("Something went wrong!");
            }
            gameField.EndMove(this);
            return;
        }
    }
}
