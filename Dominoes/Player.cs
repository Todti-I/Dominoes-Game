using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    public class Player
    {
        private readonly List<Domino> deck;

        public IReadOnlyList<Domino> Deck { get => deck.AsReadOnly(); }
        public string Name { get; }

        protected readonly GameField gameField;        

        public Player(string name, List<Domino> deck, GameField gameField)
        {
            Name = name;
            this.deck = deck;
            this.gameField = gameField;
        }

        public void MakeMove(Domino domino, DirectionMove direction)
        {
            var id = deck.IndexOf(domino);
            if (id != -1)
                MakeMove(id, direction);
        }

        public void MakeMove(int id, DirectionMove direction)
        {
            if (Deck.Count == 0 || !gameField.CanMakeMove(this) 
                || !gameField.CheckDominoForCorrectMove(Deck[id], direction))
                return;

            gameField.SetDomino(Deck[id], direction);
            deck.RemoveAt(id);
            EndMove();
        }

        public Domino TakeDomino()
        {
            if (!gameField.CanTakeDomino(this)) 
                return null;

            var domino = gameField.GetDominoFromPool();
            deck.Add(domino);
            return domino;
        }

        public void EndMove()
        {
            gameField.EndMove(this);
        }

        public override string ToString()
        {
            return $"{Name} Count: {Deck.Count}   \n{string.Join(" | ", Deck)}";
        }
    }
}
