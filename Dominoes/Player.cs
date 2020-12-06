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

        public MoveState MakeMove(Domino domino, DirectionMove direction)
        {
            var id = deck.IndexOf(domino);
            if (id != -1)
                return MakeMove(id, direction);
            return MoveState.Cancel;
        }

        public MoveState MakeMove(int id, DirectionMove direction)
        {
            if (Deck.Count == 0 || !gameField.CanMakeMove(this))
                return MoveState.Cancel;

            var state = gameField.SetDomino(Deck[id], direction);

            if (state == MoveState.Cancel)
                return MoveState.Cancel;

            deck.RemoveAt(id);
            EndMove();
            return MoveState.Successful;
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
            return $"{Name} Count: {Deck.Count} \n{string.Join(" | ", Deck)}";
        }
    }
}
