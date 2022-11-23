using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Animations;

namespace ProjectPlatform.Mapfolder
{
    internal class Coin
    {
        internal static Texture2D Texture;
        BasicAnimation normalAnimation;
        private float CollectedFrameRate = 50; //20fps
        private const float CollectAnimationRepeatTimes = 5;
        private float _collectAnimationTimes = 0;
        private float _animationX;
        bool _collected;
        public bool Destroy;
        internal Rectangle HitBox;
        private readonly float _scale;

        internal Coin(Vector2 Position)
        {
            _scale = Map.Instance.Scale* 0.375f;
            HitBox = new Rectangle((int)Position.X, (int)Position.Y, (int)((Texture.Width/12f)*_scale), (int)((Texture.Height/2f)*_scale));
            _animationX = 0;
            normalAnimation = new BasicAnimation(Texture, "idle",10, 12,(int)( Texture.Width / 12f), (int)(Texture.Width / 12f), 0);
        }

        private double _time;
        internal void Update(GameTime gameTime)
        {
            if (_collected)
            {
                
                if (_collectAnimationTimes < CollectAnimationRepeatTimes)
                {
                    _time += gameTime.ElapsedGameTime.TotalMilliseconds;
                    CollectedFrameRate --;
                    if (_time < CollectedFrameRate) return;
                    _animationX+= Texture.Width / 12f;
                    if (_animationX >= Texture.Width)
                    {
                        _animationX = 0;
                        _collectAnimationTimes++;
                    }
                    _time -= CollectedFrameRate;
                }
                else
                {
                    Destroy = true;
                }//make it fade
                
                return;
            }

            normalAnimation.Update(gameTime);

        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (_collected)
            {
                var animationFrame =
                new Rectangle((int)(_animationX), Texture.Height / 2, (int)(Texture.Width / 12f), (int)(Texture.Width / 12f));
                spriteBatch.Draw(Texture, new Vector2(HitBox.X, HitBox.Y), animationFrame, Color.White, 0f, Vector2.One, _scale, SpriteEffects.None, 0f);
                return;
            }
            normalAnimation.Draw(spriteBatch, new Vector2(HitBox.X, HitBox.Y), SpriteEffects.None, _scale);           
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
