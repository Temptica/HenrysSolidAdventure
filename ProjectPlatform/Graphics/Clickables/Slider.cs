using HenrySolidAdventure.Controller;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Graphics.Clickables
{
    internal class Slider : IClickable
    {
        public static Texture2D SliderTexture;
        public static Texture2D SliderKnobTexture;
        private readonly Vector2 _position;
        private float _value;
        private readonly float _minValue;
        private readonly float _maxValue;
        public Rectangle HitBox { get; set; }

        public float Value
        {
            get => _value;
            set => _value = Util.Clamp(value, _minValue, _maxValue);
        }

        public Slider(Vector2 position, float minValue, float maxValue, float value)
        {
            _position = position;
            _minValue = minValue;
            _maxValue = maxValue;
            Value = value;
            HitBox = new Rectangle((int)position.X, (int)position.Y, SliderTexture.Width, SliderTexture.Height);
        }
        public void Update(GameTime gameTime, Screen screen)
        {
            if (!MouseController.IsLeftClicked) return;
            var mousePos = MouseController.GetScreenPosition(screen);
            if (mousePos.X > _position.X && mousePos.X < _position.X + SliderTexture.Width)
            {
                if (mousePos.Y > _position.Y && mousePos.Y < _position.Y + SliderTexture.Height)
                {
                    Value = (mousePos.X - _position.X) / SliderTexture.Width;
                }
            }
        }
        public void Draw(Sprites sprites)
        {
            sprites.Draw(SliderTexture, Vector2.One, _position, Color.White);
            var knobPos = new Vector2(_position.X + SliderTexture.Width * Value, _position.Y - 2f);
            sprites.Draw(SliderKnobTexture, Vector2.One, knobPos, Color.White);
        }

    }
}
