using System;
using System.Collections.Generic;
using System.Text;

namespace CoincheServer
{
    class Card
    {
        public Card(char t, char n, int p)
        {
            this.Type = t;
            this.Number = n;
            this.Power = p;
        }

        public char Type { get; set; }

        public char Number { get; set; }

        public int Power { get; set; }

    }
}
