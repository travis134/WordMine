using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WordMine
{
    class CursorGameObject : GameObject
    {
        public Cursor cursor;

        public CursorGameObject(Cursor cursor)
            : base("menu/cursor", new Vector2(cursor.mouse.X, cursor.mouse.Y))
        {
            this.cursor = cursor;
            this.zindex = 1.0f;
            this.visible = false;
        }


        public CursorGameObject(String texturePath, Cursor cursor) : base(texturePath, new Vector2(cursor.mouse.X, cursor.mouse.Y))
        {
            this.cursor = cursor;
            this.zindex = 1.0f;
            this.visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            cursor.Update(gameTime);

            this.position = new Vector2(this.cursor.mouse.X, this.cursor.mouse.Y);
            
            base.Update(gameTime);
        }

    }
}
