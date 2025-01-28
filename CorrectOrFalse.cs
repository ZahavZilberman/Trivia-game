using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;
using System.Xml;

namespace standup
{
    public class CorrectOrFalse
    {
        #region ctor

        public CorrectOrFalse(Game game, int player)
        {
            playerForChellenge = game.GamePlayers.ElementAt(player);
            

            MoneyOnTheTable = playerForChellenge.PreTrapSum * 2;
            QuestionsDocument = new XDocument(XDocument.Load($@"You think you are smart xml\CorrectOrIncorrectXML.xml"));
            questions = QuestionsDocument.Root.Elements().ToList();
            playerForChellenge = game.GamePlayers.ElementAt(player);
            ActualQuestions = new List<TrueOrFalseQuestion>();

            game.IsOnCorrectOrFalseMode = true;
            //MusicPath = @"standup/CorrectOrFalse.mp3";
            InstrcutionsPath = $@"You-think-you-are-smart\CorrectOrIncorrectQuestions\Instructions.wav";
            IEnumerable<XElement> questionsIenumerable = QuestionsDocument.Root.Elements(@"Question");
            questions = questionsIenumerable.ToList();

            for (int i = 0; i < questions.Count; i++)
            {
                string textQuestionPath = questions[i].Element("Text").Value;
                string ActualText = File.ReadAllText(textQuestionPath);
                bool CorrectAnswer = bool.Parse(questions[i].Element("Answer").Value);
                TrueOrFalseQuestion ActualQuestion = new TrueOrFalseQuestion(playerForChellenge, ActualText, CorrectAnswer);
                ActualQuestions.Add(ActualQuestion);
            }

            Random rnd = new Random();
            int ChoosenQuestion = rnd.Next(1, ActualQuestions.Count + 1);
            for (int i = 0; i < ActualQuestions.Count; i++)
            {
                if (ChoosenQuestion - 1 == i)
                {
                    ChoosenQuestionRandom = new TrueOrFalseQuestion(playerForChellenge, ActualQuestions.ElementAt(i).QuestionContent, ActualQuestions.ElementAt(i).answer);
                }
            }
        }

        #endregion

        #region Propeties

        public XDocument QuestionsDocument { get; set; }
        public List<XElement> questions { get; set; }
        public Player playerForChellenge { get; set; }
        public string InstrcutionsPath { get; set; }
        public string MusicPath { get; set; }
        public List<TrueOrFalseQuestion> ActualQuestions { get; set; }
        public int MoneyOnTheTable { get; set; }
        public TrueOrFalseQuestion ChoosenQuestionRandom { get; set; }
        public ConsoleKeyInfo ChoosenAnswer { get; set; }


        #endregion
    }
}
