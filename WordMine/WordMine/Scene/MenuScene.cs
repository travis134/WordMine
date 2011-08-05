using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WordMine
{
    class MenuScene : InteractableScene
    {
        private const int BUTTON_PLAY_OFFSET_X = 535;
        private const int BUTTON_PLAY_OFFSET_Y = 150;

        private const int BUTTON_LANGUAGE_OFFSET_X = 535;
        private const int BUTTON_LANGUAGE_OFFSET_Y = 290;

        private const int BUTTON_EXIT_OFFSET_X = 535;
        private const int BUTTON_EXIT_OFFSET_Y = 370;

        private GameObject background;

        private TokenGameObject buttonPlay;
        private TokenGameObject buttonLanguage;
        private TokenGameObject buttonExit;

        private List<String> languages;
        private int languageIndex;

        public MenuScene()
            : base()
        {
            this.sceneIndex = SceneIndex.MenuScene;
        }

        public override void Start()
        {
            base.Start();

            languages = new List<String>()
            {
                "English","Spanish","French","Italian","Dutch","German","Polish"
            };
            languageIndex = 0;

            if (this.options.ContainsKey("language"))
            {
                this.options["language"] = "english";
            }
            else
            {
                this.options.Add("language", "english");
            }

            background = new GameObject("loading/menu", new Vector2(Game1.CAMERA_WIDTH / 2, Game1.CAMERA_HEIGHT / 2));
            background.noAnimate = true;
            gameObjects.Add(background);

            buttonPlay = new TokenGameObject("loading/plank", new Vector2(BUTTON_PLAY_OFFSET_X, BUTTON_PLAY_OFFSET_Y), "Play");
            buttonPlay.zindex = 0.7f;
            buttonPlay.fontOffset = new Vector2(0, 20);
            buttonPlay.fontPath = "fonts/WesternMediumLarge";
            buttonPlay.rotation = 0.1f;
            gameObjects.Add(buttonPlay);

            buttonLanguage = new TokenGameObject("loading/plank", new Vector2(BUTTON_LANGUAGE_OFFSET_X, BUTTON_LANGUAGE_OFFSET_Y), languages[languageIndex]);
            buttonLanguage.zindex = 0.7f;
            buttonLanguage.fontOffset = new Vector2(0, -15);
            buttonLanguage.fontPath = "fonts/WesternMediumLarge";
            buttonLanguage.rotation = -0.1f;
            gameObjects.Add(buttonLanguage);

            buttonExit = new TokenGameObject("loading/plank", new Vector2(BUTTON_EXIT_OFFSET_X, BUTTON_EXIT_OFFSET_Y), "Exit");
            buttonExit.zindex = 0.7f;
            buttonExit.fontPath = "fonts/WesternMediumLarge";
            gameObjects.Add(buttonExit);
        }

        public override void Stop()
        {
            base.Stop();
            this.background = null;
            this.buttonPlay = null;
            this.buttonLanguage = null;
            this.buttonExit = null;
        }

        public override void Reset()
        {
            base.Reset();
            this.Start();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (clicked == buttonPlay)
            {
                this.options["language"] = languages[languageIndex].ToLower();
                this.control = SceneControls.Next;
                this.end = true;
            }
            else if (clicked == buttonLanguage)
            {
                if(this.languageIndex < this.languages.Count - 1)
                {
                    languageIndex++;
                }else{
                    languageIndex = 0;
                }
                this.buttonLanguage.contents = languages[languageIndex];
            }
            else if (clicked == buttonExit)
            {
                this.control = SceneControls.Exit;
                this.end = true;
            }
        }
    }
}
