using Microsoft.Xna.Framework;

namespace WordMine
{
    class LogoScene : Scene
    {

        private GameObject background;

        public LogoScene()
            : base()
        {
            gameObjects = new System.Collections.Generic.List<GameObject>();
            background = new GameObject("loading/slyDuck", new Vector2(Game1.CAMERA_WIDTH / 2, Game1.CAMERA_HEIGHT / 2));
            gameObjects.Add(background);
            
        }
    }
}
