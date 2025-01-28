using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;
using System.Text;

namespace standup
{
    public class IdentifyTheIdeaQuestion
    {
        #region ctr

        public IdentifyTheIdeaQuestion(int questionNumber, List<string> SoundPathes, List<string> Clues, string answer, string inCompleteAnswer, int EndIndexOfAll)
        {
            CluesSoundPathes = new List<string>();
            ClueText = new List<string>();

            QuestionNumber = questionNumber;
            int StartIndex = ((questionNumber - 1) * 4);
            int EndIndex = (((questionNumber - 1) * 4) + 4);

            for (int i = StartIndex; i < EndIndex; i++)
            {
                CluesSoundPathes.Add($@"{SoundPathes.ElementAt(i)}");
                ClueText.Add(Clues.ElementAt(i));
            }

            CorrectAnswer = answer;
            InCompleteAnswer = inCompleteAnswer;
            Money = 10000;
            TotalSecondsForAnswer = 25;
            TimeLeft = 25;

            CluesSoundPlayers = new List<SoundPlayer>();

            foreach(string ClueSoundPath in CluesSoundPathes)
            {
                SoundPlayer soundPlayer = new SoundPlayer($@"{ClueSoundPath}");
                CluesSoundPlayers.Add(soundPlayer);
            }
        }

        #endregion

        #region Properties

        public string CorrectAnswer { get; set; }
        public int Money { get; set; }
        public int TotalSecondsForAnswer { get; set; }
        public int TimeLeft { get; set; }
        public string PathToAnswer { get; set; }
        public List<string> ClueText { get; set; }
        public List<string> CluesSoundPathes { get; set; }
        public int QuestionNumber { get; set; }
        public string InCompleteAnswer { get; set; }
        public List<SoundPlayer> CluesSoundPlayers { get; set; }


        #endregion
    }
}
