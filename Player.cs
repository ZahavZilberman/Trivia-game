using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;

namespace standup
{
    public class Player
    {
        #region ctor

        public Player(int PlayerNum, string Name)
        {
            PositionInHighScore = 0;

            PreTrapSum = 0;
            TrapSum = 0;
            GameSum = 0;
            PlayerNumber = PlayerNum;
            PlayerName = Name;
            PlayerBuzzer = null;
            PlayerNameAudioPath = null; //will change this one later
            HasThePlayerChoosenAnswer = false;


            if (PlayerNumber == 1)
            {
                PlayerBuzzer = "M";
                PlayerNameAudioPath = $@"You-think-you-are-smart\NameSounds\Player1.wav";
            }
            else if (PlayerNumber == 2)
            {
                PlayerBuzzer = "+";
                PlayerNameAudioPath = $@"You-think-you-are-smart\NameSounds\Player2.wav";
            }
            else if (PlayerNumber == 3)
            {
                PlayerBuzzer = "A";
                PlayerNameAudioPath = $@"You-think-you-are-smart\NameSounds\Player3.wav";
            }

            PlayerNameAudioPath = null;

            WrongAnswerChoosen = 0;

            PlayerPosition = 1;
        }

        #endregion

        #region Properties

        public int PreTrapSum { get; set; }

        public int TrapSum { get; set; }

        public int GameSum { get; set; }

        public int PlayerNumber { get; set; }

        public string PlayerName { get; set; }

        public string PlayerNameAudioPath { get; set; }

        public string PlayerBuzzer { get; set; }

        public int PlayerPosition { get; set; }
        public bool HasThePlayerChoosenAnswer { get; set; }
        public int WrongAnswerChoosen { get; set; }
        public int PositionInHighScore { get; set; }

        #endregion
    }
}
