using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CoincheServer
{
    class PokerManager
    {
        //SETUP

        private static Random rng = new Random();
        private List<Card> _deck;
        private List<Card> _board;
        private List<Player> _players;
        private bool _isGameStarted;
        private int _player;
        private bool _gameIsSetup;

        //GAME
        private int _lilBlind;
        private int _bigBlind;
        private int _turn;
        private int _currentlyPlaying;

        public PokerManager()
        {
            this._player = 0;
            this._isGameStarted = false;
            this._players = new List<Player>();
            this._deck = new List<Card>();
            this._deck.Add(new Card('S', '1', 13));
            this._deck.Add(new Card('S', '2', 1));
            this._deck.Add(new Card('S', '3', 2));
            this._deck.Add(new Card('S', '4', 3));
            this._deck.Add(new Card('S', '5', 4));
            this._deck.Add(new Card('S', '6', 5));
            this._deck.Add(new Card('S', '7', 6));
            this._deck.Add(new Card('S', '8', 7));
            this._deck.Add(new Card('S', '9', 8));
            this._deck.Add(new Card('S', 'X', 9));
            this._deck.Add(new Card('S', 'J', 10));
            this._deck.Add(new Card('S', 'Q', 11));
            this._deck.Add(new Card('S', 'K', 12));

            this._deck.Add(new Card('H', '1', 13));
            this._deck.Add(new Card('H', '2', 1));
            this._deck.Add(new Card('H', '3', 2));
            this._deck.Add(new Card('H', '4', 3));
            this._deck.Add(new Card('H', '5', 4));
            this._deck.Add(new Card('H', '6', 5));
            this._deck.Add(new Card('H', '7', 6));
            this._deck.Add(new Card('H', '8', 7));
            this._deck.Add(new Card('H', '9', 8));
            this._deck.Add(new Card('H', 'X', 9));
            this._deck.Add(new Card('H', 'J', 10));
            this._deck.Add(new Card('H', 'Q', 11));
            this._deck.Add(new Card('H', 'K', 12));

            this._deck.Add(new Card('D', '1', 13));
            this._deck.Add(new Card('D', '2', 1));
            this._deck.Add(new Card('D', '3', 2));
            this._deck.Add(new Card('D', '4', 3));
            this._deck.Add(new Card('D', '5', 4));
            this._deck.Add(new Card('D', '6', 5));
            this._deck.Add(new Card('D', '7', 6));
            this._deck.Add(new Card('D', '8', 7));
            this._deck.Add(new Card('D', '9', 8));
            this._deck.Add(new Card('D', 'X', 9));
            this._deck.Add(new Card('D', 'J', 10));
            this._deck.Add(new Card('D', 'Q', 11));
            this._deck.Add(new Card('D', 'K', 12));

            this._deck.Add(new Card('C', '1', 13));
            this._deck.Add(new Card('C', '2', 1));
            this._deck.Add(new Card('C', '3', 2));
            this._deck.Add(new Card('C', '4', 3));
            this._deck.Add(new Card('C', '5', 4));
            this._deck.Add(new Card('C', '6', 5));
            this._deck.Add(new Card('C', '7', 6));
            this._deck.Add(new Card('C', '8', 7));
            this._deck.Add(new Card('C', '9', 8));
            this._deck.Add(new Card('C', 'X', 9));
            this._deck.Add(new Card('C', 'J', 10));
            this._deck.Add(new Card('C', 'Q', 11));
            this._deck.Add(new Card('C', 'K', 12));

            this._board = new List<Card>();
            this._gameIsSetup = false;
            Shuffle(this._deck);

            //SETUP IS OVER

            this._lilBlind = 5;
            this._bigBlind = 10;
            this._turn = 1;
            this._currentlyPlaying = 1;
        }

        public static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public void SetupGame()
        {
            int i = 0;
            int j = 1;
            int tmp = 0;

            while (i != 5)
            {
                this._board.Add(new Card(this._deck[i].Type, this._deck[i].Number, this._deck[i].Power));
                i++;
            }
            while (j <= this.Player)
            {
                tmp = i + 2;
                while (i != tmp)
                {
                    this._players[j - 1]
                        .addCard(new Card(this._deck[i].Type, this._deck[i].Number, this._deck[i].Power));
                    i++;
                }
                j++;
            }


            foreach (Card c in this._board)
            {
                Console.WriteLine(c.Type + " " + c.Number);
            }
            this._gameIsSetup = true;
        }

        public string launchPoker(string msg, string channelId)
        {
            if (this._gameIsSetup == false)
                SetupGame();
            if (string.Equals(msg, "BOARD"))
                return (AffBoard());
            if (string.Equals(msg, "HAND"))
                return AffPlayerHand(channelId);
            Console.WriteLine(msg);
            return ("INFO: THE GAME IS HERE\r\n");
        }

        public void printDeck()
        {
            foreach (Card c in this._deck)
            {
                Console.WriteLine(c.Type + " " + c.Number);
            }
        }


        public string AffPlayerHand(string channelId)
        {
            string hand = "";

            foreach (Player p in this._players)
            {
                if (string.Equals(p.ChannelId, channelId))
                    hand += p.retHand();
            }
            return ("INFO: " + hand + "\r\n");
        }

        public string AffBoard()
        {
            string boardInfo = "";
            int i = 0;

            while (i != (2 + this._turn))
            {
                boardInfo += this._board[i].Type.ToString() + this._board[i].Number.ToString() + " ";
                i++;
            }
            return ("INFO: " + boardInfo + "\r\n");
        }

        public int nextPlayer()
        {
            return (1);
        }

        public bool IsGameStarted
        {
            get { return _isGameStarted; }
            set { _isGameStarted = value; }
        }

        public int Player
        {
            get { return _player; }
            set {_player = value;}
        }

        public void AddPlayer(int pn, string chanId)
        {
            this._players.Add(new Player(pn, chanId));
        }
    }
}
