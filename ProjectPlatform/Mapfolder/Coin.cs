using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Animations;
using OtterlyAdventure.Graphics;

namespace OtterlyAdventure.Mapfolder
{
    internal class Coin
    {
        public static List<List<int>> CollectedCoins;
        public const float Scale = 0.5f;
        internal static Texture2D Texture;
        readonly BasicAnimation normalAnimation;
        private float CollectedFrameRate = 50; //20fps
        private const float CollectAnimationRepeatTimes = 5;
        private float _collectAnimationTimes;
        private float _animationX;
        bool _collected;
        public bool Destroy;
        internal Rectangle HitBox;

        internal Coin(Vector2 Position)
        {
            CollectedCoins ??= new List<List<int>>();
            HitBox = new Rectangle((int)Position.X, (int)Position.Y, (int)((Texture.Width/12f)), (int)((Texture.Height/2f)* Scale));
            _animationX = 0;
            normalAnimation = new BasicAnimation(Texture, "idle",10, 12,(int)( Texture.Width / 12f), (int)(Texture.Width / 12f), 0);
        }

        private double _time;
        float percentage;
        internal void Update(GameTime gameTime)
        {
            if (!_collected)
            {
                normalAnimation.Update(gameTime);
                return;
            }
            if (_collectAnimationTimes < CollectAnimationRepeatTimes)
            {
                _time += gameTime.ElapsedGameTime.TotalMilliseconds;
                CollectedFrameRate--;
                if (_time < CollectedFrameRate) return;
                _animationX += Texture.Width / 12f;
                if (_animationX >= Texture.Width)
                {
                    _animationX = 0;
                    _collectAnimationTimes++;
                }

                _time -= CollectedFrameRate;
                percentage = _collectAnimationTimes / CollectAnimationRepeatTimes;
            }
            else
            {
                Destroy = true;
            } //make it fade
        }
        
        internal void Draw(Sprites spriteBatch)
        {
            if (_collected)
            {
                var animationFrame =
                new Rectangle((int)(_animationX), Texture.Height / 2, (int)(Texture.Width / 12f), (int)(Texture.Width / 12f));
                spriteBatch.Draw(Texture, new Vector2(HitBox.X, HitBox.Y), animationFrame, Color.Lerp(Color.White, Color.Transparent, percentage), 0f, Vector2.One, Scale, SpriteEffects.None, 0f);
                return;
            }
            normalAnimation.Draw(spriteBatch, new Vector2(HitBox.X, HitBox.Y), SpriteEffects.None, Scale);           
        }

        public bool Collect()
        {
            if (_collected) return false;
            _animationX = (float)(_time = 0f);
            
            _collected = true;
            return true;
        }

    }
}
