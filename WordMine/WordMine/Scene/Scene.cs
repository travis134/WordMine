using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WordMine
{
    class Scene
    {
        public List<GameObject> gameObjects;
        public SceneIndex sceneIndex;
        public SceneIndex gotoIndex;
        public SceneControls control;
        public Dictionary<String, String> options;
        public Boolean end;
        public Boolean stopped;
        public Boolean sceneReady;
        public Boolean showPopupMenu;
        public Boolean lose;

        public Scene()
        {
        }

        public virtual void Start()
        {
            this.end = false;
            this.stopped = false;
            this.gameObjects = new List<GameObject>();
            if (this.options == null)
            {
                this.options = new Dictionary<String, String>();
            }
        }

        public virtual void Reset()
        {
            this.Stop();
            this.Start();
        }

        public virtual void Stop()
        {
            this.stopped = true;
            this.sceneReady = false;
            this.gameObjects = null;
        }

        public virtual void LoadContent(ContentManager content)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.LoadContent(content);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!this.stopped)
            {
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Update(gameTime);
                    
                }
            }
            this.sceneReady = true;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (this.sceneReady)
            {
                if (!this.stopped)
                {
                    foreach (GameObject gameObject in gameObjects)
                    {
                        gameObject.Draw(spriteBatch);
                    }
                }
            }
        }
    }
}
