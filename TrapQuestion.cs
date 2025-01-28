using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;

namespace standup
{
    public class TrapQuestion
    {
        #region ctor

        public TrapQuestion(string text, List<string> answers, int answer, int AnswerRow, int AnswerNumber)
        {
            QuestionText = text;
            PossibleAnswers = answers;
            CorrectAnswer = answer;
            NumberOfQuestionInATrapGame = AnswerRow;
            QuestionNumberInDatabase = AnswerNumber;
            for (int i = 0; i < answers.Count; i++)
            {
                if (i + 1 == CorrectAnswer)
                {
                    CorrectAnswerText = answers.ElementAt(i);
                }
            }

            MoneyOnTable = 1000;
        }

        #endregion ctor

        #region Properties

        public string QuestionText { get; set; }
        public List<string> PossibleAnswers { get; set; }
        public int CorrectAnswer { get; set; }
        public int NumberOfQuestionInATrapGame { get; set; }
        public int QuestionNumberInDatabase { get; set; }
        public string CorrectAnswerText { get; set; }
        public int MoneyOnTable { get; set; }
        Player playerWhoseTryingToAnswer { get; set; }
        public bool HasTheQuestionBeenAskedAlready { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }

        #endregion
    }
}
