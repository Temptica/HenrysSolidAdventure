using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Animations;
using ProjectPlatform.Graphics;
using ProjectPlatform.Mapfolder;

namespace ProjectPlatform.EnemyFolder
{
    internal class Skeleton:RoamingEnemy
    {//somewhat smart, will track when enemies are on the same platform
        public static Dictionary<State, Texture2D> Textures;//list as some of the spritesheets are bigger than others due to the big "sword" making it very difficult having them on one sprite
        Vector2 _maxLeftPosition;
        private Vector2 _maxRightPosition;
        public Skeleton(Vector2 position):base()
        {
            Position = position;
            Animations = new()
            {//https://jesse-m.itch.io/skeleton-pack
                new Animation(Textures[State.Idle], State.Idle,Textures[State.Idle].Width/11, Textures[State.Idle].Width, Textures[State.Idle].Height, 0, 0,5),
                new Animation(Textures[State.Walking], State.Walking,Textures[State.Walking].Width/13, Textures[State.Walking].Width, Textures[State.Walking].Height, 0, 0,5),
                new Animation(Textures[State.Attacking], State.Attacking,Textures[State.Attacking].Width/18, Textures[State.Attacking].Width, Textures[State.Attacking].Height, 0, 0,5),
                new Animation(Textures[State.Hit], State.Hit,Textures[State.Hit].Width/8, Textures[State.Hit].Width, Textures[State.Hit].Height, 0, 0,5),
                new Animation(Textures[State.Dead], State.Dead,Textures[State.Dead].Width/15, Textures[State.Dead].Width, Textures[State.Dead].Height, 0, 0,5),
                new Animation(Textures[State.Other], State.Other,Textures[State.Other].Width/4, Textures[State.Other].Width, Textures[State.Other].Height, 0, 0,5)//when skeleton detects Otter
            };
            SetWalkBounds();
        }

        private void SetWalkBounds()
        {
            Map map = Map.Instance;
            //get tile the enemy will be standing on
        }
        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(Sprites spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Move(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
