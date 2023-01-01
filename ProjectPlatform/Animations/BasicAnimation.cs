using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Animations
{
    internal class BasicAnimation
    {
        public string Identifier { get; private set; }
        public float FrameRate { get; }
        public FrameList<Frame> Frames { get; protected set; }
        public Frame CurrentFrame => Frames.CurrentFrame;
        public int CurrentFrameIndex => Frames.AnimationIndex;
        public bool IsFinished{ get; set; }

        internal BasicAnimation(Texture2D texture, string identifier, float frameRate,int frameCount):this(texture,identifier, frameRate, frameCount, texture.Width / frameCount, texture.Height,0)
        {
        }
        internal BasicAnimation(Texture2D texture, string identifier, float frameRate, int frameCount, int frameWidth, int frameHeight, int beginHeight)
        {
            Identifier = identifier;
            FrameRate = 1000f/frameRate;
            Frames = new();
            for (int i = 0; i < frameCount; i++)
            {
                Frames.Add(new Frame(new Rectangle(i * frameWidth, beginHeight, frameWidth, frameHeight), texture, 1f));
            }
        }
        double _time;
        public void Update(GameTime gameTime)
        {
            _time += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_time < FrameRate) return; 
            Frames.AnimationIndex++;
            if (Frames.AnimationIndex >= Frames.Count)
            {
                Frames.AnimationIndex = 0;
                IsFinished = true;
            }
            else IsFinished = false;

            _time = 0;
        }
        public virtual void Draw(Sprites spriteBatch, Vector2 position, SpriteEffects spriteEffects, float scale, float rotation = 0, Color color = default)
        {
            if (color == default) color = Color.White;
            spriteBatch.Draw(CurrentFrame.Texture, position, CurrentFrame.FrameRectangle, color, rotation, Vector2.Zero, scale, spriteEffects, 0f);
        }
    }
}
