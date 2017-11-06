using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CoincheServer
{
    class PokerManager
    {
        private static Random rng = new Random();
        private List<Card> _deck;
        private bool _isGameStarted;
        private int _player;

        public PokerManager()
        {
            this._player = 0;
            this._isGameStarted = false;
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


            Shuffle(this._deck);
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

        public void launchPoker()
        {
            printDeck();
        }

        public void printDeck()
        {
            foreach (Card c in this._deck)
            {
                Console.WriteLine(c.Type + " " + c.Number);
            }
        }

        public bool IsGameStarted
        {
            get { return _isGameStarted; }
            set { _isGameStarted = value; }
        }

        public int Player
        {
            get { return _player; }
            set { _player = value; }
        }
    }
}
