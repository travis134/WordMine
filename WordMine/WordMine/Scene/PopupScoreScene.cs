﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WordMine
{
    class PopupScoreScene : InteractableScene
    {
        //Code to popup a window displaying these stats needs to trigger here
        /*
         * Longest Word: %longestWord
         * Highest Point Word: %highestPointWord %highestPointWordScore
         * Most Created Word: %sortedDict.ElementAt(0).Value %sortedDict.ElementAt(0).Key
         * Total Words Created: %wordsMade.Count
         * 
         */

        private GameObject background;
        public MenuItem resumeButton;
        public String longestWord;
        public String highestPointWord;
        public String mostCreatedWord;
        public int totalWordsCreated;
        public TokenGameObject congratulationsText;
        public TokenGameObject longestWordText;
        public TokenGameObject highestPointWordText;
        public TokenGameObject mostCreatedWordText;
        public TokenGameObject totalWordsCreatedText;
        public Boolean dismissed;

        public PopupScoreScene()
            : base()
        {
            this.dismissed = true;

            gameObjects = new System.Collections.Generic.List<GameObject>();

            this.cursorRectangle = new Rectangle(
                (int)cursor.mouse.X,
                (int)cursor.mouse.Y,
                1,
                1
            );
            gameObjects.Add(new CursorGameObject(cursor));

            this.sceneIndex = SceneIndex.PopupScoreScene;

            this.background = new GameObject("menu/menu", new Vector2(Game1.CAMERA_WIDTH / 2, Game1.CAMERA_HEIGHT / 2));
            this.background.zindex = 0.8f;
            gameObjects.Add(this.background);

            this.congratulationsText = new TokenGameObject(new Vector2(Game1.CAMERA_WIDTH/2, 145), "Congratulations!");
            this.resumeButton = new MenuItem(new Vector2(Game1.CAMERA_WIDTH / 2, 345), "Resume");
            this.longestWordText = new TokenGameObject(new Vector2(Game1.CAMERA_WIDTH/2, 185), "Longest Word: ...");
            this.highestPointWordText = new TokenGameObject(new Vector2(Game1.CAMERA_WIDTH/2, 225), "Highest Point Word: ...");
            this.mostCreatedWordText = new TokenGameObject(new Vector2(Game1.CAMERA_WIDTH/2, 265), "Most Created Word: ...");
            this.totalWordsCreatedText = new TokenGameObject(new Vector2(Game1.CAMERA_WIDTH/2, 305), "Total Words Created: ...");

            this.congratulationsText.zindex = .9f;
            this.resumeButton.zindex = .9f;
            this.longestWordText.zindex = .9f;
            this.highestPointWordText.zindex = .9f;
            this.mostCreatedWordText.zindex = .9f;
            this.totalWordsCreatedText.zindex = .9f;

            this.congratulationsText.fontPath = "fonts/Western";
            this.congratulationsText.fontColor = Color.Gold;
            this.resumeButton.fontPath = "fonts/Western";

            this.longestWordText.fontPath = "fonts/WesternSmall";
            this.highestPointWordText.fontPath = "fonts/WesternSmall";
            this.mostCreatedWordText.fontPath = "fonts/WesternSmall";
            this.totalWordsCreatedText.fontPath = "fonts/WesternSmall";

            gameObjects.Add(this.congratulationsText);
            gameObjects.Add(this.resumeButton);
            gameObjects.Add(this.longestWordText);
            gameObjects.Add(this.highestPointWordText);
            gameObjects.Add(this.mostCreatedWordText);
            gameObjects.Add(this.totalWordsCreatedText);
        }

        public void fillIn(String longestWord, String highestPointWord, int highestPointWordScore, String mostCreatedWord, int timescreated, int totalWordsCreated)
        {
            this.longestWord = longestWord;
            this.highestPointWord = highestPointWord + " (" + highestPointWordScore + ")";
            this.mostCreatedWord = mostCreatedWord + " (" + timescreated + " times)";
            this.totalWordsCreated = totalWordsCreated;

            this.longestWordText.contents = "Longest: " + this.longestWord;
            this.highestPointWordText.contents = "Highest Point: " + this.highestPointWord;
            this.mostCreatedWordText.contents = "Most Created: " + this.mostCreatedWord;
            this.totalWordsCreatedText.contents = "Total Words Created : " + this.totalWordsCreated;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(cursor.clicking)
            {
                dismissed = true;
            }
        }
    }
}
