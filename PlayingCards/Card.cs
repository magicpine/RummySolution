using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Resources;


namespace PlayingCards
{
    public enum Suit
    {
        Clubs,
        Diamonds,
        Spades,
        Hearts,
        None
    }
    public enum CardColor
    {
        Black,
        Red
    }
    public enum Pips
    {
        Ace = 1, //for Rummy, Ace is low
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,

        Jack = 11,
        Queen = 12,
        King = 13,

        Joker = 0
    }

    [Serializable]
    public class Card : IComparable<Card>
    {
        private Suit mSuit;
        private Pips mPips;
        private CardColor mCardColor;


        public Card(Suit suit, Pips pips)
        {
            mSuit = suit;
            mPips = pips;
            if (suit == PlayingCards.Suit.Clubs || suit == PlayingCards.Suit.Spades || suit == PlayingCards.Suit.None) //assume jokers are black
                mCardColor = PlayingCards.CardColor.Black;
            else
                mCardColor = PlayingCards.CardColor.Red;
        }

        public Suit Suit
        {
            get
            {
                return mSuit;
            }
        }

        public Pips Pips
        {
            get
            {
                return mPips;
            }
        }
        public bool IsFaceCard
        {
            get
            {
                if (this.mPips == Pips.Jack || this.mPips == Pips.Queen || this.mPips == Pips.King)
                    return true;
                else
                    return false;
            }
        }

        public CardColor CardColor
        {
            get
            {
                return mCardColor;
            }
        }
        public Image CardImage
        {
            get
            {
                ResourceManager rm = Properties.Resources.ResourceManager;
                string filename = "";
                if ((int)this.Pips <= 10) //for numbered cards 2 to 10
                {
                    filename = "_";
                    filename += ((int)this.Pips).ToString(); //want digits like 2, 3, 4
                    filename += "_of_" + this.Suit.ToString();
                }
                else //for named cards
                {
                    filename = this.Pips.ToString(); //want words like ace, jack,...
                    filename += "_of_" + this.Suit.ToString();
                    if (this.Pips != Pips.Ace) //the aces don't have the second option, but the jack,queen,king do
                        filename += "2";
                }
                filename = filename.ToLower();
                return (Image)(rm.GetObject(filename));
            }
        }

        //added this method which overrides the Equals(Object) method 

        public override bool Equals(Object OtherCardObject)
        {
            //if both are null, this method will not get called
            if (OtherCardObject == null)      //if OtherCardObject is null, return false

                return false;
            //if OtherCardObject is not a card, then we'll return false (should we raise an exception?)
            //if OtherCardObject is a card, then we'll use our Equals(Card OtherCard method)
            Card OtherCard = OtherCardObject as Card;  //the as keyword will attempt to cast OtherCardObject as a Card, and will do that if it can, but if it can't the expression will yield null
            if ((object)OtherCard == null)
                return false;  //it is not a card, so can't be equal to any card!
            return this.Equals(OtherCard);
        }


        public bool Equals(Card OtherCard)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(this, OtherCard)) //* see note at bottom
            {
                return true;
            }
            // If one is null, but not both, return false.
            if (((object)this == null) || ((object)OtherCard == null))
            {
                return false;
            }

            //otherwise look at the Suit and Pips of both cards.  If they match, they are considered equal.
            return (this.Suit == OtherCard.Suit && this.Pips == OtherCard.Pips);
        }

        public static bool operator ==(Card C1, Card C2) //valid because cards are considered immutable objects - they do not change once created.
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(C1, C2)) //* see note at bottom
            {
                return true;
            }
            // If one is null, but not both, return false.
            if (((object)C1 == null) || ((object)C2 == null))
            {
                return false;
            }

            //otherwise look at the Suit and Pips of both cards.  If they match, they are considered equal.
            return (C1.Suit == C2.Suit && C1.Pips == C2.Pips);
        }
        public static bool operator <(Card C1, Card C2)
        {
            if (C1.CompareTo(C2) == -1)
                return true;
            else
                return false;

        }

        public static bool operator >(Card C1, Card C2)
        {
            if (C1.CompareTo(C2) == 1)
                return true;
            else
                return false;

        }

        public static bool operator !=(Card C1, Card C2) //supposed to always override the != operator when overriding the == operator.
        {
            return !(C1 == C2);
        }

        // override GetHashCode so the compiler is happy
        // usual purpose of overriding is so that if it is used as a key in a Hashtable, you might want two cards with same suit and value to be deemed identical so you would make it so that GetHashCode would give back the same integer for both.  Or you might want them to be treated separately e.g. two different cards, each Ace of Spades
        //see comment way below
        public override int GetHashCode()
        {
            //return base.GetHashCode(); //this would allow a dictionary to contain two separate Ace of Spades
            return (int)this.Suit * 14 + (int)this.Pips; //this would allow a dictionary to contain only one Ace of Spades, and you could keep a count of the number of them
        }


        public override string ToString()
        {
            string strPips;
            string strSuit;
            int cardVal = (int)mPips;
            if (cardVal >= 2 && cardVal <= 10)
                strPips = cardVal.ToString();
            else
                strPips = mPips.ToString();
            strSuit = (mSuit == Suit.None) ? "" : " of " + Suit.ToString();
            return strPips + strSuit;
        }

        public string DBCardCode() //for use in generating abbreviated string codes for a DB or whatever, like C9 (9 of clubs), SA (Ace of Spades), DK (King of Diamonds) etc.
        {
            string SuitCode = mSuit.ToString().Substring(0, 1);
            string PipsCode;
            int cardVal = (int)mPips;
            if (cardVal >= 2 && cardVal <= 10)
                PipsCode = cardVal.ToString();
            else
                PipsCode = mPips.ToString().Substring(0, 1);
            return SuitCode + PipsCode;
        }

        public int CompareTo(Card other) //for ease in sorting 
        {
            //- this will order by pips and suit within pips.  Suit order is whatever the enum shows, so can switch there if desired
            int seq1 = (int)this.mPips * 4 + (int)this.mSuit;
            int seq2 = (int)other.mPips * 4 + (int)other.mSuit;
            //return seq1.CompareTo(seq2);
            if (seq1 < seq2) return -1;
            if (seq1 > seq2) return 1;
            return 0;
            //- this will order by suit and pips within suit.  Suit order is whatever the enum shows, so can switch there if desired
            //    int seq1 = (int)this.mSuit * 14 + (int)this.mPips;
            //    int seq2 = (int)other.mSuit * 14 + (int)other.mPips;
            //    //return seq1.CompareTo(seq2);
            //    if (seq1 < seq2) return -1;
            //    if (seq1 > seq2) return 1;
            //    return 0;
        }
    }
}

//* A common error in overloads of operator == is to use (a == b), (a == null), or (b == null) to check for reference equality. This instead results in a call to the overloaded operator ==, causing an infinite loop. Use ReferenceEquals or cast the type to Object, to avoid the loop.
//http://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx





//http://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx
//this is something like the recommended way of overriding the Equals(Object) method, but my way above is much more streamlined I think.
//public override bool Equals(Object OtherCardObject)
//  {
//      // if other object is null, return false
//      if (this != null && OtherCardObject == null) //Larry added the this != null portion in case the Assert.AreEqual<T> might be using this override in a weird way where the instance is null.  Hard to imagine...
//          return false;
//      if ((object)this == null && OtherCardObject == null)
//          return true;

//      //if other object cannot be cast to Card, return false
//      Card OtherCard = OtherCardObject as Card;  //got this from example - the as keyword will attempt to cast OtherCardObject as a Card, and will do that if it can, but if it can't the expression will yield null
//      if (OtherCard == null)
//          return false;  //it is not a card, so can't be equal to this card!

//      // If both are null, or both are same instance, return true.
//      // Leave this in in case the Assert.AreEqual<Type> uses this instead of what it should  which is public static bool operator ==(Card C1, Card C2)
//      if (System.Object.ReferenceEquals(this, OtherCard)) //* see note at bottom
//      {
//          return true;
//      }
//      // If one is null, but not both, return false.
//      if (((object)this == null) || ((object)OtherCard == null))
//      {
//          return false;
//      }

//      //otherwise look at the Suit and Pips of both cards.  If they match, they are considered equal.
//      return (this.Suit == OtherCard.Suit && this.Pips == OtherCard.Pips);
//  }

//All primitive types provide their own speedy version of GetHashCode(), so when you build your types it's usual just to combine the hash codes of the fields that define identity. If you have a compound key, that is you need more than one field to define uniqueness, the usual approach is to bitwise exclusive-or the hashes together:

// Collapse | Copy Code
//public class Product : IEquatable<Product>
//{
//    public string Manufacturer { get; set; }
//    public string ProductCode { get; set; }

//    public override int GetHashCode()
//    {
//        return Manufacturer.GetHashCode() ^ ProductCode.GetHashCode();
//    }

//    public bool Equals(Product other)
//    {
//        return other.Manufacturer == Manufacturer && other.ProductCode == ProductCode;
//    }
//}

//This approach works well, but make sure that the hash codes from the compound fields are not aligned in any way. If you were to have a compound key using two integer identifiers which are usually the same and were to XOR their hash codes together, the result will usually be zero and you end up in the one bucket scenario. Although this is unusual, it’s something to have in the back of your mind.
