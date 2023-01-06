using HenrySolidAdventure.Controller;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Graphics.Clickables
{
    internal class StateButton : IClickable
    {
        public bool IsClicked { get; set; }
        public Rectangle HitBox { get; set; }
        public static Texture2D ClickedTexture;
        public static Texture2D UnClickedTexture;
        private readonly Vector2 _position;
        private bool _changed;

        public StateButton(Vector2 position, bool isClicked)
        {
            IsClicked = isClicked;
            _position = position;
            HitBox = new Rectangle((int)position.X, (int)position.Y, UnClickedTexture.Width, UnClickedTexture.Height);
        }
        public void Update(Screen screen)
        {
            if (!MouseController.IsLeftClicked)
            {
                _changed = false;
                return;
            }
            if (_changed) return;

            var mousePos = MouseController.GetScreenPosition(screen);
            if (HitBox.Contains(mousePos))
            {
                IsClicked = !IsClicked;
                _changed = true;
            }
        }
        public void Draw(Sprites sprites)
        {
            sprites.Draw(IsClicked ? ClickedTexture : UnClickedTexture, Vector2.One, _position, Color.White);
        }
    }
}
