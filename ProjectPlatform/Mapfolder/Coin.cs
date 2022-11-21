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
        private const float FrameRate = 200;//5 fps
        private float AnimationX;
        bool Collected = false;
        internal Rectangle HitBox;
        private float scale;

        internal Coin(Vector2 position)
        {
            scale = 0.125f *Map.GetInstance().Scale;
            HitBox = new Rectangle((int)position.X, (int)position.Y, (int)(32*scale), (int)(32*scale));
            AnimationX = 0;
        }

        private double _time;
        internal void Update(GameTime gameTime)
        {
            _time += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_time< FrameRate) return;
            AnimationX++;
            if (AnimationX >= 4)
            {
                AnimationX = 0;
            }
            _time -= FrameRate;
        }

    }
}
