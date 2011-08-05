using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WordMine
{
    class LevelBar : TokenGameObject
    {
        //Percent of the bar that is filled
        public float barFillPercent;

        //Top most position of the filled bar
        public float barFillMax;

        public Boolean animating;
        public Boolean leveledUp;

        private int level;
        private int speed;

        public GameObject fill;
        public AnimatedInteractableGameObject top;
        public AnimatedInteractableGameObject levelUpExplosion;
        public TokenGameObject levelBarTitle;

        public LevelBar(String texturePath, Vector2 position, String barTopPath, String barFillPath)
            : base(texturePath, position)
        {
            this.levelBarTitle = new TokenGameObject(this.position + new Vector2(-2, -168), "LVL");
            this.levelBarTitle.fontPath = "fonts/WesternSmall";
            this.levelBarTitle.rotation = -0.1f;
            this.levelBarTitle.zindex = 0.6f;

            this.levelUpExplosion = new AnimatedInteractableGameObject("foreground/cloud", new Vector2(230, 140), 25, 5, 10);
            this.levelUpExplosion.zindex = 0.7f;
            this.levelUpExplosion.finished = true;
            this.levelUpExplosion.moveUp = true;

            this.top = new AnimatedInteractableGameObject(barTopPath, position + new Vector2(-2,0), 5, 5, 100);
            this.top.loop = true;
            this.top.zindex = 0.7f;

            this.fill = new GameObject(barFillPath, this.position + new Vector2(33, 159));
            this.fill.zindex = 0.6f;
            this.fill.rotation = MathHelper.Pi;

            this.contents = "1";
            this.fontPath = "fonts/Western";
            this.fontOffset = new Vector2(0, -130);
            this.fontRotation = -0.1f;

            this.barFillMax = 223;
            this.barFillPercent = 0f;
            this.speed = 1;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            this.levelBarTitle.LoadContent(content);
            this.top.LoadContent(content);
            this.fill.LoadContent(content);
            this.levelUpExplosion.LoadContent(content);

            this.top.Begin();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.levelBarTitle.Update(gameTime);
            this.levelUpExplosion.Update(gameTime);
            this.top.Update(gameTime);
            this.fill.Update(gameTime);

            
            this.animating = true;

            if (leveledUp)
            {
                this.barFillPercent = 1f;
            }

            if (this.fill.rectangle.Height < (int)(this.barFillMax * this.barFillPercent))
            {
                this.fill.rectangle.Height+=2;
                this.animating = true;
            }
            else
            {
                this.animating = false;
            }

            if (this.fill.rectangle.Height >= this.barFillMax)
            {
                this.levelUpExplosion.Begin();
                this.leveledUp = false;
                this.fill.rectangle.Height = 0;
                this.animating = true;
                this.contents = level.ToString();
            }

            this.top.position.Y = this.fill.rectangle.Y - this.fill.rectangle.Height;

            this.top.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (this.visible)
            {
                this.levelBarTitle.Draw(spriteBatch);
                this.top.Draw(spriteBatch);
                this.fill.Draw(spriteBatch);
                this.levelUpExplosion.Draw(spriteBatch);
            }
        }

        public void levelUp(int level)
        {
            this.level = level;
            leveledUp = true;
        }
    }
}
