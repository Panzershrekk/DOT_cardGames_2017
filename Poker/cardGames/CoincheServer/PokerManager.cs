using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using DotNetty.Common.Internal.Logging;
using DotNetty.Common.Utilities;

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
            Player = 0;
            IsGameStarted = false;
            _players = new List<Player>();
            _deck = new List<Card>();
            _deck.Add(new Card('S', '1', 13));
            _deck.Add(new Card('S', '2', 1));
            _deck.Add(new Card('S', '3', 2));
            _deck.Add(new Card('S', '4', 3));
            _deck.Add(new Card('S', '5', 4));
            _deck.Add(new Card('S', '6', 5));
            _deck.Add(new Card('S', '7', 6));
            _deck.Add(new Card('S', '8', 7));
            _deck.Add(new Card('S', '9', 8));
            _deck.Add(new Card('S', 'X', 9));
            _deck.Add(new Card('S', 'J', 10));
            _deck.Add(new Card('S', 'Q', 11));
            _deck.Add(new Card('S', 'K', 12));

            _deck.Add(new Card('H', '1', 13));
            _deck.Add(new Card('H', '2', 1));
            _deck.Add(new Card('H', '3', 2));
            _deck.Add(new Card('H', '4', 3));
            _deck.Add(new Card('H', '5', 4));
            _deck.Add(new Card('H', '6', 5));
            _deck.Add(new Card('H', '7', 6));
            _deck.Add(new Card('H', '8', 7));
            _deck.Add(new Card('H', '9', 8));
            _deck.Add(new Card('H', 'X', 9));
            _deck.Add(new Card('H', 'J', 10));
            _deck.Add(new Card('H', 'Q', 11));
            _deck.Add(new Card('H', 'K', 12));

            _deck.Add(new Card('D', '1', 13));
            _deck.Add(new Card('D', '2', 1));
            _deck.Add(new Card('D', '3', 2));
            _deck.Add(new Card('D', '4', 3));
            _deck.Add(new Card('D', '5', 4));
            _deck.Add(new Card('D', '6', 5));
            _deck.Add(new Card('D', '7', 6));
            _deck.Add(new Card('D', '8', 7));
            _deck.Add(new Card('D', '9', 8));
            _deck.Add(new Card('D', 'X', 9));
            _deck.Add(new Card('D', 'J', 10));
            _deck.Add(new Card('D', 'Q', 11));
            _deck.Add(new Card('D', 'K', 12));

            _deck.Add(new Card('C', '1', 13));
            _deck.Add(new Card('C', '2', 1));
            _deck.Add(new Card('C', '3', 2));
            _deck.Add(new Card('C', '4', 3));
            _deck.Add(new Card('C', '5', 4));
            _deck.Add(new Card('C', '6', 5));
            _deck.Add(new Card('C', '7', 6));
            _deck.Add(new Card('C', '8', 7));
            _deck.Add(new Card('C', '9', 8));
            _deck.Add(new Card('C', 'X', 9));
            _deck.Add(new Card('C', 'J', 10));
            _deck.Add(new Card('C', 'Q', 11));
            _deck.Add(new Card('C', 'K', 12));

            _board = new List<Card>();
            _gameIsSetup = false;

            //SETUP IS OVER

            _lilBlind = 5;
            _bigBlind = 10;
            _turn = 1;
            _played = 0;
            _currentlyPlaying = 1;
            _coinOnBoard = 0;
            _maxBet = 10;
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

            Shuffle(_deck);
            _board.Clear();
            while (i != 5)
            {
                _board.Add(new Card(_deck[i].Type, _deck[i].Number, _deck[i].Power));
                i++;
            }
            while (j <= Player)
            {
                var tmp = 0;

                tmp = i + 2;
                _players[j - 1].ClearHand();
                while (i != tmp)
                {
                    _players[j - 1]
                        .AddCard(new Card(_deck[i].Type, _deck[i].Number, _deck[i].Power));
                    i++;
                }
                j++;
            }
            ResetPlayer();
            _coinOnBoard = 0;
            _maxBet = 10;
            _currentlyPlaying = 1;
            _players[_currentlyPlaying - 1].Coin -= 5;
            _players[NextPlayer() - 1].Coin -= 10;
            _coinOnBoard += 15;
            _gameIsSetup = true;
        }

        public string LaunchPoker(string msg, string channelId)
        {
            if (_gameIsSetup == false)
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
                    var current = _currentlyPlaying;

                    _players[_currentlyPlaying - 1].HasPassed = true;
                    RotatePlayer();
                    if (PlayerInGame() == 1)
                    {
                        _played = 0;
                        _turn += 1;
                        var winner = CheckLastPlayer();
                        _turn = 1;
                        SetupGame();
                        return ("ACTION: Player " + current + " passed " + "\nWinner is " + winner +
                                "\nNew round just started" + "\r\n");
                    }
                    if (_played == PlayerInGame())
                    {
                        _played = 0;
                        _turn += 1;
                        if (_turn > 3)
                        {
                            var winner = CheckWinner();
                            _turn = 1;
                            SetupGame();
                            return ("ACTION: Player " + current + " passed " + "\n" + winner +
                                    "\nNew round just started" + "\r\n");
                        }
                        return ("ACTION: Player " + current + " passed. Starting turn " + _turn + "\r\n");
                    }
                    return ("ACTION: Player " + current + " passed\r\n");
                }
                if (msg.StartsWith("BET") && msg.Length >= 4 &&  msg.Length <= 8)
                    return (CheckBet(msg, channelId));
            }
            else
                return ("INFO: Sorry this is not your turn player " + _currentlyPlaying +
                        " is currently playing\r\n");
            return ("INFO: THE GAME IS HERE\r\n");
        }

        

        public string CheckBet(string msg, string chanId)
        {
            var i = 4;
            var betValue = "";

            while (i != msg.Length)
            {
                if (char.IsDigit(msg[i]) == false)
                    return ("INFO: Your bet is invalid\r\n");
                betValue += msg[i];
                i++;
            }
            if (int.Parse(betValue) >= _maxBet && GetPlayerById(chanId).Coin >= int.Parse((betValue)))
            {
                var current = _currentlyPlaying;

                GetPlayerById(chanId).Coin -= int.Parse(betValue);
                _coinOnBoard += int.Parse(betValue);
                _maxBet = int.Parse(betValue);
                RotatePlayer();
                _played += 1;
                if (_played == PlayerInGame())
                {
                    _turn += 1;
                    _played = 0;
                    if (_turn > 3)
                    {
                        var winner = CheckWinner();
                        _turn = 1;
                        SetupGame();
                        return ("ACTION: Player " + current + " bet value " + betValue + "\n" + winner + "\nNew round just started" + "\r\n");
                    }

                    return ("ACTION: Player " + current + " bet value " + betValue + ". Starting turn " + _turn +
                            "\r\n");
                }
                return ("ACTION: Player " + current + " bet value " + betValue + "\r\n");
            }
            return ("INFO: Your bet is invalid\r\n");
        }

        public void PrintDeck()
        {
            foreach (var c in _deck)
            {
                Console.WriteLine(c.Type + " " + c.Number);
            }
        }

        public string AffPlayerHand(string channelId)
        {
            var hand = "";

            foreach (var p in _players)
            {
                if (string.Equals(p.ChannelId, channelId))
                    hand += p.RetHand();
            }
            return ("INFO: " + hand + "\r\n");
        }

        public string AffCoin(string channelId)
        {
            var coin = _players.Where(p => string.Equals(p.ChannelId, channelId)).Aggregate("", (current, p) => current + p.Coin);
            return ("INFO: " + coin + "\r\n");
        }

        public string AffBoard()
        {
            var boardInfo = "";
            var i = 0;

            while (i != (2 + _turn))
            {
                boardInfo += _board[i].Type.ToString() + _board[i].Number.ToString() + " ";
                i++;
            }
            return ("INFO: " + boardInfo + "\r\n");
        }

        public int NextPlayer()
        {
            if (_currentlyPlaying + 1 == Player)
                return (1);
            return (_currentlyPlaying + 1);
        }

        public bool CurrentPlayerIsGood(string chanId)
        {
            foreach (var p in _players)
            {
                if (p.PlayerNbr == _currentlyPlaying && string.Equals(p.ChannelId, chanId))
                    return (true);
            }
            return (false);
        }

        public void RotatePlayer()
        {
            if (_currentlyPlaying == _players.Count)
                _currentlyPlaying = 1;
            else
                _currentlyPlaying++;

            if (_players[_currentlyPlaying - 1].HasPassed != true  && _players[_currentlyPlaying - 1].HasPassed == true) return;
            while (_players[_currentlyPlaying - 1].HasPassed == true || _players[_currentlyPlaying - 1].Lost == true)
            {
                if (_currentlyPlaying == _players.Count)
                    _currentlyPlaying = 1;
                else
                    _currentlyPlaying++;
            }
        }

        public string CheckWinner()
        {
            var comb = new Combination();
            var currentWinner = 1;
            var maxPower = 0;
            var maxpowerCard = 0;
            Player winner = null;

            foreach (var p in _players)
            {
                if (p.HasPassed == false && p.Lost == false)
                {
                    var power = 0;
                    power = comb.CheckAllComb(_board, p.Hand);
                    //Console.WriteLine("Power for player " + p.PlayerNbr + " is " + power + " and his most powerfull card is " + comb.Power);
            
                    if (power >= 0 && maxPower <= power && maxpowerCard < comb.Power)
                    {
                        maxPower = power;
                        maxpowerCard = comb.Power;
                        currentWinner = p.PlayerNbr;
                        winner = p;
                    }
                }
                    
            }
            winner.Coin += _coinOnBoard;
            return ("And the winner for " + _coinOnBoard + " is player " + currentWinner + "\r\n");
        }

        public Player GetPlayerById(string chanId)
        {
            foreach (var p in _players)
            {
                if (p.ChannelId.Equals(chanId))
                    return (p);
            }
            return (null);
        }

        public int PlayerInGame()
        {
            int i = 0;

            foreach (var p in _players)
            {
                if (p.HasPassed == false)
                    i++;
            }
            return (i);
        }

        public void ResetPlayer()
        {
            foreach (var p in _players)
            {
                p.HasPassed = false;
            }
        }

        public int CheckLastPlayer()
        {
            foreach (var p in _players)
            {
                if (p.HasPassed == false && p.HasPassed == false)
                    return (p.PlayerNbr);
            }
            return (0);
        }

        public string AffMaxBet()
        {
            return ("INFO: The current maximum bet is " + _maxBet + "\r\n");
        }

        public string AffCoinOnBoard()
        {
            return ("INFO: The number of coin on board is " + _coinOnBoard + "\r\n");
        }

        public bool IsGameStarted { get; set; }

        public int Player { get; set; }

        public void AddPlayer(int pn, string chanId)
        {
            _players.Add(new Player(pn, chanId));
        }

    }
}
