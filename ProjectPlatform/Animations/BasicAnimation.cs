using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Graphics;

namespace OtterlyAdventure.Animations
{
    internal class BasicAnimation
    {
        public Texture2D Texture { get; private set; }
        public string Identifier { get; private set; }
        public int CurrentFrameIndex { get; private set; }
        public bool IsFinished { get; private set; }
        public float FrameRate { get; }
        public virtual int FrameCount { get; private set; }
        private Rectangle FrameRectangle => new(CurrentFrameIndex * FrameWidth, BeginHeight, FrameWidth, FrameHeight);

        private readonly int FrameWidth;
        private readonly int FrameHeight;
        private readonly int BeginHeight;

        internal BasicAnimation(Texture2D texture, string identifier, float frameRate,int frameCount)
        {
            Texture = texture;
            Identifier = identifier;
            FrameRate = 1000f/frameRate;
            CurrentFrameIndex = 0;
            FrameCount = frameCount;
            FrameWidth = texture.Width / frameCount;
            FrameHeight = texture.Height;
        }
        internal BasicAnimation(Texture2D texture, string identifier, float frameRate, int framecount, int framewidth, int frameheight, int beginHeight)
        {
            Texture = texture;
            Identifier = identifier;
            FrameRate = 1000f/frameRate;
            CurrentFrameIndex = 0;
            FrameCount = framecount;
            FrameWidth = framewidth;
            FrameHeight = frameheight;
            BeginHeight = beginHeight;
        }
        double _time;
        public void Update(GameTime gameTime)
        {
            _time += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_time < FrameRate) return;
            CurrentFrameIndex++;
            if (CurrentFrameIndex >= FrameCount)
            {
                IsFinished = true;
                CurrentFrameIndex = 0;
            }
            else IsFinished = false;           
            
            _time = 0;
        }
        public virtual void Draw(Sprites spriteBatch, Vector2 position, SpriteEffects spriteEffects, float scale, float rotation = 0, Color color = default)
        {
            if (color == default) color = Color.White;
            spriteBatch.Draw(Texture, position, FrameRectangle, color, rotation, Vector2.Zero, scale, spriteEffects, 0f);
        }
    }
}
