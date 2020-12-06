using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    public class DominoRoot
    {
        public Domino Domino { get; }
        public DominoNode Left { get; }
        public DominoNode Right { get; }

        public DominoRoot(Domino domino)
        {
            Domino = domino;
            Left = new DominoNode(domino, -1, domino.FirstValue);
            Right = new DominoNode(domino, -1, domino.SecondValue);
        }
    }
}
