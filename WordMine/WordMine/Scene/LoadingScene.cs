using Microsoft.Xna.Framework;

namespace WordMine
{
    class LoadingScene : Scene
    {

        private GameObject background;
        private TokenGameObject loading;
        private string loadingText;
        private int loadingTextLength;
        private int timeSinceLast;
        private int milliseconds;

        public LoadingScene()
            : base()
        {
            gameObjects = new System.Collections.Generic.List<GameObject>();

            background = new GameObject("loading/loading_background", new Vector2(Game1.CAMERA_WIDTH / 2, Game1.CAMERA_HEIGHT / 2));
            background.noAnimate = true;
            gameObjects.Add(background);

            loadingText = "Loading...";
            loading = new TokenGameObject(new Vector2(Game1.CAMERA_WIDTH / 2, Game1.CAMERA_HEIGHT / 2), loadingText);
            loading.fontPath = "fonts/WesternLarge";
            loading.rotation = -.1f;
            milliseconds = 200;
            gameObjects.Add(loading);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            timeSinceLast += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLast > milliseconds)
            {
                timeSinceLast -= milliseconds;
                loadingTextLength++;
                if(loadingTextLength > loadingText.Length)
                {
                    loadingTextLength=0;
                }
                loading.contents = loadingText.Substring(0, loadingTextLength);
            }
        }
    }
}
