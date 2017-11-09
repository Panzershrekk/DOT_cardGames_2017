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
            this.Coin = 1500;
            this.HasPassed = false;
            this.Lost = false;
        }

        public int Coin { get; set; }

        public int PlayerNbr { get; set; }

        public string ChannelId { get; set; }

        public List<Card> Hand { get; set; }

        public bool HasPassed { get; set; }

        public bool Lost { get; set; }


        public void AddCard(Card c)
        {
           this.Hand.Add(c);
        }

        public void ClearHand()
        {
            this.Hand.Clear();
        }

        public string RetHand()
        {
            var cards = this.Hand.Aggregate("", (current, c) => current + (c.Type.ToString() + c.Number.ToString() + " "));

            return (cards);
        }
    }
}
