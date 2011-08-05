using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WordMine
{
    class InteractableGameObject : GameObject
    {
        public String hoverPath;
        public String clickPath;

        public SoundEffect click;

        public InteractableGameObject(String texturePath, Vector2 position)
            : base(texturePath, position)
        {

        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            if (clickPath != null)
            {
                click = content.Load<SoundEffect>(clickPath);
            }
        }

        public virtual void Click()
        {
            if (click != null)
            {
                click.Play();
            }
        }
    }
}
