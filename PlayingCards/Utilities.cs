using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayingCards
{
    public class Utilities
    {
        public static List<Card> EliminateDoubles(List<Card> Hand)
        {
            //will eliminate all sets of  doubles (based on pips) from the hand, and leave the hand in the same order it came in except that the doubles are removed
            //in the case of three cards with the same pips, the first two matches will be removed!
            List<Card> RemovalList = new List<Card>();
            List<int> CardIndexesToRemove = new List<int>();
            for (int i = 0; i < Hand.Count; i++)
            {
                if (!CardIndexesToRemove.Contains(i))
                {
                    for (int j = i + 1; j < Hand.Count; j++)
                    {
                        if (Hand[i].Pips == Hand[j].Pips)
                        {
                            CardIndexesToRemove.Add(i);
                            CardIndexesToRemove.Add(j);
                            break; //just mark one pair in the inner loop
                        }
                    }
                }
            }
            //At this point all the pairs should be marked, so remove them from the list from last to first
            CardIndexesToRemove.Sort(); //first sort 
            if (CardIndexesToRemove.Count > 0)
            {
                for (int i = CardIndexesToRemove.Count - 1; i >= 0; i--)
                {
                    RemovalList.Add(Hand[CardIndexesToRemove[i]]);
                    Hand.RemoveAt(CardIndexesToRemove[i]);
                }
            }
            return RemovalList;
        }

        public static void RemoveTheseCardsFromHand(List<Card> Hand, List<Card> CardsToRemove)
        //will throw an exception if the removal list contains any cards not in the hand
        {
            if (CardsToRemove == null || CardsToRemove.Count == 0)
                return;
            foreach (Card c in CardsToRemove)
            {
                if (Hand.Contains(c))
                    Hand.Remove(c);
                else
                    throw new Exception("Utilities RemoveSomeCardsFromHand was asked to remove " + c.ToString() + " from hand that did not contain it.");
            }
        }

        public static int FaceCardCount(List<Card> Hand)
        {
            int count = 0;
            foreach (Card C in Hand)
            {
                if ((int)C.Pips > 10 || (int)C.Pips == 0)
                    count++;

            }
            return count;

        }
    }
}
