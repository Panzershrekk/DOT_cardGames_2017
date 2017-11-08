using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using DotNetty.Common.Internal.Logging;

namespace CoincheServer
{
    class PokerManager
    {
        //SETUP

        private static Random _rng = new Random();
        private List<Card> _deck;
        private List<Card> _board;
        private List<Player> _players;
        private bool _gameIsSetup;

        //GAME
        private int _lilBlind;
        private int _bigBlind;
        private int _turn;
        private int _played;
        private int _currentlyPlaying;
        private int _coinOnBoard;
        private int _maxBet;

        public PokerManager()
        {
            this.Player = 0;
            this.IsGameStarted = false;
            this._players = new List<Player>();
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

            this._board = new List<Card>();
            this._gameIsSetup = false;
            Shuffle(this._deck);

            //SETUP IS OVER

            this._lilBlind = 5;
            this._bigBlind = 10;
            this._turn = 1;
            this._played = 0;
            this._currentlyPlaying = 1;
            this._coinOnBoard = 0;
            this._maxBet = 10;
        }

        public static void Shuffle<T>(IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = _rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public void SetupGame()
        {
            var i = 0;
            var j = 1;

            while (i != 5)
            {
                this._board.Add(new Card(this._deck[i].Type, this._deck[i].Number, this._deck[i].Power));
                i++;
            }
            while (j <= this.Player)
            {
                var tmp = 0;

                tmp = i + 2;
                while (i != tmp)
                {
                    this._players[j - 1]
                        .AddCard(new Card(this._deck[i].Type, this._deck[i].Number, this._deck[i].Power));
                    i++;
                }
                j++;
            }
            this._players[_currentlyPlaying - 1].Coin -= 5;
            this._players[NextPlayer() - 1].Coin -= 10;
            this._coinOnBoard += 15;
            this._gameIsSetup = true;
        }

        public string launchPoker(string msg, string channelId)
        {
            if (this._gameIsSetup == false)
                SetupGame();
            if (string.Equals(msg, "BOARD"))
                return (AffBoard());
            if (string.Equals(msg, "HAND"))
                return AffPlayerHand(channelId);
            if (string.Equals(msg, "COIN"))
                return (AffCoin(channelId));
            if (string.Equals(msg, "MAXBET"))
                return (AffMaxBet());
            if (string.Equals(msg, "COINBOARD"))
                return (AffCoinOnBoard());
            if (CurrentPlayerIsGood(channelId) == true)
            {
                if (string.Equals(msg, "PASS"))
                {
                    var current = this._currentlyPlaying;

                    this._players[this._currentlyPlaying - 1].HasPassed = true;
                    RotatePlayer();
                    if (this._played == PlayerInGame())
                    {
                        this._played = 0;
                        this._turn += 1;
                        return ("ACTION: Player " + current + " passed. Starting turn " + this._turn + "\r\n");
                    }
                    return ("ACTION: Player " + current + " passed\r\n");
                }
                if (msg.StartsWith("BET") && msg.Length <= 8)
                    return (CheckBet(msg, channelId));
            }
            else
                return ("INFO: Sorry this is not your turn player " + this._currentlyPlaying +
                        " is currently playing\r\n");
            return ("INFO: THE GAME IS HERE\r\n");
        }

        public string AffMaxBet()
        {
            return ("INFO: The current maximum bet is " + this._maxBet + "\r\n");
        }

        public string AffCoinOnBoard()
        {
            return ("INFO: The number of coin on board is " + this._coinOnBoard + "\r\n");
        }

        public string CheckBet(string msg, string chanId)
        {
            var i = 4;
            var betValue = "";

            while (i != msg.Length)
            {
                betValue += msg[i];
                i++;
            }
            if (int.Parse(betValue) >= _maxBet && GetPlayerById(chanId).Coin >= int.Parse((betValue)))
            {
                var current = this._currentlyPlaying;

                GetPlayerById(chanId).Coin -= int.Parse(betValue);
                this._coinOnBoard += int.Parse(betValue);
                this._maxBet = int.Parse(betValue);
                RotatePlayer();
                this._played += 1;
                if (this._played == PlayerInGame())
                {
                    this._turn += 1;
                    this._played = 0;
                    return ("ACTION: Player " + current + " bet value" + betValue + ". Starting turn" + this._turn +
                            "\r\n");
                }
                return ("ACTION: Player " + current + " bet value" + betValue + "\r\n");
            }
            return ("INFO: Your bet is invalid\r\n");
        }

        public void PrintDeck()
        {
            foreach (var c in this._deck)
            {
                Console.WriteLine(c.Type + " " + c.Number);
            }
        }

        public string AffPlayerHand(string channelId)
        {
            var hand = "";

            foreach (var p in this._players)
            {
                if (string.Equals(p.ChannelId, channelId))
                    hand += p.RetHand();
            }
            return ("INFO: " + hand + "\r\n");
        }

        public string AffCoin(string channelId)
        {
            var coin = this._players.Where(p => string.Equals(p.ChannelId, channelId)).Aggregate("", (current, p) => current + p.Coin);
            return ("INFO: " + coin + "\r\n");
        }

        public string AffBoard()
        {
            var boardInfo = "";
            var i = 0;

            while (i != (2 + this._turn))
            {
                boardInfo += this._board[i].Type.ToString() + this._board[i].Number.ToString() + " ";
                i++;
            }
            return ("INFO: " + boardInfo + "\r\n");
        }

        public int NextPlayer()
        {
            if (this._currentlyPlaying + 1 == this.Player)
                return (1);
            return (this._currentlyPlaying + 1);
        }

        public bool CurrentPlayerIsGood(string chanId)
        {
            foreach (var p in this._players)
            {
                if (p.PlayerNbr == this._currentlyPlaying && string.Equals(p.ChannelId, chanId))
                    return (true);
            }
            return (false);
        }

        public void RotatePlayer()
        {
            if (this._currentlyPlaying == this._players.Count)
                this._currentlyPlaying = 1;
            else
                this._currentlyPlaying++;

            if (this._players[this._currentlyPlaying - 1].HasPassed != true) return;
            while (this._players[this._currentlyPlaying - 1].HasPassed == true)
            {
                if (this._currentlyPlaying == this._players.Count)
                    this._currentlyPlaying = 1;
                else
                    this._currentlyPlaying++;
            }
        }

        /*public int CheckWinner()
        {
            
        }*/

        public Player GetPlayerById(string chanId)
        {
            foreach (var p in this._players)
            {
                if (p.ChannelId.Equals(chanId))
                    return (p);
            }
            return (null);
        }

        public int PlayerInGame()
        {
            int i = 0;

            foreach (var p in this._players)
            {
                if (p.HasPassed == false)
                    i++;
            }
            return (i);
        }

        public bool IsGameStarted { get; set; }

        public int Player { get; set; }

        public void AddPlayer(int pn, string chanId)
        {
            this._players.Add(new Player(pn, chanId));
        }
    }
}
