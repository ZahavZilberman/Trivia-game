using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;

namespace standup
{
    public class Trap
    {
        #region ctor

        public Trap(Game game)
        {
            /*
            DirectoryInfo TextDirectory = new DirectoryInfo($@"You-think-you-are-smart\TrapQuestions");
            DirectoryInfo[] AllQuestionsFolders = TextDirectory.GetDirectories();
            int AmountOfQuestions = AllQuestionsFolders.Count();
            List<DirectoryInfo> QuestionsFoldersList = AllQuestionsFolders.ToList();
            */
            CorrectAnswerRewardAndPunishment = 1000;
            TrapQuestions = new XDocument(XDocument.Load(@"You think you are smart xml\TrapXML.xml"));
            QuestionsElements = TrapQuestions.Root.Elements("Question");
            Questions = QuestionsElements.ToList();
            string Answer1Text = "";
            string Answer2Text = "";
            string Answer3Text = "";
            string Answer4Text = "";
            List<string> AllAnswers = new List<string>();
            /*
             *DirectoryInfo CorrectFolder = new DirectoryInfo($@"You-think-you-are-smart");

            foreach (DirectoryInfo Folder in AllQuestionsFolders)
            {
                DirectoryInfo ChoosenFolder = new DirectoryInfo($@"{QuestionsFoldersList.Find(x => x == Folder)}");
                CorrectFolder = Folder;
                FileInfo[] files2 = ChoosenFolder.GetFiles();
                List<FileInfo> ListOfFiles2 = files2.ToList();

                Answer1Text = File.ReadAllText($@"{QuestionsFoldersList.Find(x => x == Folder).FullName}\Answer1.txt");
                Answer2Text = File.ReadAllText($@"{QuestionsFoldersList.Find(x => x == Folder).FullName}\Answer2.txt");
                Answer3Text = File.ReadAllText($@"{QuestionsFoldersList.Find(x => x == Folder).FullName}\Answer3.txt");
                Answer4Text = File.ReadAllText($@"{QuestionsFoldersList.Find(x => x == Folder).FullName}\Answer4.txt");
                AllAnswers.Add(Answer1Text);
                AllAnswers.Add(Answer2Text);
                AllAnswers.Add(Answer3Text);
                AllAnswers.Add(Answer4Text);
            }
            */
            
            Answers = new List<string>();
            AllQuestions = new List<TrapQuestion>();
            AllPossibleAnswers = new List<List<string>>();
            for (int i = 0; i < Questions.Count; i++)
            {
                AllAnswers = new List<string>();

                string AnswerPath = "";
                for (int answerNum = 0; answerNum < 4; answerNum++)
                {
                    List<string> AnswersAll = new List<string>();
                    AnswerPath = Questions.ElementAt(i).Element($@"Answer{answerNum + 1}Text").Value;
                    string ActualAnswer = File.ReadAllText(AnswerPath);                                       
                }                

                int QuestionPositionInDatabase = int.Parse(Questions.ElementAt(i).Element("Number").Value);
                int correctAnswer = int.Parse(Questions.ElementAt(i).Element("CorrectAnswer").Value);
                string QuestionItselfPath = Questions.ElementAt(i).Element("Text").Value;
                string QuestionContent = File.ReadAllText(QuestionItselfPath);

                Answer1Text = File.ReadAllText($@"{Questions.ElementAt(i).Element("Answer1Text").Value}");
                Answer2Text = File.ReadAllText($@"{Questions.ElementAt(i).Element("Answer2Text").Value}");
                Answer3Text = File.ReadAllText($@"{Questions.ElementAt(i).Element("Answer3Text").Value}");
                Answer4Text = File.ReadAllText($@"{Questions.ElementAt(i).Element("Answer4Text").Value}");
                AllAnswers.Add(Answer1Text);
                AllAnswers.Add(Answer2Text);
                AllAnswers.Add(Answer3Text);
                AllAnswers.Add(Answer4Text);

                TrapQuestion question = new TrapQuestion(QuestionContent, AllAnswers, correctAnswer, i + 1, QuestionPositionInDatabase);
                AllQuestions.Add(question);
                            
            }

            question = null;
            QuestionText = null;
            CorrectAnswer = null;
            FilePathToInstructions = @"You-think-you-are-smart\OtherSounds\TrapInstructions.wav";
            //MusicPath = @"standup/Trap.mp3";
            game.IsOnTrapMode = true;
            MoneyOfPlayersInTheTrap = new List<int>();

            for (int i = 0; i < game.GamePlayers.Count; i++)
            {
                game.GamePlayers.ElementAt(i).TrapSum = 0;
                MoneyOfPlayersInTheTrap.Add(game.GamePlayers.ElementAt(i).TrapSum);
            }
        }

        #endregion

        #region Properties

        public XDocument TrapQuestions { get; set; }
        public List<XElement> Questions { get; set; }
        public XElement question { get; set; }
        public XElement QuestionText { get; set; }
        public XElement CorrectAnswer { get; set; }
        public int CorrectAnswerRewardAndPunishment { get; set; }
        public string FilePathToInstructions { get; set; }
        public string MusicPath { get; set; }
        List<string> Answers { get; set; }
        public IEnumerable<XElement> QuestionsElements { get; set; }
        public List<TrapQuestion> AllQuestions { get; set; }
        public List<int> MoneyOfPlayersInTheTrap { get; set; }
        List<List<string>> AllPossibleAnswers { get; set; }


        #endregion
    }
}
