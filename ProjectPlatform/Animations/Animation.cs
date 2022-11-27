using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Graphics;

namespace ProjectPlatform.Animations
{
    internal class Animation: BasicAnimation
    {
        public Frame[] Frames { get; }
        public Frame CurrentFrame => Frames[CurrentFrameIndex];
        public override int FrameCount => Frames.Length;

        public Animation(Texture2D texture, string identifier, int framecount, int framewidth, int frameheight, int beginHeight , int fps, float scale):base(texture,identifier, fps)
        {
            Frames = new Frame[framecount];
            MakeAnimation(framewidth, frameheight, beginHeight, scale);
        }
        private void MakeAnimation(int framewidth, int frameheight, int beginHeight, float scale)
        {
            for (int i = 0; i < Frames.Length; i++)
            {
                Frames[i] = new Frame(new Rectangle(i * framewidth, beginHeight, framewidth, frameheight), Texture, scale);
            }
        }        

        public override void Draw(Sprites spriteBatch, Vector2 position, SpriteEffects spriteEffects, float scale)
        {
            spriteBatch.Draw(Texture, position, CurrentFrame.FrameRectangle, Color.White, 0f, Vector2.Zero, scale, spriteEffects, 0f);
        }

    }
}
