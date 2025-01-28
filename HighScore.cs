using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;
using System.Numerics;

namespace standup
{
    public class HighScore
    {
        #region ctor

        public HighScore(Game game)
        {
            game.IsGameOver = true;
            HighScoreDocument = new XDocument(XDocument.Load(@"You think you are smart xml/HighScore.xml"));
            scores = new List<XElement>(HighScoreDocument.Root.Elements("Score"));
            gamePlayers = game.GamePlayers;
            PathToStupidGameOverSoundEffect = @"standup/OtherSounds/HighScoreSound.wav";
        }

        #endregion

        #region functions

        public void CalculatingEverythingPosition()
        {
            List<int> existingRecordNumbers = new List<int>();
            List<int> existingPositionsNumbers = new List<int>();
            List<string> existingPlayerName = new List<string>();
            List<XElement> PlayersRecords = new List<XElement>();
            List<XElement> PlayersPositions = new List<XElement>();
            List<int> PlayersRecordsNumbers = new List<int>();
            List<int> PlayersPositionsNumbers = new List<int>();
            List<string> PlayerNames = new List<string>();

            if (scores.ElementAt(0).Element("PlayerName").Value != "")
            {
                IEnumerable<XElement> existingRecords = new List<XElement>(scores.Elements("Money"));
                IEnumerable<XElement> existingPositions = new List<XElement>(scores.Elements("Position"));
                IEnumerable<XElement> exsitingPlayers = new List<XElement>(scores.Elements("PlayerName"));

                foreach (XElement record in existingRecords)
                {
                    existingPositionsNumbers.Add(int.Parse(record.Value));
                }

                foreach (XElement position in existingPositions)
                {
                    existingRecordNumbers.Add(int.Parse(position.Value));
                }
                foreach (XElement name in exsitingPlayers)
                {
                    existingPlayerName.Add(name.Value);
                }
            }

            foreach (Player player in gamePlayers)
            {
                PlayersRecordsNumbers.Add(player.GameSum);
                PlayersPositionsNumbers.Add(player.PlayerPosition);
                PlayerNames.Add(player.PlayerName);
            }

            ShowingPlayerResults();

            // the below if updates the player positions in the high score table. Thats why the function that shows
            // the players score comes before that (for the screen that shown the player positions compared to each other)

            if (existingRecordNumbers.Count > 0)
            {
                for (int playerCount = 0; playerCount < gamePlayers.Count; playerCount++)
                {
                    for (int record = 0; record < existingRecordNumbers.Count; record++)
                    {
                        if (gamePlayers.ElementAt(playerCount).GameSum < existingRecordNumbers.ElementAt(record))
                        {
                            gamePlayers.ElementAt(playerCount).PlayerPosition = gamePlayers.ElementAt(playerCount).PlayerPosition + 1;
                        }
                        else if (existingRecordNumbers.ElementAt(record) == gamePlayers.ElementAt(playerCount).GameSum)
                        {
                            gamePlayers.ElementAt(playerCount).PlayerPosition = existingPositionsNumbers.ElementAt(record);
                        }
                        else if (gamePlayers.ElementAt(playerCount).GameSum > existingRecordNumbers.ElementAt(record))
                        {
                            gamePlayers.ElementAt(playerCount).PlayerPosition = existingPositionsNumbers.ElementAt(record);

                            List<int> reserve = new List<int>();
                            foreach (int item in existingPositionsNumbers)
                            {
                                reserve.Add(item);
                            }
                            existingPositionsNumbers.RemoveRange(0, existingPositionsNumbers.Count);
                            for (int position = 0; position < reserve.Count; position++)
                            {
                                if (position == record)
                                {
                                    existingPositionsNumbers.Add(reserve.ElementAt(position) + 1);
                                }
                                else
                                {
                                    existingPositionsNumbers.Add(reserve.ElementAt(position));
                                }
                            }
                        }
                    }
                }
            }

            List<int> allrankingsAndPositions = new List<int>();
        }
        //List<

        //List<List<int>> alldetails


        /*
        public XDocument EditingHighScore()
        {
            foreach (Player player in gamePlayers)
            {
                List<string> WhatToWriteIntoXML = new List<string>();
            }
        }*/

        public void ShowingPlayerResults()
        {
            Console.Clear();
            Console.WriteLine("Game over!");
            Console.WriteLine("And the results are..");
            MakingTheConsoleWait5Seconds();

            FileStream GameOverSound = File.Open(PathToStupidGameOverSoundEffect, FileMode.Open);

            MakingTheConsoleWait5Seconds();

            Console.Clear();

            for (int Position = 0; Position < gamePlayers.Count; Position++)
            {
                foreach (Player player in gamePlayers)
                {
                    if (player.PlayerPosition == Position + 1)
                    {
                        Console.WriteLine("Number " + player.PlayerPosition.ToString() + ": " + player.PlayerName + " with " + player.GameSum + "$");
                        Console.WriteLine();
                    }
                }
            }

            foreach (Player player in gamePlayers)
            {
                if (player.PlayerPosition == 1)
                {
                    FileStream winnersound = File.Open(@"standup/winner.wav", FileMode.Open);

                    MakingTheConsoleWait2Seconds();

                    FileStream playerwinnersound = File.Open(@player.PlayerNameAudioPath, FileMode.Open);

                    MakingTheConsoleWait2Seconds();

                    if (player.GameSum > 0)
                    {
                        FileStream responseToWinner = File.Open(@"standup/WinningPositiveSum.wav", FileMode.Open);
                    }
                    else if (player.GameSum <= 0)
                    {
                        FileStream responseToWinner = File.Open(@"standup/WinningNegativeSum.wav", FileMode.Open);
                    }

                    MakingTheConsoleWait5Seconds();
                }
            }
        }

        public void MakingTheConsoleWait5Seconds()
        {
            DateTime startSound = DateTime.Now;
            double startTime = DateTime.Now.Second;

            while (startSound.Second - 5 <= startTime || startSound.Second - startTime >= (-55))
            {
                startSound = DateTime.Now;
                Console.Write(string.Empty);
            }
        }

        public void MakingTheConsoleWait2Seconds()
        {
            DateTime startSound = DateTime.Now;
            double startTime = DateTime.Now.Second;

            while (startSound.Second - 2 <= startTime || startSound.Second - startTime >= (-58))
            {
                startSound = DateTime.Now;
                Console.Write(string.Empty);
            }
        }

        #endregion

        #region Properties

        public XDocument HighScoreDocument { get; set; }
        public List<XElement> scores { get; set; }
        public List<Player> gamePlayers { get; set; }
        public bool IsHighScoreEmpty { get; set; }
        public XElement score { get; set; }
        public XElement PlayerName { get; set; }
        public XElement Money { get; set; }
        public string PathToStupidGameOverSoundEffect { get; set; }

        #endregion
    }
}
