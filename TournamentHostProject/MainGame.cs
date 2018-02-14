using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RummyBase;
using PlayingCards;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TournamentHostProject
{


    public enum GameState { PLAY, QUIT, PAUSE, GAMEOVER }

    /// <summary>
    /// Static Class to Add the Randomize to All Lists
    /// </summary>
    internal static class IEnumerableExtensions
    {
        public static IEnumerable<t> Randomize<t>(this IEnumerable<t> target)
        {
            Random r = new Random();
            return target.OrderBy(x => (r.Next()));
        }
    }
    public class PlayerHand
    {
        public int PlayerIndex { get; set; }
        public List<Card> Hand { get; set; }
        public IRummyPlayer Player { get; set; }
    }



    public class MainGame
    {
        private List<IRummyPlayer> _players;

        private long _gameGoal;
        public long CurrentGame { get; internal set; }

        private int _currentPlayerIndex;

        private List<PlayerHand> _playersHands;

        private List<int> _currentTurnMeldID;

        private GameState _currentGameState;

        private CardDeck _currentGameDeck;

        private List<Card> _currentTurnLayoffs;

        private RummyTable _table;

        private Card _currentTurnCardPickedUp;
        private GameState _holdCurrentGameState;
        private bool isPaused;

        public delegate void GameOverDelegate(int indexOfWinningPlayer, int score);
        public event GameOverDelegate GameOver;


        /// <summary>
        /// Call this on the form when you want to start a new game
        /// </summary>
        /// <param name="botsPlaying"></param>
        public MainGame(List<IRummyPlayer> botsPlaying, long GameGoal)
        {
            if (botsPlaying.Count < 1)
                throw new Exception("The minimum amount of players is one");
            if (botsPlaying.Count > 6)
                throw new Exception("The maximum amount of players is six");
            _players = botsPlaying;
            _gameGoal = GameGoal;
            CurrentGame = 0;
            _currentGameState = GameState.PLAY;
            _currentPlayerIndex = 0;
            _currentGameDeck = new CardDeck();
            _currentTurnLayoffs = new List<Card>();
            _playersHands = new List<PlayerHand>();
            _currentTurnMeldID = new List<int>();
            _table = new RummyTable();
            _table.DiscardPile = new List<Card>();
            Init();
        }


        public void TellTheOtherBotsWhatHappened(Card cardBeingDiscarded)
        {
            foreach (var item in _playersHands)
            {
                if (item.Player == _playersHands[_currentPlayerIndex])
                    continue;
                item.Player.EndTurn(_currentPlayerIndex, _currentTurnCardPickedUp, _currentTurnMeldID, _currentTurnLayoffs, cardBeingDiscarded);

            }
            _currentTurnMeldID.Clear();
            _currentTurnLayoffs.Clear();
        }

        internal void Cancel()
        {
            _currentGameState = GameState.QUIT;
        }

        public void PauseGame()
        {
            _holdCurrentGameState = _currentGameState;
            _currentGameState = GameState.PAUSE;
            isPaused = true;
        }

        public void ResumeGame()
        {
            _currentGameState = _holdCurrentGameState;
            isPaused = false;
        }

        public void Discarded(Card cardBeingDiscarded)
        {
            _table.DiscardPile.Add(cardBeingDiscarded);
        }

        public Card PickUpCard(IRummyPlayer sender, CardPiles pile)
        {
            if (sender.PlayerIndex != _currentPlayerIndex)
                throw new Exception("The Player Indexes don't align");
            Card returnitem = null;
            if (pile == CardPiles.Discard)
            {
                _playersHands[sender.PlayerIndex].Hand.Add(_table.DiscardPile[_table.DiscardPile.Count - 1]);
                returnitem = _table.DiscardPile[_table.DiscardPile.Count - 1];
                _table.DiscardPile.RemoveAt(_table.DiscardPile.Count - 1);
                _currentTurnCardPickedUp = returnitem;
            }
            else if (pile == CardPiles.Stock)
            {
                _playersHands[sender.PlayerIndex].Hand.Add(_currentGameDeck.DrawDeck[_currentGameDeck.DrawDeck.Count - 1]);
                returnitem = _currentGameDeck.DrawDeck[_currentGameDeck.DrawDeck.Count - 1];
                _currentGameDeck.DrawDeck.RemoveAt(_currentGameDeck.DrawDeck.Count - 1);
                _currentTurnCardPickedUp = null;
            }
            if (returnitem == null)
                throw new Exception("Picking up a card failed");
            return returnitem;

        }

        public void GameOverHappened()
        {
            while (isPaused)
                Thread.Sleep(1000);
            _currentGameState = GameState.GAMEOVER;
        }

        public void StartTournament()
        {

            for (int i = 0; i < _playersHands.Count; i++)
            {
                _playersHands[i].Player.BeginTournament(_gameGoal, _playersHands.Count, i, new UserRummyTable(_table));
            }

            while (_gameGoal > CurrentGame && _currentGameState != GameState.QUIT)
            {
                while (isPaused)
                    Thread.Sleep(1000);
                StartGame();
                while (isPaused)
                    Thread.Sleep(1000);
                GameLoop();

                int score = 0;
                foreach (var item in _playersHands)
                {
                    foreach (var card in item.Hand)
                    {
                        if ((int)card.Pips >= 10)
                            score += 10;
                        else
                            score += (int)card.Pips;
                    }
                }
                _table.Scores[_currentPlayerIndex] += score;
                foreach (var item in _playersHands)
                {
                    item.Player.EndGame(_currentPlayerIndex, score);
                }
                GameOver?.Invoke(_currentPlayerIndex, _table.Scores[_currentPlayerIndex]);
                CleanUpTable();
                Debug.Print(score.ToString());
                while (isPaused)
                    Thread.Sleep(1000);
                Debug.Print(CurrentGame + " is now over");
                CurrentGame++;
                _holdCurrentGameState = GameState.PLAY;
            }
            Debug.Print("All games are done");
        }

        private void CleanUpTable()
        {
            _table.DiscardPile = new List<Card>();
            _table.HandSizes = new List<int>();
            _table.RunMelds = new Dictionary<int, List<Card>>();
            _table.SetMelds = new Dictionary<int, List<Card>>() ;
        }

        private void Init()
        {
            FindOutWhoGoesFirst();
            CreateTable();
        }

        private void CreateTable()
        {
            //Create the table
            _table.DiscardPile = _table.DiscardPile;
            _table.HandSizes = new List<int>();
            _table.Scores = new List<int>();
            foreach (var item in _playersHands)
            {
                _table.HandSizes.Add(FindOutHandSize());
                _table.Scores.Add(0);
            }
            _table.RunMelds = new Dictionary<int, List<Card>>();
            _table.SetMelds = new Dictionary<int, List<Card>>();
        }

        private void DealOutNewHands()
        {
            //Shuffle it
            _currentGameDeck.Shuffle();
            //Find out how many cards need to be dealt
            int handSize = FindOutHandSize();
            //Figure out the hands 
            List<List<Card>> cardsToBeDealt = new List<List<Card>>();
            cardsToBeDealt = _currentGameDeck.PlayerStartingHands(_players.Count, handSize);
            //The top card is the discard
            PutTopCardOfDeckIntoDiscard();
            //Store the hands with the players
            for (int i = 0; i < _playersHands.Count; i++)
            {
                _playersHands[i].Hand = cardsToBeDealt[i];
            }
        }

        private void FindOutWhoGoesFirst()
        {
            //Find out who goes first
            var tmp = new List<IRummyPlayer>();
            foreach (var item in _players.Randomize())
                tmp.Add(item);

            for (int i = 0; i < _players.Count; i++)
            {
                PlayerHand player = new PlayerHand()
                {
                    Player = tmp[i]
                };
                _playersHands.Add(player);
            }

        }

        private void StartGame()
        {
            DealOutNewHands();
            //Deep Cloning
            List<List<Card>> tmp = new List<List<Card>>();
            foreach (PlayerHand item in _playersHands)
            {
                tmp.Add((List<Card>)Clone(item.Hand));
            }
            //Tell each bot thier own index
            for (int i = 0; i < _playersHands.Count; i++)
            {
                _playersHands[i].Player.BeginGame(i, tmp[i]);
            }
            _currentGameState = GameState.PLAY;
            _currentPlayerIndex = 0;
        }

        private void PutTopCardOfDeckIntoDiscard()
        {
            _table.DiscardPile.Add(_currentGameDeck.DrawDeck[_currentGameDeck.DrawDeck.Count - 1]);
            _currentGameDeck.DrawDeck.RemoveAt(_currentGameDeck.DrawDeck.Count - 1);
        }

        private void GameLoop()
        {
            //Main Game LOOP
            while (_currentGameState == GameState.PLAY || _currentGameState == GameState.PAUSE)
            {
                while (isPaused)
                    Thread.Sleep(1000);
                for (int i = 0; i < _playersHands.Count; i++)
                {
                    while (isPaused)
                        Thread.Sleep(1000);
                    _playersHands[i].Player.StartTurn();
                    while (isPaused)
                        Thread.Sleep(1000);
                    UpdateTable();
                    if (_currentGameState == GameState.GAMEOVER)
                        return;
                    _currentPlayerIndex++;
                }
                _currentPlayerIndex = 0;
            }
        }

        private void UpdateTable()
        {
            _table.DiscardPile = _table.DiscardPile;
            var hands = new List<int>();
            foreach (var item in _playersHands)
                hands.Add(item.Hand.Count);
        }


        private int FindOutHandSize()
        {
            //As per the rules found on http://rummy.com/rummyrules.html
            if (_players.Count == 2)
                return 10;
            else if (_players.Count > 2 && _players.Count < 5)
                return 7;
            else //the players are 5 or 6
                return 6;
        }

        private object Clone(object clone)
        {
            MemoryStream m = new MemoryStream();
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(m, clone);
            m.Position = 0;
            return b.Deserialize(m);
        }

        public void SetMelds(IRummyPlayer sender, List<Card> cardsInMeld)
        {
            if (sender.PlayerIndex != _currentPlayerIndex)
                throw new Exception("The Player Indexes don't align");

            Pips number = Pips.Joker;

            //check to see if the cards are in the senders hand
            foreach (var item in cardsInMeld)
            {
                if (_playersHands[_currentPlayerIndex].Hand.Contains(item) == false)
                    throw new Exception("The card wasn't in the players hand");
                else
                    _playersHands[_currentPlayerIndex].Hand.Remove(item);

                //Check to see if the SET MELDS are valid
                if (number == Pips.Joker)
                    number = item.Pips;
                else if (item.Pips != number)
                    throw new Exception("Wasn't a valid SET MELD");
            }

            //Add it to the table
            _table.SetMelds.Add(_table.SetMelds.Count, cardsInMeld);
            _currentTurnMeldID.Add(_table.SetMelds.Count);
        }


        public void RunMelds(IRummyPlayer sender, List<Card> cardsInMeld)
        {
            if (sender.PlayerIndex != _currentPlayerIndex)
                throw new Exception("The Player Indexes don't align");

            Suit suit = Suit.None;
            var runMeld = new List<int>();

            //check to see if the cards are in the senders hand
            foreach (var item in cardsInMeld)
            {
                if (_playersHands[_currentPlayerIndex].Hand.Contains(item) == false)
                    throw new Exception("The card wasn't in the players hand");
                else
                    _playersHands[_currentPlayerIndex].Hand.Remove(item);

                if (suit == Suit.None)
                    suit = item.Suit;
                else if (suit != item.Suit)
                    throw new Exception("The RUN MELD must have the same suit");

                runMeld.Add((int)item.Pips);
            }

            runMeld.Sort();
            for (int i = runMeld[0]; i < runMeld.Count; i++)
            {
                if (i != runMeld[i])
                    throw new Exception("The RUN MELD wasn't consective");
            }
            //Add it to the table
            _table.RunMelds.Add(_table.SetMelds.Count, cardsInMeld);
        }

        public void TurnLayoff(IRummyPlayer sender, int meldID, Card cardToAdd)
        {
            if (sender.PlayerIndex != _currentPlayerIndex)
                throw new Exception("The Player Indexes don't align");

            bool invalidCard = false;

            foreach (Card card in _table.SetMelds[meldID])
            {
                if (card.Pips != cardToAdd.Pips)
                {
                    invalidCard = true;
                    break;
                }
            }

            foreach (Card card in _table.RunMelds[meldID])
            {
                if (card.Suit != cardToAdd.Suit)
                {
                    invalidCard = true;
                    break;
                }
            }

            var numbers = new List<int>();
            foreach (Card card in _table.RunMelds[meldID])
                numbers.Add((int)card.Pips);
            numbers.Sort();

            if ((numbers[0] - 1) == (int)cardToAdd.Pips)
                invalidCard = false;
            else if ((numbers[numbers.Count - 1] + 1) == (int)cardToAdd.Pips)
                invalidCard = false;

            if (invalidCard)
                throw new Exception("The card is not aloud in any set in the table");

            //Remove the caard from the hand
            _playersHands[_currentPlayerIndex].Hand.Remove(cardToAdd);
            _currentTurnLayoffs.Add(cardToAdd);
        }
    }
}