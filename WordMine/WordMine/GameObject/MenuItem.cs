using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WordMine
{
    class MenuItem : TokenGameObject
    {
        public SceneIndex gotoIndex;
        public SceneControls control;

        public MenuItem(Vector2 position, String contents)
            : base(position, contents)
        {
            zindex = 0.2f;
            fontColor = Color.White;
            highlightColor = Color.White;
        }

        public MenuItem(Vector2 position, String contents, SceneControls control)
            : base(position, contents)
        {
            zindex = 0.2f;
            fontColor = Color.White;
            highlightColor = Color.White;
            this.control = control;
        }

        public MenuItem(Vector2 position, String contents, SceneControls control, SceneIndex gotoIndex)
            : base(position, contents)
        {
            zindex = 0.2f;
            fontColor = Color.White;
            highlightColor = Color.White;
            this.control = control;
            this.gotoIndex = gotoIndex;
        }
    }
}
