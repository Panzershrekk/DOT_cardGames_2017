using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
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
            Console.WriteLine("---------------newwwwwwww---------------");
            Power = 0;
            if (CheckFourOfAKind(board, playerHand) != 0)
                return (7);
            if (CheckFull(board, playerHand) != 0)
                return (6);
            if (CheckFlush(board, playerHand) != 0)
                return (5);
            if (CheckStraight(board, playerHand) != 0)
                return (4);
            if (CheckThreeOfAKind(board, playerHand) != 0)
                return (3);
            if (CheckDoublePair(board, playerHand) != 0)
                return (2);
            if (CheckPair(board, playerHand) != 0)
                return (1);
            return (0);
        }

        public int CheckPair(List<Card> board, List<Card> playerHand)
        {
            var found = 0;
            var allCard = new List<Card>(board);
            if (playerHand != null)
                allCard.AddRange(new List<Card>(playerHand));
            foreach (var firstcard in allCard)
            {
                foreach (var secondcard in allCard)
                {
                    if (firstcard.Number != secondcard.Number || firstcard.Type != secondcard.Type)
                    {
                        if (firstcard.Number == secondcard.Number)
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

        public int CheckDoublePair(List<Card> board, List<Card> playerHand)
        {
            var found = 0;
            var allCard = new List<Card>(board);
            allCard.AddRange(new List<Card>(playerHand));
            foreach (var firstcard in allCard)
            {
                foreach (var secondcard in allCard)
                {
                    if (firstcard.Number != secondcard.Number || firstcard.Type != secondcard.Type)
                    {
                        if (firstcard.Number == secondcard.Number)
                        {
                            List<Card> c = new List<Card>(allCard);
                            c.Remove(firstcard);
                            c.Remove(secondcard);
                            if (CheckPair(c, null) != 0)
                                found = 2;
                        }
                    }
                }
            }
            return (found);
        }

        public int CheckFull(List<Card> board, List<Card> playerHand)
        {
            var found = 0;
            var allCard = new List<Card>(board);
            allCard.AddRange(new List<Card>(playerHand));
            foreach (var firstcard in allCard)
            {
                foreach (var secondcard in allCard)
                {
                    if (firstcard.Number != secondcard.Number || firstcard.Type != secondcard.Type)
                    {
                        if (firstcard.Number == secondcard.Number)
                        {
                            List<Card> c = new List<Card>(allCard);
                            c.Remove(firstcard);
                            c.Remove(secondcard);
                            if (CheckThreeOfAKind(c, null) != 0)
                                found = 2;
                        }
                    }
                }
            }
            return (found);
        }

        public int CheckThreeOfAKind(List<Card> board, List<Card> playerHand)
        {
            var found = 0;
            var allCard = new List<Card>(board);
            if (playerHand != null)
                allCard.AddRange(new List<Card>(playerHand));
            foreach (var firstcard in allCard)
            {
                var duplicate = 0;
                foreach (var secondcard in allCard)
                {
                    if (firstcard.Number != secondcard.Number && firstcard.Type != secondcard.Type)
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
            allCard.Clear();
            return (found);
        }

        public int CheckStraight(List<Card> board, List<Card> playerHand) // Suite
        {
            var found = 0;
            var allCard = new List<Card>(board);
            allCard.AddRange(new List<Card>(playerHand));
            List<Card> SortedList = allCard.OrderBy(o => o.Power).ToList();
            foreach (var c in SortedList)
            {
                c.Info();
            }
            /*foreach (var firstcard in allCard)
            {
                var duplicate = 0;
                foreach (var secondcard in allCard)
                {
                    if (firstcard.Number != secondcard.Number && firstcard.Type != secondcard.Type)
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
            allCard.Clear();*/
            return (found);
        }

        public int CheckFlush(List<Card> board, List<Card> playerHand) //Couleur
        {
            var found = 0;
            var allCard = new List<Card>(board);
            allCard.AddRange(new List<Card>(playerHand));
            foreach (var firstcard in allCard)
            {
                var heart = 0;
                var spade = 0;
                var diamond = 0;
                var club = 0;

                switch (firstcard.Type)
                {
                    case 'H':
                        heart++;
                        break;
                    case 'C':
                        club++;
                        break;
                    case 'D':
                        diamond++;
                        break;
                    case 'S':
                        spade++;
                        break;
                }
                if (diamond == 5 || club == 5 || spade == 5 || heart == 5)
                {
                    found = 5;
                    Power = 5;
                }
            }
            return (found);
        }

        public int CheckFourOfAKind(List<Card> board, List<Card> playerHand)
        {
            var found = 0;
            var allCard = new List<Card>(board);
            allCard.AddRange(new List<Card>(playerHand));
            foreach (var firstcard in allCard)
            {
                var duplicate = 0;
                foreach (var secondcard in allCard)
                {
                    if (firstcard.Number != secondcard.Number && firstcard.Type != secondcard.Type)
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
