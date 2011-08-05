using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WordMine
{
    class GameObject
    {
        public String texturePath;
        public Texture2D texture;
        public Vector2 position;
        public Vector2 originalPosition;
        public Rectangle rectangle;
        public Color tintColor;
        public Color originalColor;
        public Color highlightColor;
        public Boolean visible;
        public float zindex;
        public float rotation;
        public float scale;
        public Boolean noAnimate;

        public GameObject(String texturePath, Vector2 position)
        {
            this.texturePath = texturePath;
            this.position = position;
            this.tintColor = Color.White;
            this.highlightColor = Color.Red;
            this.visible = true;
            this.zindex = 0.0f;
            this.rotation = 0f;
            this.scale = 1.0f;

            this.originalPosition = position;
        }


        public virtual void LoadContent(ContentManager content)
        {
            if (texturePath != null)
            {
                try
                {
                    this.texture = content.Load<Texture2D>(texturePath);
                    this.rectangle = new Rectangle(
                        (int)position.X - texture.Width / 2,
                        (int)position.Y - texture.Height / 2,
                        texture.Width,
                        texture.Height
                        );
                }
                catch (ContentLoadException e)
                {
                    Console.Out.WriteLine("Error in GameObject->LoadContent:\n" + e.Message);
                    this.texturePath = null;
                    this.rectangle = new Rectangle(
                        (int)position.X,
                        (int)position.Y,
                        1,
                        1
                        );
                }
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (texturePath != null)
            {
                this.rectangle.X = (int)position.X - texture.Width / 2;
                this.rectangle.Y = (int)position.Y - texture.Height / 2;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (this.visible)
            {
                if (this.texture != null)
                {
                    spriteBatch.Draw(this.texture, this.rectangle, null, this.tintColor, this.rotation, Vector2.Zero, SpriteEffects.None, this.zindex);
                    //spriteBatch.Draw(this.texture, this.position, null, this.tintColor, this.rotation, new Vector2(texture.Width / 2, texture.Height / 2), this.scale, SpriteEffects.None, this.zindex);
                }
            }
        }

        public virtual void Reset()
        {
            this.position = this.originalPosition;
        }


    }
}
