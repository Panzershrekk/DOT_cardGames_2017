using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoincheServer
{
    class Combination
    {
        public Combination()
        {
            
        }

        public int CheckPair(List<Card> board, List< Card> playerHand)
        {
            var found = 0;
            var allCard = board;
            allCard.AddRange(playerHand);
            foreach (var firstcard in allCard)
            {
                foreach (var secondcard in allCard)
                {
                    if (firstcard != secondcard)
                    {
                        if (firstcard.Number.Equals(secondcard.Number))
                            found = 1;
                    }
                }
            }
            return (found);
        }

        public int CheckThreeOfAKind(List<Card> board, List<Card> playerHand)
        {
            var found = 0;
            var allCard = board;
            allCard.AddRange(playerHand);
            foreach (var firstcard in allCard)
            {
                var duplicate = 0;
                foreach (var secondcard in allCard)
                {
                    if (firstcard != secondcard)
                    {
                        if (firstcard.Number.Equals(secondcard.Number))
                            duplicate++;
                        if (duplicate == 2)
                            found = 3;
                    }
                }
            }
            return (found);
        }

        public int CheckFourOfAKind(List<Card> board, List<Card> playerHand)
        {
            var found = 0;
            var allCard = board;
            allCard.AddRange(playerHand);
            foreach (var firstcard in allCard)
            {
                var duplicate = 0;
                foreach (var secondcard in allCard)
                {
                    if (firstcard != secondcard)
                    {
                        if (firstcard.Number.Equals(secondcard.Number))
                            duplicate++;
                        if (duplicate == 3)
                            found = 7;
                    }
                }
            }
            return (found);
        }
    }
}
