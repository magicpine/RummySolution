using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RummyBase;
using PlayingCards;
using System.Diagnostics;

namespace RummyBots
{
    public class RummyBot1 : IRummyPlayer
    {
        //private data:
        private string _Name = "testbot1";
        private int _MyPlayerIndex;
        private List<Card> _MyHand;
        private UserRummyTable _RummyTable;
        private long _GameGoal;
        private int _NumberOfPlayers;

        //properties:
        public string Name { get { return _Name; } }
        public int PlayerIndex { get { return _MyPlayerIndex; } }
        public int Score
        {
            get
            {
                int TotScore = 0;
                foreach (Card C in _MyHand)
                {
                    if (C.IsFaceCard)
                        TotScore += 10;
                    else
                        TotScore = (int)C.Pips;
                }
                return TotScore;
            }
        } //the total points in my hand (Ace=1, 2-10 = 2-10, all face cards = 10)  
        //events:
        public event ChoosePileDelegate ChoosePile; // the player will raise this event at the beginning of their turn to request which pile they want a card from, and the host will return the card in an out parameter
        public event TurnMeldsDelegate SetMelds; //optionally raise one or more times during the turn to create new set melds (3 or more cards of the same value)
        public event TurnMeldsDelegate RunMelds; //optionally raise one or more times during the turn to create new run melds (at least 3 cards of the same suit that form a run e.g. 5 Hearts, 6 Hearts, 7 Hearts
        public event TurnLayoffDelegate TurnLayoff; //optionally raise one or more times during the turn to add a card to an existing meld on the table.  The player should specify the key of the meld that it should be added to, and also pass the card from their hand that they are adding.
        public event DoneTurnDelegate DoneTurn; //the player must raise this event to signify that they have completed their turn.  They will pass on which card they are discarding(could be null if they are out of cards), and if they are now out of cards.  If they are out of cards, they are the winner and the tournament host will figure out their score.

        public void BeginTournament(long GameGoal, int PlayerCount, int PlayerIndex, UserRummyTable T)
        {
            _MyPlayerIndex = PlayerIndex;
            _RummyTable = T;
            _GameGoal = GameGoal;
            _NumberOfPlayers = PlayerCount;
            Debug.Print("Bot " + _MyPlayerIndex.ToString() + " Reporting in!");
        }

        public void BeginGame(int StartPlayerIndex, List<Card> StartingHand)
        {
            _MyHand = StartingHand;
        }
        public void StartTurn() //this signals to the player that it is their turn.  This is where they will do all their decision making and raising events 
        {
            //pick up top card from discard pile or stock pile
            CardPiles mypilechoice = CardPiles.Stock;

            //decide if top card discard looks good...
            if (_RummyTable.TopDiscardCard.Suit == Suit.Hearts) //just a silly example of choosing the card if it is hearts...
                mypilechoice = CardPiles.Discard;
            Card NewCard;
            Debug.Print("Bot " + _MyPlayerIndex + " Hand Size is " + _MyHand.Count);
            ChoosePile(this, mypilechoice, out NewCard);
            _MyHand.Add(NewCard);
            Debug.Print("Bot " + _MyPlayerIndex + " Drew a card!");
            Debug.Print("Bot " + _MyPlayerIndex + " Hand Size is " + _MyHand.Count);
            //check out new card...


            //see if we have any melds...


            //see if we can add any cards to existing melds...


            //Choose what card to get rid of...
            Card CardToDiscard;
            if (_MyHand.Count > 0)
            {
                //make intelligent choice for what card to discard...

                CardToDiscard = _MyHand[0]; //... or not that intelligent...
                _MyHand.Remove(CardToDiscard);
            }
            else
                CardToDiscard = null;

            //Notify that we are done our turn ********this is mandatory
            Debug.Print("Bot " + _MyPlayerIndex + " Discarded.  Hand Size is " + _MyHand.Count);

            if (_MyPlayerIndex == 3)
                DoneTurn(this, CardToDiscard, true);
            else
                DoneTurn(this, CardToDiscard, _MyHand.Count == 0);





        }
        public void EndTurn(int TurnPlayerIndex, Card CardTakenFromDiscards, List<int> NewMelds, List<Card> CardsLayedOff, Card DiscardedCard)
        {
            if (TurnPlayerIndex == _MyPlayerIndex) //if it's us we already know all about it
                return;

            //make a few notes...

            return;

        } 
        public void EndGame(int WinnerIndex, int GamePoints)
        //the tournament manager will notify each player, including the winner that the game is over. It is just for information and the player does not respond (but can clean up from their old hand, etc. if they want)
        {


        }

     }
}
