using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayingCards;

namespace RummyBase
{
    public enum CardPiles //this is a little enumeration to assist in a player choosing whether they want their card from the discard pile or the stock pile
    {
        Discard,
        Stock
    }

    public delegate void ChoosePileDelegate(IRummyPlayer sender, CardPiles Pile, out Card DrawCard);
    public delegate void TurnMeldsDelegate(IRummyPlayer sender, List<Card> CardsInMeld);
    public delegate void TurnLayoffDelegate(IRummyPlayer sender, int MeldID, Card CardToAdd);
    public delegate void DoneTurnDelegate(IRummyPlayer sender, Card CardBeingDiscarded, bool OutOfCards = false);

    public interface IRummyPlayer //this is like "the contract" for every Rummy Player bot
    {
        
        //read only properties:
        string Name { get; } //a player should expose a short name to identify themselves in a game against others e.g. GarretV13, Harsh2017
                             //as you make new versions, you may want to update the name each time.



        int PlayerIndex { get; }



        int Score { get; } //the total points in their hand (Ace=1, 2-10 = 2-10, all face cards = 10)  Note that the tournament manager has the ability to know exactly what cards are in a player's hand, so make sure the truthful score is returned or else the bot could be expelled from the tournament!
        void BeginTournament(long GameGoal, int PlayerCount, int PlayerIndex, UserRummyTable T); //each player is notified that the tournament is beginning and some information about the tournament.  They are told how many games are scheduled to be played, how many players are playing (which influences how many cards are dealt to each player) and what their player index is.  Most importantly they are given a reference to the the Rummy Table which they can examine at any time to see:
                                                                                                 // what the "top card" is on the discard pile (in fact the whole discard pile is supplied as a list of cards with the "top card" at the end of the list.  This way each player can see what cards other players have discarded just like they could in a real game.
                                                                                                 // what (if any) run melds and set melds players have put down  These are exposed as two Dictionaries.
                                                                                                 // the number of cards in each person's hand
                                                                                                 //public List<int> HandSizes; //the number of cards in each players hand (according to index)  e.g. HandSizes[0] is how many cards Player 0 has left
                                                                                                 // how many points each player currently has.
                                                                                                 //public List<int> Scores;  //the cumulative score for each player (according to index) e.g. Scores[2] contains how many points Player 2 has earned

        void BeginGame(int StartPlayerIndex, List<Card> StartingHand); //this signifies a new game is starting and gives each player their starting hand
        void StartTurn(); //this signals to the player that it is their turn.  This is where they will do all their decision making and raising events 
        event ChoosePileDelegate ChoosePile; // the player will raise this event at the beginning of their turn to request which pile they want a card from, and the host will return the card in an out parameter
        event TurnMeldsDelegate SetMelds; //optionally raise one or more times during the turn to create new set melds (3 or more cards of the same value)
        event TurnMeldsDelegate RunMelds; //optionally raise one or more times during the turn to create new run melds (at least 3 cards of the same suit that form a run e.g. 5 Hearts, 6 Hearts, 7 Hearts
        event TurnLayoffDelegate TurnLayoff; //optionally raise one or more times during the turn to add a card to an existing meld on the table.  The player should specify the key of the meld that it should be added to, and also pass the card from their hand that they are adding.
        event DoneTurnDelegate DoneTurn; //the player must raise this event to signify that they have completed their turn.  They will pass on which card they are discarding(could be null if they are out of cards), and if they are now out of cards.  If they are out of cards, they are the winner and the tournament host will figure out their score.
        void EndTurn(int TurnPlayerIndex, Card CardTakenFromDiscards, List<int> NewMelds, List<Card> CardsLayedOff, Card DiscardedCard); //the tournament manager will notify each player (including the player that just finished their turn) about what happened during the turn. This is to make it easier for players to follow what other players are doing.  This for information ony and no response is necessary. NewMelds is a list of new meld keys created on this turn, and CardsLayedOff is a list of what cards(if any) the turn player layed off to existing melds.
        void EndGame(int WinnerIndex, int GamePoints); //the tournament manager will notify each player, including the winner that the game is over. It is just for information and the player does not respond (but can clean up from their old hand, etc. if they want)
        
    }
}
//THREE CHANGES 
// End Turn added Discarded Card, and changed the Cards LayedOFF to a list of cards
//Added the property Player Index