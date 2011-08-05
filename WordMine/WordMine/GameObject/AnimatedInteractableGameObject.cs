using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WordMine
{
    class AnimatedInteractableGameObject : InteractableGameObject
    {

        public Rectangle sourceRect;
        public int totalFrames;
        public int rows;
        public int columns;
        public int spriteIndex1;
        public int spriteIndex2;
        public int milliseconds;
        public int timeSinceLast;
        public Boolean moveUp;
        public Boolean finished;
        public int tempY;
        public Boolean loop;

        public AnimatedInteractableGameObject(String texturePath, Vector2 position, int totalFrames, int framesPerRow, int milliseconds)
            : base(texturePath, position)
        {
            this.totalFrames = totalFrames;
            this.columns = framesPerRow;
            this.rows = totalFrames / framesPerRow;
            this.milliseconds = milliseconds;
            this.visible = false;
            this.tempY = (int) position.Y;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            this.sourceRect = new Rectangle(0, 0, this.texture.Width / this.columns, this.texture.Height / this.rows);
            this.rectangle.X = (int)this.position.X - this.sourceRect.Width / 2;
            this.rectangle.Y = (int)this.position.Y - this.sourceRect.Height / 2;
            this.rectangle.Width = this.sourceRect.Width;
            this.rectangle.Height = this.sourceRect.Height;
        }

        public void Begin()
        {
            this.visible = true;
            this.finished = false;
            this.spriteIndex1 = 0;
            this.spriteIndex2 = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timeSinceLast += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLast > milliseconds)
            {
                timeSinceLast -= milliseconds;

                if ((!this.finished) || this.loop)
                {

                    if (spriteIndex1 < columns - 1)
                    {
                        this.spriteIndex1++;
                    }
                    else
                    {
                        if (spriteIndex2 < rows - 1)
                        {
                            this.spriteIndex2++;
                            this.spriteIndex1 = 0;
                        }
                        else
                        {
                            this.spriteIndex1 = 0;
                            this.spriteIndex2 = 0;
                            this.finished = true;
                            if (!this.loop)
                            {
                                this.visible = false;
                            }
                        }
                    }
                    if (moveUp)
                    {
                        this.position.Y -= 5;
                    }

                    this.sourceRect.X = this.sourceRect.Width * this.spriteIndex1;
                    this.sourceRect.Y = this.sourceRect.Height * this.spriteIndex2;
                }
                else
                {
                    this.position.Y = this.tempY;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.visible)
            {
                if (this.texture != null)
                {
                    spriteBatch.Draw(this.texture, this.position, this.sourceRect, this.tintColor, this.rotation, new Vector2(sourceRect.Width / 2, sourceRect.Height / 2), this.scale, SpriteEffects.None, this.zindex);
                }
            }
        }
    }
}
