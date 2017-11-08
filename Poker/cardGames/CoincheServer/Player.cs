using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoincheServer
{
    class Player
    {
        public Player(int pn, string channel)
        {
            this.PlayerNbr = pn;
            this.ChannelId = channel;
            this.Hand = new List<Card>();
            this.Coin = 400;
            this.HasPassed = false;
        }

        public int Coin { get; set; }

        public int PlayerNbr { get; set; }

        public string ChannelId { get; set; }

        public List<Card> Hand { get; set; }

        public bool HasPassed { get; set; }

        public void AddCard(Card c)
        {
           this.Hand.Add(c);
        }

        public string RetHand()
        {
            var cards = this.Hand.Aggregate("", (current, c) => current + (c.Type.ToString() + c.Number.ToString() + " "));

            return (cards);
        }
    }
}
