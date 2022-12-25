using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Graphics;
using ProjectPlatform.OtterFolder;

namespace ProjectPlatform.Animations
{
    internal class Animation: BasicAnimation
    {
        public Frame[] Frames { get; }
        public Frame CurrentFrame => Frames[CurrentFrameIndex];
        public override int FrameCount => Frames.Length;

        internal State State;

        public Animation(Texture2D texture, int framecount, int fps, float scale = 1f):this(texture, State.Idle, framecount, texture.Width/framecount, texture.Height, 0,0,fps, scale)
        {
        }
        public Animation(Texture2D texture, State state, int framecount, int framewidth, int frameheight, int beginHeight, int beginWidth, int fps, float scale = 1f):base(texture,state.ToString(), fps)
        {
            Frames = new Frame[framecount];
            State = state;
            MakeAnimation(framewidth, frameheight, beginHeight, beginWidth, scale);
        }
        private void MakeAnimation(int framewidth, int frameheight, int beginHeight, int beginWidth, float scale)
        {
            for (int i = 0; i < Frames.Length; i++)
            {
                Frames[i] = new Frame(new Rectangle(i * framewidth+beginWidth, beginHeight, framewidth, frameheight), Texture, scale);
            }
        }

        public override void Draw(Sprites spriteBatch, Vector2 position, SpriteEffects spriteEffects, float scale, float rotation = 0f, Color color = default)
        {
            if (color == default) color = Color.White;
            spriteBatch.Draw(Texture, position, CurrentFrame.FrameRectangle, color, rotation, Vector2.Zero, scale, spriteEffects, 0f);
        }

    }
}
