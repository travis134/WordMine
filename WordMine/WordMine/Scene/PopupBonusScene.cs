using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WordMine
{
    class PopupBonusScene : InteractableScene
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
        public String bonusWord;
        public String partOfSpeech;
        public String similarTo;
        public String definition;
        public TokenGameObject bonusText;
        public TokenGameObject bonusWordText;
        public TokenGameObject partOfSpeechText;
        public TokenGameObject similarToText;
        public TokenGameObject definitionText;
        public Boolean dismissed;
        public Boolean quiz;

        public PopupBonusScene()
            : base()
        {
            this.dismissed = true;
            this.quiz = false;

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

            this.bonusText = new TokenGameObject(new Vector2(Game1.CAMERA_WIDTH/2, 145), "Yippy Ay Kay Yay!");
            this.resumeButton = new MenuItem(new Vector2(Game1.CAMERA_WIDTH / 2, 345), "Resume");
            this.bonusWordText = new TokenGameObject(new Vector2(Game1.CAMERA_WIDTH/2, 185), "Bonus Word: ...");
            this.partOfSpeechText = new TokenGameObject(new Vector2(Game1.CAMERA_WIDTH/2, 225), "Part of Speech: ...");
            this.similarToText = new TokenGameObject(new Vector2(Game1.CAMERA_WIDTH/2, 265), "Similar to: ...");
            this.definitionText = new TokenGameObject(new Vector2(Game1.CAMERA_WIDTH/2, 305), "Definition: ...");

            this.bonusText.zindex = .9f;
            this.resumeButton.zindex = .9f;
            this.bonusWordText.zindex = .9f;
            this.partOfSpeechText.zindex = .9f;
            this.similarToText.zindex = .9f;
            this.definitionText.zindex = .9f;

            this.bonusText.fontPath = "fonts/Western";
            this.bonusText.fontColor = Color.Green;
            this.resumeButton.fontPath = "fonts/Western";

            this.bonusWordText.fontPath = "fonts/WesternSmall";
            this.partOfSpeechText.fontPath = "fonts/WesternSmall";
            this.similarToText.fontPath = "fonts/WesternSmall";
            this.definitionText.fontPath = "fonts/WesternSmall";

            gameObjects.Add(this.bonusText);
            gameObjects.Add(this.resumeButton);
            gameObjects.Add(this.bonusWordText);
            gameObjects.Add(this.partOfSpeechText);
            gameObjects.Add(this.similarToText);
            gameObjects.Add(this.definitionText);
        }

        public void fillIn(String bonusWord, String partOfSpeech, String similarTo, String definition)
        {
            this.bonusWord = bonusWord;
            this.partOfSpeech = partOfSpeech;
            this.similarTo = similarTo;
            this.definition = definition;

            this.bonusWordText.contents = "Bonus Word: " + this.bonusWord;
            this.partOfSpeechText.contents = "Part of Speech: " + this.partOfSpeech;
            this.similarToText.contents = "Similar to: " + this.similarTo;
            this.definitionText.contents = "Definition: " + this.definition;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(cursor.clicking)
            {
                quiz = true;
                dismissed = true;
            }
        }
    }
}
