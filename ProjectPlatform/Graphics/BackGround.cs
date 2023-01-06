using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Graphics
{
    internal class BackGround
    {//from same bundle as tileset, decoration and nature
        private Texture2D[] _backgrounds;
        private float[] _scroll;
        private float _scale;
        private const float ScrollSpeed = 0.05f;


        //singleton
        private static BackGround _instance;
        public static BackGround Instance => _instance ??= new BackGround();

        private BackGround()
        {
        }
        public void Initialise(ContentManager content)
        {
            var backgroundTextures = new Texture2D[3];
            backgroundTextures[0] = content.Load<Texture2D>("Background/background_layer_1");
            backgroundTextures[1] = content.Load<Texture2D>("Background/background_layer_2");
            backgroundTextures[2] = content.Load<Texture2D>("Background/background_layer_3");
            _backgrounds = backgroundTextures;
            _scroll = new float[_backgrounds.Length];
        }

        public void Draw(Sprites spriteBatch, Vector2 screenSize = default)
        {
            if (screenSize != default)
            {
                _scale = screenSize.X / _backgrounds[0].Width;
            }
            for (var i = 0; i < _backgrounds.Length; i++)
            {
                spriteBatch.Draw(_backgrounds[i], new Vector2(_scroll[i], 0), new Rectangle(0, 0, _backgrounds[i].Width, _backgrounds[i].Height), Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0f);//scrolled background
                spriteBatch.Draw(_backgrounds[i], new Vector2(-_backgrounds[i].Width * _scale + _scroll[i], 0), new Rectangle(0, 0, _backgrounds[i].Width, _backgrounds[i].Height), Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0f); //replace another background next to the other one
            }
        }

        public void Update(GameTime time)
        {
            //scroll all backgrounds
            for (var i = 0; i < _scroll.Length; i++)
            {
                _scroll[i] += (float)(ScrollSpeed * time.ElapsedGameTime.TotalMilliseconds) * (i + 1);
                if (_scroll[i] >= _backgrounds[0].Width * _scale)
                {
                    _scroll[i] -= _backgrounds[0].Width * _scale;
                }
            }
        }

        public void Reset()
        {
            _scroll = new float[_scroll.Length];
        }
    }
}
