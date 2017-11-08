using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace CoincheServer
{
    class Combination
    {
        public Combination()
        {
            this.Power = 0;
        }

        public int CheckAllComb(List<Card> board, List<Card> playerHand)
        {
            if (CheckFourOfAKind(board, playerHand) != 0)
                return (7);
            if (CheckThreeOfAKind(board, playerHand) != 0)
                return (3);
            if (CheckPair(board, playerHand) != 0)
                return (1);
            return (0);
        }

        public int CheckPair(List<Card> board, List<Card> playerHand)
        {
            var found = 0;
            var allCard = board;
            allCard.AddRange(playerHand);
            foreach (var firstcard in allCard)
            {
                foreach (var secondcard in allCard)
                {
                    if (firstcard.Number != secondcard.Number || firstcard.Type != secondcard.Type)
                    {
                        if (firstcard.Number.Equals(secondcard.Number))
                        {
                            if (this.Power < firstcard.Power)
                                this.Power = firstcard.Power;
                            found = 1;
                        }
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
                        {
                            if (this.Power < firstcard.Power)
                                this.Power = firstcard.Power;
                            found = 3;
                        }
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
                        {
                            if (this.Power < firstcard.Power)
                                this.Power = firstcard.Power;
                            found = 7;
                        }
                    }
                }
            }
            return (found);
        }

        public int Power { get; set; }
    }
}
