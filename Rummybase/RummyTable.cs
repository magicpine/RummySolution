using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayingCards;
namespace RummyBase
{
    public class RummyTable
    {
        public Dictionary<int, List<Card>> SetMelds { get; set; }
        public Dictionary<int, List<Card>> RunMelds { get; set; }
        public List<Card> DiscardPile { get; set; }
        public List<int> HandSizes { get; set; }
        public List<int> Scores { get; set; }
    }

    public class UserRummyTable
    {
        private RummyTable _BaseTable;
        public UserRummyTable(RummyTable BaseTable)
        {
            _BaseTable = BaseTable;
        }
        public Dictionary<int, List<Card>> SetMelds
        {
            get
            {
                Dictionary<int, List<Card>> CloneMelds = new Dictionary<int, List<Card>>();
                foreach (var item in _BaseTable.SetMelds)
                {
                    CloneMelds.Add(item.Key, item.Value);
                }
                return CloneMelds;
            }
        }
        public Dictionary<int, List<Card>> RunMelds;
        public List<Card> DiscardPile; //the last card discarded is at the bottom of the list and would be the only choice to pick up
        public List<int> HandSizes; //the number of cards in each players hand (according to index)  e.g. HandSizes[0] is how many cards Player 0 has left
        public Card TopDiscardCard
        {
            get
            {
                return _BaseTable.DiscardPile[_BaseTable.DiscardPile.Count - 1];
            }

        }
        public List<int> Scores;  //the cumulative score for each player (according to index) e.g. Scores[2] contains how many points Player 2 has earned

    }
}
