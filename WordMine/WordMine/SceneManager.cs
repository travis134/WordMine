using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WordMine
{

    class SceneManager
    {
        public List<Scene> scenes;
        public LoadingScene loadingScene;
        //public PopupMenuScene popupMenuScene;
        public LogoScene logoScene;
        public int sceneIndex;
        public Boolean exit;
        ContentManager residualContent;
        ContentManager content;
        Thread loadContentThread;
        Boolean firstLoad;
        private int timeSinceLast;
        private int milliseconds;

        public SceneManager(List<Scene> scenes)
        {
            this.milliseconds = 3000;
            this.firstLoad = true;
            this.sceneIndex = 0;
            this.scenes = scenes;
            this.loadingScene = new LoadingScene();
            //this.popupMenuScene = new PopupMenuScene();
            this.logoScene = new LogoScene();
        }

        public void LoadContent(IServiceProvider services)
        {
            this.residualContent = new ContentManager(services, "Content");
            this.content = new ContentManager(services, "Content");
            this.logoScene.LoadContent(this.residualContent);
            this.loadSceneContent();
        }

        public void Update(GameTime gameTime)
        {
            if (this.timeSinceLast > this.milliseconds)
            {
                this.firstLoad = false;
            }

            if (this.firstLoad)
            {
                timeSinceLast += gameTime.ElapsedGameTime.Milliseconds;
                this.logoScene.Update(gameTime);
            }
            else
            {
                if (this.loadContentThread != null)
                {
                    if (!this.loadContentThread.Join(0))
                    {
                        this.loadingScene.Update(gameTime);
                        Thread.Sleep(10);
                    }
                    else
                    {
                        this.loadContentThread = null;
                    }
                }
                else
                {
                    this.scenes[sceneIndex].Update(gameTime);
                    if (!this.scenes[sceneIndex].sceneReady)
                    {
                        this.loadingScene.Update(gameTime);
                    }
                    /*
#if WINDOWS_PHONE
                    if(this.scenes[sceneIndex].sceneIndex == SceneIndex.MenuScene)
                    {
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                        {
                                this.exit=true;
                        }
                    }

#endif
                    */
                    if (this.scenes[sceneIndex].end)
                    {
                        if (this.scenes[sceneIndex].control == SceneControls.Next)
                        {
                            this.nextScene();
                        }
                        else if (this.scenes[sceneIndex].control == SceneControls.Previous)
                        {
                            this.previousScene();
                        }
                        else if (this.scenes[sceneIndex].control == SceneControls.Goto)
                        {
                            this.gotoScene(this.scenes[sceneIndex].gotoIndex);
                        }
                        else if (this.scenes[sceneIndex].control == SceneControls.First)
                        {
                            this.gotoSceneAtIndex(0);
                        }
                        else if (this.scenes[sceneIndex].control == SceneControls.Last)
                        {
                            this.gotoSceneAtIndex(scenes.Count - 1);
                        }
                        else if (this.scenes[sceneIndex].control == SceneControls.Exit)
                        {
                            this.exit = true;
                        }
                        else if (this.scenes[sceneIndex].control == SceneControls.Reset)
                        {
                            this.gotoSceneAtIndex(sceneIndex);
                        }
                    }

                }
            }
        }
 
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.firstLoad)
            {
                this.logoScene.Draw(spriteBatch);
            }
            else
            {

                if ((this.loadContentThread != null) || (!this.scenes[sceneIndex].sceneReady))
                {
                    this.loadingScene.Draw(spriteBatch);
                }
                else
                {
                    this.scenes[sceneIndex].Draw(spriteBatch);
                }
                /*
                if (this.scenes[sceneIndex].showPopupMenu)
                {
                    this.popupMenuScene.Draw(spriteBatch);
                }*/
            }
        }

        private void loadSceneContent()
        {
            this.loadContentThread = new Thread(new ThreadStart(this.loadSceneContentThread));

            this.loadContentThread.Start();
        }

        private void loadSceneContentThread()
        {
            if (!this.loadingScene.sceneReady)
            {
                this.loadingScene.LoadContent(this.residualContent);
            }

            this.content.Unload();
            this.scenes[sceneIndex].Reset();
            this.scenes[sceneIndex].LoadContent(this.content);
        }

        public Boolean nextScene()
        {
            Boolean flag = false;

            if (this.sceneIndex < this.scenes.Count - 1)
            {
                scenes[sceneIndex].Stop();
                scenes[sceneIndex + 1].options = scenes[sceneIndex].options;
                this.sceneIndex++;
                this.loadSceneContent();
                flag = true;
            }
            else
            {
                this.exit = true;
            }
            
            return flag;
        }

        public Boolean previousScene()
        {
            Boolean flag = false;

            if (this.sceneIndex > 0)
            {
                scenes[sceneIndex].Stop();
                scenes[sceneIndex - 1].options = scenes[sceneIndex].options;
                this.sceneIndex--;
                this.loadSceneContent();
                flag = true;
            }
            else
            {
                this.exit = true;
            }
            return flag;
        }

        public Boolean gotoScene(SceneIndex sceneIndex)
        {
            Boolean flag = false;

            for (int i = 0; i < this.scenes.Count; i++)
            {
                if (this.scenes[i].sceneIndex == sceneIndex)
                {
                    Console.WriteLine("Found scene");
                    Console.Out.WriteLine("Stopping");
                    scenes[i].Stop();
                    Console.Out.WriteLine("Transfering options");
                    scenes[i].options = scenes[this.sceneIndex].options;
                    Console.Out.WriteLine("Setting scene index");
                    this.sceneIndex = i;
                    Console.Out.WriteLine("Loading content");
                    this.loadSceneContent();
                    Console.Out.WriteLine("Returning");
                    flag = true;
                    break;
                }
            }

            return flag;
        }

        public Scene getScene(SceneIndex sceneIndex)
        {
            Scene temp = null;

            for (int i = 0; i < this.scenes.Count; i++)
            {
                if (this.scenes[i].sceneIndex == sceneIndex)
                {
                    temp = this.scenes[i];
                }
            }

            return temp;
        }

        public Boolean gotoSceneAtIndex(int sceneIndex)
        {
            Boolean flag = false;

            if ((sceneIndex >= 0) && (sceneIndex < this.scenes.Count))
            {
                this.content.Unload();
                scenes[sceneIndex].options = scenes[this.sceneIndex].options;
                this.sceneIndex = sceneIndex;
                this.loadSceneContent(); 
                flag = true;
            }
            return flag;
        }
    }
}
