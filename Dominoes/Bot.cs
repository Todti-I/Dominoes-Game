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

        public MoveState MakeMove()  // need rework
        {
            if (!gameField.CanMakeMove(this)) return MoveState.Cancel;
            var idsLeft = gameField.GetIdOfCorrectMoves(this, DirectionMove.Left);
            var idsRight = gameField.GetIdOfCorrectMoves(this, DirectionMove.Right);
            if (idsLeft.Length == 0 && idsRight.Length == 0)
                return MoveState.Cancel;

            if (idsLeft.Length == 0 || Generator.Random.Next(0, 100) >= 50 && idsRight.Length != 0)
            {
                if (MakeMove(idsRight[Generator.Random.Next(0, idsRight.Length)], DirectionMove.Right) == MoveState.Cancel)
                    MakeMove(idsLeft[Generator.Random.Next(0, idsLeft.Length)], DirectionMove.Left);
            }
            else
            {
                if (MakeMove(idsLeft[Generator.Random.Next(0, idsLeft.Length)], DirectionMove.Left) == MoveState.Cancel)
                    MakeMove(idsRight[Generator.Random.Next(0, idsRight.Length)], DirectionMove.Right);
            }

            gameField.EndMove(this);
            return MoveState.Successful;
        }
    }
}
