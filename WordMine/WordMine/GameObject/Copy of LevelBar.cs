using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WordMine
{
    class LevelBar : TokenGameObject
    {

        public float barFillPercent;
        public float barFillMax;

        public String barTopPath;
        public String barFillPath;
        public Vector2 barTopPosition;
        public Vector2 barFillPosition;
        public Rectangle barTopRectangle;
        public Rectangle barFillRectangle;
        public Texture2D barTop;
        public Texture2D barFill;

        public SpriteFont levelBarTitleFont;
        public String levelBarTitleContents;
        public Color levelBarTitleColor;
        public Color levelBarTitleBackColor;
        public Vector2 levelBarTitleOffset = Vector2.Zero;
        public String levelBarTitlePath;
        public Vector2 levelBarTitleBackOffset = Vector2.Zero;
        public float levelBarTitleRotation;
        public float levelBarTitleScale;


        public LevelBar(String texturePath, Vector2 position, String barTopPath, String barFillPath)
            : base(texturePath, position)
        {
            this.fontPath = "fonts/Default";
            this.levelBarTitlePath = "fonts/Default";
            this.barTopPath = barTopPath;
            this.barFillPath = barFillPath;
            this.barFillPercent = 1f;
            this.barFillMax = 100f;
            this.levelBarTitleScale = 1.0f;
            this.levelBarTitleColor = Color.White;
            this.levelBarTitleBackColor = Color.Black;
            this.levelBarTitleBackOffset = new Vector2(3, 3);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            this.barTop = content.Load<Texture2D>(this.barTopPath);
            this.barFill = content.Load<Texture2D>(this.barFillPath);
            this.levelBarTitleFont = content.Load<SpriteFont>(this.levelBarTitlePath);

            this.barTopRectangle = new Rectangle(
                (int)barTopPosition.X,
                (int)barTopPosition.Y,
                barTop.Width,
                barTop.Height
                );

            this.barFillRectangle = new Rectangle(
                (int)barFillPosition.X,
                (int)barFillPosition.Y,
                barFill.Width,
                barFill.Height
                );
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.barFillRectangle.Height = (int) (this.barFillMax * this.barFillPercent);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (this.visible)
            {
                if (this.barTop != null)
                {
                    spriteBatch.Draw(this.barTop, this.barTopPosition, this.barTopRectangle, this.tintColor, 0f, new Vector2(this.barTop.Width / 2, this.barTop.Height / 2), 1f, SpriteEffects.None, this.zindex + 0.02f);
                }

                if (this.barFill != null)
                {
                    spriteBatch.Draw(this.barFill, this.barFillPosition, this.barFillRectangle, this.tintColor, (float)Math.PI, new Vector2(this.barFill.Width / 2, this.barFill.Height / 2), 1f, SpriteEffects.None, this.zindex + 0.01f);
                }

                spriteBatch.DrawString(font, this.levelBarTitleContents, this.position + this.levelBarTitleOffset + this.levelBarTitleBackOffset, this.levelBarTitleBackColor, this.rotation + this.levelBarTitleRotation, Vector2.Zero, this.levelBarTitleScale, SpriteEffects.None, this.zindex + 0.001f);
                spriteBatch.DrawString(font, this.levelBarTitleContents, this.position + this.levelBarTitleOffset, this.levelBarTitleColor, this.rotation + this.levelBarTitleRotation, Vector2.Zero, this.levelBarTitleScale, SpriteEffects.None, this.zindex + 0.002f);
            }


        }
    }
}
