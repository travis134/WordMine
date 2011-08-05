using Microsoft.Xna.Framework;

namespace WordMine
{
    class InteractableScene : Scene
    {
        public InteractableGameObject highlighted;
        public InteractableGameObject lastHighlighted;
        public InteractableGameObject clicked;
        public InteractableGameObject dragged, lastDragged;
        public Cursor cursor;
        public Rectangle cursorRectangle;

        public InteractableScene()
            : base()
        {
            this.cursor = Game1.wordMineCursor;
        }

        public override void Start()
        {
            base.Start();
            this.cursorRectangle = new Rectangle(
                (int)cursor.mouse.X,
                (int)cursor.mouse.Y,
                1,
                1
            );
            gameObjects.Add(new CursorGameObject(cursor));
        }

        public override void Stop()
        {
            base.Stop();
            this.cursorRectangle = Rectangle.Empty;
            this.highlighted = null;
            this.lastHighlighted = null;
            this.clicked = null;
            this.dragged = null;
            this.lastDragged = null;
        }

        public override void Reset()
        {
            this.Stop();
            this.Start();
        }

        public override void Update(GameTime gameTime)
        {
            this.cursorRectangle.X = cursor.mouse.X;
            this.cursorRectangle.Y = cursor.mouse.Y;

            lastHighlighted = highlighted;
           
            base.Update(gameTime);

            MouseEvent();
        }

        public virtual void MouseEvent()
        {

            if (dragged == null)
            {
                foreach (GameObject interactableGameObject in gameObjects)
                {
                    if (interactableGameObject is InteractableGameObject)
                    {
                        if (interactableGameObject.visible)
                        {
                            InteractableGameObject temp = (InteractableGameObject)interactableGameObject;
                            if (interactableGameObject.rectangle.Intersects(cursorRectangle))
                            {
                                highlighted = temp;
                                if (cursor.clicking)
                                {
                                    clicked = temp;
                                }
                                else
                                {
                                    clicked = null;
                                }
                            }
                        }
                    }
                }
            }

            if (highlighted != null)
            {
                if (!highlighted.rectangle.Intersects(cursorRectangle))
                {
                    highlighted = null;
                    clicked = null;
                }
            }
        }
    }
}
