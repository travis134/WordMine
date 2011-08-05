using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WordMine
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SceneManager wordMineSceneManager;
        public static Cursor wordMineCursor;

        public static int CAMERA_WIDTH = 800;
        public static int CAMERA_HEIGHT = 480;
        public static int CAMERA_ASPECT_RATIO = CAMERA_WIDTH / CAMERA_HEIGHT;

        public static int SCREEN_WIDTH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static int SCREEN_HEIGHT = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        public static int SCREEN_ASPECT_RATIO = SCREEN_WIDTH / SCREEN_HEIGHT;

        public static String saveName = "WordMine.dat";

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = CAMERA_HEIGHT;
            graphics.PreferredBackBufferWidth = CAMERA_WIDTH;
            Content.RootDirectory = "Content";
            
#if WINDOWS_PHONE
            graphics.IsFullScreen = true;
#endif

#if WINDOWS
            this.IsMouseVisible = true; 
#endif

        }

        protected override void Initialize()
        {
            wordMineCursor = new Cursor();

            wordMineSceneManager = new SceneManager(
                new List<Scene>()
                {
                    new MenuScene(),
                    new PlayScene(),
                    new MenuScene()
                }
            );

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            
            wordMineSceneManager.LoadContent(Services);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            wordMineSceneManager.Update(gameTime);

            if (wordMineSceneManager.exit)
            {
                this.Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            wordMineSceneManager.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
