using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayingCards
{
    public class CardDeck
    //may want to make the cards themselves as static objects
    {
        public static Random StaticRandom = new Random(); //used to seed individual random number generators since several card decks could get created almost simultaneously when multi threading is happening
        private Random R = new Random(StaticRandom.Next());  //not static so can use from multiple threads

        private List<Card> mDeck = new List<Card>();
        private List<Card> mDrawDeck = new List<Card>();

        #region constructors
        public CardDeck()
        {
            CreateNormalCards();
        }
        public CardDeck(int MultipleDeckCount, bool IncludesJokers = false) //create a deck consisting of multiple normal decks.  Assume no jokers unless requested  
        {
            if (MultipleDeckCount < 1)
                throw new Exception("CardDeck - cannot create a card deck from " + MultipleDeckCount.ToString() + " decks");
            for (int i = 0; i < MultipleDeckCount; i++)
            {
                CreateNormalCards(); //each time through the loop will add a normal deck's worth of cards
                if (IncludesJokers)
                {
                    mDeck.Add(new Card(Suit.None, Pips.Joker));
                    mDeck.Add(new Card(Suit.None, Pips.Joker));
                }
            }

        }
        public CardDeck(bool IncludesJokers)
        {
            CreateNormalCards();
            if (IncludesJokers) //condition must be within normal brackets
            {
                mDeck.Add(new Card(Suit.None, Pips.Joker));
                mDeck.Add(new Card(Suit.None, Pips.Joker));
            }

        }

        private void CreateNormalCards()
        {
            //Card c;
            //c = new Card(Suit.Clubs, Pips.Ace); mCards.Add(c);
            //c = new Card(Suit.Clubs, Pips.Two); mCards.Add(c);
            mDeck.Add(new Card(Suit.Clubs, Pips.Ace));
            mDeck.Add(new Card(Suit.Clubs, Pips.Two));
            mDeck.Add(new Card(Suit.Clubs, (Pips)3));
            mDeck.Add(new Card(Suit.Clubs, (Pips)4));
            mDeck.Add(new Card(Suit.Clubs, (Pips)5));
            mDeck.Add(new Card(Suit.Clubs, (Pips)6));
            mDeck.Add(new Card(Suit.Clubs, (Pips)7));
            mDeck.Add(new Card(Suit.Clubs, (Pips)8));
            mDeck.Add(new Card(Suit.Clubs, (Pips)9));
            mDeck.Add(new Card(Suit.Clubs, (Pips)10));
            mDeck.Add(new Card(Suit.Clubs, (Pips)11));
            mDeck.Add(new Card(Suit.Clubs, (Pips)12));
            mDeck.Add(new Card(Suit.Clubs, (Pips)13));
            mDeck.Add(new Card(Suit.Diamonds, Pips.Ace));
            mDeck.Add(new Card(Suit.Diamonds, (Pips)2));
            mDeck.Add(new Card(Suit.Diamonds, (Pips)3));
            mDeck.Add(new Card(Suit.Diamonds, (Pips)4));
            mDeck.Add(new Card(Suit.Diamonds, (Pips)5));
            mDeck.Add(new Card(Suit.Diamonds, (Pips)6));
            mDeck.Add(new Card(Suit.Diamonds, (Pips)7));
            mDeck.Add(new Card(Suit.Diamonds, (Pips)8));
            mDeck.Add(new Card(Suit.Diamonds, (Pips)9));
            mDeck.Add(new Card(Suit.Diamonds, (Pips)10));
            mDeck.Add(new Card(Suit.Diamonds, (Pips)11));
            mDeck.Add(new Card(Suit.Diamonds, (Pips)12));
            mDeck.Add(new Card(Suit.Diamonds, (Pips)13));
            mDeck.Add(new Card(Suit.Hearts, Pips.Ace));
            mDeck.Add(new Card(Suit.Hearts, (Pips)2));
            mDeck.Add(new Card(Suit.Hearts, (Pips)3));
            mDeck.Add(new Card(Suit.Hearts, (Pips)4));
            mDeck.Add(new Card(Suit.Hearts, (Pips)5));
            mDeck.Add(new Card(Suit.Hearts, (Pips)6));
            mDeck.Add(new Card(Suit.Hearts, (Pips)7));
            mDeck.Add(new Card(Suit.Hearts, (Pips)8));
            mDeck.Add(new Card(Suit.Hearts, (Pips)9));
            mDeck.Add(new Card(Suit.Hearts, (Pips)10));
            mDeck.Add(new Card(Suit.Hearts, (Pips)11));
            mDeck.Add(new Card(Suit.Hearts, (Pips)12));
            mDeck.Add(new Card(Suit.Hearts, (Pips)13));
            mDeck.Add(new Card(Suit.Spades, Pips.Ace));
            mDeck.Add(new Card(Suit.Spades, (Pips)2));
            mDeck.Add(new Card(Suit.Spades, (Pips)3));
            mDeck.Add(new Card(Suit.Spades, (Pips)4));
            mDeck.Add(new Card(Suit.Spades, (Pips)5));
            mDeck.Add(new Card(Suit.Spades, (Pips)6));
            mDeck.Add(new Card(Suit.Spades, (Pips)7));
            mDeck.Add(new Card(Suit.Spades, (Pips)8));
            mDeck.Add(new Card(Suit.Spades, (Pips)9));
            mDeck.Add(new Card(Suit.Spades, (Pips)10));
            mDeck.Add(new Card(Suit.Spades, (Pips)11));
            mDeck.Add(new Card(Suit.Spades, (Pips)12));
            mDeck.Add(new Card(Suit.Spades, (Pips)13));

        }

        #endregion

        #region properties

        public int Count { get { return mDeck.Count; } }
        
        public List<Card> DrawDeck { get { return new List<Card>(mDrawDeck); } } //return a clone so it cannot be changed
        
        public Random RandomForDeck { get { return R; } } //games using this deck can use the deck's random generator for shuffling their drawdeck, since precautions will be taken that a deck is only used from one thread at a time.


        #endregion

        #region methods
        public void Shuffle()
        {
            List<Card> templist = new List<Card>();
            templist.AddRange(mDeck); // temporarily transfer all the cards to another list
            mDeck.Clear(); // clear original list
            // now randomly add all the cards in the temporary list to the original list but in random order
            while (templist.Count > 0)
            {
                int randomcardindex = R.Next(templist.Count);
                mDeck.Add(templist[randomcardindex]);
                templist.RemoveAt(randomcardindex);
            }
        }

        public List<List<Card>> PlayerStartingHands(int PlayerCount, int CardCountForEach, bool ShuffleBeforeDealing = true)
        {
            if (ShuffleBeforeDealing) this.Shuffle();
            List<List<Card>> PlayerHandsList = new List<List<Card>>();
            for (int i = 0; i < PlayerCount; i++)// create blank card list for each player
            {
                PlayerHandsList.Add(new List<Card>());
            }
            int cardindex = 0;
            for (int p = 0; p < PlayerCount; p++)
            {
                for (int c = 0; c < CardCountForEach; c++)
                {
                    PlayerHandsList[p].Add(mDeck[cardindex]);
                    cardindex++;
                }
            }
            //the remaining cards form the drawdeck
            mDrawDeck = new List<Card>();
            mDrawDeck.AddRange(mDeck.GetRange(PlayerCount * CardCountForEach, mDeck.Count - (PlayerCount * CardCountForEach)));
            return PlayerHandsList;
        }
        public List<List<Card>> PlayerStartingHands(int PlayerCount, bool ShuffleBeforeDealing = true) //overloaded method to deal all the cards to playercount specified
        {
            if (ShuffleBeforeDealing) this.Shuffle();
            List<List<Card>> PlayerHandsList = new List<List<Card>>();
            for (int i = 0; i < PlayerCount; i++)// create blank card list for each player
            {
                PlayerHandsList.Add(new List<Card>());
            }
            for (int i = 0; i < mDeck.Count; i++)
            {
                PlayerHandsList[i % PlayerCount].Add(mDeck[i]);
            }
            return PlayerHandsList;
        }
        //Deal method for OldMaid, for example, because you can specify how many cards to use out of the deck  In Old Maid, use one less than the deck count.
        public List<List<Card>> PlayerStartingHandsFromPartOfDeck(int PlayerCount, int CardCount, bool ShuffleBeforeDealing = true) //overloaded method to deal all the cards to playercount specified
        {
            if (ShuffleBeforeDealing) this.Shuffle();
            List<List<Card>> PlayerHandsList = new List<List<Card>>();
            for (int i = 0; i < PlayerCount; i++)// create blank card list for each player
            {
                PlayerHandsList.Add(new List<Card>());
            }
            for (int i = 0; i < CardCount; i++)
            {
                PlayerHandsList[i % PlayerCount].Add(mDeck[i]);
            }
            //the remaining cards form the drawdeck
            mDrawDeck = new List<Card>();
            mDrawDeck.AddRange(mDeck.GetRange(CardCount , mDeck.Count - CardCount));
            return PlayerHandsList;
        }

        #endregion

    }
}
