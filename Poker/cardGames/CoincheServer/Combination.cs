using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CoincheServer
{
    public class Combination
    {
        public Combination()
        {
            this.Power = 0;
        }

        public int CheckAllComb(List<Card> board, List<Card> playerHand)
        {
            Console.WriteLine("---------------newwwwwwww---------------");
            Power = 0;

            if (CheckRoyalFlush(board, playerHand) != 0)
                return (9000);
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
                    if (firstcard.Number != secondcard.Number || firstcard.Type != secondcard.Type)
                    {
                        if (firstcard.Number.Equals(secondcard.Number))
                            duplicate++;
                        if (duplicate == 2)
                        {
                            if (Power < firstcard.Power)
                                Power = firstcard.Power;
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
            var cardIt = 0;
            var allCard = new List<Card>(board);
            allCard.AddRange(new List<Card>(playerHand));
            List<Card> SortedList = allCard.OrderBy(o => o.Power).ToList();
            SortedList.AddRange(SortedList);
            while (cardIt != 7)
            {
                if (SortedList[cardIt + 1].Power == SortedList[cardIt].Power + 1 &&
                    SortedList[cardIt + 2].Power == SortedList[cardIt].Power + 2 &&
                    SortedList[cardIt + 3].Power == SortedList[cardIt].Power + 3 &&
                    SortedList[cardIt + 4].Power == SortedList[cardIt].Power + 4)
                {
                    found = 4;
                    Power = 4;
                }
                cardIt++;
            }
            return (found);
        }

        public int CheckFlush(List<Card> board, List<Card> playerHand) //Couleur
        {
            var found = 0;
            var allCard = new List<Card>(board);
            var heart = 0;
            var spade = 0;
            var diamond = 0;
            var club = 0;

            allCard.AddRange(new List<Card>(playerHand));
            foreach (var firstcard in allCard)
            {
                if (firstcard.Type.Equals('H'))
                    heart++;
                else if (firstcard.Type.Equals('C'))
                    club++;
                else if (firstcard.Type.Equals('D'))
                    diamond++;
                else if (firstcard.Type.Equals('S'))
                    spade++;
                if (diamond >= 5 || club >= 5 || spade >= 5 || heart >= 5)
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
                    if (firstcard.Number != secondcard.Number || firstcard.Type != secondcard.Type)
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

        public int CheckRoyalFlush(List<Card> board, List<Card> playerHand)
        {
            var found = 0;
            var cardIt = 0;
            var allCard = new List<Card>(board);
            allCard.AddRange(new List<Card>(playerHand));
            List<Card> SortedList = allCard.OrderBy(o => o.Power).ToList();
            SortedList.AddRange(SortedList);
            while (cardIt != 3)
            {
                if (SortedList[cardIt].Power == 9 &&
                    SortedList[cardIt + 1].Power == 10 &&
                    SortedList[cardIt + 2].Power == 11 &&
                    SortedList[cardIt + 3].Power == 12 &&
                    SortedList[cardIt + 4].Power == 13 &&
                    SortedList[cardIt + 1].Type == SortedList[cardIt].Type &&
                    SortedList[cardIt + 2].Type == SortedList[cardIt].Type &&
                    SortedList[cardIt + 3].Type == SortedList[cardIt].Type &&
                    SortedList[cardIt + 4].Type == SortedList[cardIt].Type)
                {
                    found = 9000;
                    Power = 9000;
                }
                cardIt++;
            }
            return (found);
        }

        public int Power { get; set; }
    }
}
