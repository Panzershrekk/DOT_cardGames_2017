using System;
using System.Collections.Generic;
using System.Text;

namespace CoincheServer
{
    class Player
    {
        private int _playerNbr;
        private string _channelId;
        private List<Card> _hand;
        private int _coin;
        private bool _hasPassed;

        public Player(int pn, string channel)
        {
            this._playerNbr = pn;
            this._channelId = channel;
            this._hand = new List<Card>();
            this._coin = 400;
            this._hasPassed = false;
        }

        public int Coin
        {
            get { return _coin; }
            set { _coin = value; }
        }

        public int PlayerNbr
        {
            get { return _playerNbr; }
            set { _playerNbr = value; }
        }

        public string ChannelId
        {
            get { return _channelId; }
            set { _channelId = value; }
        }

        public List<Card> Hand
        {
            get { return _hand; }
            set { _hand = value; }
        }

        public bool HasPassed
        {
            get { return _hasPassed; }
            set { _hasPassed = value; }
        }

        public void addCard(Card c)
        {
           this._hand.Add(c);
        }

        public string retHand()
        {
            string cards = "";

            foreach (Card c in this._hand)
            {
                cards += (c.Type.ToString() + c.Number.ToString() + " ");
            }
            return (cards);
        }
    }
}
