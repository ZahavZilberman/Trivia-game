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
    public class ChooseCategory
    {
        #region ctor

        public ChooseCategory(Game game, int PlayerToChooseCategory)
        {
            ChooseCategorySoundPath = $@"You-think-you-are-smart\OtherSounds\choosecategory.wav";
            PlayerNumber = PlayerToChooseCategory;
            playerTurn = game.GamePlayers.ElementAt(PlayerToChooseCategory - 1); // The variable values are 1-3, the lst indexes are 0-2
            categoriesDocument = new XDocument(XDocument.Load((@"You think you are smart xml\Categories.xml")));
            categoriesElements = new List<XElement>(categoriesDocument.Root.Elements("Category"));
            AllCategoriesNames = new List<string>();
            AllCategories = new List<string>();
            foreach (XElement category in categoriesElements)
            {
                AllCategories.Add(category.Value);
            }
            AllCategoriesNames = AllCategories;

            randomchoice = new Random(0);
            for (int RandomCategoryCount = 0; RandomCategoryCount < AllCategories.Count; RandomCategoryCount++)
            {
                RandomChoiceIfNeeded = randomchoice.Next(0, 1);
            }

            game.IsOnChooseCatgoryMode = true;
            PathToPlayerNameSound = playerTurn.PlayerNameAudioPath;


            //MusicPath = @"standup/choosecatgeory.mp3";
        }

        #endregion

        #region Properties

        public int PlayerNumber { get; set; }
        public Player playerTurn { get; set; }
        public XDocument categoriesDocument { get; set; }
        public List<XElement> categoriesElements { get; set; }
        public List<string> AllCategoriesNames { get; set; }
        public Random randomchoice { get; set; }
        public string choosenCategory { get; set; }
        public string PathToPlayerNameSound { get; set; }
        public string MusicPath { get; set; }
        public string ChooseCategorySoundPath { get; set; }
        List<string> AllCategories { get; set; }
        public int RandomChoiceIfNeeded { get; set; }
        public int MoneyForThisCategoryQuestion { get; set; }

        public bool IsRandomCategoryHasBeenChoose { get; set; }


        #endregion
    }
}
