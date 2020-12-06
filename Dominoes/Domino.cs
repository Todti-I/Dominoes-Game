using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    public class Domino
    {
        public int FirstValue { get; }
        public int SecondValue { get; }

        public Domino(int firstValue, int secondValue)
        {
            FirstValue = firstValue;
            SecondValue = secondValue;
        }

        public bool Contains(int value)
        {
            return FirstValue == value || SecondValue == value;
        }

        public override string ToString()
        {
            return $"{FirstValue} {SecondValue}";
        }
    }
}
