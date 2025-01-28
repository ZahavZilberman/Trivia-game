using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using System.Numerics;
using static System.Formats.Asn1.AsnWriter;
using NAudio.Utils;
using System.Speech.Synthesis;
using NAudio.Wave;
using System.Net.Http.Headers;


namespace standup
{
    public class Game
    {
        public static volatile bool _stopRequestedForEveryoneAnswering = false;
        public static volatile bool _stopRequestedForSomeoneAnswering = false;

        #region ctor

        public Game(List<Player> players, int NumOfQuestions, int NumOfPlayers)
        {
            PlayerToChooseCategory = players.ElementAt(0);
            CountOfQuestion = 1;
            NumOfGameQuestions = NumOfQuestions;
            PlayersNum = NumOfPlayers;
            GamePlayers = new List<Player>();
            GamePlayers = players;
            IsOnMultiplyMode = false;
            IsOnChooseCatgoryMode = false;
            IsOnCorrectOrFalseMode = false;
            IsOnIdentifyTheIdeaMode = false;
            IsOnTrapMode = false;
            IsOnNormalMode = false;
            IdentifyTheIdeaAlreadyAskedQuestions = new List<IdentifyTheIdeaQuestion>();
            CorrectOrFalseAlreadyAskedQuestions = new List<TrueOrFalseQuestion>();
            NormalAlreadyAskedQuestions = new List<NormalQuestion>();
            TrapAlreadyAskedQuestions = new List<TrapQuestion>();

            string currentPlayerturnName = GamePlayers.ElementAt(0).PlayerName;
            /*
            XDocument questions = new XDocument($@"You think you are smart xml\Questions.xml");
            XElement questionsXMLstart = questions.Root;
            List<XElement> questionsXML = new List<XElement>(questionsXMLstart.Elements("Question"));

            for (int CurrentQuestionNum = 0; CurrentQuestionNum == NumOfQuestions; CurrentQuestionNum++)
            {
                XElement text = questionsXML.ElementAt(CurrentQuestionNum).Element("QuestionText");
                XElement soundElement = questionsXML.ElementAt(CurrentQuestionNum).Element("Sound");
                string textFilePath = text.Value;
                string soundPath = soundElement.Value;
                string textPath = Path.GetFullPath(textFilePath);
                IEnumerable<string> textLines = File.ReadAllLines(textPath);
                List<string> actualText = textLines.ToList<string>();
                List<string> QuestionText = new List<string>();
                QuestionText = File.ReadAllLines(textPath).ToList();                
            }
            */

            /*
            ChooseCategory QuestionCategoryDetails = ChooseCategoryFunction(game, game.GamePlayers.ElementAt(0));

            foreach (Player player in GamePlayers)
            {
                if (player.PlayerNumber == QuestionCategoryDetails.PlayerNumber)
                {
                    PlayerToChooseCategory = player;
                }
            }
            */
        }

        #endregion

        #region Instructions part

        public void Instructions()
        {
            Console.Clear();

            #region The general instructions and buzzers

            #region Instructions

            Console.Clear();
            Console.WriteLine($@"So you've choosen {NumOfGameQuestions} questions..");
            Console.WriteLine();

            Console.WriteLine("Now, The general instructions.. you can hear them..");
            Console.WriteLine();
            Console.WriteLine("Press any key to skip this repetitive shit.");

            List<string> instructions = new List<string>();
            instructions.Add("Hello and welcome to the standup trivia game.");
            instructions.Add("The game contains 4 modes: identify the idea, true or false, normal questions, and a secret at the end.");
            instructions.Add("You will usually simply get a question with 4 possible answers, you need to answer it correctly.");
            instructions.Add("The instructions of the other modes will be explained later.");
            string[] InstructionLines = new string[instructions.Count];
            for (int i = 0; i < instructions.Count; i++)
            {
                InstructionLines[i] = instructions.ElementAt(i);
            }
            //CreatingAndPlayingAudio($@"You-think-you-are-smart\OtherSounds\instructions.wav", true, null, instructions.ToArray());
            CreatingAndPlayingAudio(@"You-think-you-are-smart\OtherSounds\Instructions.wav", string.Empty, InstructionLines, true);
            CreatingAndPlayingAudio(@"You-think-you-are-smart\OtherSounds\SkippedInstructions.wav", "Ouch! It hurts! I'm gonna get another seizure!", null, true);
            PlayingAndWaitingForAudio(@"You-think-you-are-smart\OtherSounds\Instructions.wav", @"You-think-you-are-smart\OtherSounds\SkippedInstructions.wav", false, true);


            #region old code

            /*
            int startSecond = DateTime.Now.Second;
            int startMinute = DateTime.Now.Minute;
            int endSecond;
            int endMinute;
            if (startSecond <= 22)
            {
                endMinute = startMinute;
                endSecond = startSecond + 38;
            }
            else
            {
                endMinute = startMinute + 1;
                endSecond = startSecond - 22;
            }

            double SecondsThatPassed = 0;

            InstructionsAudio.Play();
            while (!Console.KeyAvailable && SecondsThatPassed < 38)
            {
                Thread.Sleep(100);
                SecondsThatPassed = SecondsThatPassed + 0.1;
            }
            
            if (Console.KeyAvailable == true)
            {
                InstructionsAudio.Stop();
                InstructionsOtherAudio.Play();
                Thread.Sleep(3750);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }

                Console.Clear();
            }
            if (SecondsThatPassed >= 38)/*DateTime.Now.Second == endSecond && DateTime.Now.Minute == endMinute*/
            //{
            //Console.Clear();
            //Thread.Sleep(2000);
            //while (Console.KeyAvailable)
            //{
            //    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            //}
            //}

            #endregion

            #endregion

            #region Buzzers

            Console.WriteLine("Player 1 buzzer button is: M");
            Console.WriteLine("Player 2 buzzer is: +");
            Console.WriteLine("Player 3 buzzer is: A");
            Console.WriteLine("Press on enter to confirm you will remember this.");
            Console.WriteLine();
            Console.ReadLine();
            Console.Clear();

            #endregion

        }
        #endregion

        #endregion

        #region Game start
        public void GameStart(Game game)
        {
            #region Choosing category and determating if the next question is going to be normal or else            
            /*
            for (int CurrentQuestionNum = 0; CurrentQuestionNum == 0NumOfQuestions; CurrentQuestionNum++)
            {
                XElement text = questionsXML.ElementAt(CurrentQuestionNum).Element("QuestionText");
                XElement soundElement = questionsXML.ElementAt(CurrentQuestionNum).Element("Sound");
                string textFilePath = text.Value;
                string soundPath = soundElement.Value;
                string textPath = Path.GetFullPath(textFilePath);
                IEnumerable<string> textLines = File.ReadAllLines(textPath);
                List<string> actualText = textLines.ToList<string>();
                List<string> QuestionText = new List<string>();
                QuestionText = File.ReadAllLines(textPath).To;
                
                SystemSound sound = new SystemSound
                sound.Play();
            }
            */


            Random FactGameChance = new Random();
            int WillFactGameHappenRange = FactGameChance.Next(1, 10);
            bool WillFactGameHappen = WillFactGameHappenRange == 1 || WillFactGameHappenRange == 10;

            Random WhoAmIGameChance = new Random();
            int WillWhoAmIHappenRange = FactGameChance.Next(11, 20);
            bool WillWhoAmIHappen = WillFactGameHappenRange == 11 || WillFactGameHappenRange == 20;

            if (WillFactGameHappen && !WillWhoAmIHappen)
            {
                IsOnCorrectOrFalseMode = true;
                CorrectOrFalse FactGame = new CorrectOrFalse(game, game.GamePlayers.ElementAt(0).PlayerNumber);
                IsOnNormalMode = false;
                string note = "just to make sure this is why the console app sometimes shuts off";
            }
            else if (!WillFactGameHappen && WillWhoAmIHappen)
            {
                IsOnIdentifyTheIdeaMode = true;
                IdentifyTheIdea whoamigame = new IdentifyTheIdea(game);
                IsOnNormalMode = false;
                string note2 = "just to make sure this is why the console app sometimes shuts off";
            }
            else if (WillFactGameHappen && WillWhoAmIHappen)
            {
                IsOnNormalMode = true;
            }
            else if (!WillWhoAmIHappen && !WillFactGameHappen)
            {
                IsOnNormalMode = false;
            }

            #endregion

            //IsOnNormalMode = true;

            if (IsOnNormalMode)
            {
                //IdentifyTheIdeaGame(game, false);
                //CorrectOrFalseGame(game);
                //TrapMode(game);
                ChooseCategory QuestionCategoryDetails = ChooseCategoryFunction(game, game.GamePlayers.ElementAt(0));

                game = NormalQuestion(game, QuestionCategoryDetails, false, false, 10000);
                return;
                /*
                while(game.CountOfQuestion < game.NumOfGameQuestions)
                {
                    NextTurn(game, game.PlayerToChooseCategory);
                }
                */
            }
        }

        #endregion

        #region Next turn

        public void NextTurn(Game game, Player playerToChooseCategory)
        {
            for (int i = 0; i < game.NumOfGameQuestions; i++)
            {
                game.CountOfQuestion = i + 1;

                #region Each question..

                #region determating if the next question is going to be normal or else            
                /*
                for (int CurrentQuestionNum = 0; CurrentQuestionNum == 0NumOfQuestions; CurrentQuestionNum++)
                {
                    XElement text = questionsXML.ElementAt(CurrentQuestionNum).Element("QuestionText");
                    XElement soundElement = questionsXML.ElementAt(CurrentQuestionNum).Element("Sound");
                    string textFilePath = text.Value;
                    string soundPath = soundElement.Value;
                    string textPath = Path.GetFullPath(textFilePath);
                    IEnumerable<string> textLines = File.ReadAllLines(textPath);
                    List<string> actualText = textLines.ToList<string>();
                    List<string> QuestionText = new List<string>();
                    QuestionText = File.ReadAllLines(textPath).To;

                    SystemSound sound = new SystemSound
                    sound.Play();
                }
                */

                Random FactGameChance = new Random();
                int WillFactGameHappenRange = FactGameChance.Next(1, 9);
                bool WillFactGameHappen = WillFactGameHappenRange == 8 || WillFactGameHappenRange == 2;

                Random WhoAmIGameChance = new Random();
                int WillWhoAmIHappenRange = WhoAmIGameChance.Next(1, 9);
                bool WillWhoAmIHappen = WillWhoAmIHappenRange == 7 || WillWhoAmIHappenRange == 1;

                if (WillFactGameHappen && !WillWhoAmIHappen)
                {
                    IsOnCorrectOrFalseMode = true;
                    IsOnIdentifyTheIdeaMode = false;
                    IsOnNormalMode = false;
                    string note = "just to make sure this is why the console app sometimes shuts off";
                }
                else if (!WillFactGameHappen && WillWhoAmIHappen)
                {
                    IsOnIdentifyTheIdeaMode = true;
                    IsOnNormalMode = false;
                    IsOnCorrectOrFalseMode = false;
                    string note2 = "just to make sure this is why the console app sometimes shuts off";
                }
                else if (WillFactGameHappen && WillWhoAmIHappen)
                {
                    IsOnIdentifyTheIdeaMode = false;
                    IsOnNormalMode = true;
                    IsOnCorrectOrFalseMode = false;
                }
                else if (!WillWhoAmIHappen && !WillFactGameHappen)
                {
                    IsOnIdentifyTheIdeaMode = false;
                    IsOnNormalMode = true;
                    IsOnCorrectOrFalseMode = false;
                }

                #endregion

                if (IsOnNormalMode)
                {
                    //
                    //CorrectOrFalseGame(game);
                    //TrapMode(game);
                    ChooseCategory QuestionCategoryDetails2 = ChooseCategoryFunction(game, game.GamePlayers.ElementAt(0));
                    if (QuestionCategoryDetails2 == null)
                    {
                        return;
                    }
                    game = NormalQuestion(game, QuestionCategoryDetails2, false, true, 10000);

                    /*
                    while(game.CountOfQuestion < game.NumOfGameQuestions)
                    {
                        NextTurn(game, game.PlayerToChooseCategory);
                    }
                    */
                }
                if (IsOnIdentifyTheIdeaMode)
                {
                    game = IdentifyTheIdeaGame(game, false);
                    i = i - 1;
                }
                if (IsOnCorrectOrFalseMode)
                {
                    game = CorrectOrFalseGame(game);
                    i = i - 1;
                }

                #endregion
            }
            game = TrapMode(game);
            GameOver(game);
            return;
        }

        #endregion

        #region Choose Category

        public ChooseCategory? ChooseCategoryFunction(Game game, Player PlayerToChoose)
        {
            ChooseCategory choosingthis = new ChooseCategory(game, PlayerToChoose.PlayerNumber);

            Console.Clear();
            Console.WriteLine($@"{PlayerToChoose.PlayerName} (${PlayerToChoose.PreTrapSum}), choose category:");
            Console.WriteLine();

            Random DisplayingCategories1 = new Random();
            int FirstCategoryDisplayed = DisplayingCategories1.Next(0, choosingthis.AllCategoriesNames.Count);
            Random DisplayingCategories2 = new Random();
            int SecondCategoryDisplayed = DisplayingCategories1.Next(0, choosingthis.AllCategoriesNames.Count);
            while (SecondCategoryDisplayed == FirstCategoryDisplayed)
            {
                SecondCategoryDisplayed = DisplayingCategories1.Next(0, choosingthis.AllCategoriesNames.Count);
            }

            Random MoneyFirstCategory = new Random();
            int Money1stCategory = (MoneyFirstCategory.Next(1, 4)) * 1000;
            Random MoneySecondCategory = new Random();
            int Money2ndCategory = (MoneyFirstCategory.Next(1, 4)) * 1000;

            Console.WriteLine($@"1. {choosingthis.AllCategoriesNames.ElementAt(FirstCategoryDisplayed)} (${Money1stCategory})");
            Console.WriteLine();
            Console.WriteLine($@"2. {choosingthis.AllCategoriesNames.ElementAt(SecondCategoryDisplayed)} (${Money2ndCategory})");
            Console.WriteLine();

            if (!File.Exists($@"You-think-you-are-smart\NameSounds\Player{PlayerToChoose.PlayerNumber}.wav"))
            {
                throw new Exception("WTF?");
            }
            SoundPlayer PlayerSound = new SoundPlayer($@"You-think-you-are-smart\NameSounds\Player{PlayerToChoose.PlayerNumber}.wav");
            PlayerSound.PlaySync();
            PlayerSound.Dispose();
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            /*
            SoundPlayer ChooseCategory = new SoundPlayer($@"You-think-you-are-smart\OtherSounds\choosecategory.wav");
            ChooseCategory.Play();
            Thread.Sleep(2000);
            ChooseCategory.Dispose();
            */
            CreatingAndPlayingAudio($@"You-think-you-are-smart\OtherSounds\choosecategory.wav", "Choose a damn category!");
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            int ChoosenCategoryNumber = 0;

            Random doubleM = new Random();
            int chanceOfDoubleRound = doubleM.Next(1, 7);
            bool IsDoubleRound = chanceOfDoubleRound == 3;

            #region The nightmare of the automatic respond after 13 seconds

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            int startSecond = DateTime.Now.Second;
            int startMinute = DateTime.Now.Minute;
            int endSecond;
            int endMinute;
            if (startSecond <= 47)
            {
                endMinute = startMinute;
                endSecond = startSecond + 13;
            }
            else
            {
                endMinute = startMinute + 1;
                endSecond = startSecond - 47;
            }
            double SecondsThatPassed = 0;
            SoundPlayer CategoryMusic = new SoundPlayer($@"You-think-you-are-smart\OtherSounds\CategoryMusic.wav");
            CategoryMusic.Play();
            string TheKeyPressed = "";

            while (!Console.KeyAvailable || (DateTime.Now.Second == endSecond && DateTime.Now.Minute == endMinute))
            {
                Thread.Sleep(100);
                SecondsThatPassed = SecondsThatPassed + 0.1;
                if (SecondsThatPassed >= 13)
                {
                    break;
                }
            }

            if (SecondsThatPassed >= 13)
            {
                if(File.Exists($@"You-think-you-are-smart\OtherSounds\choosecategory.wav"))
                {
                    File.Delete($@"You-think-you-are-smart\OtherSounds\choosecategory.wav");
                }

                CategoryMusic.Stop();
                CategoryMusic.Dispose();
                choosingthis.IsRandomCategoryHasBeenChoose = true;

                if (choosingthis.RandomChoiceIfNeeded == 0)
                {
                    choosingthis.choosenCategory = choosingthis.AllCategoriesNames.ElementAt(FirstCategoryDisplayed);
                    choosingthis.MoneyForThisCategoryQuestion = Money1stCategory;
                }
                if (choosingthis.RandomChoiceIfNeeded == 1)
                {
                    choosingthis.choosenCategory = choosingthis.AllCategoriesNames.ElementAt(SecondCategoryDisplayed);
                    choosingthis.MoneyForThisCategoryQuestion = Money2ndCategory;
                }

                CreatingAndPlayingAudio($@"You-think-you-are-smart\OtherSounds\CategoryGameChooses.wav", "Hey? Are you sleeping or something? I'll choose the category for you.");
                /*
                SoundPlayer ZahavChoosesCategory = new SoundPlayer($@"You-think-you-are-smart\OtherSounds\CategoryGameChooses.wav");
                ZahavChoosesCategory.Play();
                Thread.Sleep(5000);
                ZahavChoosesCategory.Dispose();
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                */
                Console.Clear();
                Console.Beep();
                if (choosingthis.RandomChoiceIfNeeded == 0)
                {
                    Console.WriteLine($@"1. {choosingthis.AllCategoriesNames.ElementAt(FirstCategoryDisplayed)} (${Money1stCategory})");
                    Thread.Sleep(1000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                }
                if (choosingthis.RandomChoiceIfNeeded == 1)
                {
                    Console.WriteLine($@"2. {choosingthis.AllCategoriesNames.ElementAt(SecondCategoryDisplayed)} (${Money2ndCategory})");
                    Thread.Sleep(1000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                }
            }

            else
            {
                if (File.Exists($@"You-think-you-are-smart\OtherSounds\choosecategory.wav"))
                {
                    File.Delete($@"You-think-you-are-smart\OtherSounds\choosecategory.wav");
                }

                choosingthis.IsRandomCategoryHasBeenChoose = false;
                ConsoleKeyInfo ChoosenCategory;

                //ChooseCategoryMusic.Play(); 
                if (Console.KeyAvailable == true)
                {
                    ChoosenCategory = Console.ReadKey(true);
                    while (ChoosenCategory.Key.ToString() != "D1" && ChoosenCategory.Key.ToString() != "D2" && ChoosenCategory.Key.ToString() != "Escape")
                    {
                        ChoosenCategory = Console.ReadKey(true);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                    }

                    CategoryMusic.Stop();
                    TheKeyPressed = ChoosenCategory.Key.ToString();
                    Console.Beep();
                    Console.Clear();

                    if (TheKeyPressed == "D1")
                    {
                        Console.Beep();
                        Console.WriteLine($@"1. {choosingthis.AllCategoriesNames.ElementAt(FirstCategoryDisplayed)} (${Money1stCategory})");
                        Thread.Sleep(1000);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                        choosingthis.choosenCategory = choosingthis.AllCategoriesNames.ElementAt(FirstCategoryDisplayed);

                        if (IsDoubleRound)
                        {
                            choosingthis.MoneyForThisCategoryQuestion = Money1stCategory * 2;
                        }
                        /*
                        else
                        {
                          */
                        choosingthis.MoneyForThisCategoryQuestion = Money1stCategory;
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                        //}

                    }
                    if (TheKeyPressed == "D2")
                    {
                        Console.Beep();
                        Console.WriteLine($@"2. {choosingthis.AllCategoriesNames.ElementAt(SecondCategoryDisplayed)} $({Money2ndCategory})");
                        Thread.Sleep(1000);
                        choosingthis.choosenCategory = choosingthis.AllCategoriesNames.ElementAt(SecondCategoryDisplayed);

                        if (IsDoubleRound)
                        {
                            choosingthis.MoneyForThisCategoryQuestion = Money2ndCategory * 2;
                        }

                        choosingthis.MoneyForThisCategoryQuestion = Money2ndCategory;
                        //}
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                    }

                    #region If the player pauses the game..

                    if (TheKeyPressed == "Escape")
                    {
                        Console.Beep();
                        Console.Clear();
                        Console.WriteLine("The game has been paused.");
                        Console.WriteLine("Press enter to continue the game.");
                        Console.WriteLine("Press space for a new game with the same players.");
                        Console.WriteLine("Press e to exit.");
                        Console.WriteLine("Press n for a new game without the same players.");
                        ConsoleKeyInfo choiceOfEscape = Console.ReadKey(true);
                        string choiceEscapeText = choiceOfEscape.Key.ToString();
                        while (choiceEscapeText.ToUpper() != "N" && choiceEscapeText.ToUpper() != "E" && choiceEscapeText != "Enter" && choiceEscapeText != "Spacebar")
                        {
                            choiceOfEscape = Console.ReadKey(true);
                            choiceEscapeText = choiceOfEscape.Key.ToString();
                            // this time let's just do nothing, no note
                        }

                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }

                        if (choiceEscapeText == "Enter")
                        {
                            choosingthis = ChooseCategoryFunction(game, PlayerToChoose);
                            return choosingthis;
                        }
                        if (choiceEscapeText == "Spacebar")
                        {
                            List<Player> players = new List<Player>();
                            for (int PlayerNum = 0; PlayerNum < game.GamePlayers.Count; PlayerNum++)
                            {
                                Player newPlayer = new Player(PlayerNum + 1, game.GamePlayers.ElementAt(PlayerNum).PlayerName);
                                players.Add(newPlayer);
                            }
                            Game NewGame = new Game(players, game.NumOfGameQuestions, players.Count);
                            Console.Clear();
                            NewGame.NextTurn(NewGame, NewGame.GamePlayers.ElementAt(0));
                        }
                        if (choiceEscapeText.ToUpper() == "E")
                        {
                            Environment.Exit(0);
                        }
                        if (choiceEscapeText.ToUpper() == "N")
                        {
                            return null;
                        }
                    }

                    #endregion
                }
            }

            if (DateTime.Now.Second == endSecond && DateTime.Now.Minute == endMinute)
            {
                // I think this is what happens if the user doesn't skip the instructions
                if (choosingthis.RandomChoiceIfNeeded == 0)
                {
                    choosingthis.choosenCategory = choosingthis.AllCategoriesNames.ElementAt(FirstCategoryDisplayed);
                }
                if (choosingthis.RandomChoiceIfNeeded == 1)
                {
                    choosingthis.choosenCategory = choosingthis.AllCategoriesNames.ElementAt(SecondCategoryDisplayed);
                }
            }

            #endregion

            CreatingAndPlayingAudio($@"You-think-you-are-smart\CategorySounds\QuestionIsAbout...wav", "The question is abouttttt..");
            /*
            SoundPlayer QuestionAbout = new SoundPlayer($@"You-think-you-are-smart\CategorySounds\QuestionIsAbout...wav");
            QuestionAbout.Play();
            Thread.Sleep(2000);
            QuestionAbout.Dispose();
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }
            */

            CreatingAndPlayingAudio($@"You-think-you-are-smart\CategorySounds\{choosingthis.choosenCategory}.wav", choosingthis.choosenCategory);
            /*
            SoundPlayer Topic = new SoundPlayer($@"You-think-you-are-smart\CategorySounds\{choosingthis.choosenCategory}.wav");
            Topic.Play();
            Thread.Sleep(3000);
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }
            Topic.Stop();
            */

            CreatingAndPlayingAudio($@"You-think-you-are-smart\CategorySounds\1000dollars.wav", "We're playing on only 1000 dollars.", null, true);
            CreatingAndPlayingAudio($@"You-think-you-are-smart\CategorySounds\2000dollars.wav", "We have on the garbage can 2000 dollars.", null, true);
            CreatingAndPlayingAudio($@"You-think-you-are-smart\CategorySounds\3000dollars.wav", "We have on the garbage can 3000 dollars.", null, true);
            CreatingAndPlayingAudio($@"You-think-you-are-smart\CategorySounds\4000dollars.wav", "We have on the garbage can 4000 dollars.", null, true);
            CreatingAndPlayingAudio($@"You-think-you-are-smart\CategorySounds\6000dollars.wav", "Answer right and you will have almost over 9000.. I mean, 6000 dollars.", null, true);

            SoundPlayer money1000 = new SoundPlayer($@"You-think-you-are-smart\CategorySounds\1000dollars.wav");
            SoundPlayer money2000 = new SoundPlayer($@"You-think-you-are-smart\CategorySounds\2000dollars.wav");
            SoundPlayer money3000 = new SoundPlayer($@"You-think-you-are-smart\CategorySounds\3000dollars.wav");
            SoundPlayer money4000 = new SoundPlayer($@"You-think-you-are-smart\CategorySounds\4000dollars.wav");
            SoundPlayer money6000 = new SoundPlayer($@"You-think-you-are-smart\CategorySounds\6000dollars.wav");

            if (choosingthis.MoneyForThisCategoryQuestion == 1000)
            {
                money1000.PlaySync();
                //Thread.Sleep(3000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                money1000.Dispose();
                File.Delete(money1000.SoundLocation);
            }
            else if (choosingthis.MoneyForThisCategoryQuestion == 2000)
            {
                money2000.PlaySync();
                //Thread.Sleep(3000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                money2000.Dispose();
                File.Delete(money2000.SoundLocation);
            }
            else if (choosingthis.MoneyForThisCategoryQuestion == 3000)
            {
                money3000.PlaySync();
                //Thread.Sleep(3000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                money3000.Dispose();
                File.Delete(money3000.SoundLocation);
            }
            else if (choosingthis.MoneyForThisCategoryQuestion == 4000)
            {
                money4000.PlaySync();
                //Thread.Sleep(3000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                money4000.Dispose();
                File.Delete(money4000.SoundLocation);
            }
            else if (choosingthis.MoneyForThisCategoryQuestion == 6000)
            {
                money6000.PlaySync();
                //Thread.Sleep(3000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                money6000.Dispose();
                File.Delete(money6000.SoundLocation);
            }


            Console.Clear();
            string BlankSpace = "";
            for (int i = 0; i < 25; i++)
            {
                BlankSpace = BlankSpaces(i + 1);
                Console.Write($@"{BlankSpace}Question {game.CountOfQuestion}");
                Thread.Sleep(40);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                Console.Clear();
            }

            Console.Clear();


            if (IsDoubleRound)
            {
                Console.WriteLine("The double round!");
                if (!choosingthis.IsRandomCategoryHasBeenChoose)
                {
                    if (TheKeyPressed == "D1")
                    {
                        choosingthis.MoneyForThisCategoryQuestion = Money1stCategory * 2;
                    }
                    if (TheKeyPressed == "D2")
                    {
                        choosingthis.MoneyForThisCategoryQuestion = Money2ndCategory * 2;
                    }
                }
                if (choosingthis.IsRandomCategoryHasBeenChoose)
                {
                    if (choosingthis.RandomChoiceIfNeeded == 0)
                    {
                        choosingthis.MoneyForThisCategoryQuestion = Money1stCategory * 2;
                    }
                    if (choosingthis.RandomChoiceIfNeeded == 1)
                    {
                        choosingthis.MoneyForThisCategoryQuestion = Money2ndCategory * 2;
                    }
                }

                CreatingAndPlayingAudio($@"You-think-you-are-smart\CategorySounds\DoubleRound.wav", "Prepare yourself for the double round! Every sum of money is doubled! But the money still won't be over 9000!.");
                /*
                SoundPlayer DoubleRound = new SoundPlayer($@"You-think-you-are-smart\CategorySounds\DoubleRound.wav");
                DoubleRound.Play();
                Thread.Sleep(3500);
                DoubleRound.Dispose();
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                */
                if (choosingthis.MoneyForThisCategoryQuestion == 1000)
                {
                    money1000.PlaySync();
                    //Thread.Sleep(3000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                }
                else if (choosingthis.MoneyForThisCategoryQuestion == 2000)
                {
                    money2000.PlaySync();
                    //Thread.Sleep(3000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                }
                else if (choosingthis.MoneyForThisCategoryQuestion == 3000)
                {
                    money3000.PlaySync();
                    //Thread.Sleep(3000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                }
                else if (choosingthis.MoneyForThisCategoryQuestion == 4000)
                {
                    money4000.PlaySync();
                    //Thread.Sleep(3000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                }
                else if (choosingthis.MoneyForThisCategoryQuestion == 6000)
                {
                    money6000.PlaySync();
                    //Thread.Sleep(3000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                }
            }

            money1000.Dispose();
            money2000.Dispose();
            money3000.Dispose();
            money4000.Dispose();
            money6000.Dispose();

            //choosingthis.PlayerNumber;

            int PlayerChoosingNext = 0;
            Console.Clear();

            if (PlayerToChoose.PlayerNumber < 3)
            {
                if (game.GamePlayers.Count == 1)
                {
                    choosingthis.PlayerNumber = 1;
                }
                if (GamePlayers.Count == 2)
                {
                    if (choosingthis.playerTurn.PlayerName == game.GamePlayers.ElementAt(1).PlayerName)
                    {
                        choosingthis.PlayerNumber = 1;
                    }
                    if (choosingthis.playerTurn.PlayerName == game.GamePlayers.ElementAt(0).PlayerName)
                    {
                        choosingthis.PlayerNumber = 2;
                    }
                }
                if (GamePlayers.Count == 3)
                {
                    if (choosingthis.playerTurn.PlayerName == game.GamePlayers.ElementAt(0).PlayerName)
                    {
                        choosingthis.PlayerNumber = 2;
                    }
                    if (choosingthis.playerTurn.PlayerName == game.GamePlayers.ElementAt(1).PlayerName)
                    {
                        choosingthis.PlayerNumber = 3;
                    }
                    if (choosingthis.playerTurn.PlayerName == game.GamePlayers.ElementAt(2).PlayerName)
                    {
                        choosingthis.PlayerNumber = 1;
                    }
                }
            }

            if (PlayerToChoose.PlayerNumber == 3)
            {
                choosingthis.PlayerNumber = 1;
            }

            if(File.Exists($@"You-think-you-are-smart\OtherSounds\choosecategory.wav"))
            {
                File.Delete($@"You-think-you-are-smart\OtherSounds\choosecategory.wav");
            }

            return choosingthis;

            // Now call the "Normal game questioning"..

            //SoundPlayer Money = new SoundPlayer($@"");

            /*
            ConsoleKeyInfo ChoosenCategory = Console.ReadKey(true);
            while (ChoosenCategory.Key.ToString() != "D1" && ChoosenCategory.Key.ToString() != "D2")
            {
                Console.WriteLine("Press on a number between 1-2 only!");
                ChoosenCategory = Console.ReadKey(true);
            }
            */


            //Console.Clear();
            //Console.WriteLine($@"The question is about: {}");

        }

        #endregion

        #region Timer for all modes but trap mode

        public static void RunTimerForNormalModeEveryone()
        {
            int count = 0;
            while (!_stopRequestedForEveryoneAnswering && count < 10)
            {
                for (int i = 0; i <= 10; i++)
                {
                    count = i;
                    Thread.Sleep(1000); // Wait for one second
                }
            }

        }

        public static void RunTimerForNormalModeSomeone()
        {
            int count = 0;

            while (!_stopRequestedForSomeoneAnswering && count < 10)
            {
                for (int i = 0; i <= 10; i++)
                {
                    Console.Write($@" {10 - i}");
                    count = i;
                    Thread.Sleep(1000); // Wait for one second
                }
            }

        }

        public static void RunTimerForWhoAmIAndCorrectOrFalseModes()
        {
            for (int i = 0; i <= 10; i++)
            {
                Console.Write($@" {10 - i}");
                Thread.Sleep(1000); // Wait for one second
            }
        }

        #endregion

        #region Thread for playing game over music

        public static void RunGameOverMusic()
        {

        }

        #endregion

        #region Normal question happening

        public Game NormalQuestion(Game game, ChooseCategory QuestionDetails, bool HasTheQuestionBeenReadItself, bool IsANewQuestionLoaded, int TheChoosenQuestion)
        {
            if (IsANewQuestionLoaded)
            {
                for (int i = 0; i < game.GamePlayers.Count; i++)
                {
                    game.GamePlayers.ElementAt(i).HasThePlayerChoosenAnswer = false;
                    game.GamePlayers.ElementAt(i).WrongAnswerChoosen = 0;
                }
                /*
                foreach(Player player in game.GamePlayers)
                {
                    player.HasThePlayerChoosenAnswer = false;
                    player.WrongAnswerChoosen = 0;
                }
                */
            }

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            NormalQuestion TheQuestion = new NormalQuestion(game, QuestionDetails.MoneyForThisCategoryQuestion, "", game.CountOfQuestion, 0, 0, "", QuestionDetails.choosenCategory);

            #region Opening object data            

            DirectoryInfo SubjectTextDirectory = new DirectoryInfo($@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}");
            //SubjectTextDirectory.
            DirectoryInfo[] AllQuestionsFolders = SubjectTextDirectory.GetDirectories();

            int ChoosenQuestion = 0;
            int AmountOfQuestions = AllQuestionsFolders.Count();

            if (!HasTheQuestionBeenReadItself)
            {
                Random rnd = new Random();
                ChoosenQuestion = rnd.Next(0, AmountOfQuestions);
                TheChoosenQuestion = ChoosenQuestion;
            }
            else
            {
                ChoosenQuestion = TheChoosenQuestion;
            }

            List<DirectoryInfo> QuestionsFoldersList = AllQuestionsFolders.ToList();
            string Answer1Text = "";
            string Answer2Text = "";
            string Answer3Text = "";
            string Answer4Text = "";
            List<string> AllAnswers = new List<string>();
            DirectoryInfo CorrectFolder = new DirectoryInfo($@"You-think-you-are-smart");

            foreach (DirectoryInfo Folder in AllQuestionsFolders)
            {
                if (ChoosenQuestion == QuestionsFoldersList.IndexOf(Folder))
                {

                    DirectoryInfo ChoosenFolder = new DirectoryInfo($@"{QuestionsFoldersList.Find(x => x == Folder)}");
                    CorrectFolder = Folder;
                    FileInfo[] files2 = ChoosenFolder.GetFiles();
                    List<FileInfo> ListOfFiles2 = files2.ToList();

                    foreach (FileInfo file in ListOfFiles2)
                    {
                        if (file.Name != "Answer1.txt" && file.Name != "Answer2.txt" && file.Name != "Answer3.txt" && file.Name != "Answer4.txt")
                        {
                            //$@"C:\Users\zahav\OneDrive\Desktop\my own software\Standup3\Standup3\bin\Debug\net8.0\You-think-you-are-smart\QuestionsText\Subject3\Question5\Answer1.txt"
                            {
                                #region Openining its properties from the XML

                                XDocument QuestionXML = new XDocument(XDocument.Load($@"You think you are smart xml\Questions.xml"));
                                IEnumerable<XElement> AllQuestions = QuestionXML.Root.Elements("Question");
                                List<XElement> AllQuestionsList = AllQuestions.ToList();
                                List<XElement> TheSubjectQuestions = new List<XElement>();

                                foreach (XElement AnyQuestion in AllQuestionsList)
                                {
                                    XElement QuestionSubject = AnyQuestion.Element("Subject");
                                    if (QuestionSubject.Value == QuestionDetails.choosenCategory)
                                    {
                                        TheSubjectQuestions.Add(AnyQuestion);
                                    }
                                }


                                foreach (XElement question in TheSubjectQuestions)
                                {
                                    XElement QuestionNameElement = question.Element("Name");
                                    string QuestionName = QuestionNameElement.Value;

                                    XElement textPath = question.Element("Text");
                                    string XMLTextPath = textPath.Value;

                                    //if(XMLTextPath == $@"")
                                    string CheckedPath = $@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}\{QuestionsFoldersList.Find(x => x == Folder).Name}\{file.Name}";
                                    if (XMLTextPath == CheckedPath)
                                    {
                                        #region setting up the sound

                                        string WantedVoice2 = string.Empty;

                                        using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
                                        {
                                            PromptBuilder promptBuilder = new PromptBuilder();

                                            foreach (InstalledVoice voice in synthesizer.GetInstalledVoices())
                                            {
                                                if (voice.VoiceInfo.Name.Contains("David"))
                                                {
                                                    WantedVoice2 = voice.VoiceInfo.Name;
                                                }
                                            }
                                            synthesizer.SelectVoice(WantedVoice2);
                                            string[] AllQuestionText = File.ReadAllLines(XMLTextPath);
                                            foreach (string text in AllQuestionText)
                                            {
                                                promptBuilder.AppendText(text);
                                            }

                                            synthesizer.SetOutputToWaveFile($@"You-think-you-are-smart\QuestionSpeech\current{int.Parse(question.Element("Number").Value)}.wav");
                                            synthesizer.Speak(promptBuilder);
                                        }

                                        #endregion

                                        TheQuestion.QuestionNumber = int.Parse(question.Element("Number").Value);
                                        TheQuestion.SoundPath = $@"You-think-you-are-smart\QuestionSpeech\current{int.Parse(question.Element("Number").Value)}.wav";
                                        TheQuestion.CorrectAnswer = int.Parse(question.Element("Answer").Value);
                                        TheQuestion.QuestionNumber = int.Parse(question.Element("Number").Value);
                                        TheQuestion.Subject = question.Element("Subject").Value;
                                        TheQuestion.QuestionName = QuestionName;
                                        TheQuestion.QuestionText = XMLTextPath;
                                    }
                                }


                                #endregion

                                Answer1Text = File.ReadAllText($@"{QuestionsFoldersList.Find(x => x == Folder).FullName}\Answer1.txt");
                                Answer2Text = File.ReadAllText($@"{QuestionsFoldersList.Find(x => x == Folder).FullName}\Answer2.txt");
                                Answer3Text = File.ReadAllText($@"{QuestionsFoldersList.Find(x => x == Folder).FullName}\Answer3.txt");
                                Answer4Text = File.ReadAllText($@"{QuestionsFoldersList.Find(x => x == Folder).FullName}\Answer4.txt");

                                AllAnswers.Add(Answer1Text);
                                AllAnswers.Add(Answer2Text);
                                AllAnswers.Add(Answer3Text);
                                AllAnswers.Add(Answer4Text);

                                #region setting up the sound of answers

                                string WantedVoice = string.Empty;

                                using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
                                {
                                    PromptBuilder promptBuilder = new PromptBuilder();

                                    foreach (InstalledVoice voice in synthesizer.GetInstalledVoices())
                                    {
                                        if (voice.VoiceInfo.Name.Contains("David"))
                                        {
                                            WantedVoice = voice.VoiceInfo.Name;
                                        }
                                    }
                                    synthesizer.SelectVoice(WantedVoice);
                                    foreach (string text in AllAnswers)
                                    {
                                        promptBuilder.AppendText(text);
                                    }

                                    synthesizer.SetOutputToWaveFile($@"You-think-you-are-smart\QuestionSpeech\currentAnswers.wav");
                                    synthesizer.Speak(promptBuilder);
                                }

                                TheQuestion.PossibleAnswersSoundPath = ($@"You-think-you-are-smart\QuestionSpeech\currentAnswers.wav");

                                #endregion

                                break;
                            }
                        }
                    }
                }
            }

            /*
            FileInfo[] files = SubjectTextDirectory.GetFiles();

            List<FileInfo> ListOfFiles = files.ToList();
            foreach (FileInfo file in ListOfFiles)
            {
                if (ChoosenQuestion == ListOfFiles.IndexOf(file))
                {
                    TheQuestion.QuestionText = ($@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}\{file.Name}");

                    #region Openining its properties from the XML

                    XDocument QuestionXML = new XDocument(XDocument.Load($@"You think you are smart xml\Questions.xml"));
                    IEnumerable<XElement> AllQuestions = QuestionXML.Root.Elements("Question");
                    List<XElement> AllQuestionsList = AllQuestions.ToList();
                    foreach (XElement question in AllQuestionsList)
                    {
                        XElement textPath = question.Element("Text");
                        string XMLTextPath = textPath.Value;
                        if (XMLTextPath == TheQuestion.QuestionText)
                        {
                            TheQuestion.QuestionNumber = int.Parse(question.Element("Number").Value);
                            TheQuestion.SoundPath = question.Element("Sound").Value;
                            TheQuestion.CorrectAnswer = int.Parse(question.Element("Answer").Value);
                            TheQuestion.QuestionNumber = int.Parse(question.Element("Number").Value);
                            TheQuestion.Subject = question.Element("Subject").Value;
                        }
                    }


                    #endregion
                }
            }
            */

            Console.Clear();
            Console.WriteLine($@"{TheQuestion.Subject}                                                             ${TheQuestion.MoneyOnTable}");
            Console.WriteLine("");
            Console.WriteLine("");
            string[] QuestionActualTextArray = File.ReadAllLines(TheQuestion.QuestionText);
            /*
            bool ShouldItBeCopied = true;
            if(game.NormalAlreadyAskedQuestions.Count > 0)
            {
                foreach(NormalQuestion question in game.NormalAlreadyAskedQuestions)
                {
                    if(TheQuestion.QuestionName == question.QuestionName)
                    {
                        ShouldItBeCopied = false;
                    }
                }
            }
            if(ShouldItBeCopied)
            {
                game.NormalAlreadyAskedQuestions.Add(TheQuestion);
            }
            else
            {
                // regenrate a question
                while (!ShouldItBeCopied)
                {
                    Random rnd = new Random();
                    ChoosenQuestion = rnd.Next(0, AmountOfQuestions);
                    //TheQuestion = new NormalQuestion(game, )
                    foreach (NormalQuestion question in NormalAlreadyAskedQuestions)
                    {
                        if (TheQuestion.QuestionText == TheQuestion.QuestionText)
                        {
                            ShouldItBeCopied = true;
                        }
                    }
                }
                game.NormalAlreadyAskedQuestions.Add(TheQuestion);
            }
            */
            List<string> QuestionActualTextList = new List<string>();
            QuestionActualTextList = QuestionActualTextArray.ToList();

            #endregion

            #region Writing the question

            List<int> WrongAnswersGiven = new List<int>();
            foreach (Player player in game.GamePlayers)
            {
                if (player.HasThePlayerChoosenAnswer && player.WrongAnswerChoosen != 0)
                {
                    if (player.WrongAnswerChoosen == 1)
                    {
                        AllAnswers.Remove(Answer1Text);
                    }
                    if (player.WrongAnswerChoosen == 2)
                    {
                        AllAnswers.Remove(Answer2Text);
                    }
                    if (player.WrongAnswerChoosen == 3)
                    {
                        AllAnswers.Remove(Answer3Text);
                    }
                    if (player.WrongAnswerChoosen == 4)
                    {
                        AllAnswers.Remove(Answer4Text);
                    }
                }

            }

            foreach (string QuestionActualContent in QuestionActualTextList)
            {
                Console.WriteLine($@"{QuestionActualContent}");
            }
            Console.WriteLine("");

            foreach (string answer in AllAnswers)
            {
                Console.WriteLine($"{answer}");
            }

            bool Player1AnsweredOrNot = false;
            bool Player2AnsweredOrNot = false;
            bool Player3AnsweredOrNot = true;

            if (game.GamePlayers.Count == 1)
            {
                Player1AnsweredOrNot = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
            }
            else if (game.GamePlayers.Count == 2)
            {
                Player1AnsweredOrNot = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                Player2AnsweredOrNot = game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer;
            }
            else if (game.GamePlayers.Count == 3)
            {
                Player1AnsweredOrNot = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                Player2AnsweredOrNot = game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer;
                Player3AnsweredOrNot = game.GamePlayers.ElementAt(2).HasThePlayerChoosenAnswer;
            }

            Console.WriteLine("");
            if (game.GamePlayers.Count == 1)
            {
                if (!Player1AnsweredOrNot)
                {
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (Wrong..)");
                    Console.WriteLine("");
                }
            }

            if (game.GamePlayers.Count == 2)
            {
                if (!Player1AnsweredOrNot)
                {
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) (Wrong..)");
                    Console.WriteLine("");
                }

                if (!Player2AnsweredOrNot)
                {
                    Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) ({game.GamePlayers.ElementAt(1).PlayerBuzzer})");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) (Wrong..)");
                    Console.WriteLine("");
                }
            }

            if (game.GamePlayers.Count == 3)
            {
                if (!Player1AnsweredOrNot)
                {
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) (Wrong..)");
                    Console.WriteLine("");
                }

                if (!Player2AnsweredOrNot)
                {
                    Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) ({game.GamePlayers.ElementAt(1).PlayerBuzzer})");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) (Wrong..)");
                    Console.WriteLine("");
                }

                if (!Player3AnsweredOrNot)
                {
                    Console.WriteLine($@"3. {game.GamePlayers.ElementAt(2).PlayerName} (${game.GamePlayers.ElementAt(2).PreTrapSum}) ({game.GamePlayers.ElementAt(2).PlayerBuzzer})");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine($@"3. {game.GamePlayers.ElementAt(2).PlayerName} (${game.GamePlayers.ElementAt(2).PreTrapSum}) (Wrong..)");
                }
            }

            #endregion

            // yuval wants hint for the normal questions as well
            // yuval wants a chaser or something
            Console.WriteLine();

            #region The nightmare of the automatic respond after 10 seconds

            #region Opening the basic time

            int startSecond = DateTime.Now.Second;
            int startMinute = DateTime.Now.Minute;
            int endSecond;
            int endMinute;
            if (startSecond <= 50)
            {
                endMinute = startMinute;
                endSecond = startSecond + 10;
            }
            else
            {
                endMinute = startMinute + 1;
                endSecond = startSecond - 50;
            }

            #endregion

            #region The time for the question itself to be read

            bool someoneanswered = false;
            ConsoleKeyInfo IfItWasAlreadyPressed = new ConsoleKeyInfo();
            foreach (Player player in game.GamePlayers)
            {
                if (player.HasThePlayerChoosenAnswer)
                {
                    someoneanswered = true;
                }
            }

            if (!someoneanswered)
            {
                //Console.WriteLine("5 seconds to read the question itself..");
                //Console.WriteLine("Next 10 seconds will be to actually answer.");
                double audiolength1 = 0;

                using (var questioncontentsound1 = new AudioFileReader($@"{TheQuestion.SoundPath}"))
                using (var outputDevice1 = new WaveOutEvent())
                {
                    double audioLengthMs1 = questioncontentsound1.TotalTime.TotalMilliseconds;
                    audiolength1 = audioLengthMs1;
                }
                /*
                using (var questioncontentsound = new AudioFileReader($@"{TheQuestion.SoundPath}"))
                using (var outputDevice = new WaveOutEvent())
                {
                    double audioLengthMs = questioncontentsound.TotalTime.TotalMilliseconds;
                    audiolength = audioLengthMs;
                    outputDevice.Init(questioncontentsound);
                    outputDevice.Play();
                }
                */

                SoundPlayer tryItAnotherWay = new SoundPlayer(TheQuestion.SoundPath);
                tryItAnotherWay.Play();

                double TimeThatPassed = 0;
                double secondstopass = Math.Round(audiolength1) / 1000;
                while (!Console.KeyAvailable && TimeThatPassed < secondstopass)
                {
                    Thread.Sleep(1000);
                    TimeThatPassed = TimeThatPassed + 1;

                    if (TimeThatPassed >= secondstopass)
                    {
                        break;
                    }
                }
                if (TimeThatPassed >= secondstopass)
                {
                    tryItAnotherWay.Dispose();
                    File.Delete($@"{TheQuestion.SoundPath}");

                    double audioanswerlength2 = 0;
                    using (var questioncontentsound2 = new AudioFileReader($@"{TheQuestion.PossibleAnswersSoundPath}"))
                    using (var outputDevice2 = new WaveOutEvent())
                    {
                        double audioLengthMs2 = questioncontentsound2.TotalTime.TotalMilliseconds;
                        audioanswerlength2 = audioLengthMs2;
                    }

                    SoundPlayer TryItAnotherWay2 = new SoundPlayer($@"{TheQuestion.PossibleAnswersSoundPath}");
                    TryItAnotherWay2.Play();

                    double TimeThatPassedForAnswrs = 0;
                    double secondstopassForAnswer = Math.Round(audioanswerlength2) / 1000;

                    while (!Console.KeyAvailable && TimeThatPassedForAnswrs < secondstopassForAnswer)
                    {
                        Thread.Sleep(1000);
                        TimeThatPassedForAnswrs = TimeThatPassedForAnswrs + 1;

                        if (TimeThatPassedForAnswrs >= secondstopassForAnswer)
                        {
                            break;
                        }
                    }
                    if (Console.KeyAvailable)
                    {
                        TryItAnotherWay2.Stop();
                        TryItAnotherWay2.Dispose();
                        File.Delete($@"{TheQuestion.PossibleAnswersSoundPath}");
                        //WasAKeyAlreadyPressed = true;
                        //IfItWasAlreadyPressed = Console.ReadKey(true);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }
                    }
                    else
                    {
                        TryItAnotherWay2.Stop();
                        TryItAnotherWay2.Dispose();
                        File.Delete($@"{TheQuestion.PossibleAnswersSoundPath}");
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }
                    }
                }
                else
                {
                    tryItAnotherWay.Stop();
                    tryItAnotherWay.Dispose();
                    File.Delete($@"{TheQuestion.SoundPath}");
                    File.Delete($@"{TheQuestion.PossibleAnswersSoundPath}");
                    //WasAKeyAlreadyPressed = true;
                    //IfItWasAlreadyPressed = Console.ReadKey(true);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                    }
                }
                //SoundPlayer QuestionSaying = new SoundPlayer($@"{TheQuestion.SoundPath}");
                //QuestionSaying.Play();
                //Thread.Sleep(5000);
                //QuestionSaying.Dispose();
                /*
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                */
            }

            #endregion

            //Thread timerThread = new Thread(new ThreadStart(RunTimerForNormalModeEveryone));
            //timerThread.Start();

            /*

            if (WasAKeyAlreadyPressed)
            {
                if (IfItWasAlreadyPressed.Key.ToString().ToUpper() != "M" && IfItWasAlreadyPressed.Key.ToString().ToUpper() != "A" && IfItWasAlreadyPressed.Key.ToString().ToUpper() == "OEMPLUS")
                {
                                    
                        SoundPlayer buzzerexpection = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\HandlingIncorrectBuzzerPressing.wav");
                        buzzerexpection.Play();
                        Thread.Sleep(5000);
                        throw new Exception($@"Sorry, can't handle incorrect key press at this mode. I tried for over 20 hours to find a solution for this and failed. Restart the game");
                        
                            //Console.WriteLine("Press only on one of your buzzers - A/M/+");
                            // a sound can be put in here
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                            }
                            consoleKeyInfo = Console.ReadKey(true);
                            Console.Write($@"{10 - SecondsThatPassed}");
                            Thread.Sleep(1000);
                            SecondsThatPassed = SecondsThatPassed + 1;

                            //consoleKeyInfo = Console.ReadKey(true);
                        
                    

                }
            }

        */
            double SecondsThatPassed = 0;
            /*
            var audioFile = new AudioFileReader(@"You-think-you-are-smart\OtherSounds\BackGroundMusic.mp3");
            var outputDevice = new WaveOutEvent();

            outputDevice.Init(audioFile);
            outputDevice.Play();
            */
            SoundPlayer QuestionMusic = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\NormalQuestionMusic.wav");
            QuestionMusic.Play();
            string TheKeyPressed = "";
            Console.WriteLine("");
            ConsoleKeyInfo buzzer = new ConsoleKeyInfo();
            if (TheKeyPressed == "over 9000")
            {
                buzzer = IfItWasAlreadyPressed;
            }
            else
            {
                while (!Console.KeyAvailable && SecondsThatPassed < 10)
                {
                    Thread.Sleep(1000);
                    SecondsThatPassed = SecondsThatPassed + 1;
                    Console.Write($@"{10 - SecondsThatPassed}");

                    if (SecondsThatPassed == 10)
                    {
                        break;
                    }

                    (int left, int top) test = Console.GetCursorPosition();
                    Console.SetCursorPosition(test.left - 1, test.top);
                }
            }

            #region In case nobody answers to begin with

            if (SecondsThatPassed == 10)
            {
                if (TheKeyPressed != "over 9000")
                {
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                    }
                    /*
                    outputDevice.Stop();
                    outputDevice.Dispose();
                    audioFile.Dispose();
                    */
                    QuestionMusic.Stop();
                    QuestionMusic.Dispose();

                    CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\NoOneAnswered.wav", "Hey hey hey, stop sleeping! Do you think God will answer for you?");

                    /*
                    SoundPlayer NoAnswer = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\NoOneAnswered.wav");
                    NoAnswer.Play();
                    Thread.Sleep(6500);
                    NoAnswer.Dispose();
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                    */
                    Console.Clear();
                    Console.Beep();
                    Console.WriteLine($@"The correct answer (in my opinion) is:");
                    Console.WriteLine($@"");
                    string CorrectAnswer = "";
                    string CorrectAnswerContent = File.ReadAllText($@"{QuestionsFoldersList.Find(x => x == CorrectFolder).FullName}\Answer{TheQuestion.CorrectAnswer}.txt");
                    // ($@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}\{QuestionsFoldersList.Find(x => x == CorrectFolder).Name}\Answer{TheQuestion.CorrectAnswer}.txt");

                    Console.WriteLine($@"{CorrectAnswerContent}");

                    Thread.Sleep(2000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }

                    #region Correct the answer if you want

                    Console.WriteLine();
                    Console.WriteLine($@"If you think my answer is wrong, you can correct it. Press c to do so, or press enter to continue.");
                    Console.CursorVisible = false;
                    ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                    while(consoleKeyInfo.Key.ToString().ToUpper() != "ENTER" && consoleKeyInfo.Key.ToString().ToUpper() != "C")
                    {
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                        consoleKeyInfo = Console.ReadKey(true);
                    }
                    if(consoleKeyInfo.Key.ToString().ToUpper() == "ENTER")
                    {
                        Console.CursorVisible = true;
                        // continue than..
                    }
                    if (consoleKeyInfo.Key.ToString().ToUpper() == "C")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Ok than, write the correct answer here. Your answer number will remain, just its text would change:");
                        Console.WriteLine();
                        Console.CursorVisible = true;
                        string AnswerOpinion = Console.ReadLine();                        
                        while(AnswerOpinion == string.Empty)
                        {
                            AnswerOpinion = Console.ReadLine();
                        }

                        #region Update..

                        UpdateAnswer("Normal", AnswerOpinion, TheQuestion);
                        Console.WriteLine();
                        Console.WriteLine("Change of answer applied succesfully!");
                        Console.WriteLine();
                        Thread.Sleep(3000);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }

                        #endregion
                    }


                    #endregion

                    Console.Clear();
                    return game;
                }
                else
                {
                    return game;
                }
            }

            #endregion

            else
            {
                /*
                if(TheQuestion.AreEveryoneWrong)
                {
                    QuestionMusic.Stop();

                    SoundPlayer EveryoneWrong = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\EveryoneAreWrong.wav");
                    EveryoneWrong.Play();
                    Thread.Sleep(4000);

                    Console.Clear();
                    Console.Beep();
                }*/

                //ChooseCategoryMusic.Play(); 

                if (Console.KeyAvailable == true)
                {
                    ConsoleKeyInfo consoleKeyInfo = new ConsoleKeyInfo();

                    consoleKeyInfo = Console.ReadKey(true);

                    #region Throwing wrong key press                       

                    if (consoleKeyInfo.Key.ToString().ToUpper() != "A" && consoleKeyInfo.Key.ToString().ToUpper() != "M" && consoleKeyInfo.Key.ToString().ToUpper() != "OEMPLUS")
                    {
                        CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\HandlingIncorrectBuzzerPressing.wav", "Sorry. Can't handle incorrect buzzer pressing at the moment. Restart the game.");
                        /*
                        SoundPlayer buzzerexpection = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\HandlingIncorrectBuzzerPressing.wav");
                        buzzerexpection.Play();
                        Thread.Sleep(5000);
                        */
                        throw new Exception($@"Sorry, can't handle incorrect key press at this mode. I tried for over 20 hours to find a solution for this and failed. Restart the game");
                        /*
                         * 
                            //Console.WriteLine("Press only on one of your buzzers - A/M/+");
                            // a sound can be put in here
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                            }
                            consoleKeyInfo = Console.ReadKey(true);
                            Console.Write($@"{10 - SecondsThatPassed}");
                            Thread.Sleep(1000);
                            SecondsThatPassed = SecondsThatPassed + 1;

                            //consoleKeyInfo = Console.ReadKey(true);
                        */
                    }


                    #endregion

                    #region Ensuring relevant buzzer has been pressed


                    if (consoleKeyInfo.Key.ToString().ToUpper() == "M" || consoleKeyInfo.Key.ToString().ToUpper() == "A" || consoleKeyInfo.Key.ToString().ToUpper() == "OEMPLUS")
                    {
                        bool IsTheBuzzerCorrect = false;

                        if (game.GamePlayers.Count == 1)
                        {
                            if (consoleKeyInfo.Key.ToString().ToUpper() == "M")
                            {
                                IsTheBuzzerCorrect = true;
                            }
                        }
                        if (game.GamePlayers.Count == 2)
                        {
                            if (consoleKeyInfo.Key.ToString().ToUpper() == "M" || consoleKeyInfo.Key.ToString().ToUpper() == "OEMPLUS")
                            {
                                IsTheBuzzerCorrect = true;
                            }
                        }
                        if (game.GamePlayers.Count == 3)
                        {
                            if (consoleKeyInfo.Key.ToString().ToUpper() == "M" || consoleKeyInfo.Key.ToString().ToUpper() == "OEMPLUS" || consoleKeyInfo.Key.ToString().ToUpper() == "A")
                            {
                                IsTheBuzzerCorrect = true;
                            }
                        }

                        if (!IsTheBuzzerCorrect)
                        {
                            #region Throwing wrong key press                       

                            CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\HandlingIncorrectBuzzerPressing.wav", "Sorry. Can't handle incorrect buzzer pressing at the moment. Restart the game.");

                            throw new Exception($@"Sorry, can't handle incorrect key press at this mode. I tried for over 20 hours to find a solution for this and failed. Restart the game");
                            /*
                             * 
                                //Console.WriteLine("Press only on one of your buzzers - A/M/+");
                                // a sound can be put in here
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }
                                consoleKeyInfo = Console.ReadKey(true);
                                Console.Write($@"{10 - SecondsThatPassed}");
                                Thread.Sleep(1000);
                                SecondsThatPassed = SecondsThatPassed + 1;

                                //consoleKeyInfo = Console.ReadKey(true);
                            */

                            #endregion
                        }

                        #endregion

                        #region What to do when a player buzzers, before he answers..

                        string ActivatedBuzzer = consoleKeyInfo.Key.ToString().ToUpper();

                        #region If it's a player who already answered..

                        bool Player1AnsweredAlready = false;
                        bool Player2AnsweredAlready = false;
                        bool Player3AnsweredAlready = false;

                        if (game.GamePlayers.Count == 1)
                        {
                            Player1AnsweredAlready = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && ActivatedBuzzer == "M";
                        }
                        else if (game.GamePlayers.Count == 2)
                        {
                            Player1AnsweredAlready = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && ActivatedBuzzer == "M";
                            Player2AnsweredAlready = game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer && ActivatedBuzzer == "OEMPLUS";
                        }
                        else if (game.GamePlayers.Count == 3)
                        {
                            Player1AnsweredAlready = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && ActivatedBuzzer == "M";
                            Player2AnsweredAlready = game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer && ActivatedBuzzer == "OEMPLUS";
                            Player3AnsweredAlready = game.GamePlayers.ElementAt(2).HasThePlayerChoosenAnswer && ActivatedBuzzer == "A";
                        }

                        if (game.GamePlayers.Count == 2)
                        {
                            if (Player1AnsweredAlready || Player2AnsweredAlready)
                            {
                                CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\PressingBuzzerAgainException.wav", "Smartass! You've already answered this question! So you just maniacally crashed the game again."); ;

                                throw new Exception(" The sound you just heard explains this.. Restart the game. ");
                            }
                        }
                        if (game.GamePlayers.Count == 3)
                        {
                            if (Player1AnsweredAlready || Player2AnsweredAlready || Player3AnsweredAlready)
                            {
                                CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\PressingBuzzerAgainException.wav", "Smartass! You've already answered this question! So you just maniacally crashed the game again."); ;

                                throw new Exception(" The sound you just heard explains this.. Restart the game. ");
                            }
                        }

                        #endregion

                        double SecondsForAnswer = 0;

                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                        Player playerAnswering = new Player(1, "");

                        ConsoleKeyInfo ChoosenAnswer = new ConsoleKeyInfo();
                        if (ActivatedBuzzer.ToUpper() == "M")
                        {
                            playerAnswering = new Player(game.GamePlayers.ElementAt(0).PlayerNumber, game.GamePlayers.ElementAt(0).PlayerName);
                            playerAnswering = game.GamePlayers.ElementAt(0);
                            playerAnswering.PlayerNumber = 1;
                        }
                        if (ActivatedBuzzer.ToUpper() == "A")
                        {
                            playerAnswering = new Player(game.GamePlayers.ElementAt(2).PlayerNumber, game.GamePlayers.ElementAt(2).PlayerName);
                            playerAnswering = game.GamePlayers.ElementAt(2);
                            playerAnswering.PlayerNumber = 3;
                        }
                        if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                        {
                            playerAnswering = new Player(game.GamePlayers.ElementAt(1).PlayerNumber, game.GamePlayers.ElementAt(1).PlayerName);
                            playerAnswering = game.GamePlayers.ElementAt(1);
                            playerAnswering.PlayerNumber = 2;
                        }

                        Console.Beep();
                        Console.WriteLine("");
                        Console.WriteLine($@"{game.GamePlayers.ElementAt(playerAnswering.PlayerNumber - 1).PlayerName} is answering..");
                        //Console.WriteLine("");

                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }

                        //Thread timerThread2 = new Thread(new ThreadStart(RunTimerForNormalModeSomeone));
                        //double SecondsForAnswer = 10;
                        //timerThread2.Start();
                        ConsoleKeyInfo anotherpress = new ConsoleKeyInfo();
                        while (!Console.KeyAvailable && SecondsForAnswer < 10)
                        {
                            Thread.Sleep(1000);
                            SecondsForAnswer = SecondsForAnswer + 1;
                            Console.Write($@"{10 - SecondsForAnswer}");

                            if (SecondsForAnswer == 10)
                            {
                                break;
                            }

                            (int left, int top) test = Console.GetCursorPosition();
                            Console.SetCursorPosition(test.left - 1, test.top);
                            /*

                            Console.Write($@"{10 - SecondsForAnswer}");
                            SecondsForAnswer = SecondsForAnswer + 1;
                            Thread.Sleep(1000);
                            (int left, int top) test2 = Console.GetCursorPosition();
                            Console.SetCursorPosition(test2.left - 1, test2.top);
                            */
                            // do nothing and remain stuck at this part of the code
                        }

                        #endregion


                        if (Console.KeyAvailable)
                        {
                            ChoosenAnswer = Console.ReadKey(true);

                            #region Waiting for proper input

                            #region Throwing wrong key press                       

                            if (ChoosenAnswer.Key.ToString().ToUpper() != "D1" && ChoosenAnswer.Key.ToString().ToUpper() != "D2" && ChoosenAnswer.Key.ToString() != "D3" && ChoosenAnswer.Key.ToString() != "D4")
                            {
                                CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\HandlingIncorrectBuzzerPressing.wav", "Sorry, can't handle incorrect buzzer pressing at the moment. Restart the game.");

                                throw new Exception($@" Sorry, can't handle incorrect key press at this mode. I tried for over 20 hours to find a solution for this and failed. Restart the game");
                                /*
                                 * 
                                    //Console.WriteLine("Press only on one of your buzzers - A/M/+");
                                    // a sound can be put in here
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }
                                    consoleKeyInfo = Console.ReadKey(true);
                                    Console.Write($@"{10 - SecondsThatPassed}");
                                    Thread.Sleep(1000);
                                    SecondsThatPassed = SecondsThatPassed + 1;

                                    //consoleKeyInfo = Console.ReadKey(true);
                                */
                            }


                            #endregion

                            /*
                             * 
                            while (ChoosenAnswer.Key.ToString() != "D1" && ChoosenAnswer.Key.ToString() != "D2" && ChoosenAnswer.Key.ToString() != "D3" && ChoosenAnswer.Key.ToString() != "D4" && SecondsForAnswer < 10)
                            {
                                ChoosenAnswer = Console.ReadKey(true);
                                while (ChoosenAnswer.Key.ToString() != "D1" && ChoosenAnswer.Key.ToString() != "D2" && ChoosenAnswer.Key.ToString() != "D3" && ChoosenAnswer.Key.ToString() != "D4")
                                {
                                    //Console.WriteLine("Press only on the answer numbers (1-4)!");
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }
                                    Thread.Sleep(1000);
                                    Console.Write($@"{10 - SecondsForAnswer}");
                                    SecondsForAnswer = SecondsForAnswer + 1;
                                }
                            }
                            */
                            #endregion

                        }

                        if (SecondsForAnswer == 10)
                        {
                            #region what happens if the player buzzers but doesn't answer
                            /*
                            outputDevice.Stop();
                            outputDevice.Dispose();
                            audioFile.Dispose();
                            */
                            QuestionMusic.Stop();
                            QuestionMusic.Dispose();

                            CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\NoOneAnswered.wav", "Hey hey hey! Stop sleeping! Do you think god will answer for you?");


                            if (ActivatedBuzzer.ToUpper() == "M")
                            {
                                playerAnswering = game.GamePlayers.ElementAt(0);
                                playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                            }
                            else if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                            {
                                playerAnswering = game.GamePlayers.ElementAt(1);
                                playerAnswering.PlayerName = game.GamePlayers.ElementAt(1).PlayerName;
                                playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(1).PlayerNumber;
                            }
                            else if (ActivatedBuzzer.ToUpper() == "A")
                            {
                                playerAnswering = game.GamePlayers.ElementAt(2);
                                playerAnswering.PlayerName = game.GamePlayers.ElementAt(2).PlayerName;
                                playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(2).PlayerNumber;
                            }

                            playerAnswering.PreTrapSum = playerAnswering.PreTrapSum - TheQuestion.MoneyOnTable;

                            Console.Beep();
                            Console.WriteLine();
                            Console.WriteLine("What, you thought you could get away with it?");
                            Console.WriteLine($@"{playerAnswering.PlayerName} now has ${playerAnswering.PreTrapSum}");
                            Console.WriteLine();
                            playerAnswering.HasThePlayerChoosenAnswer = true;
                            game.GamePlayers.ElementAt(playerAnswering.PlayerNumber - 1).HasThePlayerChoosenAnswer = true;
                            bool HasEveryoneAnswered = false;

                            if (game.GamePlayers.Count == 1)
                            {
                                HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                            }
                            if (game.GamePlayers.Count == 2)
                            {
                                HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer;
                            }
                            if (game.GamePlayers.Count == 3)
                            {
                                HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(2).HasThePlayerChoosenAnswer;
                            }
                            if (!HasEveryoneAnswered)
                            {
                                game = NormalQuestion(game, QuestionDetails, true, false, ChoosenQuestion);
                                return game;
                            }
                            else
                            {
                                Thread.Sleep(1000);
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                                }

                                CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\EveryoneAreWrong.wav", "After your patheic attempts, maybe I would be better off showing you my stupid opinion regarding the correct answer.");

                                Console.Clear();
                                Console.Beep();
                                Console.WriteLine($@"The correct answer (in my opinion) is:");
                                Console.WriteLine($@"");
                                string CorrectAnswer = "";
                                string CorrectAnswerContent = File.ReadAllText($@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}\{QuestionsFoldersList.Find(x => x == CorrectFolder).Name}\Answer{TheQuestion.CorrectAnswer}.txt");

                                Console.WriteLine($@"{CorrectAnswerContent}");

                                Thread.Sleep(2000);
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }

                                #region Correct the answer if you want

                                Console.WriteLine();
                                Console.WriteLine($@"If you think my answer is wrong, you can correct it. Press c to do so, or press enter to continue.");
                                Console.CursorVisible = false;
                                ConsoleKeyInfo consoleKeyInfo2 = Console.ReadKey(true);
                                while (consoleKeyInfo2.Key.ToString().ToUpper() != "ENTER" && consoleKeyInfo2.Key.ToString().ToUpper() != "C")
                                {
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }
                                    consoleKeyInfo2 = Console.ReadKey(true);
                                }
                                if (consoleKeyInfo2.Key.ToString().ToUpper() == "ENTER")
                                {
                                    Console.CursorVisible = true;
                                    // continue than..
                                }
                                if (consoleKeyInfo2.Key.ToString().ToUpper() == "C")
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Ok than, write the correct answer here. Your answer number will remain, just its text would change:");
                                    Console.WriteLine();
                                    Console.CursorVisible = true;
                                    string AnswerOpinion = Console.ReadLine();
                                    while (AnswerOpinion == string.Empty)
                                    {
                                        AnswerOpinion = Console.ReadLine();
                                    }

                                    #region Update..

                                    UpdateAnswer("Normal", AnswerOpinion, TheQuestion);
                                    Console.WriteLine();
                                    Console.WriteLine("Change of answer applied succesfully!");
                                    Console.WriteLine();
                                    Thread.Sleep(3000);
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }

                                    #endregion
                                }


                                #endregion


                                Console.Clear();
                                return game;
                            }

                            #endregion
                        }

                        if ((ChoosenAnswer.Key.ToString() == "D1" || ChoosenAnswer.Key.ToString() == "D2" || ChoosenAnswer.Key.ToString() == "D3" || ChoosenAnswer.Key.ToString() == "D4"))
                        {
                            #region When he does answer..

                            _stopRequestedForSomeoneAnswering = true;

                            playerAnswering.HasThePlayerChoosenAnswer = true;

                            if (ChoosenAnswer.Key.ToString() == $@"D{TheQuestion.CorrectAnswer.ToString()}")
                            {
                                #region if the answer is correct..

                                Console.WriteLine("");
                                Console.WriteLine($@"{TheQuestion.CorrectAnswer}");

                                if (ActivatedBuzzer.ToUpper() == "M")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(0);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                                }
                                else if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(1);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(1).PlayerName;
                                    playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(1).PlayerNumber;
                                }
                                else if (ActivatedBuzzer.ToUpper() == "A")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(2);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(2).PlayerName;
                                    playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(2).PlayerNumber;
                                }

                                playerAnswering.PreTrapSum = playerAnswering.PreTrapSum + TheQuestion.MoneyOnTable;

                                CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\CorrectAnswer.wav", "Holy yes shit!");

                                Console.WriteLine("");
                                Console.WriteLine("Oh yeah! Correct Answer!");
                                Console.WriteLine("");
                                Console.WriteLine($@"{playerAnswering.PlayerName} now has ${playerAnswering.PreTrapSum}.");
                                /* stopping by to explain sound file
                                 * SoundPlayer explanation = new SoundPlayer($@"path for explanation .wav file")
                                 * explanation.Play();
                                 * Thread.Sleep(sound file length);
                                 * than, no additional user kep press is required
                                 */
                                Console.WriteLine("Press enter to continue..");
                                Console.ReadLine();

                                #region Correct the answer if you want

                                Console.WriteLine();
                                Console.WriteLine($@"If you think my answer is wrong, you can correct it. Press c to do so, or press enter to continue.");
                                Console.CursorVisible = false;
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }

                                ConsoleKeyInfo consoleKeyInfo3 = Console.ReadKey(true);
                                while (consoleKeyInfo3.Key.ToString().ToUpper() != "ENTER" && consoleKeyInfo3.Key.ToString().ToUpper() != "C")
                                {
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }
                                    consoleKeyInfo3 = Console.ReadKey(true);
                                }
                                if (consoleKeyInfo3.Key.ToString().ToUpper() == "ENTER")
                                {
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }

                                    Console.CursorVisible = true;
                                    // continue than..
                                }
                                if (consoleKeyInfo3.Key.ToString().ToUpper() == "C")
                                {
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }

                                    Console.WriteLine();
                                    Console.WriteLine("Ok than, write the correct answer here. Your answer number will remain, just its text would change:");
                                    Console.WriteLine();
                                    Console.CursorVisible = true;
                                    string AnswerOpinion = Console.ReadLine();
                                    while (AnswerOpinion == string.Empty)
                                    {
                                        AnswerOpinion = Console.ReadLine();
                                    }

                                    #region Update..

                                    UpdateAnswer("Normal", AnswerOpinion, TheQuestion);
                                    Console.WriteLine();
                                    Console.WriteLine("Change of answer applied successfully!");
                                    Console.WriteLine();
                                    Thread.Sleep(3000);
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }

                                    #endregion
                                }


                                #endregion


                                Console.Clear();
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }
                                return game;

                                #endregion
                            }


                            if (ChoosenAnswer.Key.ToString() != $@"D{TheQuestion.CorrectAnswer.ToString()}")
                            {
                                #region If the answer is wrong..

                                _stopRequestedForSomeoneAnswering = true;

                                if (ActivatedBuzzer.ToUpper() == "M")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(0);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                                }
                                else if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(1);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(1).PlayerName;
                                    playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(1).PlayerNumber;
                                }
                                else if (ActivatedBuzzer.ToUpper() == "A")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(2);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(2).PlayerName;
                                    playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(2).PlayerNumber;
                                }

                                playerAnswering.PreTrapSum = playerAnswering.PreTrapSum - TheQuestion.MoneyOnTable;


                                Console.WriteLine("");
                                Console.WriteLine($@"{ChoosenAnswer.Key.ToString().Substring(1)}");

                                CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\IncorrectAnswer.wav", "This is the correct answer.. In your dreams.");

                                Console.Beep();
                                Console.WriteLine();
                                Console.WriteLine("Wrong!");
                                Console.WriteLine($@"{playerAnswering.PlayerName} now has ${playerAnswering.PreTrapSum}");
                                Console.WriteLine();
                                playerAnswering.HasThePlayerChoosenAnswer = true;
                                playerAnswering.WrongAnswerChoosen = int.Parse($@"{ChoosenAnswer.Key.ToString().Substring(1)}");

                                game.GamePlayers.ElementAt(playerAnswering.PlayerNumber - 1).HasThePlayerChoosenAnswer = true;
                                bool HasEveryoneAnswered = false;

                                if (game.GamePlayers.Count == 1)
                                {
                                    HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                                }
                                if (game.GamePlayers.Count == 2)
                                {
                                    HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer;
                                }
                                if (game.GamePlayers.Count == 3)
                                {
                                    HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(2).HasThePlayerChoosenAnswer;
                                }
                                if (!HasEveryoneAnswered)
                                {
                                    game = NormalQuestion(game, QuestionDetails, true, false, ChoosenQuestion);
                                    return game;
                                }
                                else
                                {
                                    Thread.Sleep(1000);
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                                    }

                                    CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\EveryoneAreWrong.wav", "After your failed attempts, it's time for me to show you my stupid opinion regarding the correct answer.");

                                    Console.Clear();
                                    Console.Beep();
                                    Console.WriteLine($@"The correct answer (in my opinion) is:");
                                    Console.WriteLine($@"");
                                    string CorrectAnswer = "";
                                    string CorrectAnswerContent = File.ReadAllText($@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}\{QuestionsFoldersList.Find(x => x == CorrectFolder).Name}\Answer{TheQuestion.CorrectAnswer}.txt");


                                    Console.WriteLine($@"{CorrectAnswerContent}");

                                    Thread.Sleep(2000);
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }

                                    #region Correct the answer if you want

                                    Console.WriteLine();
                                    Console.WriteLine($@"If you think my answer is wrong, you can correct it. Press c to do so, or press enter to continue.");
                                    Console.CursorVisible = false;
                                    ConsoleKeyInfo consoleKeyInfo4 = Console.ReadKey(true);
                                    while (consoleKeyInfo4.Key.ToString().ToUpper() != "ENTER" && consoleKeyInfo4.Key.ToString().ToUpper() != "C")
                                    {
                                        while (Console.KeyAvailable)
                                        {
                                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                        }
                                        consoleKeyInfo4 = Console.ReadKey(true);
                                    }
                                    if (consoleKeyInfo4.Key.ToString().ToUpper() == "ENTER")
                                    {
                                        Console.CursorVisible = true;
                                        // continue than..
                                    }
                                    if (consoleKeyInfo4.Key.ToString().ToUpper() == "C")
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine("Ok than, write the correct answer here. Your answer number will remain, just its text would change:");
                                        Console.WriteLine();
                                        Console.CursorVisible = true;
                                        string AnswerOpinion = Console.ReadLine();
                                        while (AnswerOpinion == string.Empty)
                                        {
                                            AnswerOpinion = Console.ReadLine();
                                        }

                                        #region Update..

                                        UpdateAnswer("Normal", AnswerOpinion, TheQuestion);
                                        Console.WriteLine();
                                        Console.WriteLine("Change of answer applied succesfully!");
                                        Console.WriteLine();
                                        Thread.Sleep(3000);
                                        while (Console.KeyAvailable)
                                        {
                                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                        }

                                        #endregion
                                    }


                                    #endregion


                                    Console.Clear();
                                    return game;
                                }




                                // Now to continue to the other buzzers..

                                #endregion
                            }
                        }
                    }
                }
                //Thread timerThread2 = new Thread(new ThreadStart(RunTimerForNormalMode));
                //ChoosenCategory = Console.ReadKey(true);


                //TheKeyPressed = consoleKeyInfo.Key.ToString();


                Console.Beep();
                return game;

                //Thread.Sleep(1000);
                /*
                if(IsDoubleRound)
                {

                choosingthis.MoneyForThisCategoryQuestion = Money1stCategory * 2;
                */
                /*
                else
                {
                  */
                //QuestionDetails.MoneyForThisCategoryQuestion = Money1stCategory;
                //}

            }

            /*
            if (TheKeyPressed == "D2")
            {
                Console.Beep();
                //Console.WriteLine($@"2. {QuestionDetails.AllCategoriesNames.ElementAt(SecondCategoryDisplayed)} ({Money2ndCategory} dollars)");
                Thread.Sleep(1000);
                //QuestionDetails.choosenCategory = QuestionDetails.AllCategoriesNames.ElementAt(SecondCategoryDisplayed);
                /*
                if (IsDoubleRound)
                {
                    choosingthis.MoneyForThisCategoryQuestion = Money2ndCategory * 2;
                }
                else
                {
                 */
            //QuestionDetails.MoneyForThisCategoryQuestion = Money2ndCategory;
            //}

            //}



            /*while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
            }*/



        }
        /*
        if (DateTime.Now.Second == endSecond && DateTime.Now.Minute == endMinute)
        {
            // I think this is what happens if the user doesn't skip the instructions
            if (QuestionDetails.RandomChoiceIfNeeded == 0)
            {
                QuestionDetails.choosenCategory = QuestionDetails.AllCategoriesNames.ElementAt(FirstCategoryDisplayed);
            }
            if (QuestionDetails.RandomChoiceIfNeeded == 1)
            {
                QuestionDetails.choosenCategory = QuestionDetails.AllCategoriesNames.ElementAt(SecondCategoryDisplayed);
            }
        }
        */
        #endregion

        #endregion

        #endregion

        #region Trap mode

        public Game TrapMode(Game game)
        {
            Console.Clear();

            Trap trap = new Trap(game);

            #region Instructions

            #region Instructions part

            #region The general instructions and buzzers

            #region Instructions

            Console.Clear();
            Console.WriteLine("We are now going into trap mode. ");


            Console.WriteLine("Now, The general instructions.. you can hear them..");
            Console.WriteLine();
            Console.WriteLine("Press any key to skip this repetitive shit.");

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            string TrapInstructions = "Now you're gonna get trapped in a black hole.. I mean, in the middle of nowhere. Where many questions will appear. To each question, every few seconds, a possible answer will appear. Buzzer when the correct answer appears. Your money before entering the trap will not be effected either way. Answer wrong and you don't get any money from the trap.";

            //PlayingAndWaitingForAudio(trap.FilePathToInstructions, @"You-think-you-are-smart\OtherSounds\SkippedInstructions.wav");

            CreatingAndPlayingAudio(trap.FilePathToInstructions, TrapInstructions, null, true);
            CreatingAndPlayingAudio(@"You-think-you-are-smart\OtherSounds\SkippedInstructions.wav", "Ouch! It hurts! I'm gonna have another seizure!", null, true);

            PlayingAndWaitingForAudio(trap.FilePathToInstructions, @"You-think-you-are-smart\OtherSounds\SkippedInstructions.wav");
            /*
            SoundPlayer InstructionsAudio = PlayingInMultiThreading(trap.FilePathToInstructions, TrapInstructions);
            SoundPlayer InstructionsAudio = new SoundPlayer(trap.FilePathToInstructions);
            double SecondsThatPassedInstructions = 0;
            int startSecond = DateTime.Now.Second;
            int startMinute = DateTime.Now.Minute;
            int endSecond;
            int endMinute;
            if (startSecond <= 43)
            {
                endMinute = startMinute;
                endSecond = startSecond + 17;
            }
            else
            {
                endMinute = startMinute + 1;
                endSecond = startSecond - 43;
            }

            double audioLength = 0;

            using (var audioFile = new AudioFileReader(trap.FilePathToInstructions))
            using (var outputDevice1 = new WaveOutEvent())
            {
                double audioLengthMs1 = audioFile.TotalTime.TotalMilliseconds;
                audioLength = Math.Round(audioLengthMs1);
            }



            InstructionsAudio.Play();
            while (!Console.KeyAvailable && SecondsThatPassedInstructions < (audioLength / 1000))
            {
                Thread.Sleep(100);
                SecondsThatPassedInstructions = SecondsThatPassedInstructions + 0.1;
            }
            SoundPlayer InstructionsOtherAudio = new SoundPlayer(@"You-think-you-are-smart\OtherSounds\SkippedInstructions.wav");
            if (Console.KeyAvailable == true)
            {
                InstructionsAudio.Stop();
                InstructionsAudio.Dispose();
                InstructionsOtherAudio.Play();
                Thread.Sleep(3750);
                InstructionsOtherAudio.Dispose();
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }

                Console.Clear();
            }
            if (SecondsThatPassedInstructions >= 18)
            {
                Console.Clear();
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                InstructionsAudio.Dispose();
            }
            */
            #endregion


            #endregion

            #endregion

            #endregion

            #region Loop of questions

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
            }

            Random rnd = new Random();
            List<TrapQuestion> TrapQuestionsAnswered = new List<TrapQuestion>();

            for (int i = 0; i < 4; i++)
            {
                Console.Clear();

                int ChoosenQuestion = rnd.Next(0, trap.Questions.Count);
                TrapQuestion question = trap.AllQuestions.ElementAt(ChoosenQuestion);
                while (TrapQuestionsAnswered.Contains(question))
                {
                    ChoosenQuestion = rnd.Next(0, trap.Questions.Count);
                    question = trap.AllQuestions.ElementAt(ChoosenQuestion);
                }

                TrapQuestionsAnswered.Add(question);

                #region The time for the question itself to be read

                Console.WriteLine("");
                //Console.WriteLine("4 seconds to read the question itself..");
                //Console.WriteLine("Next 5 seconds will be to actually answer.");
                Console.WriteLine($@"{question.QuestionText}");
                Thread.Sleep(2000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }


                #endregion

                #region Writing the answers and responses

                List<int> WrongAnswersGiven = new List<int>();

                Console.WriteLine("");

                for (int PossibleAnswer = 0; PossibleAnswer < 4; PossibleAnswer++)
                {
                    Console.Clear();


                    Console.WriteLine($@"Question: {question.QuestionText}");

                    Console.WriteLine("");

                    Console.WriteLine($@"Answer: {question.PossibleAnswers.ElementAt(PossibleAnswer)}");
                    Console.WriteLine("");
                    Console.WriteLine("");

                    if (game.GamePlayers.Count == 1)
                    {

                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).TrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                        Console.WriteLine("");
                    }

                    if (game.GamePlayers.Count == 2)
                    {
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).TrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                        Console.WriteLine("");
                        Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).TrapSum}) ({game.GamePlayers.ElementAt(1).PlayerBuzzer})");
                        Console.WriteLine("");
                    }

                    if (game.GamePlayers.Count == 3)
                    {
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).TrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                        Console.WriteLine("");
                        Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).TrapSum}) ({game.GamePlayers.ElementAt(1).PlayerBuzzer})");
                        Console.WriteLine("");
                        Console.WriteLine($@"3. {game.GamePlayers.ElementAt(2).PlayerName} (${game.GamePlayers.ElementAt(2).TrapSum}) ({game.GamePlayers.ElementAt(2).PlayerBuzzer})");
                    }

                    #endregion

                    #region The nightmare of the timer

                    double SecondsThatPassed = 0;
                    string TheKeyPressed = "";
                    Console.WriteLine("");
                    ConsoleKeyInfo buzzer = new ConsoleKeyInfo();
                    //timerThread.Start();

                    while (!Console.KeyAvailable && SecondsThatPassed < 2.5)
                    {
                        Thread.Sleep(500);
                        SecondsThatPassed = SecondsThatPassed + 0.5;
                        if (SecondsThatPassed == 2.5)
                        {
                            break;
                        }
                    }

                    if (SecondsThatPassed >= 2.5)
                    {

                    }

                    else if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                        TheKeyPressed = consoleKeyInfo.Key.ToString();

                        #region Throwing wrong key press

                        if (consoleKeyInfo.Key.ToString().ToUpper() != "A" && consoleKeyInfo.Key.ToString().ToUpper() != "M" && consoleKeyInfo.Key.ToString().ToUpper() != "OEMPLUS")
                        {
                            CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\HandlingIncorrectBuzzerPressing.wav", "Sorry. Can't handle incorrect buzzer pressing at the moment. Restart the game.");

                            throw new Exception($@"Sorry, can't handle incorrect key press at this mode. I tried for over 20 hours to find a solution for this and failed. Restart the game");
                        }

                        #endregion

                        else
                        {
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }

                            #region What to do when a player buzzers

                            string ActivatedBuzzer = consoleKeyInfo.Key.ToString().ToUpper();

                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                            }

                            Player playerAnswering = new Player(1, "");

                            int ChoosenAnswer = 0;
                            ChoosenAnswer = PossibleAnswer;

                            if (ActivatedBuzzer.ToUpper() == "M")
                            {
                                playerAnswering = new Player(game.GamePlayers.ElementAt(0).PlayerNumber, game.GamePlayers.ElementAt(0).PlayerName);
                                playerAnswering = game.GamePlayers.ElementAt(0);
                                playerAnswering.PlayerNumber = 1;
                            }
                            if (ActivatedBuzzer.ToUpper() == "A")
                            {
                                playerAnswering = new Player(game.GamePlayers.ElementAt(2).PlayerNumber, game.GamePlayers.ElementAt(2).PlayerName);
                                playerAnswering = game.GamePlayers.ElementAt(2);
                                playerAnswering.PlayerNumber = 3;
                            }
                            if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                            {
                                playerAnswering = new Player(game.GamePlayers.ElementAt(1).PlayerNumber, game.GamePlayers.ElementAt(1).PlayerName);
                                playerAnswering = game.GamePlayers.ElementAt(1);
                                playerAnswering.PlayerNumber = 2;
                            }

                            if (playerAnswering.TrapSum == 0)
                            {
                                question.MoneyOnTable = 1000;
                            }
                            if (playerAnswering.TrapSum == 1000)
                            {
                                question.MoneyOnTable = 500;
                            }
                            if (playerAnswering.TrapSum != 1000 && playerAnswering.TrapSum != 0)
                            {
                                question.MoneyOnTable = playerAnswering.TrapSum;
                            }

                            Console.Beep();
                            Console.WriteLine("");

                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                            }

                            //Thread timerThread2 = new Thread(new ThreadStart(RunTimerForNormalModeSomeone));
                            //double SecondsForAnswer = 10;
                            //timerThread2.Start();

                            #endregion

                            playerAnswering.HasThePlayerChoosenAnswer = true;
                            int ChoosenAnswerNumber = ChoosenAnswer + 1;
                            if (ChoosenAnswerNumber == question.CorrectAnswer)
                            {
                                #region if the answer is correct..

                                Console.WriteLine("");
                                Console.WriteLine($@"{question.CorrectAnswer}");

                                if (ActivatedBuzzer.ToUpper() == "M")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(0);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                                }
                                else if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(1);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(1).PlayerName;
                                    playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(1).PlayerNumber;
                                }
                                else if (ActivatedBuzzer.ToUpper() == "A")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(2);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(2).PlayerName;
                                    playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(2).PlayerNumber;
                                }

                                playerAnswering.TrapSum = playerAnswering.TrapSum + question.MoneyOnTable;

                                CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\CorrectAnswer.wav", "Oh yes shit!");

                                Console.WriteLine("");
                                Console.WriteLine("Oh yeah! Correct Answer!");
                                Console.WriteLine("");
                                Console.WriteLine($@"{playerAnswering.PlayerName} now has ${playerAnswering.TrapSum}.");
                                /* stopping by to explain sound file
                                 * SoundPlayer explanation = new SoundPlayer($@"path for explanation .wav file")
                                 * explanation.Play();
                                 * Thread.Sleep(sound file length);
                                 * than, no additional user kep press is required
                                 */

                    #region Correct the answer if you want

                    Console.WriteLine();
                    Console.WriteLine($@"If you think my answer is wrong, you can correct it. Press c to do so, or press enter to continue.");
                    Console.CursorVisible = false;
                    ConsoleKeyInfo consoleKeyInfo5 = Console.ReadKey(true);
                    while(consoleKeyInfo5.Key.ToString().ToUpper() != "ENTER" && consoleKeyInfo5.Key.ToString().ToUpper() != "C")
                    {
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                        consoleKeyInfo5 = Console.ReadKey(true);
                    }
                    if(consoleKeyInfo5.Key.ToString().ToUpper() == "ENTER")
                    {
                        Console.CursorVisible = true;
                        // continue than..
                    }
                    if (consoleKeyInfo5.Key.ToString().ToUpper() == "C")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Ok than, write the correct answer here. Your answer number will remain, just its text would change:");
                        Console.WriteLine();
                        Console.CursorVisible = true;
                        string AnswerOpinion = Console.ReadLine();                        
                        while(AnswerOpinion == string.Empty)
                        {
                            AnswerOpinion = Console.ReadLine();
                        }

                        #region Update..

                        UpdateAnswer("Trap", AnswerOpinion, null, question);
                        Console.WriteLine();
                        Console.WriteLine("Change of answer applied succesfully!");
                        Console.WriteLine();
                        Thread.Sleep(3000);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }

                        #endregion
                    }


                    #endregion


                      Thread.Sleep(2000);
                      while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }



                                break;

                                #endregion
                            }


                            if (ChoosenAnswerNumber != question.CorrectAnswer)
                            {
                                #region If the answer is wrong..

                                _stopRequestedForSomeoneAnswering = true;

                                if (ActivatedBuzzer.ToUpper() == "M")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(0);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                                }
                                else if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(1);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(1).PlayerName;
                                    playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(1).PlayerNumber;
                                }
                                else if (ActivatedBuzzer.ToUpper() == "A")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(2);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(2).PlayerName;
                                    playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(2).PlayerNumber;
                                }

                                if (playerAnswering.TrapSum - question.MoneyOnTable < 0)
                                {
                                    playerAnswering.TrapSum = 0;
                                }
                                else
                                {
                                    playerAnswering.TrapSum = 0;
                                }

                                CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\IncorrectAnswer.wav", "Oh no shit!");

                                /*
                                SoundPlayer Incorrect = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\IncorrectAnswer.wav");
                                Incorrect.Play();
                                Incorrect.Dispose();
                                Thread.Sleep(1200);
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }
                                */
                                Console.Beep();
                                Console.WriteLine("");
                                Console.WriteLine("Wrong!");
                                Console.WriteLine($@"{playerAnswering.PlayerName} now has ${playerAnswering.TrapSum}");
                                Console.WriteLine("");
                                playerAnswering.HasThePlayerChoosenAnswer = true;
                                //playerAnswering.WrongAnswerChoosen = int.Parse($@"{ChoosenAnswer.Key.ToString().Substring(1)}");

                                game.GamePlayers.ElementAt(playerAnswering.PlayerNumber - 1).HasThePlayerChoosenAnswer = true;

                                Thread.Sleep(2000);
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }

                                // Now to continue to the other answers..

                                #endregion
                            }
                        }
                    }
                    //bool Player1AnsweredOrNot = false;
                    //bool Player2AnsweredOrNot = false;
                    //bool Player3AnsweredOrNot = true;

                }

                #endregion

            }

            #endregion

            Console.Clear();

            return game;
        }

        #endregion

        #region Blank spaces

        public string BlankSpaces(int numberOfSpaces)
        {
            return new string(' ', numberOfSpaces);
        }


        #endregion

        #region Game over part

        public void GameOver(Game game)
        {
            Console.Clear();

            /*

            SoundPlayer CorrectOrFalseInto = PlayingInMultiThreading($@"You-think-you-are-smart\OtherSounds\CorrectOrFalseStatement.wav", "We've reached the game of true or false!");
            CorrectOrFalseInto.Play();

            double audioLength = 0;

            using (var audioFile = new AudioFileReader($@"You-think-you-are-smart\OtherSounds\CorrectOrFalseStatement.wav"))
            using (var outputDevice1 = new WaveOutEvent())
            {
                double audioLengthMs1 = audioFile.TotalTime.TotalMilliseconds;
                audioLength = Math.Round(audioLengthMs1);
            }

            string BlankSpace = "";
            double MiliSecondsThatPassed = 0;
            for (int i = 0; i < 45; i++)
            {
                BlankSpace = BlankSpaces(i + 1);
                Console.Write($@"{BlankSpace}Correct or incorrect..");
                Thread.Sleep(10);
                MiliSecondsThatPassed = MiliSecondsThatPassed + 10;
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                if (i < 44)
                {
                    Console.Clear();
                }
            }

            if (MiliSecondsThatPassed < audioLength)
            {
                int gap = (int)audioLength - (int)MiliSecondsThatPassed;
                Thread.Sleep(gap);
            }

            CorrectOrFalseInto.Stop();
            File.Delete(CorrectOrFalseInto.SoundLocation);
            CorrectOrFalseInto.Dispose();

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }
            */
            #region Final tital screen

            SoundPlayer GameOverSoundEffect = new SoundPlayer($@"You-think-you-are-smart\OtherSounds\GameOverSoundEffect.wav");
            GameOverSoundEffect.Play();

            double audioLength = 0;

            using (var audioFile = new AudioFileReader($@"You-think-you-are-smart\OtherSounds\GameOverSoundEffect.wav"))
            using (var outputDevice1 = new WaveOutEvent())
            {
                double audioLengthMs1 = audioFile.TotalTime.TotalMilliseconds;
                audioLength = Math.Round(audioLengthMs1);
            }

            // we'll find later a solution to that line
            double secondsThatPassed = 0;

            string BlankSpace = "";
            for (int i = 0; i < 30; i++)
            {
                BlankSpace = BlankSpaces(i + 1);
                Console.Write($@"{BlankSpace}Final results!!111");
                Thread.Sleep(40);
                secondsThatPassed = secondsThatPassed + 0.04;
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                if (i == 29)
                {
                    break;
                }
                Console.Clear();
            }
            if (secondsThatPassed < (audioLength / 1000))
            {
                Thread.Sleep((int)audioLength - (int)(secondsThatPassed * 1000));
            }

            GameOverSoundEffect.Dispose();


            CreatingAndPlayingAudio($@"You-think-you-are-smart\OtherSounds\GameOverSpeech.wav", "Ok that was tedious. Let's go to the final results, and than, I'm gonna jump out of the window.");

            Console.Clear();

            #endregion

            #region Showing game results

            SoundPlayer HighScoreSoundEffect = new SoundPlayer($@"You-think-you-are-smart\OtherSounds\HighScoreSoundEffect.wav");
            HighScoreSoundEffect.Play();
            Thread.Sleep(2000);
            HighScoreSoundEffect.Dispose();
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            List<int> FindingTheMiddleSum = new List<int>();

            List<int> AllPlayersSum = new List<int>();

            foreach (Player player in game.GamePlayers)
            {
                int total = player.PreTrapSum + player.TrapSum;
                AllPlayersSum.Add(total);
                FindingTheMiddleSum.Add(total);
            }

            Console.Beep();

            int WorstSum = 1000000;
            int MiddleSum = 1000000;
            int BestSum = 1000000;

            Player WinnerPlayer = new Player(10, "");
            Player MiddlePlayer = new Player(10, "");
            Player WorstPlayer = new Player(10, "");
            bool EveryoneDraw = false;
            bool Draw = false;

            #region If there are 3 players..

            if (game.GamePlayers.Count == 3)
            {
                BestSum = AllPlayersSum.Max();
                FindingTheMiddleSum.Remove(BestSum);
                WorstSum = AllPlayersSum.Min();
                FindingTheMiddleSum.Remove(WorstSum);
                MiddleSum = FindingTheMiddleSum.ElementAt(0);

                #region in the case of draws..

                EveryoneDraw = BestSum == FindingTheMiddleSum.ElementAt(0) && FindingTheMiddleSum.ElementAt(0) == WorstSum;

                if (BestSum == FindingTheMiddleSum.ElementAt(0) || FindingTheMiddleSum.ElementAt(0) == WorstSum)
                {
                    if (!EveryoneDraw)
                    {
                        Draw = true;
                        if (BestSum == FindingTheMiddleSum.ElementAt(0))
                        {
                            WinnerPlayer.PlayerPosition = 1;
                            MiddlePlayer.PlayerPosition = 1;
                            WorstPlayer.PlayerPosition = 2;
                        }
                        if (FindingTheMiddleSum.ElementAt(0) == WorstSum)
                        {
                            WinnerPlayer.PlayerPosition = 1;
                            MiddlePlayer.PlayerPosition = 2;
                            WorstPlayer.PlayerPosition = 2;
                        }
                    }

                }
                if (EveryoneDraw)
                {
                    WinnerPlayer.PlayerPosition = 1;
                    MiddlePlayer.PlayerPosition = 1;
                    WorstPlayer.PlayerPosition = 1;
                }

                if (EveryoneDraw)
                {
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${BestSum})");
                    Console.WriteLine("");
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(1).PlayerName} (${MiddleSum})");
                    Console.WriteLine("");
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(2).PlayerName} (${WorstSum})");
                }
                if (Draw && !EveryoneDraw)
                {
                    if (BestSum == FindingTheMiddleSum.ElementAt(0))
                    {
                        #region If the two players win together..

                        if ((game.GamePlayers.ElementAt(0).PreTrapSum + game.GamePlayers.ElementAt(0).TrapSum == BestSum) && game.GamePlayers.ElementAt(1).PreTrapSum + game.GamePlayers.ElementAt(1).TrapSum == BestSum)
                        {
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${BestSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(1).PlayerName} (${MiddleSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(2).PlayerName} (${WorstSum})");
                        }
                        if ((game.GamePlayers.ElementAt(0).PreTrapSum + game.GamePlayers.ElementAt(0).TrapSum == BestSum) && game.GamePlayers.ElementAt(2).PreTrapSum + game.GamePlayers.ElementAt(2).TrapSum == BestSum)
                        {
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${BestSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(2).PlayerName} (${MiddleSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${WorstSum})");
                        }
                        if ((game.GamePlayers.ElementAt(1).PreTrapSum + game.GamePlayers.ElementAt(1).TrapSum == BestSum) && game.GamePlayers.ElementAt(2).PreTrapSum + game.GamePlayers.ElementAt(2).TrapSum == BestSum)
                        {
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(1).PlayerName} (${BestSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(2).PlayerName} (${MiddleSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(0).PlayerName} (${WorstSum})");
                        }

                        #endregion

                        /*
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerPosition} (${BestSum})");
                        Console.WriteLine("");
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(1).PlayerPosition} (${MiddleSum})");
                        Console.WriteLine("");
                        Console.WriteLine($@"2. {game.GamePlayers.ElementAt(2).PlayerPosition} (${WorstSum})");
                        */
                    }
                    #region If a player wins and the others are equal..

                    if (BestSum > FindingTheMiddleSum.ElementAt(0) && FindingTheMiddleSum.ElementAt(0) == WorstSum)
                    {
                        if (game.GamePlayers.ElementAt(0).PreTrapSum + game.GamePlayers.ElementAt(0).TrapSum == BestSum)
                        {
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${BestSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${MiddleSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(2).PlayerName} (${WorstSum})");
                        }
                        else if (game.GamePlayers.ElementAt(1).PreTrapSum + game.GamePlayers.ElementAt(1).TrapSum == BestSum)
                        {
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(1).PlayerName} (${BestSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(0).PlayerName} (${MiddleSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(2).PlayerName} (${WorstSum})");
                        }
                        else if (game.GamePlayers.ElementAt(2).PreTrapSum + game.GamePlayers.ElementAt(2).TrapSum == BestSum)
                        {
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(2).PlayerName} (${BestSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(0).PlayerName} (${MiddleSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${WorstSum})");
                        }
                    }

                    #endregion

                }

                #endregion

                if (!Draw && !EveryoneDraw)
                {
                    WinnerPlayer = game.GamePlayers.Find(x => x.PreTrapSum + x.TrapSum == BestSum);
                    MiddlePlayer = game.GamePlayers.Find(x => x.PreTrapSum + x.TrapSum == FindingTheMiddleSum.ElementAt(0));
                    WorstPlayer = game.GamePlayers.Find(x => x.PreTrapSum + x.TrapSum == WorstSum);

                    Console.WriteLine($@"1. {WinnerPlayer.PlayerName} (${BestSum})");
                    Console.WriteLine("");
                    Console.WriteLine($@"2. {MiddlePlayer.PlayerName} (${MiddleSum})");
                    Console.WriteLine("");
                    Console.WriteLine($@"3. {WorstPlayer.PlayerName} (${WorstSum})");
                }
            }

            #endregion

            #region If there are 2 players..

            else if (game.GamePlayers.Count == 2)
            {
                BestSum = AllPlayersSum.Max();
                WorstSum = AllPlayersSum.Min();

                if (BestSum == WorstSum)
                {
                    Draw = true;
                    EveryoneDraw = true;

                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${BestSum})");
                    Console.WriteLine("");
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(1).PlayerName} (${WorstSum})");
                }

                if (!Draw && !EveryoneDraw)
                {
                    WinnerPlayer = game.GamePlayers.Find(x => x.PreTrapSum + x.TrapSum == BestSum);
                    WorstPlayer = game.GamePlayers.Find(x => x.PreTrapSum + x.TrapSum == WorstSum);

                    Console.WriteLine($@"1. {WinnerPlayer.PlayerName} (${BestSum})");
                    Console.WriteLine("");
                    Console.WriteLine($@"2. {WorstPlayer.PlayerName} (${WorstSum})");
                }
            }

            #endregion

            else if (game.GamePlayers.Count == 1)
            {
                Draw = false;
                EveryoneDraw = false;
                WinnerPlayer = game.GamePlayers.ElementAt(0);
                BestSum = AllPlayersSum.ElementAt(0);
                Console.WriteLine($@"1. {WinnerPlayer.PlayerName} (${BestSum})");
            }

            //AllPlayersSum.Sort();

            #endregion

            #region Results

            SoundPlayer result = new SoundPlayer();

            if (EveryoneDraw || Draw)
            {
                if (BestSum == FindingTheMiddleSum.ElementAt(0))
                {
                    CreatingAndPlayingAudio($@"You-think-you-are-smart\OtherSounds\Draw.wav", "It's a draw!");

                    //result.SoundLocation = $@"You-think-you-are-smart\OtherSounds\Draw.wav";
                }
                else
                {
                    CreatingAndPlayingAudio($@"You-think-you-are-smart\OtherSounds\Draw.wav", "And the biggest loser.. Wait, I mean the winner is..");

                    //result.SoundLocation = $@"You-think-you-are-smart\OtherSounds\WinnerIs.wav";
                }
            }
            else
            {
                CreatingAndPlayingAudio($@"You-think-you-are-smart\OtherSounds\Victory.wav", "And the biggest loser.. Wait, I mean the winner is..");

                //result.SoundLocation = $@"You-think-you-are-smart\OtherSounds\WinnerIs.wav";
            }
            /*
            result.Play();
            Thread.Sleep(2000);
            result.Dispose();
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }            
            */
            SoundPlayer WinnerPlayerSound = new SoundPlayer();

            bool DoWeHaveAWinner = BestSum > FindingTheMiddleSum.ElementAt(0) || game.GamePlayers.Count == 1;

            #region Getting the winner's sound file

            if (DoWeHaveAWinner)
            {
                if (game.GamePlayers.Count == 1)
                {
                    WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player1.wav";
                }
                if (game.GamePlayers.Count == 2)
                {
                    if (game.GamePlayers.ElementAt(0).TrapSum + game.GamePlayers.ElementAt(0).PreTrapSum == BestSum)
                    {
                        WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player1.wav";
                    }
                    if (game.GamePlayers.ElementAt(1).TrapSum + game.GamePlayers.ElementAt(1).PreTrapSum == BestSum)
                    {
                        WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player2.wav";
                    }
                }
                if (game.GamePlayers.Count == 3)
                {
                    if (game.GamePlayers.ElementAt(0).TrapSum + game.GamePlayers.ElementAt(0).PreTrapSum == BestSum)
                    {
                        WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player1.wav";
                    }
                    if (game.GamePlayers.ElementAt(1).TrapSum + game.GamePlayers.ElementAt(1).PreTrapSum == BestSum)
                    {
                        WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player2.wav";
                    }
                    if (game.GamePlayers.ElementAt(2).TrapSum + game.GamePlayers.ElementAt(2).PreTrapSum == BestSum)
                    {
                        WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player3.wav";
                    }
                }
            }

            #endregion

            /*
            if (WinnerPlayer.PlayerName == game.GamePlayers.ElementAt(0).PlayerName)
            {
                WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player1.wav";
            }
            if (WinnerPlayer.PlayerName == game.GamePlayers.ElementAt(1).PlayerName)
            {
                WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player2.wav";
            }
            if (WinnerPlayer.PlayerName == game.GamePlayers.ElementAt(2).PlayerName)
            {
                WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player3.wav";
            }
            */
            if (DoWeHaveAWinner)
            {
                WinnerPlayerSound.PlaySync();
                WinnerPlayerSound.Dispose();
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
            }

            if (DoWeHaveAWinner)
            {
                SoundPlayer ResultGoodOrBad = new SoundPlayer();

                if (BestSum > 0)
                {
                    CreatingAndPlayingAudio($@"You-think-you-are-smart\OtherSounds\PositiveScore.wav", "You sir, are fucking awesome!");

                    //ResultGoodOrBad.SoundLocation = $@"You-think-you-are-smart\OtherSounds\PositiveScore.wav";
                }
                if (BestSum <= 0)
                {
                    CreatingAndPlayingAudio($@"You-think-you-are-smart\OtherSounds\NegativeScore.wav", "Can't believe you won with such a pathetic result.");

                    //ResultGoodOrBad.SoundLocation = $@"You-think-you-are-smart\OtherSounds\NegativeScore.wav";
                }
            }

            #endregion

            #region Writing All results To XML

            bool IsThereANewWR = false;

            List<XElement> AllScoreElements = new List<XElement>();
            List<int> AllScores = new List<int>();
            List<int> WorstToBestScores = new List<int>();
            List<int> NoCurrentRecordsOnly = new List<int>();

            HighScore highScore = new HighScore(game);
            List<string> WhatToWriteForXML = new List<string>();
            WhatToWriteForXML.Add("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            WhatToWriteForXML.Add("<HighScore>");

            #region Redone

            foreach (XElement score in highScore.scores)
            {
                WhatToWriteForXML.Add($@"<Score>");
                WhatToWriteForXML.Add($@"<PlayerName>{score.Element("PlayerName").Value}</PlayerName>");
                WhatToWriteForXML.Add($@"<Money>{score.Element("Money").Value}</Money>");
                WhatToWriteForXML.Add($@"<Position>{score.Element("Position").Value}</Position>");
                WhatToWriteForXML.Add($@"</Score>");
            }
            foreach (Player player in game.GamePlayers)
            {
                WhatToWriteForXML.Add($@"<Score>");
                WhatToWriteForXML.Add($@"<PlayerName>{player.PlayerName}</PlayerName>");
                WhatToWriteForXML.Add($@"<Money>{player.PreTrapSum + player.TrapSum}</Money>");
                WhatToWriteForXML.Add($@"<Position>{0}</Position>");
                WhatToWriteForXML.Add($@"</Score>");
            }

            WhatToWriteForXML.Add($@"</HighScore>");

            highScore.HighScoreDocument.RemoveNodes();
            File.Delete($@"You think you are smart xml\HighScore.xml");
            File.WriteAllLines($@"You think you are smart xml\HighScore.xml", WhatToWriteForXML);

            #endregion


            #region Old way
            /*
            foreach (XElement score in highScore.scores)
            {
                AllScores.Add(int.Parse(score.Element("Money").Value));
                WorstToBestScores.Add(int.Parse(score.Element("Money").Value));
                NoCurrentRecordsOnly.Add(int.Parse(score.Element("Money").Value));
                
                //WhatToWriteForXML.Add($@"<Score>");
                //WhatToWriteForXML.Add($@"<PlayerName>{score.Element("PlayerName").Value}</PlayerName>");
                //WhatToWriteForXML.Add($@"<Money>{score.Element("Money").Value}</Money>");
                //WhatToWriteForXML.Add($@"<Position>{score.Element("Position").Value}</Position>");
                //WhatToWriteForXML.Add($@"</Score>");
                
            }
            */

            /*
            foreach (Player player in game.GamePlayers)
            {
                AllScores.Add(player.PreTrapSum + player.TrapSum);
                WorstToBestScores.Add(player.PreTrapSum + player.TrapSum);
                
                //WhatToWriteForXML.Add("<Score>");
                //WhatToWriteForXML.Add($@"<PlayerName>{player.PlayerName}</PlayerName>");
                //WhatToWriteForXML.Add($@"<Money>{player.PreTrapSum + player.TrapSum}</Money>");
                //WhatToWriteForXML.Add($@"<Position>{player.PlayerPosition}</Position>");
                //WhatToWriteForXML.Add($@"</Score>");
                
            }

            WorstToBestScores.Sort();
            

            List<int> CalculatedPositions = new List<int>(new int[AllScores.Count]);

            int position = 1;
            CalculatedPositions[AllScores.Count - 1] = position;


            for (int i = AllScores.Count - 2; i >= 0; i--)
            {
                // any xelement needs to be edited its position value.
                // You can use the setvalue function, but it requires making the xml file the normal way
                // you can re-create the string list, since everything but the position int value is identical
                // to calculate the position value, here's how you do it:
                // you go throught the score list (for loop),
                // and for each element, you find its position (index of) in the sorted list
                // the position value is the (total elements count) - (the score index in the sorted list)
                // than you simply change the order

                if (AllScores[i] == AllScores[i + 1])
                {
                    CalculatedPositions[i] = position;
                }
                else
                {
                    // If scores are different, increment the rank
                    // and assign the new rank
                    position += 1;
                    CalculatedPositions[i] = position;
                }

                //position = AllScores.Count - WorstToBestScores.IndexOf(AllScores[i]);
                //CalculatedPositions.Add(position);
            }

            NoCurrentRecordsOnly.Sort();

            int BestExistingScore = 100;

            BestExistingScore = NoCurrentRecordsOnly.ElementAt(NoCurrentRecordsOnly.Count - 1);

            foreach (Player player in game.GamePlayers)
            {
                int score = player.PreTrapSum + player.TrapSum;
            */
            /*
            int PositionIn = AllScores.Count - WorstToBestScores.IndexOf(score);
            player.PositionInHighScore = PositionIn;
            if (score == CalculatedPositions.ElementAt(0) || PositionIn == 1)
            {
                IsThereANewWR = true;
            }
            */
            /*
            if(WorstToBestScores.ElementAt(WorstToBestScores.Count - 1) == score)
            {
                IsThereANewWR = true;
            }
            */
            /*
            if(score > BestExistingScore)
            {
                IsThereANewWR = true;
            }
        }
            */
            /*
            for (int i = CalculatedPositions.Count; i > 0; i--)
            {
                int CurrentMoney = WorstToBestScores.ElementAt(i - 1);

                bool IsExistInDataBase = true;

                Player CurrentPlayerForThis = new Player(1, "");

                foreach(Player player in game.GamePlayers)
                {
                    if(CurrentMoney == player.PreTrapSum + player.TrapSum)
                    {
                        IsExistInDataBase = false;
                        CurrentPlayerForThis = player;
                    }
                }

                if(IsExistInDataBase)
                {
                    XElement score = highScore.scores.Find(x => int.Parse(x.Element("Money").Value) == CurrentMoney);
                    WhatToWriteForXML.Add($@"<Score>");
                    WhatToWriteForXML.Add($@"<PlayerName>{score.Element("PlayerName").Value}</PlayerName>");
                    WhatToWriteForXML.Add($@"<Money>{score.Element("Money").Value}</Money>");
                    if(i == CalculatedPositions.Count)
                    {
                        WhatToWriteForXML.Add($@"<Position>{CalculatedPositions.ElementAt(i - 1).ToString()}</Position>");
                    }
                    else if (i < CalculatedPositions.Count)
                    {
                        if(WorstToBestScores.ElementAt(i - 1) == WorstToBestScores.ElementAt(i))
                        {
                            WhatToWriteForXML.Add($@"<Position>{CalculatedPositions.ElementAt(i).ToString()}</Position>");
                        }
                        else
                        {
                            WhatToWriteForXML.Add($@"<Position>{CalculatedPositions.ElementAt(i - 1).ToString()}</Position>");
                        }
                    }
                    WhatToWriteForXML.Add($@"</Score>");
                }
                else
                {
                    WhatToWriteForXML.Add($@"<Score>");
                    WhatToWriteForXML.Add($@"<PlayerName>{CurrentPlayerForThis.PlayerName}</PlayerName>");
                    WhatToWriteForXML.Add($@"<Money>{CurrentPlayerForThis.PreTrapSum + CurrentPlayerForThis.TrapSum}</Money>");

                    if (i == CalculatedPositions.Count)
                    {
                        WhatToWriteForXML.Add($@"<Position>{CalculatedPositions.ElementAt(i - 1).ToString()}</Position>");
                    }
                    else if (i < CalculatedPositions.Count)
                    {
                        if (WorstToBestScores.ElementAt(i - 1) == WorstToBestScores.ElementAt(i))
                        {
                            WhatToWriteForXML.Add($@"<Position>{CalculatedPositions.ElementAt(i).ToString()}</Position>");
                        }
                        else
                        {
                            WhatToWriteForXML.Add($@"<Position>{CalculatedPositions.ElementAt(i - 1).ToString()}</Position>");
                        }
                    }

                    WhatToWriteForXML.Add($@"</Score>");
                }                
            }

            WhatToWriteForXML.Add($@"</HighScore>");

            highScore.HighScoreDocument.RemoveNodes();
            File.Delete($@"You think you are smart xml\HighScore.xml");
            File.WriteAllLines($@"You think you are smart xml\HighScore.xml", WhatToWriteForXML);

            */

            #endregion

            #endregion

            #region re-opening the new XML file and displaying the records

            XDocument HighScoreNewDocument = new XDocument(XDocument.Load($@"You think you are smart xml\HighScore.xml"));
            XElement root = HighScoreNewDocument.Root;
            IEnumerable<XElement> ScoresElements = root.Elements("Score");
            List<XElement> scores = ScoresElements.ToList();

            #region Redone

            #region The top 5 records

            Console.Clear();

            IsThereANewWR = false;

            List<int> MoneyWorstToBestAfter = new List<int>();

            foreach (XElement score in scores)
            {
                MoneyWorstToBestAfter.Add(int.Parse(score.Element("Money").Value));
            }

            MoneyWorstToBestAfter.Sort();
            int ActualPosition = 1;

            for (int ScoreNumber = 0; ScoreNumber < 5; ScoreNumber++)
            {
                int NumberToReduce = ScoreNumber + 1;
                XElement score = scores.Find(x => int.Parse(x.Element("Money").Value) == MoneyWorstToBestAfter.ElementAt(MoneyWorstToBestAfter.Count - NumberToReduce));
                if (ScoreNumber > 0)
                {
                    if (MoneyWorstToBestAfter.ElementAt(MoneyWorstToBestAfter.Count - NumberToReduce) == MoneyWorstToBestAfter.ElementAt(MoneyWorstToBestAfter.Count - ScoreNumber))
                    {
                        Console.WriteLine($@"{ActualPosition}. {score.Element("PlayerName").Value} (${score.Element("Money").Value})");
                    }
                    else if (MoneyWorstToBestAfter.ElementAt(MoneyWorstToBestAfter.Count - NumberToReduce) < MoneyWorstToBestAfter.ElementAt(MoneyWorstToBestAfter.Count - ScoreNumber))
                    {
                        ActualPosition = ActualPosition + 1;
                        Console.WriteLine($@"{ActualPosition}. {score.Element("PlayerName").Value} (${score.Element("Money").Value})");
                    }
                }
                else
                {
                    Console.WriteLine($@"{ActualPosition}. {score.Element("PlayerName").Value} (${score.Element("Money").Value})");
                }

                bool ShouldWeFinish = ScoreNumber + 1 == scores.Count;
                if (ShouldWeFinish)
                {
                    if (scores.Count < 5)
                    {
                        for (int allScore = scores.Count; allScore < 5; allScore++)
                        {
                            Console.WriteLine($@"{allScore + 1}.");
                        }
                        ScoreNumber = 4;
                        break;
                    }
                    else
                    {
                        ScoreNumber = 4;
                        break;
                    }

                }
            }

            int BestScore = MoneyWorstToBestAfter.ElementAt(MoneyWorstToBestAfter.Count - 1);
            foreach (Player player in game.GamePlayers)
            {
                if (player.PreTrapSum + player.TrapSum == BestScore)
                {
                    if (MoneyWorstToBestAfter.Count > 1)
                    {
                        if (MoneyWorstToBestAfter.ElementAt(MoneyWorstToBestAfter.Count - 1) > MoneyWorstToBestAfter.ElementAt(MoneyWorstToBestAfter.Count - 2))
                        {
                            IsThereANewWR = true;
                        }
                    }
                    else
                    {
                        IsThereANewWR = true;
                    }
                }
            }

            if (IsThereANewWR)
            {
                CreatingAndPlayingAudio($@"You-think-you-are-smart\OtherSounds\newWR.wav", "Well, you broke this software's world record. But so what!");
                /*
                SoundPlayer newWRSound = new SoundPlayer($@"You-think-you-are-smart\OtherSounds\newWR.wav");
                newWRSound.Play();
                Thread.Sleep(2700);
                newWRSound.Dispose();
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                */
            }

            #endregion

            #region The worse record

            int WorstMoney = MoneyWorstToBestAfter.ElementAt(0);
            XElement WorstScoreElement = scores.Find(x => int.Parse(x.Element("Money").Value) == WorstMoney);
            Console.WriteLine("");
            Console.WriteLine($@"Smartass.. {WorstScoreElement.Element("PlayerName").Value} (${WorstScoreElement.Element("Money").Value})");
            Console.WriteLine("");

            #endregion

            #region Old way

            /*

            Console.Clear();

            for (int ScoreNumber = 0; ScoreNumber < 5; ScoreNumber++)
            {                
                XElement score = scores.ElementAt(ScoreNumber);
                Console.WriteLine($@"{score.Element("Position").Value}. {score.Element("PlayerName").Value} (${score.Element("Money").Value})");
                Console.WriteLine("");
                bool ShouldWeFinish = ScoreNumber + 1 == scores.Count;
                if(ShouldWeFinish)
                {
                    ScoreNumber = 4;
                    break;
                }
                
            }

            XElement WorstScoreElement = scores.ElementAt(scores.Count - 1);
            Console.WriteLine($@"Smartass.. {WorstScoreElement.Element("PlayerName").Value} (${WorstScoreElement.Element("Money").Value})");

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            
            foreach (XElement score in scores)
            {
                Console.WriteLine($@"{score.Element("Position").Value}. {score.Element("PlayerName").Value} (${score.Element("Money").Value})");
                Console.WriteLine("");
                
                if(int.Parse(score.Element("Position").Value) == scores.Count)
                {
                    Console.WriteLine($@"Smartass.. {score.Element("PlayerName").Value} (${score.Element("Money").Value})");
                }
            }            
            

            if (IsThereANewWR)
            {
                SoundPlayer newWRSound = new SoundPlayer($@"You-think-you-are-smart\OtherSounds\newWR.wav");
                newWRSound.Play();
                Thread.Sleep(2700);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
            }

            */

            #endregion

            Console.WriteLine("Press enter for a new game with new players,");
            Console.WriteLine("Press space for a new game with the same players.");
            Console.WriteLine("Press e to exit the game.");

            ConsoleKeyInfo choice = Console.ReadKey(true);
            while (choice.Key.ToString().ToUpper() != "ENTER" && choice.Key.ToString().ToUpper() != "SPACEBAR" && choice.Key.ToString().ToUpper() != "E")
            {
                Console.WriteLine("Press only on space or enter or e!");
                choice = Console.ReadKey(true);
            }
            string ChoiceIs = choice.Key.ToString();

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
            }

            if (ChoiceIs.ToUpper() == "ENTER")
            {
                if (File.Exists($@"You-think-you-are-smart\NameSounds\Player1.wav"))
                {
                    File.Delete($@"You-think-you-are-smart\NameSounds\Player1.wav");
                }
                if (File.Exists($@"You-think-you-are-smart\NameSounds\Player2.wav"))
                {
                    File.Delete($@"You-think-you-are-smart\NameSounds\Player2.wav");
                }
                if (File.Exists($@"You-think-you-are-smart\NameSounds\Player3.wav"))
                {
                    File.Delete($@"You-think-you-are-smart\NameSounds\Player3.wav");
                }

                return;
            }
            if (ChoiceIs.ToUpper() == "SPACEBAR")
            {
                List<Player> players = new List<Player>();
                for (int PlayerNum = 0; PlayerNum < game.GamePlayers.Count; PlayerNum++)
                {
                    Player newPlayer = new Player(PlayerNum + 1, game.GamePlayers.ElementAt(PlayerNum).PlayerName);
                    players.Add(newPlayer);
                }
                Game NewGame = new Game(players, game.NumOfGameQuestions, players.Count);
                Console.Clear();
                NewGame.NextTurn(NewGame, NewGame.GamePlayers.ElementAt(0));
            }
            if (ChoiceIs.ToUpper() == "E")
            {

                if (File.Exists($@"You-think-you-are-smart\NameSounds\Player1.wav"))
                {
                    File.Delete($@"You-think-you-are-smart\NameSounds\Player1.wav");
                }
                if (File.Exists($@"You-think-you-are-smart\NameSounds\Player2.wav"))
                {
                    File.Delete($@"You-think-you-are-smart\NameSounds\Player2.wav");
                }
                if (File.Exists($@"You-think-you-are-smart\NameSounds\Player3.wav"))
                {
                    File.Delete($@"You-think-you-are-smart\NameSounds\Player3.wav");
                }

                Environment.Exit(0);
            }

            return;

            #endregion

            #endregion
        }

        #endregion

        #region Correct Or false

        public Game CorrectOrFalseGame(Game game)
        {
            #region Choosing player to answer

            Random number = new Random();
            int ChoosenNumber = 0;

            if (game.GamePlayers.Count == 3)
            {
                ChoosenNumber = number.Next(0, 3);
            }
            if (game.GamePlayers.Count == 2)
            {
                ChoosenNumber = number.Next(0, 2);
            }
            if (game.GamePlayers.Count == 1)
            {
                ChoosenNumber = 0;
            }

            #endregion

            #region Intrudction and start game

            Console.Clear();
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            //CreatingAndPlayingAudio($@"You-think-you-are-smart\OtherSounds\CorrectOrFalseStatement.wav", "We've reached the game of true or false!");

            SoundPlayer CorrectOrFalseInto = PlayingInMultiThreading($@"You-think-you-are-smart\OtherSounds\CorrectOrFalseStatement.wav", "We've reached the game of true or false!");
            CorrectOrFalseInto.Play();

            double audioLength = 0;

            using (var audioFile = new AudioFileReader($@"You-think-you-are-smart\OtherSounds\CorrectOrFalseStatement.wav"))
            using (var outputDevice1 = new WaveOutEvent())
            {
                double audioLengthMs1 = audioFile.TotalTime.TotalMilliseconds;
                audioLength = Math.Round(audioLengthMs1);
            }

            string BlankSpace = "";
            double MiliSecondsThatPassed = 0;
            for (int i = 0; i < 45; i++)
            {
                BlankSpace = BlankSpaces(i + 1);
                Console.Write($@"{BlankSpace}Correct or incorrect..");
                Thread.Sleep(10);
                MiliSecondsThatPassed = MiliSecondsThatPassed + 10;
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                if (i < 44)
                {
                    Console.Clear();
                }
            }

            if (MiliSecondsThatPassed < audioLength)
            {
                int gap = (int)audioLength - (int)MiliSecondsThatPassed;
                Thread.Sleep(gap);
            }

            CorrectOrFalseInto.Stop();
            CorrectOrFalseInto.Dispose();
            File.Delete(CorrectOrFalseInto.SoundLocation);

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            Console.WriteLine("");
            Console.WriteLine("Try this or not?");
            Console.WriteLine("Y/y for yes, N/n for no");
            Console.WriteLine("");

            SoundPlayer playerToChoose = new SoundPlayer($@"You-think-you-are-smart\NameSounds\Player{ChoosenNumber + 1}.wav");
            playerToChoose.PlaySync();
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }
            playerToChoose.Dispose();

            string instructionsString = ("We're on true or false game. On the screen will appear a statement. You have to guess correctly the stupid opinion of the game's developer regarding whatever this statement is correct or not. If you answer right, your money is doubled. If you're wrong, I steal all your money. Choose if you want to try this or not.");

            CreatingAndPlayingAudio($@"You-think-you-are-smart\CorrectOrIncorrectQuestions\Instructions.wav", instructionsString, null, true);

            double InstructionsLength = 0;

            using (var audioFile = new AudioFileReader($@"You-think-you-are-smart\CorrectOrIncorrectQuestions\Instructions.wav"))
            using (var outputDevice1 = new WaveOutEvent())
            {
                double audioLengthMs1 = audioFile.TotalTime.TotalMilliseconds;
                audioLength = Math.Round(audioLengthMs1);
            }

            SoundPlayer instructions = new SoundPlayer($@"You-think-you-are-smart\CorrectOrIncorrectQuestions\Instructions.wav");
            instructions.Play();
            
            double SecondsThatPassed = 0;
            ConsoleKeyInfo Choice = new ConsoleKeyInfo();

            while (!Console.KeyAvailable)
            {

            }

            if (Console.KeyAvailable)
            {
                Choice = Console.ReadKey(true);
                while (Choice.Key.ToString().ToUpper() != "Y" && Choice.Key.ToString().ToUpper() != "N")
                {
                    Choice = Console.ReadKey(true);
                }

                instructions.Stop();
                instructions.Dispose();
                File.Delete($@"You-think-you-are-smart\CorrectOrIncorrectQuestions\Instructions.wav");

                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }

                if (Choice.Key.ToString().ToUpper() == "N")
                {
                    Console.WriteLine("");
                    Console.WriteLine("No!");

                    CreatingAndPlayingAudio($@"You-think-you-are-smart\CorrectOrIncorrectQuestions\IfDisAgrees.wav", "Your mom didn't allow to take risks as a teenager, didn't she?");

                    //SoundPlayer DisAgree = new SoundPlayer($@"You-think-you-are-smart\CorrectOrIncorrectQuestions\IfDisAgrees.wav");
                    //DisAgree.Play();
                    //Thread.Sleep(1000);
                    /*
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                    */
                    Thread.Sleep(2000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                    Console.Clear();
                    return game;
                }
                if (Choice.Key.ToString().ToUpper() == "Y")
                {
                    Console.WriteLine("");
                    Console.WriteLine("Yes!");
                    CreatingAndPlayingAudio($@"You-think-you-are-smart\CorrectOrIncorrectQuestions\IfAgrees.wav", "Wow! You actually agree? Do you trust the game developer's stupid opinions?");
                    /*
                    SoundPlayer Agree = new SoundPlayer($@"You-think-you-are-smart\CorrectOrIncorrectQuestions\IfAgrees.wav");
                    Agree.Play();
                    Thread.Sleep(5000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                    */
                    #region the actual game

                    Console.Clear();
                    game.IsOnCorrectOrFalseMode = true;
                    CorrectOrFalse TrueFalseGame = new CorrectOrFalse(game, ChoosenNumber);
                    bool ShouldThisBeAdded = true;
                    if (game.CorrectOrFalseAlreadyAskedQuestions.Count > 0)
                    {
                        foreach (TrueOrFalseQuestion question in CorrectOrFalseAlreadyAskedQuestions)
                        {
                            if (question.QuestionContent == TrueFalseGame.ChoosenQuestionRandom.QuestionContent)
                            {
                                ShouldThisBeAdded = false;
                            }
                        }
                    }
                    if (ShouldThisBeAdded)
                    {
                        game.CorrectOrFalseAlreadyAskedQuestions.Add(TrueFalseGame.ChoosenQuestionRandom);
                    }
                    else
                    {
                        // regenrate a question
                        while (!ShouldThisBeAdded)
                        {
                            TrueFalseGame = new CorrectOrFalse(game, ChoosenNumber);
                            foreach (TrueOrFalseQuestion question in CorrectOrFalseAlreadyAskedQuestions)
                            {
                                if (question.QuestionContent == TrueFalseGame.ChoosenQuestionRandom.QuestionContent)
                                {
                                    ShouldThisBeAdded = true;
                                }
                            }
                        }
                        game.CorrectOrFalseAlreadyAskedQuestions.Add(TrueFalseGame.ChoosenQuestionRandom);
                    }

                    Console.WriteLine($@"{TrueFalseGame.ChoosenQuestionRandom.QuestionContent}");
                    Console.WriteLine("");
                    Console.WriteLine("Yes");
                    Console.WriteLine("No");

                    #region Opening the basic time

                    int startSecond = DateTime.Now.Second;
                    int startMinute = DateTime.Now.Minute;
                    int endSecond;
                    int endMinute;
                    if (startSecond <= 30)
                    {
                        endMinute = startMinute;
                        endSecond = startSecond + 30;
                    }
                    else
                    {
                        endMinute = startMinute + 1;
                        endSecond = startSecond - 30;
                    }

                    #endregion

                    double SecondsThatPassedInGame = 0;

                    string TheKeyPressed = "";
                    Console.WriteLine("");
                    ConsoleKeyInfo buzzer = new ConsoleKeyInfo();
                    while (!Console.KeyAvailable && SecondsThatPassedInGame < 30)
                    {
                        Thread.Sleep(1000);

                        Console.Clear();
                        Console.WriteLine($@"{TrueFalseGame.ChoosenQuestionRandom.QuestionContent}");
                        Console.WriteLine("");
                        Console.WriteLine("Yes");
                        Console.WriteLine("No");
                        Console.WriteLine();

                        SecondsThatPassedInGame = SecondsThatPassedInGame + 1;
                        Console.Write($@" {30 - SecondsThatPassedInGame}");
                        if (SecondsThatPassed >= 30)
                        {
                            break;
                        }
                    }

                    if (SecondsThatPassedInGame >= 30)
                    {
                        #region If the player doesn't answer..

                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }

                        CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\NoOneAnswered.wav", "Smartass! But don't worry, I lies when I said I'll take all your money if you don't provide the correct answer! Wait.. Or maybe not.");
                        /*
                        SoundPlayer NoAnswer = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\NoOneAnswered.wav");
                        NoAnswer.Play();
                        Thread.Sleep(6500);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                        */
                        Console.Beep();
                        Console.WriteLine("");
                        Console.WriteLine("Time's up! What, you thought you could get away with it?");
                        Console.WriteLine($@"{TrueFalseGame.playerForChellenge.PlayerName} now has $0");
                        Console.WriteLine("");
                        //string CorrectAnswerContent = File.ReadAllText($@"{QuestionsFoldersList.Find(x => x == CorrectFolder).FullName}\Answer{TheQuestion.CorrectAnswer}.txt");
                        // ($@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}\{QuestionsFoldersList.Find(x => x == CorrectFolder).Name}\Answer{TheQuestion.CorrectAnswer}.txt");

                        //Console.WriteLine($@"{CorrectAnswerContent}");

                        Thread.Sleep(3000);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                        Console.Clear();
                        return game;

                        #endregion
                    }
                    if (Console.KeyAvailable)
                    {
                        if (Console.KeyAvailable == true)
                        {
                            ConsoleKeyInfo consoleKeyInfo = new ConsoleKeyInfo();
                            consoleKeyInfo = Console.ReadKey(true);

                            #region Throwing wrong key press                       

                            if (consoleKeyInfo.Key.ToString().ToUpper() != "Y" && consoleKeyInfo.Key.ToString().ToUpper() != "N")
                            {
                                CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\HandlingIncorrectBuzzerPressing.wav", "Sorry, can't handle incorrect buzzer pressing at the moment. Restart the game.");

                                //Thread.Sleep(5000);
                                throw new Exception($@"Sorry, can't handle incorrect key press at this mode. I tried for over 20 hours to find a solution for this and failed. Restart the game");
                                /*
                                 * 
                                    //Console.WriteLine("Press only on one of your buzzers - A/M/+");
                                    // a sound can be put in here
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }
                                    consoleKeyInfo = Console.ReadKey(true);
                                    Console.Write($@"{10 - SecondsThatPassed}");
                                    Thread.Sleep(1000);
                                    SecondsThatPassed = SecondsThatPassed + 1;

                                    //consoleKeyInfo = Console.ReadKey(true);
                                */
                            }


                            #endregion

                            if (consoleKeyInfo.Key.ToString().ToUpper() == "Y" || consoleKeyInfo.Key.ToString().ToUpper() == "N")
                            {
                                Console.Clear();

                                if (consoleKeyInfo.Key.ToString().ToUpper() == "Y")
                                {
                                    TrueFalseGame.ChoosenQuestionRandom.AnswerChoice = true;
                                    Console.WriteLine($@"{TrueFalseGame.ChoosenQuestionRandom.QuestionContent}");
                                    Console.WriteLine("");
                                    Console.WriteLine("Yes");
                                    Console.WriteLine("");
                                }
                                if (consoleKeyInfo.Key.ToString().ToUpper() == "N")
                                {
                                    TrueFalseGame.ChoosenQuestionRandom.AnswerChoice = false;
                                    Console.WriteLine($@"{TrueFalseGame.ChoosenQuestionRandom.QuestionContent}");
                                    Console.WriteLine("");
                                    Console.WriteLine("No");
                                    Console.WriteLine("");
                                }
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }

                                if (TrueFalseGame.ChoosenQuestionRandom.answer == TrueFalseGame.ChoosenQuestionRandom.AnswerChoice)
                                {
                                    #region If the player is correct

                                    int NewSum = game.GamePlayers.Find(x => x == TrueFalseGame.playerForChellenge).PreTrapSum * 2;
                                    game.GamePlayers.Find(x => x == TrueFalseGame.playerForChellenge).PreTrapSum = NewSum;

                                    CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\CorrectAnswer.wav", "Oh yes shit!");

                                    //SoundPlayer CorrectAnswerResponse = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\CorrectAnswer.wav");
                                    //CorrectAnswerResponse.Play();
                                    //Thread.Sleep(600);
                                    /*
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }
                                    */
                                    Console.WriteLine($@"{TrueFalseGame.playerForChellenge.PlayerName} Now has ${NewSum}.");
                                    Thread.Sleep(2000);
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }
                                    #region Correct the answer if you want

                                    Console.WriteLine();
                                    if(TrueFalseGame.ChoosenQuestionRandom.answer)
                                    {
                                        Console.WriteLine($@"If you think the statement is incorrect, you can change this. Press c to do so, or press enter to continue.");
                                    }
                                    if (!TrueFalseGame.ChoosenQuestionRandom.answer)
                                    {
                                        Console.WriteLine($@"If you think the statement is correct, you can change this. Press c to do so, or press enter to continue.");
                                    }
                                    Console.CursorVisible = false;
                                    ConsoleKeyInfo consoleKeyInfo6 = Console.ReadKey(true);
                                    while (consoleKeyInfo6.Key.ToString().ToUpper() != "ENTER" && consoleKeyInfo6.Key.ToString().ToUpper() != "C")
                                    {
                                        while (Console.KeyAvailable)
                                        {
                                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                        }
                                        consoleKeyInfo6 = Console.ReadKey(true);
                                    }
                                    if (consoleKeyInfo6.Key.ToString().ToUpper() == "ENTER")
                                    {
                                        Console.CursorVisible = true;
                                        // continue than..
                                    }
                                    if (consoleKeyInfo6.Key.ToString().ToUpper() == "C")
                                    {
                                        XDocument documentToEdit = TrueFalseGame.QuestionsDocument;
                                        XElement root = documentToEdit.Root;
                                        foreach(XElement ThisQuestion in TrueFalseGame.questions)
                                        {
                                            string ItsOriginalText = File.ReadAllText(ThisQuestion.Element("Text").Value);
                                            if(TrueFalseGame.ChoosenQuestionRandom.QuestionContent == ItsOriginalText)
                                            {
                                                #region rewriting xml file

                                                XElement xElementAnswer = ThisQuestion.Element("Answer");
                                                if(xElementAnswer.Value == "True")
                                                {
                                                    TrueFalseGame.questions.Find(x => x == ThisQuestion).Element("Answer").Value = "False";
                                                }
                                                else if(xElementAnswer.Value == "False")
                                                {
                                                    TrueFalseGame.questions.Find(x => x == ThisQuestion).Element("Answer").Value = "True";
                                                }

                                                root.RemoveNodes();
                                                root.Add(TrueFalseGame.questions);
                                                documentToEdit.RemoveNodes();
                                                documentToEdit.Add(root);

                                                File.Delete($@"You think you are smart xml\CorrectOrIncorrectXML.xml");
                                                documentToEdit.Save($@"You think you are smart xml\CorrectOrIncorrectXML.xml");

                                                #endregion

                                                Console.WriteLine();
                                                Console.WriteLine("Correct answer change applied successfully!");
                                                Thread.Sleep(2000);
                                                while (Console.KeyAvailable)
                                                {
                                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                                }

                                            }
                                        }
                                        /*

                                        Console.WriteLine();
                                        Console.WriteLine("Ok than, write the correct answer here. Your answer number will remain, just its text would change:");
                                        Console.WriteLine();
                                        Console.CursorVisible = true;
                                        string AnswerOpinion = Console.ReadLine();
                                        while (AnswerOpinion == string.Empty)
                                        {
                                            AnswerOpinion = Console.ReadLine();
                                        }

                                        #region Update..

                                        UpdateAnswer("Trap", AnswerOpinion, null, question);
                                        Console.WriteLine();
                                        Console.WriteLine("Change of answer applied succesfully!");
                                        Console.WriteLine();
                                        Thread.Sleep(3000);
                                        while (Console.KeyAvailable)
                                        {
                                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                        }

                                        #endregion

                                        */
                                    }


                                    #endregion

                                    game.IsOnCorrectOrFalseMode = false;
                                    Console.Clear();
                                    return game;

                                    #endregion
                                }
                                if (TrueFalseGame.ChoosenQuestionRandom.answer != TrueFalseGame.ChoosenQuestionRandom.AnswerChoice)
                                {
                                    #region If the player is wrong

                                    game.GamePlayers.Find(x => x == TrueFalseGame.playerForChellenge).PreTrapSum = 0;
                                    TrueFalseGame.playerForChellenge.PreTrapSum = 0;

                                    CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\IncorrectAnswer.wav", "Oh no shit!");
                                    /*
                                    SoundPlayer InCorrectAnswerResponse = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\IncorrectAnswer.wav");
                                    InCorrectAnswerResponse.Play();
                                    Thread.Sleep(1000);
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }
                                    */
                                    Console.WriteLine($@"{TrueFalseGame.playerForChellenge.PlayerName} Now has ${TrueFalseGame.playerForChellenge.PreTrapSum}.");
                                    Thread.Sleep(2000);
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }

                                    #region Correct the answer if you want

                                    Console.WriteLine();
                                    if (TrueFalseGame.ChoosenQuestionRandom.answer)
                                    {
                                        Console.WriteLine($@"If you think the statement is incorrect, you can change this. Press c to do so, or press enter to continue.");
                                    }
                                    if (!TrueFalseGame.ChoosenQuestionRandom.answer)
                                    {
                                        Console.WriteLine($@"If you think the statement is correct, you can change this. Press c to do so, or press enter to continue.");
                                    }
                                    Console.CursorVisible = false;
                                    ConsoleKeyInfo consoleKeyInfo7 = Console.ReadKey(true);
                                    while (consoleKeyInfo7.Key.ToString().ToUpper() != "ENTER" && consoleKeyInfo7.Key.ToString().ToUpper() != "C")
                                    {
                                        while (Console.KeyAvailable)
                                        {
                                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                        }
                                        consoleKeyInfo7 = Console.ReadKey(true);
                                    }
                                    if (consoleKeyInfo7.Key.ToString().ToUpper() == "ENTER")
                                    {
                                        Console.CursorVisible = true;
                                        // continue than..
                                    }
                                    if (consoleKeyInfo7.Key.ToString().ToUpper() == "C")
                                    {
                                        XDocument documentToEdit = TrueFalseGame.QuestionsDocument;
                                        XElement root = documentToEdit.Root;
                                        foreach (XElement ThisQuestion in TrueFalseGame.questions)
                                        {
                                            string ItsOriginalText = File.ReadAllText(ThisQuestion.Element("Text").Value);
                                            if (TrueFalseGame.ChoosenQuestionRandom.QuestionContent == ItsOriginalText)
                                            {
                                                #region rewriting xml file

                                                XElement xElementAnswer = ThisQuestion.Element("Answer");
                                                if (xElementAnswer.Value == "True")
                                                {
                                                    TrueFalseGame.questions.Find(x => x == ThisQuestion).Element("Answer").Value = "False";
                                                }
                                                else if (xElementAnswer.Value == "False")
                                                {
                                                    TrueFalseGame.questions.Find(x => x == ThisQuestion).Element("Answer").Value = "True";
                                                }

                                                root.RemoveNodes();
                                                root.Add(TrueFalseGame.questions);
                                                documentToEdit.RemoveNodes();
                                                documentToEdit.Add(root);

                                                File.Delete($@"You think you are smart xml\CorrectOrIncorrectXML.xml");
                                                documentToEdit.Save($@"You think you are smart xml\CorrectOrIncorrectXML.xml");

                                                #endregion

                                                Console.WriteLine();
                                                Console.WriteLine("Correct answer change applied successfully!");
                                                Thread.Sleep(2000);
                                                while (Console.KeyAvailable)
                                                {
                                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                                }

                                            }
                                        }
                                        /*

                                        Console.WriteLine();
                                        Console.WriteLine("Ok than, write the correct answer here. Your answer number will remain, just its text would change:");
                                        Console.WriteLine();
                                        Console.CursorVisible = true;
                                        string AnswerOpinion = Console.ReadLine();
                                        while (AnswerOpinion == string.Empty)
                                        {
                                            AnswerOpinion = Console.ReadLine();
                                        }

                                        #region Update..

                                        UpdateAnswer("Trap", AnswerOpinion, null, question);
                                        Console.WriteLine();
                                        Console.WriteLine("Change of answer applied succesfully!");
                                        Console.WriteLine();
                                        Thread.Sleep(3000);
                                        while (Console.KeyAvailable)
                                        {
                                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                        }

                                        #endregion

                                        */
                                    }


                                    #endregion


                                    game.IsOnCorrectOrFalseMode = false;
                                    Console.Clear();
                                    return game;

                                    #endregion                                    
                                }

                                return game;
                            }
                        }

                    }

                    #endregion
                }
            }

            #endregion

            return game;

            //Player Player
        }

        #endregion

        #region Identify the idea game

        public Game IdentifyTheIdeaGame(Game game, bool IntroductionHasHappend)
        {
            double SecondsThatPassed = 0;

            #region Intrudction part

            if (!IntroductionHasHappend)
            {
                Console.Clear();

                string BlankSpace = "";
                for (int i = 0; i < 30; i++)
                {
                    BlankSpace = BlankSpaces(i + 1);
                    Console.Write($@"{BlankSpace}Identify the idea..");
                    Thread.Sleep(10);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                    Console.Clear();
                }

                BlankSpace = BlankSpaces(31);
                Console.Write($@"{BlankSpace}Identify the idea..");

                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }

                SoundPlayer IdentifyTheIdeaIntroduction = PlayingInMultiThreading($@"You-think-you-are-smart\IdeaIdentifyQuestions\Instructions.wav", "We're on Identify the idea game. On the screen will appear part of a name of an opinion or a theory about something. You have to guess the full name. Every several seconds, a clue will appear suggesting what the name is. The earlier you answer, the more money you will earn or lose. Good luck.");
                IdentifyTheIdeaIntroduction.Play();

                //SoundPlayer IdentifyTheIdeaIntroduction = new SoundPlayer($@"You-think-you-are-smart\IdentifyTheIdeaQuestions\Instructions.wav");
                //IdentifyTheIdeaIntroduction.Play();

                double audioLength = 0;

                using (var audioFile = new AudioFileReader(IdentifyTheIdeaIntroduction.SoundLocation))
                using (var outputDevice1 = new WaveOutEvent())
                {
                    double audioLengthMs1 = audioFile.TotalTime.TotalMilliseconds;
                    audioLength = Math.Round(audioLengthMs1);
                }
                /*
                int startSecond = DateTime.Now.Second;
                int startMinute = DateTime.Now.Minute;
                int endSecond;
                int endMinute;
                if (startSecond <= 43)
                {
                    endMinute = startMinute;
                    endSecond = startSecond + 17;
                }
                else
                {
                    endMinute = startMinute + 1;
                    endSecond = startSecond - 43;
                }
                */

                CreatingAndPlayingAudio(@"You-think-you-are-smart\OtherSounds\SkippedInstructions.wav", "Ouch! It hurts! I'm gonna get another seizure!", null, true);
                SoundPlayer InstructionsOtherAudio = new SoundPlayer(@"You-think-you-are-smart\OtherSounds\SkippedInstructions.wav");

                while (!Console.KeyAvailable/* && (DateTime.Now.Second == endSecond && DateTime.Now.Minute != endMinute)*/ && SecondsThatPassed < (audioLength / 1000))
                {
                    Thread.Sleep(100);
                    SecondsThatPassed = SecondsThatPassed + 0.1;
                    if (SecondsThatPassed >= (audioLength / 1000))
                    {
                        break;
                    }
                }
                

                if (Console.KeyAvailable == true)
                {
                    IdentifyTheIdeaIntroduction.Stop();
                    IdentifyTheIdeaIntroduction.Dispose();
                    File.Delete($@"You-think-you-are-smart\IdeaIdentifyQuestions\Instructions.wav");
                    InstructionsOtherAudio.PlaySync();
                    //Thread.Sleep(3750);
                    InstructionsOtherAudio.Dispose();
                    File.Delete(@"You-think-you-are-smart\OtherSounds\SkippedInstructions.wav");
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }

                    Console.Clear();
                }

                if (SecondsThatPassed >= (audioLength / 1000))
                {
                    Console.Clear();
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }

                    IdentifyTheIdeaIntroduction.Dispose();
                    File.Delete($@"You-think-you-are-smart\IdeaIdentifyQuestions\Instructions.wav");
                    InstructionsOtherAudio.Dispose();
                    File.Delete(@"You-think-you-are-smart\OtherSounds\SkippedInstructions.wav");
                }
            }


            #endregion

            #region The game itself

            foreach (Player player in game.GamePlayers)
            {
                player.HasThePlayerChoosenAnswer = false;
                player.WrongAnswerChoosen = 0;
            }

            Console.Clear();

            // the clues can appear from above
            // but this also means withhin every clue, the console will have to be cleared
            // maybe it will be easier to put the incomplete above, so we won't have to clear the console very time
            // timer will have to appear along with the clues
            // a reminder, for the writing itself, console.ReadLine() will do the trick
            // the sum of money also has to be written and changed accordingly.
            // bottom line: very complicated, take the weekend for this

            IdentifyTheIdea IdentifyTheIdeaObject = new IdentifyTheIdea(game);
            bool HasNoOneAnswered = true;
            bool IsAfterAPlayerWrong = false;
            double ExtraSecondsUntilNextClue = 0;

            double SecondsThatPassed3 = 0;

            for (int i = 0; i < 4; i++)
            {

                if (IsAfterAPlayerWrong)
                {
                    i = i - 1;
                    IsAfterAPlayerWrong = false;
                }
                else
                {
                    ExtraSecondsUntilNextClue = 0;
                    SecondsThatPassed = 0;
                    SecondsThatPassed3 = 0;
                }
                Console.Clear();

                Console.WriteLine(IdentifyTheIdeaObject.choosenQuestion.InCompleteAnswer);
                Console.WriteLine("");
                bool IsCluesDone = false;
                for (int y = 0; !IsCluesDone; y++)
                {
                    Console.WriteLine($@"{y + 1}. {IdentifyTheIdeaObject.choosenQuestion.ClueText.ElementAt(y)}");
                    if (y == i)
                    {
                        IsCluesDone = true;
                    }
                }

                #region Writing players details

                List<int> WrongAnswersGiven = new List<int>();

                Console.WriteLine("");

                bool Player1AnsweredOrNot = false;
                bool Player2AnsweredOrNot = false;
                bool Player3AnsweredOrNot = true;

                if (game.GamePlayers.Count == 1)
                {
                    Player1AnsweredOrNot = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                }
                else if (game.GamePlayers.Count == 2)
                {
                    Player1AnsweredOrNot = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                    Player2AnsweredOrNot = game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer;
                }
                else if (game.GamePlayers.Count == 3)
                {
                    Player1AnsweredOrNot = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                    Player2AnsweredOrNot = game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer;
                    Player3AnsweredOrNot = game.GamePlayers.ElementAt(2).HasThePlayerChoosenAnswer;
                }

                Console.WriteLine("");
                if (game.GamePlayers.Count == 1)
                {
                    if (!Player1AnsweredOrNot)
                    {
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (Wrong..)");
                        Console.WriteLine("");
                    }
                }

                if (game.GamePlayers.Count == 2)
                {
                    if (!Player1AnsweredOrNot)
                    {
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) (Wrong..)");
                        Console.WriteLine("");
                    }

                    if (!Player2AnsweredOrNot)
                    {
                        Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) ({game.GamePlayers.ElementAt(1).PlayerBuzzer})");
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) (Wrong..)");
                        Console.WriteLine("");
                    }
                }

                if (game.GamePlayers.Count == 3)
                {
                    if (!Player1AnsweredOrNot)
                    {
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) (Wrong..)");
                        Console.WriteLine("");
                    }

                    if (!Player2AnsweredOrNot)
                    {
                        Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) ({game.GamePlayers.ElementAt(1).PlayerBuzzer})");
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) (Wrong..)");
                        Console.WriteLine("");
                    }

                    if (!Player3AnsweredOrNot)
                    {
                        Console.WriteLine($@"3. {game.GamePlayers.ElementAt(2).PlayerName} (${game.GamePlayers.ElementAt(2).PreTrapSum}) ({game.GamePlayers.ElementAt(2).PlayerBuzzer})");
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine($@"3. {game.GamePlayers.ElementAt(2).PlayerName} (${game.GamePlayers.ElementAt(2).PreTrapSum}) (Wrong..)");
                    }
                }

                #endregion

                SoundPlayer playingCurrentClue;

                playingCurrentClue = PlayingInMultiThreading(IdentifyTheIdeaObject.choosenQuestion.CluesSoundPlayers.ElementAt(i).SoundLocation, IdentifyTheIdeaObject.choosenQuestion.ClueText.ElementAt(i));

                double audioLengthOfClue = 0;

                using (var audioFile = new AudioFileReader(IdentifyTheIdeaObject.choosenQuestion.CluesSoundPlayers.ElementAt(i).SoundLocation))
                using (var outputDevice1 = new WaveOutEvent())
                {
                    double audioLengthMs1 = audioFile.TotalTime.TotalMilliseconds;
                    audioLengthOfClue = Math.Round(audioLengthMs1);
                }

                playingCurrentClue.Play();
                //IdentifyTheIdeaObject.choosenQuestion.CluesSoundPlayers.ElementAt(i).Play();


                if (ExtraSecondsUntilNextClue == 0)
                {
                    SecondsThatPassed3 = 0;
                }
                else
                {
                    if(audioLengthOfClue <= 10000)
                    {
                        SecondsThatPassed3 = 10 - ExtraSecondsUntilNextClue;
                    }
                    else
                    {
                        SecondsThatPassed3 = audioLengthOfClue - ExtraSecondsUntilNextClue;
                    }
                }

                string TheKeyPressed = "";
                Console.WriteLine("");
                ConsoleKeyInfo buzzer = new ConsoleKeyInfo();

                /*
                 * while (!Console.KeyAvailable && SecondsThatPassed < 10)
            {
                Thread.Sleep(1000);
                SecondsThatPassed = SecondsThatPassed + 1;
                Console.Write($@"{10 - SecondsThatPassed}");
                
                if (SecondsThatPassed == 10)
                {
                    break;
                }

                (int left, int top) test = Console.GetCursorPosition();
                Console.SetCursorPosition(test.left - 1, test.top);
            }
                */
                if (audioLengthOfClue <= 10000)
                {
                    while (!Console.KeyAvailable && SecondsThatPassed3 <= 10)
                    {
                        Thread.Sleep(1000);
                        if (IdentifyTheIdeaObject.choosenQuestion.Money == 9750)
                        {
                            (int left, int top) test2 = Console.GetCursorPosition();
                            Console.SetCursorPosition(5, test2.top);
                            Console.Write(" ");
                            Console.SetCursorPosition(0, test2.top);
                        }
                        if (IdentifyTheIdeaObject.choosenQuestion.Money == 750)
                        {
                            (int left, int top) test4 = Console.GetCursorPosition();
                            Console.SetCursorPosition(4, test4.top);
                            Console.Write(" ");
                            Console.SetCursorPosition(0, test4.top);
                        }
                        if (IdentifyTheIdeaObject.choosenQuestion.Money == 0)
                        {
                            (int left, int top) test5 = Console.GetCursorPosition();
                            Console.SetCursorPosition(2, test5.top);
                            Console.Write("   ");
                            Console.SetCursorPosition(0, test5.top);
                        }

                        Console.Write($@"${IdentifyTheIdeaObject.choosenQuestion.Money}");
                        SecondsThatPassed3 = SecondsThatPassed3 + 1;
                        IdentifyTheIdeaObject.choosenQuestion.Money = IdentifyTheIdeaObject.choosenQuestion.Money - 250;
                        if (SecondsThatPassed3 > 10)
                        {
                            IdentifyTheIdeaObject.choosenQuestion.Money = IdentifyTheIdeaObject.choosenQuestion.Money + 250;
                            break;
                        }

                        (int left, int top) test = Console.GetCursorPosition();
                        Console.SetCursorPosition(0, test.top);
                        IdentifyTheIdeaObject.choosenQuestion.TimeLeft = IdentifyTheIdeaObject.choosenQuestion.TimeLeft - 1;
                    }
                }
                else
                {
                    while (!Console.KeyAvailable && SecondsThatPassed3 < audioLengthOfClue)
                    {
                        Thread.Sleep(1000);
                        if (IdentifyTheIdeaObject.choosenQuestion.Money == 9750)
                        {
                            (int left, int top) test2 = Console.GetCursorPosition();
                            Console.SetCursorPosition(5, test2.top);
                            Console.Write(" ");
                            Console.SetCursorPosition(0, test2.top);
                        }
                        if (IdentifyTheIdeaObject.choosenQuestion.Money == 750)
                        {
                            (int left, int top) test4 = Console.GetCursorPosition();
                            Console.SetCursorPosition(4, test4.top);
                            Console.Write(" ");
                            Console.SetCursorPosition(0, test4.top);
                        }
                        if (IdentifyTheIdeaObject.choosenQuestion.Money == 0)
                        {
                            (int left, int top) test5 = Console.GetCursorPosition();
                            Console.SetCursorPosition(2, test5.top);
                            Console.Write("   ");
                            Console.SetCursorPosition(0, test5.top);
                        }

                        Console.Write($@"${IdentifyTheIdeaObject.choosenQuestion.Money}");
                        SecondsThatPassed3 = SecondsThatPassed3 + 1;
                        //IdentifyTheIdeaObject.choosenQuestion.Money = IdentifyTheIdeaObject.choosenQuestion.Money - 250;
                        if (SecondsThatPassed3 >= audioLengthOfClue)
                        {
                            IdentifyTheIdeaObject.choosenQuestion.Money = IdentifyTheIdeaObject.choosenQuestion.Money + 250;
                            break;
                        }

                        (int left, int top) test = Console.GetCursorPosition();
                        Console.SetCursorPosition(0, test.top);

                        IdentifyTheIdeaObject.choosenQuestion.TimeLeft = IdentifyTheIdeaObject.choosenQuestion.TimeLeft - 1;
                    }
                }
                if (SecondsThatPassed3 >= 10 && SecondsThatPassed3 > audioLengthOfClue)
                {
                    ExtraSecondsUntilNextClue = 0;
                    // than continue to the next clue
                    if (i == 3)
                    {
                        Console.Write($@" $0");
                    }
                    IsAfterAPlayerWrong = false;
                }
                if (Console.KeyAvailable)
                {
                    IdentifyTheIdeaObject.choosenQuestion.CluesSoundPlayers.ElementAt(i).Stop();

                    Console.Beep();

                    ConsoleKeyInfo consoleKeyInfo = new ConsoleKeyInfo();
                    consoleKeyInfo = Console.ReadKey(true);

                    #region Throwing wrong key press                       

                    if (consoleKeyInfo.Key.ToString().ToUpper() != "A" && consoleKeyInfo.Key.ToString().ToUpper() != "M" && consoleKeyInfo.Key.ToString().ToUpper() != "OEMPLUS")
                    {
                        playingCurrentClue.Stop();
                        playingCurrentClue.Dispose();

                        CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\HandlingIncorrectBuzzerPressing.wav", "Stop pressing the wrong buzzer even in other modes!");

                        #region delete all clue files

                        foreach (string pathForClue in IdentifyTheIdeaObject.choosenQuestion.CluesSoundPathes)
                        {
                            File.Delete(pathForClue);
                        }

                        #endregion

                        //buzzerexpection.PlaySync();
                        //Thread.Sleep(5000);
                        throw new Exception($@"Sorry, can't handle incorrect key press at this mode. I tried for over 20 hours to find a solution for this and failed. Restart the game");
                        /*
                         * 
                            //Console.WriteLine("Press only on one of your buzzers - A/M/+");
                            // a sound can be put in here
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                            }
                            consoleKeyInfo = Console.ReadKey(true);
                            Console.Write($@"{10 - SecondsThatPassed}");
                            Thread.Sleep(1000);
                            SecondsThatPassed = SecondsThatPassed + 1;

                            //consoleKeyInfo = Console.ReadKey(true);
                        */
                    }


                    #endregion

                    #region Ensuring relevant buzzer has been pressed

                    if (consoleKeyInfo.Key.ToString().ToUpper() == "M" || consoleKeyInfo.Key.ToString().ToUpper() == "A" || consoleKeyInfo.Key.ToString().ToUpper() == "OEMPLUS")
                    {
                        bool IsTheBuzzerCorrect = false;

                        if (game.GamePlayers.Count == 1)
                        {
                            if (consoleKeyInfo.Key.ToString().ToUpper() == "M")
                            {
                                IsTheBuzzerCorrect = true;
                            }
                        }
                        if (game.GamePlayers.Count == 2)
                        {
                            if (consoleKeyInfo.Key.ToString().ToUpper() == "M" || consoleKeyInfo.Key.ToString().ToUpper() == "OEMPLUS")
                            {
                                IsTheBuzzerCorrect = true;
                            }
                        }
                        if (game.GamePlayers.Count == 3)
                        {
                            if (consoleKeyInfo.Key.ToString().ToUpper() == "M" || consoleKeyInfo.Key.ToString().ToUpper() == "OEMPLUS" || consoleKeyInfo.Key.ToString().ToUpper() == "A")
                            {
                                IsTheBuzzerCorrect = true;
                            }
                        }

                        if (!IsTheBuzzerCorrect)
                        {
                            #region Throwing wrong key press                       

                            CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\HandlingIncorrectBuzzerPressing.wav", "Stop pressing on the wrong buzzer in other modes!");
                            //buzzerexpection.PlaySync();
                            //Thread.Sleep(5000);
                            #region delete all clue files

                            foreach (string pathForClue in IdentifyTheIdeaObject.choosenQuestion.CluesSoundPathes)
                            {
                                File.Delete(pathForClue);
                            }

                            #endregion

                            throw new Exception($@"Sorry, can't handle incorrect key press at this mode. I tried for over 20 hours to find a solution for this and failed. Restart the game");
                            /*
                             * 
                                //Console.WriteLine("Press only on one of your buzzers - A/M/+");
                                // a sound can be put in here
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }
                                consoleKeyInfo = Console.ReadKey(true);
                                Console.Write($@"{10 - SecondsThatPassed}");
                                Thread.Sleep(1000);
                                SecondsThatPassed = SecondsThatPassed + 1;

                                //consoleKeyInfo = Console.ReadKey(true);
                            */

                            #endregion
                        }
                    }

                    #endregion

                    string ActivatedBuzzer = consoleKeyInfo.Key.ToString();

                    #region What to do when a player buzzers, before he answers

                    #region If it's a player who already answered..

                    bool Player1AnsweredAlready = false;
                    bool Player2AnsweredAlready = false;
                    bool Player3AnsweredAlready = false;

                    if (game.GamePlayers.Count == 1)
                    {
                        Player1AnsweredAlready = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && ActivatedBuzzer == "M";
                    }
                    else if (game.GamePlayers.Count == 2)
                    {
                        Player1AnsweredAlready = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && ActivatedBuzzer == "M";
                        Player2AnsweredAlready = game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer && ActivatedBuzzer == "OEMPLUS";
                    }
                    else if (game.GamePlayers.Count == 3)
                    {
                        Player1AnsweredAlready = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && ActivatedBuzzer == "M";
                        Player2AnsweredAlready = game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer && ActivatedBuzzer == "OEMPLUS";
                        Player3AnsweredAlready = game.GamePlayers.ElementAt(2).HasThePlayerChoosenAnswer && ActivatedBuzzer == "A";
                    }

                    if (game.GamePlayers.Count == 2)
                    {
                        if (Player1AnsweredAlready || Player2AnsweredAlready)
                        {
                            CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\PressingBuzzerAgainException.wav", "Smartass! You've already answered this question! Do you meant the game to explode?");

                            //SoundPlayer ReBuzzerException = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\PressingBuzzerAgainException.wav");
                            //ReBuzzerException.PlaySync();
                            //Thread.Sleep(5000);
                            #region delete all clue files

                            foreach (string pathForClue in IdentifyTheIdeaObject.choosenQuestion.CluesSoundPathes)
                            {
                                File.Delete(pathForClue);
                            }

                            #endregion

                            throw new Exception(" The sound you just heard explains this.. Restart the game. ");
                        }
                    }
                    if (game.GamePlayers.Count == 3)
                    {
                        if (Player1AnsweredAlready || Player2AnsweredAlready || Player3AnsweredAlready)
                        {
                            CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\PressingBuzzerAgainException.wav", "Smartass! You've already answered this question! You sure are gonna lose a lot of money now..");

                            #region delete all clue files

                            foreach (string pathForClue in IdentifyTheIdeaObject.choosenQuestion.CluesSoundPathes)
                            {
                                File.Delete(pathForClue);
                            }

                            #endregion


                            //SoundPlayer ReBuzzerException = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\PressingBuzzerAgainException.wav");
                            //ReBuzzerException.PlaySync();
                            //Thread.Sleep(5000);
                            throw new Exception(" The sound you just heard explains this.. Restart the game. ");
                        }
                    }

                    #endregion

                    #region Waiting for answer from him..

                    double SecondsForAnswer = 0;

                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                    Player playerAnswering = new Player(1, "");

                    ConsoleKeyInfo ChoosenAnswer = new ConsoleKeyInfo();
                    if (ActivatedBuzzer.ToUpper() == "M")
                    {
                        playerAnswering = new Player(game.GamePlayers.ElementAt(0).PlayerNumber, game.GamePlayers.ElementAt(0).PlayerName);
                        playerAnswering = game.GamePlayers.ElementAt(0);
                        playerAnswering.PlayerNumber = 1;
                    }
                    if (ActivatedBuzzer.ToUpper() == "A")
                    {
                        playerAnswering = new Player(game.GamePlayers.ElementAt(2).PlayerNumber, game.GamePlayers.ElementAt(2).PlayerName);
                        playerAnswering = game.GamePlayers.ElementAt(2);
                        playerAnswering.PlayerNumber = 3;
                    }
                    if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                    {
                        playerAnswering = new Player(game.GamePlayers.ElementAt(1).PlayerNumber, game.GamePlayers.ElementAt(1).PlayerName);
                        playerAnswering = game.GamePlayers.ElementAt(1);
                        playerAnswering.PlayerNumber = 2;
                    }

                    ExtraSecondsUntilNextClue = SecondsThatPassed3;

                    Console.Beep();
                    Console.WriteLine("");
                    Console.WriteLine($@"{game.GamePlayers.ElementAt(playerAnswering.PlayerNumber - 1).PlayerName} is Answering..");
                    Console.WriteLine("");

                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }

                    //Thread timerThread2 = new Thread(new ThreadStart(RunTimerForNormalModeSomeone));
                    //double SecondsForAnswer = 10;
                    //timerThread2.Start();

                    //Thread count = new Thread(new ThreadStart(RunTimerForWhoAmIAndCorrectOrFalseModes));
                    //count.Start();

                    /*
                     * Console.Write($@" {10 - SecondsForAnswer}");
                            SecondsForAnswer = SecondsForAnswer + 1;
                            Thread.Sleep(1000);
                    */
                    DateTime now = DateTime.Now;
                    string PlayerAnswer = Console.ReadLine();

                    DateTime after = DateTime.Now;
                    bool AfterMinute = after.Minute > now.Minute && after.Second >= now.Second - 30;
                    bool SameMinute = after.Minute == now.Minute && after.Second >= now.Second + 30;

                    if (now.Year == 9999)
                    {
                        #region the same as the player not answering

                        IsAfterAPlayerWrong = true;

                        CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\NoOneAnswered.wav", "Hey hey hey! Stop sleeping! Do you think god will answer for you?");
                        //SoundPlayer NoAnswer = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\NoOneAnswered.wav");
                        //NoAnswer.PlaySync();
                        //Thread.Sleep(6500);

                        //NoAnswer.Dispose();
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }


                        if (ActivatedBuzzer.ToUpper() == "M")
                        {
                            playerAnswering = game.GamePlayers.ElementAt(0);
                            playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                        }
                        else if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                        {
                            playerAnswering = game.GamePlayers.ElementAt(1);
                            playerAnswering.PlayerName = game.GamePlayers.ElementAt(1).PlayerName;
                            playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(1).PlayerNumber;
                        }
                        else if (ActivatedBuzzer.ToUpper() == "A")
                        {
                            playerAnswering = game.GamePlayers.ElementAt(2);
                            playerAnswering.PlayerName = game.GamePlayers.ElementAt(2).PlayerName;
                            playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(2).PlayerNumber;
                        }

                        playerAnswering.PreTrapSum = playerAnswering.PreTrapSum - IdentifyTheIdeaObject.choosenQuestion.Money;

                        Console.Beep();
                        Console.WriteLine();
                        Console.WriteLine("It's been over 30 seconds..");
                        Console.WriteLine("What, you thought you could get away with it?");
                        Console.WriteLine($@"{playerAnswering.PlayerName} now has ${playerAnswering.PreTrapSum}");
                        Console.WriteLine();
                        playerAnswering.HasThePlayerChoosenAnswer = true;
                        game.GamePlayers.ElementAt(playerAnswering.PlayerNumber - 1).HasThePlayerChoosenAnswer = true;
                        bool HasEveryoneAnswered = false;
                        HasNoOneAnswered = false;

                        if (game.GamePlayers.Count == 1)
                        {
                            HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                        }
                        if (game.GamePlayers.Count == 2)
                        {
                            HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer;
                        }
                        if (game.GamePlayers.Count == 3)
                        {
                            HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(2).HasThePlayerChoosenAnswer;
                        }
                        if (!HasEveryoneAnswered)
                        {
                            Thread.Sleep(2000);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                            //game = IdentifyTheIdeaGame(game, true);
                            //return game;
                        }
                        else
                        {
                            Thread.Sleep(1000);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }

                            CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\EveryoneAreWrong.wav", "After your failed attempts, I might be better off showing you my stupid opinion regarding the current answer.");
                            /*
                            SoundPlayer EveryoneAreWrong = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\EveryoneAreWrong.wav");
                            EveryoneAreWrong.Play();
                            Thread.Sleep(3500);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                            */
                            Console.Clear();
                            Console.Beep();
                            Console.WriteLine($@"The correct answer (in my opinion) is:");
                            Console.WriteLine($@"");
                            string CorrectAnswer = "";
                            string CorrectAnswerContent = $@"{IdentifyTheIdeaObject.choosenQuestion.CorrectAnswer}";

                            #region delete all clue files

                            foreach (string pathForClue in IdentifyTheIdeaObject.choosenQuestion.CluesSoundPathes)
                            {
                                File.Delete(pathForClue);
                            }

                            #endregion


                            Console.WriteLine($@"{CorrectAnswerContent}");

                            Thread.Sleep(3000);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                            }
                            Console.Clear();
                            return game;
                        }

                        #endregion
                    }

                    else
                    {
                        #region If the player is right

                        if (PlayerAnswer.ToUpper() == IdentifyTheIdeaObject.choosenQuestion.CorrectAnswer.ToUpper())
                        {
                            #region if the answer is correct..     

                            if (ActivatedBuzzer.ToUpper() == "M")
                            {
                                playerAnswering = game.GamePlayers.ElementAt(0);
                                playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                            }
                            else if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                            {
                                playerAnswering = game.GamePlayers.ElementAt(1);
                                playerAnswering.PlayerName = game.GamePlayers.ElementAt(1).PlayerName;
                                playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(1).PlayerNumber;
                            }
                            else if (ActivatedBuzzer.ToUpper() == "A")
                            {
                                playerAnswering = game.GamePlayers.ElementAt(2);
                                playerAnswering.PlayerName = game.GamePlayers.ElementAt(2).PlayerName;
                                playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(2).PlayerNumber;
                            }

                            HasNoOneAnswered = false;
                            IsAfterAPlayerWrong = false;
                            if (playerAnswering.PreTrapSum + IdentifyTheIdeaObject.choosenQuestion.Money < 0)
                            {
                                playerAnswering.PreTrapSum = (playerAnswering.PreTrapSum + IdentifyTheIdeaObject.choosenQuestion.Money) - 250;
                            }
                            else
                            {
                                playerAnswering.PreTrapSum = (playerAnswering.PreTrapSum + IdentifyTheIdeaObject.choosenQuestion.Money) + 250;
                            }

                            CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\CorrectAnswer.wav", "Oh yes shit!");

                            Console.WriteLine("");
                            Console.WriteLine("Oh yeah! Correct Answer!");
                            Console.WriteLine("");

                            #region delete all clue files

                            foreach (string pathForClue in IdentifyTheIdeaObject.choosenQuestion.CluesSoundPathes)
                            {
                                File.Delete(pathForClue);
                            }

                            #endregion


                            Console.WriteLine($@"{playerAnswering.PlayerName} now has ${playerAnswering.PreTrapSum}.");
                            /* stopping by to explain sound file
                             * SoundPlayer explanation = new SoundPlayer($@"path for explanation .wav file")
                             * explanation.Play();
                             * Thread.Sleep(sound file length);
                             * than, no additional user kep press is required
                             */
                            ExtraSecondsUntilNextClue = 0;
                            SecondsThatPassed = 0;
                            SecondsThatPassed3 = 0;
                            Console.WriteLine("");
                            Console.WriteLine("Press enter to continue..");
                            Console.ReadLine();
                            Console.Clear();
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                            }
                            return game;

                            #endregion
                        }

                        #endregion

                        #region If the answer is wrong..

                        _stopRequestedForSomeoneAnswering = true;

                        if (ActivatedBuzzer.ToUpper() == "M")
                        {
                            playerAnswering = game.GamePlayers.ElementAt(0);
                            playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                        }
                        else if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                        {
                            playerAnswering = game.GamePlayers.ElementAt(1);
                            playerAnswering.PlayerName = game.GamePlayers.ElementAt(1).PlayerName;
                            playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(1).PlayerNumber;
                        }
                        else if (ActivatedBuzzer.ToUpper() == "A")
                        {
                            playerAnswering = game.GamePlayers.ElementAt(2);
                            playerAnswering.PlayerName = game.GamePlayers.ElementAt(2).PlayerName;
                            playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(2).PlayerNumber;
                        }

                        HasNoOneAnswered = false;

                        if (playerAnswering.PreTrapSum - IdentifyTheIdeaObject.choosenQuestion.Money < 0)
                        {
                            playerAnswering.PreTrapSum = (playerAnswering.PreTrapSum - IdentifyTheIdeaObject.choosenQuestion.Money) - 250;
                        }
                        else
                        {
                            playerAnswering.PreTrapSum = (playerAnswering.PreTrapSum - IdentifyTheIdeaObject.choosenQuestion.Money) + 250;
                        }

                        //playerAnswering.PreTrapSum = (playerAnswering.PreTrapSum - IdentifyTheIdeaObject.choosenQuestion.Money) - 250;

                        IsAfterAPlayerWrong = true;

                        Console.WriteLine("");

                        CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\IncorrectAnswer.wav", "Oh no shit!");

                        Console.Beep();
                        Console.WriteLine();
                        Console.WriteLine("Wrong!");
                        Console.WriteLine($@"{playerAnswering.PlayerName} now has ${playerAnswering.PreTrapSum}");
                        Console.WriteLine();
                        playerAnswering.HasThePlayerChoosenAnswer = true;

                        game.GamePlayers.ElementAt(playerAnswering.PlayerNumber - 1).HasThePlayerChoosenAnswer = true;
                        bool HasEveryoneAnswered = false;

                        if (game.GamePlayers.Count == 1)
                        {
                            HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                        }
                        if (game.GamePlayers.Count == 2)
                        {
                            HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer;
                        }
                        if (game.GamePlayers.Count == 3)
                        {
                            HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(2).HasThePlayerChoosenAnswer;
                        }
                        if (!HasEveryoneAnswered)
                        {
                            Thread.Sleep(2000);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                            double MoneytODecrease = 0;
                            if (i == 0)
                            {
                                MoneytODecrease = 7500;
                            }
                            if (i == 1)
                            {
                                MoneytODecrease = 5000;
                            }
                            if (i == 2)
                            {
                                MoneytODecrease = 2500;
                            }
                            if (i == 3)
                            {
                                MoneytODecrease = 0;
                            }
                            double calculating = IdentifyTheIdeaObject.choosenQuestion.Money - MoneytODecrease;
                            ExtraSecondsUntilNextClue = calculating / 250;

                            //game = IdentifyTheIdeaGame(game, true);
                            //return game;
                        }
                        else
                        {
                            Thread.Sleep(1000);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }

                            CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\EveryoneAreWrong.wav", "After your failed attempts, I might be better off showing you my stupid opinion regarding the correct answer.");

                            Console.Clear();

                            #region delete all clue files

                            foreach (string pathForClue in IdentifyTheIdeaObject.choosenQuestion.CluesSoundPathes)
                            {
                                File.Delete(pathForClue);
                            }

                            #endregion


                            Console.Beep();
                            Console.WriteLine($@"The correct answer (in my opinion) is:");
                            Console.WriteLine($@"");
                            string CorrectAnswer = "";
                            string CorrectAnswerContent = IdentifyTheIdeaObject.choosenQuestion.CorrectAnswer;

                            Console.WriteLine($@"{CorrectAnswerContent}");

                            Thread.Sleep(3000);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                            }
                            ExtraSecondsUntilNextClue = 0;
                            SecondsThatPassed = 0;
                            SecondsThatPassed3 = 0;
                            Console.Clear();
                            return game;
                        }
                        // Now to continue to the other buzzers..

                        #endregion
                    }

                    #endregion

                    #endregion
                }
            }

            #region If nobody bothered to answer or time out..

            if (HasNoOneAnswered || IdentifyTheIdeaObject.choosenQuestion.Money == 0)
            {
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                }

                CreatingAndPlayingAudio($@"You-think-you-are-smart\QuestionSound\NoOneAnswered.wav", "Hey, stop sleeping! Do you think God will answer for you?");
                /*
SoundPlayer NoAnswer = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\NoOneAnswered.wav");
NoAnswer.Play();
Thread.Sleep(6500);
while (Console.KeyAvailable)
{
    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
}
*/
                Console.Clear();

                #region delete all clue files

                foreach (string pathForClue in IdentifyTheIdeaObject.choosenQuestion.CluesSoundPathes)
                {
                    File.Delete(pathForClue);
                }

                #endregion


                Console.Beep();
                Console.WriteLine($@"The correct answer (in my opinion) is:");
                Console.WriteLine($@"");
                string CorrectAnswer = IdentifyTheIdeaObject.choosenQuestion.CorrectAnswer;

                //string CorrectAnswerContent = File.ReadAllText($@"
                // ($@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}\{QuestionsFoldersList.Find(x => x == CorrectFolder).Name}\Answer{TheQuestion.CorrectAnswer}.txt");

                Console.WriteLine($@"{CorrectAnswer}");

                Thread.Sleep(3000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                Console.Clear();
                return game;
            }

            #endregion

            #endregion

            Console.Beep();
            return game;
        }

        #endregion

        #region text to speech audio taken care of

        public static void CreatingAndPlayingAudio(string path, string? text = null, string[]? LongText = null, bool OnlyCreateAudio = false)
        {
            #region setting up everything

            #region setting up the sound

            string WantedVoice2 = string.Empty;

            using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
            {
                foreach (InstalledVoice voice in synthesizer.GetInstalledVoices())
                {
                    if (voice.VoiceInfo.Name.Contains("David"))
                    {
                        WantedVoice2 = voice.VoiceInfo.Name;
                    }
                }
                synthesizer.SelectVoice(WantedVoice2);

                synthesizer.SetOutputToWaveFile(path);
                if (string.IsNullOrEmpty(text))
                {
                    PromptBuilder promptBuilder = new PromptBuilder();
                    foreach (string textLine in LongText)
                    {
                        promptBuilder.AppendText(text);
                    }
                    //synthesizer.Speak(promptBuilder);
                    synthesizer.Speak(string.Join(" ", LongText));
                }
                else if (LongText == null)
                {
                    synthesizer.Speak(text);
                }
            }

            #endregion

            if (!OnlyCreateAudio)
            {
                double audioLength = 0;

                using (var audioFile = new AudioFileReader(path))
                using (var outputDevice1 = new WaveOutEvent())
                {
                    double audioLengthMs1 = audioFile.TotalTime.TotalMilliseconds;
                    audioLength = Math.Round(audioLengthMs1);
                }

                SoundPlayer PlaySound = new SoundPlayer(path);
                PlaySound.PlaySync();

                //Thread.Sleep((int)audioLength);
                PlaySound.Dispose();

                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                }

                File.Delete(path);
            }
            else
            {
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                }

                return;
            }

            #endregion
        }

        public static void PlayingAndWaitingForAudio(string path, string? OtherPath = null, bool KeepTheFirstFile = false, bool KeepTheSecond = false)
        {
            #region all the stuff

            double audioLength = 0;

            using (var audioFile = new AudioFileReader(path))
            using (var outputDevice1 = new WaveOutEvent())
            {
                double audioLengthMs1 = audioFile.TotalTime.TotalMilliseconds;
                audioLength = Math.Round(audioLengthMs1);
            }

            double SecondsThatPassed = 0;

            SoundPlayer TheAudio = new SoundPlayer(path);

            TheAudio.Play();
            while (!Console.KeyAvailable && SecondsThatPassed < (audioLength / 1000))
            {
                Thread.Sleep(100);
                SecondsThatPassed = SecondsThatPassed + 0.1;
            }
            SoundPlayer OtherAudioPlay = new SoundPlayer(OtherPath);

            double OtheraudioLength = 0;

            using (var OtheraudioFile = new AudioFileReader(path))
            using (var outputDevice2 = new WaveOutEvent())
            {
                double audioLengthMs2 = OtheraudioFile.TotalTime.TotalMilliseconds;
                OtheraudioLength = Math.Round(audioLengthMs2);
            }

            if (Console.KeyAvailable == true)
            {
                TheAudio.Stop();
                TheAudio.Dispose();
                OtherAudioPlay.PlaySync();
                OtherAudioPlay.Dispose();
                Thread.Sleep(2000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }

                Console.Clear();
            }
            if (SecondsThatPassed >= (audioLength / 1000))/*DateTime.Now.Second == endSecond && DateTime.Now.Minute == endMinute*/
            {
                Console.Clear();
                Thread.Sleep(2000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
            }
            if (!KeepTheFirstFile && KeepTheSecond)
            {
                File.Delete(path);
                return;
            }
            else if (!KeepTheFirstFile && !KeepTheSecond)
            {
                File.Delete(path);
                File.Delete(OtherPath);
                return;
            }
            else if (KeepTheFirstFile && KeepTheSecond)
            {
                return;
            }

            #endregion
        }

        public static SoundPlayer PlayingInMultiThreading(string path, string? text = null, string[]? LongText = null)
        {
            #region setting up the sound

            string WantedVoice2 = string.Empty;

            using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
            {
                foreach (InstalledVoice voice in synthesizer.GetInstalledVoices())
                {
                    if (voice.VoiceInfo.Name.Contains("David"))
                    {
                        WantedVoice2 = voice.VoiceInfo.Name;
                    }
                }
                synthesizer.SelectVoice(WantedVoice2);

                synthesizer.SetOutputToWaveFile(path);
                if (string.IsNullOrEmpty(text))
                {
                    PromptBuilder promptBuilder = new PromptBuilder();
                    foreach (string textLine in LongText)
                    {
                        promptBuilder.AppendText(text);
                    }
                    //synthesizer.Speak(promptBuilder);
                    synthesizer.Speak(string.Join(" ", LongText));
                }
                else if (LongText == null)
                {
                    synthesizer.Speak(text);
                }
            }

            #endregion

            SoundPlayer playnow = new SoundPlayer(path);
            return playnow;
            // delete manually the audio file
        }

        #endregion

        #region Answer Update

        public void UpdateAnswer(string mode, string CorrectedAnswer, NormalQuestion question = null, TrapQuestion trapQustion = null, TrueOrFalseQuestion trueOrFalseQuestion = null, IdentifyTheIdeaQuestion ideaQuestion = null)
        {
            if(mode.ToUpper() == "NORMAL")
            {
                DirectoryInfo directoryInfo = new DirectoryInfo($@"You-think-you-are-smart\QuestionsText\{question.Subject}\Question{question.QuestionNumber}");
                string PathToAnswer = $@"{directoryInfo.FullName}\Answer{question.CorrectAnswer}.txt";
                File.Delete(PathToAnswer);
                File.WriteAllText(PathToAnswer, $"{question.CorrectAnswer}. {CorrectedAnswer}");
            }
            else if(mode.ToUpper() == "TRAP")
            {
                string PathToAnswer = $@"You-think-you-are-smart\TrapQuestions\Question{trapQustion.QuestionNumberInDatabase}\answer{trapQustion.CorrectAnswer}.txt";
                File.Delete(PathToAnswer);
                File.WriteAllText(PathToAnswer, $"{CorrectedAnswer}");
            }
            else if(mode.ToUpper() == "IDEA")
            {

            }
            else if(mode.ToUpper() == "TRUEORFALSE")
            {

            }
            return;
        }

        #endregion

        #region Properties

        public int CountOfQuestion { get; set; }
        public Player PlayerToChooseCategory { get; set; }

        public List<Player> GamePlayers { get; set; }

        public int PlayersNum { get; set; }

        public int NumOfGameQuestions { get; set; }

        public bool IsOnMultiplyMode { get; set; }

        public bool IsOnCorrectOrFalseMode { get; set; }

        public bool IsOnTrapMode { get; set; }

        public bool IsOnIdentifyTheIdeaMode { get; set; }

        public bool IsOnChooseCatgoryMode { get; set; }

        public string PathToInstructions { get; set; }

        public bool IsGameOver { get; set; }
        public bool IsOnNormalMode { get; set; }
        public List<TrueOrFalseQuestion> CorrectOrFalseAlreadyAskedQuestions { get; set; }
        public List<TrapQuestion> TrapAlreadyAskedQuestions { get; set; }
        public List<NormalQuestion> NormalAlreadyAskedQuestions { get; set; }
        public List<IdentifyTheIdeaQuestion> IdentifyTheIdeaAlreadyAskedQuestions { get; set; }


        #endregion
    }
}