using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WordMine
{
    class PopupMenuScene : InteractableScene
    {

        private GameObject background;
        public MenuItem resumeButton;
        private MenuItem newGameButton;
        private MenuItem exitButton;
        public Boolean dismissed;
        public Boolean exit;
        public Boolean newGame;

        public PopupMenuScene()
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

            this.sceneIndex = SceneIndex.PopupMenuScene;

            this.background = new GameObject("menu/menu", new Vector2(Game1.CAMERA_WIDTH / 2, Game1.CAMERA_HEIGHT / 2));
            this.background.zindex = 0.8f;
            gameObjects.Add(this.background);

            this.resumeButton = new MenuItem(new Vector2(Game1.CAMERA_WIDTH / 2, 170), "Resume");
            this.newGameButton = new MenuItem(new Vector2(Game1.CAMERA_WIDTH / 2, 250), "New Game");
            this.exitButton = new MenuItem(new Vector2(Game1.CAMERA_WIDTH / 2, 330), "Exit");

            this.resumeButton.fontPath = "fonts/Western";
            this.newGameButton.fontPath = "fonts/Western";
            this.exitButton.fontPath = "fonts/Western";

            resumeButton.zindex = .9f;
            newGameButton.zindex = .9f;
            exitButton.zindex = .9f;

            gameObjects.Add(this.resumeButton);
            gameObjects.Add(this.newGameButton);
            gameObjects.Add(this.exitButton);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (clicked == resumeButton)
            {
                dismissed = true;
            }
            if (clicked == exitButton)
            {
                exit = true;
            }
            if (clicked == newGameButton)
            {
                newGame = true;
            }
        }
    }
}
