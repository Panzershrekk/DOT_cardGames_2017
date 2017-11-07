using System;
using System.Collections.Generic;
using System.Text;

namespace CoincheServer
{
    class Card
    {
        private char _type;
        private char _number;
        private int _power;

        public Card(char t, char n, int p)
        {
            this._type = t;
            this._number = n;
            this._power = p;
        }

        public char Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public char Number
        {
            get { return _number; }
            set { _number = value; }
        }

        public int Power
        {
            get { return _power; }
            set { _power = value; }
        }
    }
}
