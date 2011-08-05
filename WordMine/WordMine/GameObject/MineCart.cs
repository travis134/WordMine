using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WordMine
{
    class MineCart : TokenGameObject, IComparable<MineCart>
    {
        public int x;
        public int y;
        public MineCartType mineCartType;
        public bool bonus;
        public Rectangle sourceRect;
        public int totalFrames;
        public int rows;
        public int columns;
        public int spriteIndex1;
        public int spriteIndex2;
        public int milliseconds;
        public int timeSinceLast;
        public Boolean finished;
        public int number;
        public Symbol symbol;
        public Boolean loop;

        public MineCart()
            : base("foreground/mineCarts", Vector2.Zero)
        {
            this.bonus = false;
            this.number = -1;
            this.symbol = Symbol.none;
            this.mineCartType = MineCartType.Gold1;
            this.fontPath = "fonts/Western";
            this.fontOffset.Y = -15;
            this.fontBackOffset = new Vector2(3, 3);

            this.totalFrames = 36;
            this.columns = 9;
            this.rows = totalFrames / 9;
            this.milliseconds = 100;

            this.noAnimate = true;

            this.finished = true;
            
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.zindex = (float)(0.3+((1.2-(((float)this.y/(float)10) + .3f))/(float)10));
            switch (mineCartType)
            {
                case MineCartType.Gold1:
                    this.spriteIndex2 = 3;
                    loop = false;
                    break;
                case MineCartType.Gold2:
                    this.spriteIndex2 = 2;
                    loop = false;
                    break;
                case MineCartType.Gold3:
                    this.spriteIndex2 = 1;
                    loop = false;
                    break;
                case MineCartType.TNT:
                    this.spriteIndex2 = 0;
                    this.finished = false;
                    this.loop = true;
                    break;
            }

            timeSinceLast += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLast > milliseconds)
            {
                timeSinceLast -= milliseconds;
                if ((!this.finished)||(this.loop))
                {
                        if (spriteIndex1 < columns - 1)
                        {
                            this.spriteIndex1++;
                        }
                        else
                        {
                            this.spriteIndex1 = 0;
                            this.finished = true;
                        }
                        this.sourceRect.X = this.sourceRect.Width * this.spriteIndex1;
                }
             
                
            }
            this.sourceRect.Y = this.sourceRect.Height * this.spriteIndex2;

            this.rectangle.Width = 50;
            this.rectangle.Height = 51;
            this.rectangle.X = (int)position.X - (57 / 2) + 7;
            this.rectangle.Y = (int)position.Y - (94/ 2) + 7;
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
            if (visible)
            {
                spriteBatch.DrawString(font, this.contents, this.fontPosition + this.fontOffset + this.fontBackOffset, this.fontBackColor, this.rotation + this.fontRotation, Vector2.Zero, this.fontScale, SpriteEffects.None, this.zindex + 0.001f);
                spriteBatch.DrawString(font, this.contents, this.fontPosition + this.fontOffset, this.fontColor, this.rotation + this.fontRotation, Vector2.Zero, this.fontScale, SpriteEffects.None, this.zindex + 0.002f);
            }
        }

        public int CompareTo(MineCart other)
        {
            return other.y- this.y;
        }
    }
}
