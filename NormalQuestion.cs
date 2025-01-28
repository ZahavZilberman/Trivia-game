using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;

namespace standup
{
    public class NormalQuestion
    {
        public NormalQuestion(Game game, int MoneyIfRight, string QuestionContent, int QuestionInGame, int answer, int QuestionNumberInDatabase, string sound, string subject)
        {
            MoneyOnTable = MoneyIfRight;
            QuestionText = QuestionContent;
            QuestionRowInGame = QuestionInGame;
            CorrectAnswer = answer;
            QuestionNumber = QuestionNumberInDatabase;
            SoundPath = sound;
            Subject = subject;
            MoneyIfWrong = MoneyOnTable * (-1);
            AreEveryoneWrong = false;
            IsSomeoneAnswering = false;
            QuestionName = "";
        }

        public int MoneyOnTable { get; set; }
        public string QuestionText { get; set; }
        public int QuestionRowInGame { get; set; }
        public int CorrectAnswer { get; set; }
        public int QuestionNumber { get; set; }
        public string SoundPath { get; set; }
        public string Subject { get; set; }
        public int MoneyIfWrong { get; set; }
        public bool AreEveryoneWrong { get; set; }
        public bool IsSomeoneAnswering { get; set; }
        public string QuestionName { get; set; }
        public string PossibleAnswersSoundPath { get; set; }
    }
}
