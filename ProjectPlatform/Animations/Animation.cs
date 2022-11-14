using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPlatform.Animations
{
    internal class Animation
    {
        public Texture2D Texture { get; set; }
        public State AnimationState { get; set; }
        public Frame[] Frames { get; }
        public int CurrentFrameIndex { get; private set; }
        public int FrameRate { get; }
        public Frame CurrentFrame => Frames[CurrentFrameIndex];

        public Animation(Texture2D texture, State animationState, int framecount, int framewidth, int frameheight, int beginHeight , int fps, float scale)
        {
            Texture = texture;
            AnimationState = animationState;
            Frames = new Frame[framecount];
            CurrentFrameIndex = 0;
            FrameRate = fps;
            MakeAnimation(framewidth, frameheight, beginHeight, scale);
        }
        private void MakeAnimation(int framewidth, int frameheight, int beginHeight, float scale)
        {
            for (int i = 0; i < Frames.Length; i++)
            {
                Frames[i] = new Frame(new Rectangle(i * framewidth, beginHeight, framewidth, frameheight), Texture, scale);
            }
        }
        public void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds % FrameRate == 0)
            {
                CurrentFrameIndex++;
                if (CurrentFrameIndex >= Frames.Length)
                {
                    CurrentFrameIndex = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(Texture, position, CurrentFrame.FrameRectangle, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, 0f);
        }

    }
}
