using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;
using System.Globalization;

using NAudio;
using NAudio.Wave;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;

namespace standup
{
    class Program
    {
        #region Miscellnous

        const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        const uint ENABLE_EXTENDED_FLAGS = 0x0080;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        internal const uint WM_SETICON = 0x80;
        internal static readonly IntPtr IDI_APPLICATION = new IntPtr(0x7F00);

        private struct CONSOLE_FONT_INFO_EX
        {
            internal uint cbSize;
            internal uint nFont;
            internal Coord dwFontSize;
            internal int FontFamily;
            internal int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            internal string FaceName;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Coord
        {
            internal short X;
            internal short Y;

            internal Coord(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        private const int StdOutputHandle = -11;
        private const uint FontType = 0x00040000;
        private const int TMPF_TRUETYPE = 0x04;


        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetCurrentConsoleFontEx(IntPtr consoleOutput, bool maximumWindow, ref CONSOLE_FONT_INFO_EX consoleCurrentFontEx);

        private static void SetConsoleFont(string fontName, short fontSize)
        {
            IntPtr hnd = GetStdHandle(StdOutputHandle);
            if (hnd != IntPtr.Zero)
            {
                CONSOLE_FONT_INFO_EX cfi = new CONSOLE_FONT_INFO_EX
                {
                    cbSize = (uint)Marshal.SizeOf<CONSOLE_FONT_INFO_EX>(),
                    FaceName = fontName,
                    dwFontSize = new Coord(fontSize, fontSize)
                };
                SetCurrentConsoleFontEx(hnd, false, ref cfi);
            }
        }

        #endregion

        public static void Main()
        {
            DisableQuickEditMode();
            IntPtr consoleHandle = GetConsoleWindow();
            IntPtr iconHandle = LoadIcon(IntPtr.Zero, IDI_APPLICATION);

            SendMessage(consoleHandle, WM_SETICON, new IntPtr(1), iconHandle); // Icon for the window
            SendMessage(consoleHandle, WM_SETICON, new IntPtr(0), iconHandle); // Small icon in the taskbar
            SetConsoleFont("Consolas", 28);

            string NameSoundPath = $@"You-think-you-are-smart\NameSounds";
            if (!Directory.Exists(NameSoundPath))
            {
                Directory.CreateDirectory(NameSoundPath);
            }
            string QuestionSpeech = $@"You-think-you-are-smart\QuestionSpeech";
            if (!Directory.Exists(QuestionSpeech))
            {
                Directory.CreateDirectory(QuestionSpeech);
            }
            string CategorySounds = $@"You-think-you-are-smart\CategorySounds";
            if (!Directory.Exists(CategorySounds))
            {
                Directory.CreateDirectory(CategorySounds);
            }
            string QuestionT = $@"You-think-you-are-smart\CategorySounds";
            if (!Directory.Exists(CategorySounds))
            {
                Directory.CreateDirectory(CategorySounds);
            }

            /*
            using (var synthesizer = new SpeechSynthesizer())
            {
                synthesizer.SetOutputToWaveFile($@"You-think-you-are-smart\test.wav");
                synthesizer.Speak("Hello, this is a test of the text to speech conversion.");
            } // Automatic disposal should ensure file is closed here
            
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToWaveFile($@"You-think-you-are-smart\test.wav");
            PromptBuilder builder = new PromptBuilder();
            builder.AppendText("Hello, this is a test of the text to speech conversion.");
            synthesizer.Speak(builder);
            synthesizer.Dispose();            

            */
            // You may have to add the special command that clears the console          

            Console.Title = "A wannabe-standup trivia game";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.SetBufferSize(174, 100);

            for (int repeat = 0; repeat < double.MaxValue; repeat++)
            {
                Console.Clear();

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                #region Pre game start..

                Console.WriteLine("We'll soon start this nonesense..");
                bool IsTwiceGame = false;

                //string[] openedText = (File.ReadAllLines(@"You-think-you-are-smart\HowManyTimesyYouPlayedToday.txt"));
                DateTime currentDate = DateTime.Now;
                string day = currentDate.DayOfYear.ToString();

                string[] otherText = new string[1];
                otherText[0] = day;

                /*
                for (int line = 0; line < openedText.Length; line++)
                {
                    string ThatDay = openedText[line];
                    int dayTime = int.Parse(day);
                    int ThatDayTime = int.Parse(ThatDay);
                    if (dayTime == ThatDayTime)
                    {
                        IsTwiceGame = true;
                        string PlayTwiceTodayPath = (@"You-think-you-are-smart\OtherSounds\PlayTodayAgain.wav");
                        SoundPlayer PlayTwiceToday = new SoundPlayer(PlayTwiceTodayPath);
                        PlayTwiceToday.Play();
                        Thread.Sleep(5000);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }
                        break;                        
                        
                    }
                }
                */
                /*
                string[] newText = new string[openedText.Length];
                for (int line = 0; line < openedText.Length; line++)
                {
                    newText[line] = openedText[line];
                }
                newText[openedText.Length - 1] = day;
                File.WriteAllLines(@"You-think-you-are-smart\HowManyTimesyYouPlayedToday.txt", newText);
                */
                if (!IsTwiceGame)
                {
                    DayOfWeek week = new DayOfWeek();
                    week = currentDate.DayOfWeek;

                    if (week.ToString() == "Saturday")
                    {
                        CreatingAndPlayingAudio($@"You-think-you-are-smart\OtherSounds\Saturday.wav", "How dare you play on saturday! God will punish you!");
                    }

                    else if (currentDate.DayOfYear == 1)
                    {
                        string NewYearPath = (@"You-think-you-are-smart\OtherSounds\NewYear.wav");

                        SoundPlayer NewYearSound = new SoundPlayer(NewYearPath);
                        CreatingAndPlayingAudio(NewYearPath, "The new year is gonna suck, doesn't it?");
                    }
                    else if(currentDate.Hour >= 0 && currentDate.Hour <= 6)
                    {
                        string CrazyNightPath = @"You-think-you-are-smart\OtherSounds\Night.wav";
                        CreatingAndPlayingAudio(CrazyNightPath, "Are you crazy? Playing at night? Go to sleep already!");
                    }
                    else
                    {
                        string DefaultPath = (@"You-think-you-are-smart\OtherSounds\Opening.wav");

                        CreatingAndPlayingAudio(DefaultPath, "Welcome to the dumbest game ever! But enjoy.");
                    }
                    
                }

                #endregion                

                 #region Game settings or creating questions

                Console.Clear();
                //Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Welcome to the edition of zahav to the dumbest game ever!");
                //Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("This game is a game of questions and trivia. It has nothing to do with intelligence btw.");
                //Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.Write("Test.. ");
                //Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You have to choose the right answer for every question. You get and lose money for every answer.");
                Console.WriteLine();
                //Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("Done reading? click 'c' to continue and confirm that you understand the rules and start this idiotic test, or press R to create your own questions for the game!");

                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                }

                ConsoleKeyInfo okay = Console.ReadKey(true);
                while (okay.Key.ToString().ToUpper() != "C" && okay.Key.ToString().ToUpper() != "R")
                {
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                    }
                    //Console.WriteLine("Press c, not other keys!");
                    okay = Console.ReadKey(true);
                }

                string choice = okay.Key.ToString();
                
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                }
                ButtonRespondSound();
                Console.Clear();
                if(choice.ToUpper() == "C")
                {
                    Console.WriteLine("Let's start this baby's game!");
                    Console.WriteLine("Hello! how many players are you, from 1 to 3?");
                    int numOfPlayers = 0;
                    ConsoleKeyInfo playersNum = Console.ReadKey(true);
                    while (playersNum.Key.ToString() != "D1" && playersNum.Key.ToString() != "D2" && playersNum.Key.ToString() != "D3" && playersNum.Key.ToString() != "Escape")
                    {
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }
                        playersNum = Console.ReadKey(true);
                    }

                    #region If the player pauses the game..

                    if (playersNum.Key.ToString() == "Escape")
                    {
                        Console.Beep();
                        Console.Clear();
                        Console.WriteLine("The game has been paused.");
                        Console.WriteLine("Press e to exit.");
                        Console.WriteLine("Press r to restart the game.");
                        ConsoleKeyInfo choiceOfEscape = Console.ReadKey(true);
                        string choiceEscapeText = choiceOfEscape.Key.ToString();
                        while (choiceEscapeText.ToUpper() != "E" && (choiceEscapeText.ToUpper() != "R"))
                        {
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                            choiceOfEscape = Console.ReadKey(true);
                            choiceEscapeText = choiceOfEscape.Key.ToString();
                            // this time let's just do nothing, no note
                        }

                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                        if (choiceEscapeText.ToUpper() == "E")
                        {
                            Environment.Exit(0);
                        }
                        if (choiceEscapeText.ToUpper() == "R")
                        {
                            Main();
                            return;
                        }
                    }

                    #endregion

                    Console.Beep();
                    Console.Clear();
                    string ThePlayerNumKey = playersNum.Key.ToString();
                    numOfPlayers = int.Parse(ThePlayerNumKey.Substring(1));
                    string? playerName = null;
                    List<string> playerNames = new List<string>(numOfPlayers);

                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                    }

                    Console.WriteLine("So you are " + numOfPlayers + " players.");
                    Console.WriteLine();
                    List<Player> players = new List<Player>();

                    for (int countPlayerNames = 1; countPlayerNames <= numOfPlayers; countPlayerNames++)
                    {
                        Console.WriteLine("Enter the name of player " + countPlayerNames.ToString() + ": (english only!)");
                        playerName = Console.ReadLine();
                        while (playerName.Contains(" ") || playerName == "" || playerName.Length >= 20)
                        {
                            Console.WriteLine("Don't include spaces or blanks and make the name short!");
                            playerName = Console.ReadLine();
                        }
                        playerNames.Add(playerName);
                        Player player = new Player(countPlayerNames, playerName);
                        Console.Beep();
                        players.Add(player);

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
                            synthesizer.SetOutputToWaveFile($@"You-think-you-are-smart\NameSounds\Player{countPlayerNames}.wav");
                            synthesizer.Speak($"{playerName}");
                        }
                        /*
                        SpeechSynthesizer synthesizer = new SpeechSynthesizer();                    


                        PromptBuilder builder = new PromptBuilder();
                        builder.StartVoice(VoiceGender.Male, VoiceAge.Adult);
                        builder.EndVoice();
                        PromptStyle d = new PromptStyle();
                        d.Volume = PromptVolume.Loud;
                        d.Emphasis = PromptEmphasis.Strong;
                        builder.StartStyle(d);
                        builder.EndStyle();
                        builder.AppendText($@"{playerName}");
                        synthesizer.SetOutputToWaveFile($@"You-think-you-are-smart\NameSounds\Player{countPlayerNames}.wav");
                        synthesizer.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult);                    
                        synthesizer.Speak(builder);                    
                        synthesizer.Dispose();
                        */
                        /*
                        using (var reader = new WaveFileReader($@"You-think-you-are-smart\NameSounds\Player{countPlayerNames}.wav"))
                        using (var converter = WaveFormatConversionStream.CreatePcmStream(reader))
                        using (var adpcmStream = new WaveFormatConversionStream(new AdpcmWaveFormat(8000, 1), converter))
                        using (var writer = new WaveFileWriter($@"You-think-you-are-smart\NameSounds\Player{countPlayerNames}", adpcmStream.WaveFormat))
                        {
                            WaveFileWriter.CreateWaveFile($@"You-think-you-are-smart\NameSounds\Player{countPlayerNames}", adpcmStream);
                            //adpcmStream.CopyTo(writer);
                        } 
                        */
                    }

                    Console.WriteLine();

                    List<int> NumOfQuestionsInTotal = new List<int>();

                    int ChoosenQuestions = 0;
                    Console.WriteLine("How many questions do you want? (up to 9)");
                    ConsoleKeyInfo questionsNum = Console.ReadKey(true);

                    while (!questionsNum.Key.ToString().Contains("D") || questionsNum.Key.ToString().ToUpper() == "D" && questionsNum.Key.ToString() != "Escape")
                    {
                        Console.WriteLine("Press on a number");
                        questionsNum = Console.ReadKey(true);
                    }
                    Console.Beep();

                    #region If the player pauses the game..

                    if (questionsNum.Key.ToString() == "Escape")
                    {
                        Console.Beep();
                        Console.Clear();
                        Console.WriteLine("The game has been paused.");
                        Console.WriteLine("Press e to exit.");
                        Console.WriteLine("Press r to restart the game.");
                        ConsoleKeyInfo choiceOfEscape = Console.ReadKey(true);
                        string choiceEscapeText = choiceOfEscape.Key.ToString();
                        while (choiceEscapeText.ToUpper() != "E" && (choiceEscapeText.ToUpper() != "R"))
                        {
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                            choiceOfEscape = Console.ReadKey(true);
                            choiceEscapeText = choiceOfEscape.Key.ToString();
                            // this time let's just do nothing, no note
                        }

                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                        if (choiceEscapeText.ToUpper() == "E")
                        {
                            Environment.Exit(0);
                        }
                        if (choiceEscapeText.ToUpper() == "R")
                        {
                            Main();
                            return;
                        }
                    }

                    #endregion

                    string TheQuestionNumKey = questionsNum.Key.ToString();
                    ChoosenQuestions = int.Parse(TheQuestionNumKey.Substring(1));

                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                    }

                    Console.WriteLine("Ok, so you've choosen " + ChoosenQuestions.ToString() + " questions.");

                    Game game = new Game(players, ChoosenQuestions, numOfPlayers);
                    //GetAllObjectsAndStart(game);
                    //game.GameOver(game);
                    game.Instructions();
                    Console.Clear();
                    game.NextTurn(game, game.GamePlayers.ElementAt(0));
                }
                else if(choice.ToUpper() == "R")
                {
                    Console.WriteLine("Which game mode you wanna add the question to?");
                    Console.WriteLine("Press N for normal question, T for trap question, C for true or false question, or I for an identify the idea question.");
                    ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                    while(consoleKeyInfo.Key.ToString() != "N" && consoleKeyInfo.Key.ToString() != "T" && consoleKeyInfo.Key.ToString() != "C" && consoleKeyInfo.Key.ToString() != "I")
                    {
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }
                        consoleKeyInfo = Console.ReadKey(true);
                    }
                    string choiceOfCreate = consoleKeyInfo.Key.ToString().ToUpper();
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                    }

                    Console.Clear();

                    #region Normal qustion

                    if (choiceOfCreate == "N")
                    {
                        Console.WriteLine("Would you like to add a category or add a question to an existing category?");
                        Console.WriteLine("Press C for adding a new category, or N for creating a question to an existing category.");
                        ConsoleKeyInfo WhatToDo = new ConsoleKeyInfo();
                        WhatToDo = Console.ReadKey(true);
                        while (WhatToDo.Key.ToString().ToUpper() != "C" && WhatToDo.Key.ToString().ToUpper() != "N")
                        {
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                            WhatToDo = Console.ReadKey(true);
                        }
                        string EditChoice = WhatToDo.Key.ToString().ToUpper();
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }
                        Console.Clear();
                        if(EditChoice == "C")
                        {
                            #region adding category..

                            Console.WriteLine("Give a name to the category.. and you're done.");
                            string CategoryName = Console.ReadLine();
                            while(CategoryName == string.Empty)
                            {
                                CategoryName = Console.ReadLine();
                            }

                            #region Updating it..

                            XDocument categories = new XDocument(XDocument.Load($@"You think you are smart xml\Categories.xml"));
                            XElement root = categories.Root;
                            IEnumerable<XElement> categoriesElements = root.Elements("Category");
                            List<XElement> categoriesList = new List<XElement>();
                            categoriesList = categoriesElements.ToList();
                            XElement NewCategory = new XElement(XName.Get("Category"));
                            NewCategory.SetValue(CategoryName);
                            categoriesList.Add(NewCategory);
                            root.RemoveNodes();
                            root.Add(categoriesList);
                            categories.RemoveNodes();
                            categories.Add(root);
                            File.Delete($@"You think you are smart xml\Categories.xml");
                            categories.Save($@"You think you are smart xml\Categories.xml");

                            Directory.CreateDirectory($@"You-think-you-are-smart\QuestionsText\{CategoryName}");

                            #endregion

                            Console.Clear();
                            Console.WriteLine("Category added sucessfully. You'd better now restart the game.");
                            Console.ReadLine();
                            Environment.Exit(0);

                            #endregion
                        }
                        else if(EditChoice == "N")
                        {
                            #region Adding a new question..

                            Console.WriteLine("Choose the category the question will belong to:");
                            Console.WriteLine();

                            XDocument categories = new XDocument(XDocument.Load($@"You think you are smart xml\Categories.xml"));
                            XElement root = categories.Root;
                            IEnumerable<XElement> categoriesElements = root.Elements("Category");
                            List<XElement> categoriesList = new List<XElement>();
                            categoriesList = categoriesElements.ToList();
                            List<string> CategoryNames = new List<string>();
                            foreach(XElement category in categoriesList)
                            {
                                CategoryNames.Add(category.Value);
                            }
                            for(int i = 0; i < categoriesList.Count; i++)
                            {
                                Console.WriteLine($@"{i + 1}. {CategoryNames.ElementAt(i)}");
                                Console.WriteLine();
                            }

                            Console.WriteLine("Enter the number (no need to type the name) of the category you want your question to belong to.");
                            string ChoosenNumber = Console.ReadLine();
                            while(!int.TryParse(ChoosenNumber, out int number))
                            {
                                Console.WriteLine("That's not a number.");
                                ChoosenNumber = Console.ReadLine();
                            }
                            int chooseNumberInt = int.Parse(ChoosenNumber);
                            while(chooseNumberInt <= 0 || chooseNumberInt > CategoryNames.Count)
                            {
                                Console.WriteLine("Invalid number.");
                                ChoosenNumber = Console.ReadLine();
                                while (!int.TryParse(ChoosenNumber, out int number))
                                {
                                    Console.WriteLine("That's not a number.");
                                    ChoosenNumber = Console.ReadLine();
                                }
                                chooseNumberInt = int.Parse(ChoosenNumber);
                            }
                            string WhichCategoryYouChoose = CategoryNames.ElementAt(chooseNumberInt - 1);
                            Console.Clear();
                            Console.Beep();
                            Console.WriteLine($"So the category will be {WhichCategoryYouChoose}.");
                            Console.WriteLine($@"Press enter to continue.");
                            Console.ReadLine();
                            Console.WriteLine();
                            Console.WriteLine("Enter the content of the question. Feel free to add it in more than one line.");
                            Console.WriteLine();
                            List<string> QuestionLines = new List<string>();
                            string CurrentLine = "";
                            bool HasThePlayerDoneWithLines = false;
                            while(!HasThePlayerDoneWithLines)
                            {
                                CurrentLine = Console.ReadLine();
                                QuestionLines.Add(CurrentLine);
                                Console.WriteLine("Click c to add another line, or s to finish writing the question now.");
                                Console.WriteLine();
                                ConsoleKeyInfo Continue = Console.ReadKey(true);
                                while (Continue.Key.ToString().ToUpper() != "C" && Continue.Key.ToString().ToUpper() != "S")
                                {
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                                    }
                                    Continue = Console.ReadKey(true);
                                }
                                string ChoiceOfContinue = Continue.Key.ToString().ToUpper();
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                                }
                                if(ChoiceOfContinue.ToUpper() == "C")
                                {
                                    Console.Beep();
                                    Console.WriteLine("Another line:");
                                    // continue
                                }
                                else if(ChoiceOfContinue.ToUpper() == "S")
                                {
                                    Console.Beep();
                                    HasThePlayerDoneWithLines = true;
                                }
                            }
                            Console.Clear();
                            string Answer = "";
                            List<string> Answers = new List<string>();
                            Console.WriteLine("Type the 4 possible answers, one by one:");
                            for(int i = 0; i < 4; i++)
                            {
                                Console.WriteLine($"{i + 1}.");
                                Answer = Console.ReadLine();
                                Answers.Add($@"{i + 1}. {Answer}");
                            }
                            Console.WriteLine();
                            string CorrectAnswer = "";
                            Console.WriteLine("Now type the number of the correct answer (between 1 to 4). 1 would the be the first answer you wrote, and 4 would be the last:");
                            CorrectAnswer = Console.ReadLine();
                            while(!int.TryParse(CorrectAnswer, out int answer))
                            {
                                CorrectAnswer = Console.ReadLine();
                            }
                            int CorrectNumber = int.Parse(CorrectAnswer);
                            while (int.Parse(CorrectAnswer) != 1 && int.Parse(CorrectAnswer) != 2 && int.Parse(CorrectAnswer) != 3 && int.Parse(CorrectAnswer) != 4)
                            {
                                CorrectAnswer = Console.ReadLine();
                                while (!int.TryParse(CorrectAnswer, out int answer))
                                {
                                    CorrectAnswer = Console.ReadLine();
                                }
                            }
                            CorrectNumber = int.Parse(CorrectAnswer);
                            Console.WriteLine($@"So than, {Answers.ElementAt(CorrectNumber - 1)} is the correct answer.");
                            Thread.Sleep(2000);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }

                            XDocument GameQuestions = new XDocument(XDocument.Load($@"You think you are smart xml\Questions.xml"));
                            XElement rootQuestions = GameQuestions.Root;
                            IEnumerable<XElement> Questions = rootQuestions.Elements("Question");
                            List<XElement> questionsList = new List<XElement>();
                            questionsList = Questions.ToList();

                            int NumberOfQuestion = Questions.Count() + 1;
                            Directory.CreateDirectory($@"You-think-you-are-smart\QuestionsText\{WhichCategoryYouChoose}\Question{NumberOfQuestion}");
                            File.WriteAllLines($@"You-think-you-are-smart\QuestionsText\{WhichCategoryYouChoose}\Question{NumberOfQuestion}\Question{NumberOfQuestion}.txt", QuestionLines);
                            for(int i = 0; i < 4; i++)
                            {
                                File.WriteAllText($@"You-think-you-are-smart\QuestionsText\{WhichCategoryYouChoose}\Question{NumberOfQuestion}\Answer{i + 1}.txt", Answers.ElementAt(i));
                            }

                            #region writing it to xml file

                            XElement text = new XElement(XName.Get("Text"));
                            text.SetValue($@"You-think-you-are-smart\QuestionsText\{WhichCategoryYouChoose}\Question{NumberOfQuestion}\Question{NumberOfQuestion}.txt");
                            XElement sound = new XElement(XName.Get("Sound"));
                            sound.SetValue($@"You-think-you-are-smart\QuestionSound\Subject1\Question1.wav");
                            XElement AnswerElement = new XElement(XName.Get("Answer"));
                            AnswerElement.SetValue(CorrectNumber.ToString());
                            XElement NumberElement = new XElement(XName.Get("Number"));
                            NumberElement.SetValue($@"{NumberOfQuestion.ToString()}");
                            XElement SubjectElement = new XElement(XName.Get("Subject"));
                            SubjectElement.SetValue($@"{WhichCategoryYouChoose}");
                            XElement NameElement = new XElement(XName.Get("Name"));
                            NameElement.SetValue($@"Question{NumberOfQuestion.ToString()}");

                            XElement GeneralQuesion = new XElement(XName.Get("Question"));
                            GeneralQuesion.Add(text);
                            GeneralQuesion.Add(sound);
                            GeneralQuesion.Add(AnswerElement);
                            GeneralQuesion.Add(NumberElement);
                            GeneralQuesion.Add(SubjectElement);
                            GeneralQuesion.Add(NameElement);

                            rootQuestions.Add(GeneralQuesion);

                            GameQuestions.RemoveNodes();
                            GameQuestions.Add(rootQuestions);

                            GameQuestions.Save($@"You think you are smart xml\Questions.xml");

                            #endregion

                            Console.Clear();
                            Console.WriteLine("Your question has been added successfully!");
                            Console.WriteLine("Exit the game now before adding another one.");
                            Console.ReadLine();
                            Environment.Exit(0);

                            #endregion
                        }                       
                    }

                    #endregion

                    else if (choiceOfCreate == "T")
                    {
                        #region Adding a new trap question..

                        Console.Beep();
                        Console.WriteLine("Enter the content of the question. Only one line.");
                        Console.WriteLine();
                        string TheQuestionLine = Console.ReadLine();
                        Console.Beep();
                        Console.WriteLine();
                        string Answer = "";
                        List<string> Answers = new List<string>();
                        Console.WriteLine("Type the 4 possible answers, one by one:");
                        for (int i = 0; i < 4; i++)
                        {
                            Console.WriteLine($"{i + 1}.");
                            Answer = Console.ReadLine();
                            Answers.Add(Answer);
                            Console.Beep();
                        }
                        Console.WriteLine();
                        string CorrectAnswer = "";
                        Console.WriteLine("Now type the number of the correct answer (between 1 to 4). 1 would the be the first answer you wrote, and 4 would be the last:");
                        CorrectAnswer = Console.ReadLine();
                        while (!int.TryParse(CorrectAnswer, out int answer))
                        {
                            Console.WriteLine("That's not a number.");
                            CorrectAnswer = Console.ReadLine();
                        }
                        int CorrectNumber = int.Parse(CorrectAnswer);
                        while (int.Parse(CorrectAnswer) != 1 && int.Parse(CorrectAnswer) != 2 && int.Parse(CorrectAnswer) != 3 && int.Parse(CorrectAnswer) != 4)
                        {
                            Console.WriteLine("Invalid number.");
                            CorrectAnswer = Console.ReadLine();
                            while (!int.TryParse(CorrectAnswer, out int answer))
                            {
                                Console.WriteLine("That's not a number.");
                                CorrectAnswer = Console.ReadLine();
                            }
                        }
                        Console.Beep();
                        CorrectNumber = int.Parse(CorrectAnswer);
                        Console.WriteLine($@"So than, {Answers.ElementAt(CorrectNumber - 1)} is the correct answer.");
                        Thread.Sleep(2000);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }

                        XDocument GameQuestions = new XDocument(XDocument.Load($@"You think you are smart xml\TrapXML.xml"));
                        XElement rootQuestions = GameQuestions.Root;
                        IEnumerable<XElement> Questions = rootQuestions.Elements("Question");
                        List<XElement> questionsList = new List<XElement>();
                        questionsList = Questions.ToList();

                        int NumberOfQuestion = Questions.Count() + 1;
                        Directory.CreateDirectory($@"You-think-you-are-smart\TrapQuestions\Question{NumberOfQuestion}");
                        File.WriteAllText($@"You-think-you-are-smart\TrapQuestions\Question{NumberOfQuestion}\Question{NumberOfQuestion}.txt", TheQuestionLine);
                        for (int i = 0; i < 4; i++)
                        {
                            File.WriteAllText($@"You-think-you-are-smart\TrapQuestions\Question{NumberOfQuestion}\answer{i + 1}.txt", Answers.ElementAt(i));
                        }

                        #region writing it to xml file

                        XElement text = new XElement(XName.Get("Text"));
                        text.SetValue($@"You-think-you-are-smart\TrapQuestions\Question{NumberOfQuestion}\Question{NumberOfQuestion}.txt");
                        XElement answer1 = new XElement(XName.Get("Answer1Text"));
                        answer1.SetValue($@"You-think-you-are-smart\TrapQuestions\Question{NumberOfQuestion}\answer1.txt");
                        XElement answer2 = new XElement(XName.Get("Answer2Text"));
                        answer2.SetValue($@"You-think-you-are-smart\TrapQuestions\Question{NumberOfQuestion}\answer2.txt");
                        XElement answer3 = new XElement(XName.Get("Answer3Text"));
                        answer3.SetValue($@"You-think-you-are-smart\TrapQuestions\Question{NumberOfQuestion}\answer3.txt");
                        XElement answer4 = new XElement(XName.Get("Answer4Text"));
                        answer4.SetValue($@"You-think-you-are-smart\TrapQuestions\Question{NumberOfQuestion}\answer4.txt");
                        XElement AnswerElement = new XElement(XName.Get("CorrectAnswer"));
                        AnswerElement.SetValue(CorrectNumber.ToString());
                        XElement NumberElement = new XElement(XName.Get("Number"));
                        NumberElement.SetValue($@"{NumberOfQuestion.ToString()}");

                        XElement TrapQuestion = new XElement(XName.Get("Question"));
                        TrapQuestion.Add(text);
                        TrapQuestion.Add(answer1);
                        TrapQuestion.Add(answer2);
                        TrapQuestion.Add(answer3);
                        TrapQuestion.Add(answer4);
                        TrapQuestion.Add(AnswerElement);
                        TrapQuestion.Add(NumberElement);

                        rootQuestions.Add(TrapQuestion);
                        GameQuestions.RemoveNodes();
                        GameQuestions.Add(rootQuestions);
                        GameQuestions.Save(@"You think you are smart xml\TrapXML.xml");

                        /*
                        XElement text = new XElement(XName.Get("Text"));
                        text.SetValue($@"You-think-you-are-smart\QuestionsText\{WhichCategoryYouChoose}\Question{NumberOfQuestion}\Question{NumberOfQuestion}.txt");
                        XElement sound = new XElement(XName.Get("Sound"));
                        sound.SetValue($@"You-think-you-are-smart\QuestionSound\Subject1\Question1.wav");
                        XElement AnswerElement = new XElement(XName.Get("Answer"));
                        AnswerElement.SetValue(CorrectNumber.ToString());
                        XElement NumberElement = new XElement(XName.Get("Number"));
                        NumberElement.SetValue($@"{NumberOfQuestion.ToString()}");
                        XElement SubjectElement = new XElement(XName.Get("Subject"));
                        SubjectElement.SetValue($@"{WhichCategoryYouChoose}");
                        XElement NameElement = new XElement(XName.Get("Name"));
                        NameElement.SetValue($@"Question{NumberOfQuestion.ToString()}");

                        XElement GeneralQuesion = new XElement(XName.Get("Question"));
                        GeneralQuesion.Add(text);
                        GeneralQuesion.Add(sound);
                        GeneralQuesion.Add(AnswerElement);
                        GeneralQuesion.Add(NumberElement);
                        GeneralQuesion.Add(SubjectElement);
                        GeneralQuesion.Add(NameElement);

                        rootQuestions.Add(GeneralQuesion);

                        GameQuestions.RemoveNodes();
                        GameQuestions.Add(rootQuestions);

                        GameQuestions.Save($@"You think you are smart xml\Questions.xml");
                        */
                        #endregion

                        Console.Clear();
                        Console.WriteLine("Your question has been added successfully!");
                        Console.WriteLine("Exit the game now before adding another one.");
                        Console.ReadLine();
                        Environment.Exit(0);

                        #endregion
                    }
                    else if(choiceOfCreate == "I")
                    {
                        #region Adding an idea idenfication question..

                        Console.Beep();
                        Console.WriteLine("Enter the name of the idea/theory. Only one line:");
                        Console.WriteLine();
                        string idea = Console.ReadLine();
                        Console.Beep();
                        Console.WriteLine();
                        List<string> Clues = new List<string>();
                        Console.WriteLine("Type the 4 clues, one by one:");
                        string Clue = "";
                        for (int i = 0; i < 4; i++)
                        {
                            Console.WriteLine($"{i + 1}.");
                            Clue = Console.ReadLine();
                            Clues.Add(Clue);
                            Console.Beep();
                        }

                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }

                        Console.WriteLine();
                        Console.WriteLine("Enter the incomplete answer that will appear on the screen (part of the idea's name):");
                        string PartialAnswer = Console.ReadLine();

                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }

                        XDocument GameQuestions = new XDocument(XDocument.Load($@"You think you are smart xml\IdentifyTheIdeaXML.xml"));
                        XElement rootQuestions = GameQuestions.Root;
                        IEnumerable<XElement> Questions = rootQuestions.Elements("Question");
                        List<XElement> questionsList = new List<XElement>();
                        questionsList = Questions.ToList();

                        int NumberOfQuestion = Questions.Count() + 1;
                        Directory.CreateDirectory($@"You-think-you-are-smart\IdeaIdentifyQuestions\Question{NumberOfQuestion}");
                        File.WriteAllText($@"You-think-you-are-smart\IdeaIdentifyQuestions\Question{NumberOfQuestion}\answer.txt", idea);
                        for (int i = 0; i < 4; i++)
                        {
                            File.WriteAllText($@"You-think-you-are-smart\IdeaIdentifyQuestions\Question{NumberOfQuestion}\clue{i + 1}.txt", Clues.ElementAt(i));
                        }

                        #region writing it to xml file

                        XElement answerElement = new XElement(XName.Get("Answer"));
                        answerElement.SetValue($@"You-think-you-are-smart\IdeaIdentifyQuestions\Question{NumberOfQuestion}\answer.txt");

                        XElement Clue1 = new XElement(XName.Get("Clue1"));
                        XElement textClue1 = new XElement(XName.Get("Text"));
                        textClue1.SetValue($@"You-think-you-are-smart\IdeaIdentifyQuestions\Question{NumberOfQuestion}\clue1.txt");
                        XElement SoundClue1 = new XElement(XName.Get("Sound"));
                        SoundClue1.SetValue($@"You-think-you-are-smart\IdeaIdentifyQuestions\Question{NumberOfQuestion}\clue1.wav");
                        Clue1.Add(textClue1);
                        Clue1.Add(SoundClue1);

                        XElement Clue2 = new XElement(XName.Get("Clue2"));
                        XElement textClue2 = new XElement(XName.Get("Text"));
                        textClue2.SetValue($@"You-think-you-are-smart\IdeaIdentifyQuestions\Question{NumberOfQuestion}\clue2.txt");
                        XElement SoundClue2 = new XElement(XName.Get("Sound"));
                        SoundClue2.SetValue($@"You-think-you-are-smart\IdeaIdentifyQuestions\Question{NumberOfQuestion}\clue2.wav");
                        Clue2.Add(textClue2);
                        Clue2.Add(SoundClue2);

                        XElement Clue3 = new XElement(XName.Get("Clue3"));
                        XElement textClue3 = new XElement(XName.Get("Text"));
                        textClue3.SetValue($@"You-think-you-are-smart\IdeaIdentifyQuestions\Question{NumberOfQuestion}\clue3.txt");
                        XElement SoundClue3 = new XElement(XName.Get("Sound"));
                        SoundClue3.SetValue($@"You-think-you-are-smart\IdeaIdentifyQuestions\Question{NumberOfQuestion}\clue3.wav");
                        Clue3.Add(textClue3);
                        Clue3.Add(SoundClue3);

                        XElement Clue4 = new XElement(XName.Get("Clue4"));
                        XElement textClue4 = new XElement(XName.Get("Text"));
                        textClue4.SetValue($@"You-think-you-are-smart\IdeaIdentifyQuestions\Question{NumberOfQuestion}\clue4.txt");
                        XElement SoundClue4 = new XElement(XName.Get("Sound"));
                        SoundClue4.SetValue($@"You-think-you-are-smart\IdeaIdentifyQuestions\Question{NumberOfQuestion}\clue4.wav");
                        Clue4.Add(textClue4);
                        Clue4.Add(SoundClue4);

                        XElement NumberElement = new XElement(XName.Get("Number"));
                        NumberElement.SetValue($@"{NumberOfQuestion.ToString()}");
                        XElement InCompleteAnswerElemeent = new XElement(XName.Get("InCompleteAnswer"));
                        InCompleteAnswerElemeent.SetValue($@"{PartialAnswer}______");

                        XElement IdeaIdentifyQuestion = new XElement(XName.Get("Question"));
                        IdeaIdentifyQuestion.Add(answerElement);
                        IdeaIdentifyQuestion.Add(Clue1);
                        IdeaIdentifyQuestion.Add(Clue2);
                        IdeaIdentifyQuestion.Add(Clue3);
                        IdeaIdentifyQuestion.Add(Clue4);
                        IdeaIdentifyQuestion.Add(NumberElement);
                        IdeaIdentifyQuestion.Add(InCompleteAnswerElemeent);

                        rootQuestions.Add(IdeaIdentifyQuestion);
                        GameQuestions.RemoveNodes();
                        GameQuestions.Add(rootQuestions);
                        GameQuestions.Save(@"You think you are smart xml\IdentifyTheIdeaXML.xml");

                        /*
                        XElement text = new XElement(XName.Get("Text"));
                        text.SetValue($@"You-think-you-are-smart\TrapQuestions\Question{NumberOfQuestion}\Question{NumberOfQuestion}.txt");
                        XElement answer1 = new XElement(XName.Get("Answer1Text"));
                        answer1.SetValue($@"You-think-you-are-smart\TrapQuestions\Question{NumberOfQuestion}\answer1.txt");
                        XElement answer2 = new XElement(XName.Get("Answer2Text"));
                        answer2.SetValue($@"You-think-you-are-smart\TrapQuestions\Question{NumberOfQuestion}\answer2.txt");
                        XElement answer3 = new XElement(XName.Get("Answer3Text"));
                        answer3.SetValue($@"You-think-you-are-smart\TrapQuestions\Question{NumberOfQuestion}\answer3.txt");
                        XElement answer4 = new XElement(XName.Get("Answer4Text"));
                        answer4.SetValue($@"You-think-you-are-smart\TrapQuestions\Question{NumberOfQuestion}\answer4.txt");
                        XElement AnswerElement = new XElement(XName.Get("CorrectAnswer"));
                        AnswerElement.SetValue(CorrectNumber.ToString());
                        XElement NumberElement = new XElement(XName.Get("Number"));
                        NumberElement.SetValue($@"{NumberOfQuestion.ToString()}");

                        XElement TrapQuestion = new XElement(XName.Get("Question"));
                        TrapQuestion.Add(text);
                        TrapQuestion.Add(answer1);
                        TrapQuestion.Add(answer2);
                        TrapQuestion.Add(answer3);
                        TrapQuestion.Add(answer4);
                        TrapQuestion.Add(AnswerElement);
                        TrapQuestion.Add(NumberElement);

                        rootQuestions.Add(TrapQuestion);
                        GameQuestions.RemoveNodes();
                        GameQuestions.Add(rootQuestions);
                        GameQuestions.Save(@"You think you are smart xml\TrapXML.xml");
                        */

                        #endregion

                        Console.Clear();
                        Console.WriteLine("The idea has been added! You better exit the software now.");
                        Console.ReadLine();
                        Environment.Exit(0);

                        #endregion
                    }
                    else if (choiceOfCreate == "C")
                    {
                        Console.Beep();
                        Console.WriteLine("Type a statement in a single line (only):");
                        string statement = Console.ReadLine();
                        Console.Beep();
                        Console.WriteLine();
                        string TrueOrFalse = "";
                        Console.WriteLine("Is it true or false? Press t for true, or f for false.");
                        ConsoleKeyInfo TrueOrFalseKey = new ConsoleKeyInfo();
                        TrueOrFalseKey = Console.ReadKey(true);
                        while(TrueOrFalseKey.Key.ToString().ToUpper() != "T" && TrueOrFalseKey.Key.ToString().ToUpper() != "F")
                        {
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                            TrueOrFalseKey = Console.ReadKey(true);
                        }
                        string key = TrueOrFalseKey.Key.ToString().ToUpper();
                        if(key == "F")
                        {
                            TrueOrFalse = "False";
                        }
                        else if(key == "T")
                        {
                            TrueOrFalse = "True";

                        }

                        XDocument xDocument = new XDocument(XDocument.Load($@"You think you are smart xml\CorrectOrIncorrectXML.xml"));
                        XElement root = xDocument.Root;
                        IEnumerable<XElement> questions = root.Elements("Question");
                        int ThisQuestionNumber = questions.Count() + 1;

                        File.WriteAllText($@"You-think-you-are-smart\CorrectOrIncorrectQuestions\Question{ThisQuestionNumber}.txt", statement);

                        #region writing to the xml

                        XElement text = new XElement(XName.Get("Text"));
                        text.SetValue($@"You-think-you-are-smart\CorrectOrIncorrectQuestions\Question{ThisQuestionNumber}.txt");
                        XElement answer = new XElement(XName.Get("Answer"));
                        answer.SetValue($"{TrueOrFalse}");

                        XElement TheWholeQuestion = new XElement(XName.Get("Question"));
                        TheWholeQuestion.Add(text);
                        TheWholeQuestion.Add(answer);

                        root.Add(TheWholeQuestion);
                        xDocument.RemoveNodes();
                        xDocument.Add(root);
                        xDocument.Save($@"You think you are smart xml\CorrectOrIncorrectXML.xml");

                        #endregion

                        Console.Clear();
                        Console.WriteLine("The statement has been added!");
                        Console.WriteLine("You better close the game now.");
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }

                #endregion
            }
        }

        public static void ButtonRespondSound()
        {
            Console.Beep();
        }

        public static void GetAllObjectsAndStart(Game game)
        {
            Trap trap = new Trap(game);
            IdentifyTheIdea whoami = new IdentifyTheIdea(game);
            CorrectOrFalse correctorfalse = new CorrectOrFalse(game, 1);
        }

        public static void DisableQuickEditMode()
        {
            IntPtr consoleHandle = GetStdHandle(-10);  // -10 is STD_INPUT_HANDLE
            if (!GetConsoleMode(consoleHandle, out uint consoleMode))
            {
                Console.WriteLine("Failed to get console mode.");
                return;
            }

            // Remove the quick edit mode and enable extended flags
            consoleMode &= ~ENABLE_QUICK_EDIT_MODE;
            consoleMode |= ENABLE_EXTENDED_FLAGS;

            if (!SetConsoleMode(consoleHandle, consoleMode))
            {
                Console.WriteLine("Failed to set console mode.");
            }
        }

        public static void CreatingAndPlayingAudio(string path, string? text = null, string[]? LongText = null)
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
                if(string.IsNullOrEmpty(text))
                {
                    PromptBuilder promptBuilder = new PromptBuilder();
                    foreach (string textLine in LongText)
                    {
                        promptBuilder.AppendText(text);
                    }
                    synthesizer.Speak(promptBuilder);
                }
                else if(LongText == null)
                {
                    synthesizer.Speak(text);
                }
            }

            #endregion
            /*
            double audioLength = 0;

            using (var audioFile = new AudioFileReader($@"You-think-you-are-smart\OtherSounds\saturday.wav"))
            using (var outputDevice1 = new WaveOutEvent())
            {
                double audioLengthMs1 = audioFile.TotalTime.TotalMilliseconds;
                audioLength = Math.Round(audioLengthMs1);
            }
            */
            SoundPlayer PlaySound = new SoundPlayer(path);
            PlaySound.PlaySync();
            //Thread.Sleep((int)audioLength);
            PlaySound.Dispose();

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
            }

            File.Delete(path);

            #endregion
        }
    }
}
