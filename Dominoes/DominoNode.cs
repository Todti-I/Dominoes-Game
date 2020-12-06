using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    public class DominoNode
    { 
        public Domino Domino { get; }
        public int OpenValue { get; }
        public int CloseValue { get; }
        public DominoNode NextDomino { get; private set; }

        public DominoNode(Domino domino, int closeValue, int openValue)
        {
            Domino = domino;
            CloseValue = closeValue;
            OpenValue = openValue;
        }

        public void SetNext(Domino domino)
        {
            if (!domino.Contains(OpenValue)) 
                throw new Exception("Ни одно значение не совпадает");

            if (domino.FirstValue == OpenValue)
                NextDomino = new DominoNode(domino, OpenValue, domino.SecondValue);
            else if (domino.SecondValue == OpenValue)
                NextDomino = new DominoNode(domino, OpenValue, domino.FirstValue);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (CloseValue != -1)
                sb.Append($"{CloseValue} ");
            sb.Append($"{OpenValue} -> {NextDomino}");
            return sb.ToString();
        }
    }
}
