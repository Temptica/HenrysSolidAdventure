using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Animations;
using OtterlyAdventure.Graphics;
using OtterlyAdventure.Interface;
using OtterlyAdventure.OtterFolder;

namespace OtterlyAdventure.EnemyFolder
{
    internal class Slime:RoamingEnemy
    {//slimes are dumb and won't track
        public static Texture2D Texture;
        public Slime(Vector2 position)
        {
            Position = position;
            var frameWidth = Texture.Width / 8;
            var frameHeight = Texture.Height/3;
            Animations = new List<Animation>
            {
                new(Texture, OtterFolder.State.Idle, 4, frameWidth, frameHeight, 0, 0, 8),
                new(Texture, OtterFolder.State.Walking, 4, frameWidth, frameHeight, 0, frameWidth * 4, 8),
                new(Texture, OtterFolder.State.Attacking, 5, frameWidth, frameHeight, frameHeight, 0, 8),
                new(Texture, OtterFolder.State.Hit, 4, frameWidth, frameHeight, frameHeight, frameWidth * 5, 8),
                new(Texture, OtterFolder.State.Dead, 4, frameWidth, frameHeight, frameHeight * 2, frameWidth, 8)
            };
            CurrentHp = BaseHp = 6;
            Damage = 3;
            DefineWalkablePath();
        }
        //public override void Update(GameTime gameTime)
        //{
        //    base.Update(gameTime);
        //}

        //public override void Draw(Sprites spriteBatch)
        //{
        //    base.Draw(spriteBatch);
        //}

        
    }
}
