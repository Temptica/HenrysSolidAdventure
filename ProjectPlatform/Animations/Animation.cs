using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Graphics;
using OtterlyAdventure.OtterFolder;

namespace OtterlyAdventure.Animations
{
    internal class Animation: BasicAnimation
    {
        public FrameList<Frame> Frames { get; }
        public Frame CurrentFrame => Frames[CurrentFrameIndex];
        public override int FrameCount => Frames.Count;

        internal State State;

        public Animation(Texture2D texture, int frameCount, int fps, float scale = 1f):this(texture, State.Idle, frameCount, texture.Width/frameCount, texture.Height, 0,0,fps, scale)
        {
        }
        public Animation(Texture2D texture, State state, int frameCount, int frameWidth, int frameHeight, int beginHeight, int beginWidth, int fps, float scale = 1f):base(texture,state.ToString(), fps)
        {
            Frames = new FrameList<Frame>();
            State = state;
            MakeAnimation(frameCount,texture.Width,frameWidth, frameHeight, beginHeight, beginWidth, scale);
        }
        private void MakeAnimation(int frameCount,int textureWidth,int frameWidth, int frameHeight, int beginHeight, int beginWidth, float scale)
        {
            int frameX = beginWidth;
            int frameY = beginHeight;
            for (int i = 0; i < frameCount; i++)
            {
                if (frameX >= textureWidth) {
                    frameY += frameHeight;//if on end of texture width, go to next height
                    frameX = 0;
                }
                Frames.Add(new Frame(new Rectangle(frameX, frameY, frameWidth, frameHeight), Texture, scale));
                frameX += frameWidth;
            }
        }

        public override void Draw(Sprites spriteBatch, Vector2 position, SpriteEffects spriteEffects, float scale, float rotation = 0f, Color color = default)
        {
            if (color == default) color = Color.White;
            spriteBatch.Draw(Texture, position, CurrentFrame.FrameRectangle, color, rotation, Vector2.Zero, scale, spriteEffects, 0f);
        }

    }
}
