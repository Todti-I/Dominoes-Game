using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    public class GameField
    {
        private readonly List<Player> players = new List<Player>();
        private int currentPlayerId;
        private readonly List<Domino> pool;

        private bool IsFirstMove = true;

        public IReadOnlyList<Player> Players { get => players.AsReadOnly(); }     
        public Player CurrentPlayer { get => Players[currentPlayerId]; }
        public IReadOnlyList<Domino> Pool { get => pool.AsReadOnly(); }

        public DominoRoot Root { get; private set; }
        public DominoNode LastLeft { get;  private set; }
        public DominoNode LastRight { get; private set; }

        public event Action EventStartTurn;
        public event Action<Domino, DirectionMove> EventSetDomino;
        public event Action EventGetDomino;
        public event Action EventEndTurn;
        public event Action<GameState> EventGameOver;

        public GameField()
        {
            pool = Generator.CreatePool();
            players.Add(new Player("PLAYER", Generator.CreateDeckPlayer(pool, 7), this));
            for (var i = 0; i < 1; i++)
                players.Add(new Bot($"BOT_{i}", Generator.CreateDeckPlayer(pool, 7), this));
            currentPlayerId = 0;

            EventStartTurn?.Invoke();
        }

        public MoveState SetDomino(Domino domino, DirectionMove direction)
        {
            if (IsFirstMove)
            {
                IsFirstMove = false;
                Root = new DominoRoot(domino);
                LastLeft = Root.Left;
                LastRight = Root.Right;
            }
            else if (direction == DirectionMove.Left && domino.Contains(LastLeft.OpenValue))
            {
                LastLeft.SetNext(domino);
                LastLeft = LastLeft.NextDomino;
            }
            else if (direction == DirectionMove.Right && domino.Contains(LastRight.OpenValue))
            {
                LastRight.SetNext(domino);
                LastRight = LastRight.NextDomino;
            }
            else return MoveState.Cancel;
            EventSetDomino?.Invoke(domino, direction);
            return MoveState.Successful;
        }

        public Domino GetDominoFromPool()
        {
            var id = Generator.Random.Next(0, pool.Count);
            var domino = pool[id];
            pool.RemoveAt(id);
            EventGetDomino?.Invoke();
            return domino;
        }

        public void EndMove(Player player)
        {
            if (CanMakeMove(player))
            {
                if (CheckGameState() != GameState.Progress)
                    return;
                EventEndTurn?.Invoke();
                NextPlayer();
            }
        }

        public bool CanMakeMove(Player player)
        {
            return player == CurrentPlayer;
        }

        public bool CanTakeDomino(Player player)
        {
            return !IsFirstMove && CanMakeMove(player) && pool.Count != 0 && CheckPlayerForNoMoves(CurrentPlayer);
        }

        public GameState CheckGameState()
        {
            if (pool.Count == 0 && Players.All(p => CheckPlayerForNoMoves(p)))
            {
                EventGameOver?.Invoke(GameState.Draw);
                return GameState.Draw;
            }
            if (Players.Any(p => p.Deck.Count == 0))
            {
                EventGameOver?.Invoke(GameState.Win);
                return GameState.Win;
            }
            return GameState.Progress;
        }

        public Player GetWinner()
        {
            foreach (var player in Players)
            {
                if (player.Deck.Count == 0)
                    return player;
            }
            return null;
        }

        public int[] GetIdOfCorrectMoves(Player player, DirectionMove direction)
        {
            var result = new List<int>();
            for (var i = 0; i < player.Deck.Count; i++)
            {
                if (CheckDominoForCorrectMove(player.Deck[i], direction))
                    result.Add(i);    
            }
            return result.ToArray();
        }

        public bool CheckPlayerForNoMoves(Player player)
        {
            //if (IsFirstMove) return false;
            foreach (var domino in player.Deck)
            {
                if (CheckDominoForCorrectMove(domino, DirectionMove.Left)
                    || CheckDominoForCorrectMove(domino, DirectionMove.Right))
                    return false;
            }
            return true;
        }

        public bool CheckDominoForCorrectMove(Domino domino, DirectionMove direction)
        {
            if (IsFirstMove)
                return domino.FirstValue == domino.SecondValue;

            if (direction == DirectionMove.Left && domino.Contains(LastLeft.OpenValue))
                return domino.Contains(LastLeft.OpenValue);
            else if (direction == DirectionMove.Right && domino.Contains(LastRight.OpenValue))
                return domino.Contains(LastRight.OpenValue);

            return false;
        }

        private void NextPlayer()
        {
            if (currentPlayerId + 1 >= Players.Count)
                currentPlayerId = 0;
            else currentPlayerId++;

            EventStartTurn?.Invoke();
        }
    }
}
