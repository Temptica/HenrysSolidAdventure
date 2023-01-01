using System;
using HenrySolidAdventure.Animations;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Shop
{
    internal class Store
    {
        public static Texture2D Texture;
        float FrameX;
        float framesPerSecond;
        BasicAnimation Animation;
        private readonly int FoodCount;
        private StatsBuff StatsBuff;
        private Random rng;
        private bool interacting = false;
        Rectangle HitBox;

        public Store(Vector2 position)
        {
            rng = new Random();
            FoodCount = rng.Next(3, 6);
            StatsBuff = new StatsBuff();
            Animation = new BasicAnimation(Texture, "idle", 5, 6, (int)(Texture.Width / 6f),Texture.Height, 0);
            HitBox = new Rectangle((int)position.X, (int)position.Y, (int)(Texture.Width / 6f), Texture.Height);
        }
        public void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);
        }
        public void Draw(Sprites spriteBatch)
        {
            Animation.Draw(spriteBatch, new Vector2(HitBox.X, HitBox.Y), SpriteEffects.None,1f);
        }
        public void GetMenu()
        {

        }
        
    }
}
