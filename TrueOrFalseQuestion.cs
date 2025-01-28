using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;

namespace standup
{
    public class TrueOrFalseQuestion
    {
        public TrueOrFalseQuestion(Player playerToAnswer, string QuestionText, bool CorrectAnswer)
        {
            QuestionContent = QuestionText;
            answer = CorrectAnswer;
        }
        public bool answer { get; set; }
        public bool AnswerChoice { get; set; }
        public string QuestionContent { get; set; }

    }
}
