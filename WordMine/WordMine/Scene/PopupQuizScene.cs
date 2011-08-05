using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WordMine
{
    class PopupQuizScene : InteractableScene
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
        public List<String> rightSentences;
        public List<String> wrongSentences;

        public MenuItem sentence1Text;
        public MenuItem sentence2Text;
        public MenuItem sentence3Text;
        public TokenGameObject quizText;
        public Boolean dismissed;

        public bonusword bonusWord;

        public Random rand;

        public int correct;

        public PopupQuizScene()
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

            rand = new Random();

            this.wrongSentences = new List<String>()
            {
                "His <*word*> knows no limits.",
                "I <*word*> a good red apple after dinner.",
                "This <*word*> is intended for recreation.",
                "She put her <*word*> into its crib",
                "He rang the <*word*> to signal the end of class.",
                "Could you hand me that <*word*> for this drill?",
                "We are <*word*> to school.",
                "I have to <*word*> the dishes.",
                "You should go <*word*> dinner.",
                "I <*word*> for my upcoming quiz."
            };

            this.rightSentences = new List<String>()
            {
                "The turtle is very <*word*>.",
                "I met a <*word*> person today.",
                "The <*word*> rabbit is funny.",
                "I like the <*word*> guy in my class.",
                "There is a <*word*> girl on the playground"
            };

            
            this.quizText = new TokenGameObject(new Vector2(Game1.CAMERA_WIDTH/2, 145), "Which one of these is correct?");
            this.sentence1Text = new MenuItem(new Vector2(Game1.CAMERA_WIDTH / 2, 225), "...");
            this.sentence2Text = new MenuItem(new Vector2(Game1.CAMERA_WIDTH/2, 265), "...");
            this.sentence3Text = new MenuItem(new Vector2(Game1.CAMERA_WIDTH/2, 305), "...");
            
            this.quizText.zindex = .9f;
            this.sentence1Text.zindex = .9f;
            this.sentence2Text.zindex = .9f;
            this.sentence3Text.zindex = .9f;
            
            this.quizText.fontPath = "fonts/WesternSmall";
            this.quizText.fontColor = Color.Gold;
            
            this.sentence1Text.fontPath = "fonts/WesternExtraSmall";
            this.sentence2Text.fontPath = "fonts/WesternExtraSmall";
            this.sentence3Text.fontPath = "fonts/WesternExtraSmall";
            
            gameObjects.Add(this.quizText);
            gameObjects.Add(this.sentence1Text);
            gameObjects.Add(this.sentence2Text);
            gameObjects.Add(this.sentence3Text);
        }

        public void fillIn(bonusword word)
        {
            bonusWord = word;

            this.correct = rand.Next(0, 3);

            if (correct == 0)
            {
                this.sentence1Text.contents = this.rightSentences[rand.Next(this.rightSentences.Count)].Replace("<*word*>", word.word);
                this.sentence2Text.contents = this.wrongSentences[rand.Next(this.wrongSentences.Count)].Replace("<*word*>", word.word);
                this.sentence3Text.contents = this.wrongSentences[rand.Next(this.wrongSentences.Count)].Replace("<*word*>", word.word);
            }
            else if (correct == 1)
            {
                this.sentence2Text.contents = this.rightSentences[rand.Next(this.rightSentences.Count)].Replace("<*word*>", word.word);
                this.sentence1Text.contents = this.wrongSentences[rand.Next(this.wrongSentences.Count)].Replace("<*word*>", word.word);
                this.sentence3Text.contents = this.wrongSentences[rand.Next(this.wrongSentences.Count)].Replace("<*word*>", word.word);
            }
            else if (correct == 2)
            {
                this.sentence3Text.contents = this.rightSentences[rand.Next(this.rightSentences.Count)].Replace("<*word*>", word.word);
                this.sentence1Text.contents = this.wrongSentences[rand.Next(this.wrongSentences.Count)].Replace("<*word*>", word.word);
                this.sentence2Text.contents = this.wrongSentences[rand.Next(this.wrongSentences.Count)].Replace("<*word*>", word.word);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (clicked == this.sentence1Text && this.correct == 0)
            {
                this.dismissed = true;
            }
            else if (clicked == this.sentence2Text && this.correct == 1)
            {
                this.dismissed = true;
            }
            else if (clicked == this.sentence3Text && this.correct == 2)
            {
                this.dismissed = true;
            }
        }
    }
}
