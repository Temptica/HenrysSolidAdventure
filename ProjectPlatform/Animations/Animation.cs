using HenrySolidAdventure.Characters;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Animations
{
    internal class Animation: BasicAnimation
    {
        internal State State;

        public Animation(Texture2D texture, int frameCount, int fps, float scale = 1f):this(texture, State.Idle,frameCount,fps, scale)
        {
        }

        public Animation(Texture2D texture, State state, int frameCount, int fps, float scale = 1f) : this(texture, state, frameCount, texture.Width / frameCount, texture.Height, 0, 0, fps, scale)
        {

        }
        public Animation(Texture2D texture, State state, int frameCount, int frameWidth, int frameHeight, int beginHeight, int beginWidth, int fps, float scale = 1f):base(texture,state.ToString(), fps, frameCount,frameWidth,frameHeight,beginHeight,true)
        {
            Frames = new FrameList<Frame>();
            State = state;
            MakeAnimation(texture, frameCount, texture.Width,frameWidth, frameHeight, beginHeight, beginWidth, scale);
        }
        private void MakeAnimation(Texture2D texture, int frameCount,int textureWidth,int frameWidth, int frameHeight, int beginHeight, int beginWidth, float scale)
        {
            int frameX = beginWidth;
            int frameY = beginHeight;
            for (int i = 0; i < frameCount; i++)
            {
                if (frameX >= textureWidth) {
                    frameY += frameHeight;//if on end of texture width, go to next height
                    frameX = 0;
                }
                Frames.Add(new Frame(new Rectangle(frameX, frameY, frameWidth, frameHeight), texture, scale));
                frameX += frameWidth;
            }
        }
        
        public override void Draw(Sprites spriteBatch, Vector2 position, SpriteEffects spriteEffects, float scale, float rotation = 0f, Color color = default)
        {
            if (color == default) color = Color.White;

            spriteBatch.Draw(CurrentFrame.Texture, position, CurrentFrame.FrameRectangle, color, rotation, Vector2.Zero, scale, spriteEffects, 0f);
        }

    }
}

