using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WordMine
{
    class TokenGameObject : InteractableGameObject
    {

        public SpriteFont font;
        public String contents;
        public Color fontColor;
        public Color fontBackColor;
        public Vector2 fontBackOffset = Vector2.Zero;
        public String fontPath;
        public Vector2 fontPosition;
        public Vector2 fontOffset = Vector2.Zero;
        public float fontRotation;
        public float fontScale;

        public TokenGameObject(String texturePath, Vector2 position)
            : base(texturePath, position)
        {
            this.contents = "";
            this.fontColor = Color.White;
            this.fontBackColor = Color.Black;
            this.fontPath = "fonts/Default";
            this.fontScale = 1.0f;
            this.fontBackOffset = new Vector2(3, 3);
        }


        public TokenGameObject(Vector2 position, String contents)
            : base(null, position)
        {
            this.contents = contents;
            this.fontColor = Color.White;
            this.fontBackColor = Color.Black;
            this.fontPath = "fonts/Default";
            this.fontScale = 1.0f;
            this.fontBackOffset = new Vector2(3, 3);
        }

        public TokenGameObject(Vector2 position)
            : base(null, position)
        {
            this.contents = "";
            this.fontColor = Color.White;
            this.fontBackColor = Color.Black;
            this.fontPath = "fonts/Default";
            this.fontScale = 1.0f;
            this.fontBackOffset = new Vector2(3, 3);
        }

        public TokenGameObject(String texturePath, Vector2 position, String contents)
            : base(texturePath, position)
        {
            this.contents = contents;
            this.fontColor = Color.White;
            this.fontBackColor = Color.Black;
            this.fontPath = "fonts/Default";
            this.fontScale = 1.0f;
            this.fontBackOffset = new Vector2(3, 3);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            this.font = content.Load<SpriteFont>(fontPath);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.fontScale = Math.Min(1, this.rectangle.Width / this.font.MeasureString(contents).X);

            this.fontPosition.X = this.position.X - ((this.font.MeasureString(contents).X * this.fontScale)/ 2);
            this.fontPosition.Y = this.position.Y - ((this.font.MeasureString(contents).Y * this.fontScale)/ 2);
            this.fontPosition.Y -= ((this.font.MeasureString(contents).X * this.fontScale)/ 2) * (float)Math.Sin(this.fontRotation + this.rotation);
            
            if (this.texture == null)
            {
                this.rectangle.X = (int)(this.fontPosition).X;
                this.rectangle.Y = (int)(this.fontPosition).Y;
                /*if (this.rotation == 0.0f)
                {
                    this.rectangle.Y = (int)(this.fontPosition).Y + (int)((this.font.MeasureString(contents).Y * this.fontScale) / 2);
                }*/
                this.rectangle.Width = (int)this.font.MeasureString(contents).X;
                this.rectangle.Height = (int)this.font.MeasureString(contents).Y;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (visible)
            {
                spriteBatch.DrawString(font, this.contents, this.fontPosition + this.fontOffset + this.fontBackOffset, this.fontBackColor, this.rotation + this.fontRotation, Vector2.Zero, this.fontScale, SpriteEffects.None, this.zindex + 0.001f);
                spriteBatch.DrawString(font, this.contents, this.fontPosition + this.fontOffset, this.fontColor, this.rotation + this.fontRotation, Vector2.Zero, this.fontScale, SpriteEffects.None, this.zindex + 0.002f);
            }
        }
    }
}
