using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WordMine
{
    public class Cursor
    {

        public MouseState previousMouse;
        public Vector2 pointClicked;
        public MouseState mouse;
        public Boolean dragging;
        public Boolean stoppedDragging;
        public Boolean clicking;
        public Boolean moving;
        public Boolean active;

        public Cursor()
        {
            this.mouse = Mouse.GetState();
            this.previousMouse = mouse;
        }

        public void Update(GameTime gameTime)
        {
            this.mouse = Mouse.GetState();

            if ((new Vector2(mouse.X, mouse.Y)) != (new Vector2(previousMouse.X, previousMouse.Y)))
            {
                this.moving = true;
            }
            else
            {
                this.moving = false;
            }

            if (((mouse.LeftButton == ButtonState.Pressed) && (previousMouse.LeftButton == ButtonState.Pressed)))
            {
                this.dragging = true;
            }
            else
            {
                this.dragging = false;
            }

            if (((mouse.LeftButton == ButtonState.Released) && (previousMouse.LeftButton == ButtonState.Pressed)))
            {
                this.stoppedDragging = true;
            }
            else
            {
                this.stoppedDragging = false;
            }

            if ((mouse.LeftButton == ButtonState.Released) && (previousMouse.LeftButton == ButtonState.Pressed))
            {
                this.pointClicked = new Vector2(this.mouse.X, this.mouse.Y);
                this.clicking = true;
            }
            else
            {
                this.clicking = false;
            }

            this.active = (clicking || dragging || moving);

            this.previousMouse = this.mouse;
        }
    }
}
