using System.Collections.Generic;
using HenrySolidAdventure.Animations;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.MapFolder
{
    internal class Coin
    {
        public static List<List<int>> CollectedCoins;
        public const float Scale = 0.5f;
        internal static Texture2D Texture;
        private readonly BasicAnimation _normalAnimation;
        private float _collectedFrameRate = 50; //20fps
        private const float CollectAnimationRepeatTimes = 5;
        private float _collectAnimationTimes;
        private float _animationX;
        private bool _collected;
        public bool Destroy;
        internal Rectangle HitBox;

        internal Coin(Vector2 position)
        {
            CollectedCoins ??= new List<List<int>>();
            HitBox = new Rectangle((int)position.X, (int)position.Y, (int)((Texture.Width/12f)), (int)((Texture.Height/2f)* Scale));
            _animationX = 0;
            _normalAnimation = new BasicAnimation(Texture, "idle",10, 12,(int)( Texture.Width / 12f), (int)(Texture.Width / 12f), 0);
        }

        private double _time;
        private float _percentage;
        internal void Update(GameTime gameTime)
        {
            if (!_collected)
            {
                _normalAnimation.Update(gameTime);
                return;
            }
            if (_collectAnimationTimes < CollectAnimationRepeatTimes)//makes it fade
            {
                _time += gameTime.ElapsedGameTime.TotalMilliseconds;
                _collectedFrameRate--;
                if (_time < _collectedFrameRate) return;
                _animationX += Texture.Width / 12f;
                if (_animationX >= Texture.Width)
                {
                    _animationX = 0;
                    _collectAnimationTimes++;
                }

                _time -= _collectedFrameRate;
                _percentage = _collectAnimationTimes / CollectAnimationRepeatTimes;
            }
            else
            {
                Destroy = true;
            } 
        }
        
        internal void Draw(Sprites spriteBatch)
        {
            if (_collected)
            {
                var animationFrame =
                new Rectangle((int)(_animationX), Texture.Height / 2, (int)(Texture.Width / 12f), (int)(Texture.Width / 12f));
                spriteBatch.Draw(Texture, new Vector2(HitBox.X, HitBox.Y), animationFrame, Color.Lerp(Color.White, Color.Transparent, _percentage), 0f, Vector2.One, Scale, SpriteEffects.None, 0f);
                return;
            }
            _normalAnimation.Draw(spriteBatch, new Vector2(HitBox.X, HitBox.Y), SpriteEffects.None, Scale);           
        }

        public bool Collect()
        {
            if (_collected) return false;
            _animationX = (float)(_time = 0f);
            
            _collected = true;
            StatsController.AddCoin();
            AudioController.Instance.PlayEffect(SoundEffects.Coin);
            DiscordRichPresence.Instance.UpdateCoins();
            return true;
        }

    }
}
