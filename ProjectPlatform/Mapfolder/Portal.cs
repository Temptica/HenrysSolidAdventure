using HenrySolidAdventure.Animations;
using HenrySolidAdventure.Characters.HeroFolder;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.MapFolder
{
    internal class Portal
    {
        public Vector2 Position { get; private set; }
        public static Texture2D Texture;

        public Rectangle HitBox => new((int)Position.X, (int)Position.Y, _basicAnimation.CurrentFrame.FrameRectangle.Width, _basicAnimation.CurrentFrame.FrameRectangle.Height);
        private readonly BasicAnimation _basicAnimation;
        public Portal(Vector2 position)
        {
            Position = position;
            _basicAnimation = new BasicAnimation(Texture, "portal", 4f,5);
        }

        public bool Update(GameTime gameTime)
        {
            _basicAnimation.Update(gameTime);
            return Hero.Instance.HitBox.Intersects(HitBox);
        }
        public void Draw(Sprites sprites)
        {
            _basicAnimation.Draw(sprites,Position,SpriteEffects.None,1f);

        }
    }
}
