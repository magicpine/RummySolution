using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PlayingCards;
using RummyBase;
using RummyBots;
using ChrisTest;
using System.Diagnostics;
using System.Collections;

namespace TournamentHostProject
{
    public partial class Form1 : Form
    {
        private List<IRummyPlayer> _players;
        private MainGame _mainGame;
        private long _gameGoal = 10;

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            _players = new List<IRummyPlayer>();

            //TODO use the constructor to bring in a list of players
            for (int i = 0; i < 4; i++)
            {
                _players.Add(new RummyBot1());
                _players[i].ChoosePile += Bots_ChoosePile;
                _players[i].DoneTurn += Bots_DoneTurn;
                _players[i].RunMelds += Bots_RunMelds;
                _players[i].SetMelds += Bots_SetMelds;
                _players[i].TurnLayoff += Bots_TurnLayoff;

                mainPnl.Controls.Add(new PlayerPanel((i * 60) + 10, _players[i].Name));
            }
            gameGoalLbl.Text = string.Format("Game Goal: {0}", _gameGoal);

        }

        private void Bots_TurnLayoff(IRummyPlayer sender, int MeldID, Card CardToAdd)
        {
            _mainGame.TurnLayoff(sender, MeldID, CardToAdd);
        }

        private void Bots_SetMelds(IRummyPlayer sender, List<Card> CardsInMeld)
        {
            _mainGame.SetMelds(sender, CardsInMeld);
        }

        private void Bots_RunMelds(IRummyPlayer sender, List<Card> CardsInMeld)
        {
            _mainGame.RunMelds(sender, CardsInMeld);
        }

        private void Bots_DoneTurn(IRummyPlayer sender, Card CardBeingDiscarded, bool OutOfCards = false)
        {
            _mainGame.TellTheOtherBotsWhatHappened(CardBeingDiscarded);

            if (OutOfCards == true)
            {
                Debug.Print("Game is now over");
                _mainGame.GameOverHappened();
            }
            else
            {
                _mainGame.Discarded(CardBeingDiscarded);
                Debug.Print("Turn Ended");
            }

        }

        private void Bots_ChoosePile(IRummyPlayer sender, CardPiles Pile, out Card DrawCard)
        {
            DrawCard = _mainGame.PickUpCard(sender, Pile);
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            startBtn.Enabled = false;
            pauseBtn.Enabled = true;
            cancelBtn.Enabled = true;
            _mainGame = new MainGame(_players, _gameGoal);
            _mainGame.GameOver += _mainGame_GameOver;
            Task.Run(() =>_mainGame.StartTournament());
        }

        private void _mainGame_GameOver(int indexOfWinningPlayer, int score)
        {
            currentGameLbl.Text = string.Format("Current Game: {0}", _mainGame.CurrentGame);
            mainPnl.Controls[indexOfWinningPlayer].Controls[1].Text = string.Format("Score: {0}", score);
        }

        private void pauseBtn_Click(object sender, EventArgs e)
        {
            if (pauseBtn.Text == "Pause")
            {
                _mainGame.PauseGame();
                pauseBtn.Text = "Resume";
            }
            else
            {
                _mainGame.ResumeGame();
                pauseBtn.Text = "Pause";
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            _mainGame.Cancel();
            startBtn.Enabled = true;
            pauseBtn.Enabled = false;
            cancelBtn.Enabled = false;
        }
    }
}
