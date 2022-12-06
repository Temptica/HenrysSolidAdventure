using Microsoft.Xna.Framework;
using ProjectPlatform.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Animations;
using ProjectPlatform.PathFinding;
using ProjectPlatform.Mapfolder;
using ProjectPlatform.Interface;
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Primitives;
using Roy_T.AStar.Paths;
//https://github.com/roy-t/AStar

namespace ProjectPlatform.EnemyFolder
{
    internal class Bat : TrackingEnemy, IGameObject
    {
        //flying tracking enemy. will start flying to you when you are 15 tiles away, regardless of walls. Once detected, it will keep tracking

        public static Texture2D Texture;
        Vector2 _velocity;
        //const float flyingSpeed = 0.5f;
        //const float roamingSpeed = 0.5f;
        //public Pathfinding _pathFinding;
        private Rectangle _deadTexture;
        public Bat(Vector2 position)
        {
            rng = new Random();
            _velocity = Vector2.Zero;
            Animations = new List<Animation> { new(Texture, 4, 10) };

            Position = position;
            HitBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            _deadTexture = new Rectangle((Texture.Width / 4) * 2, 0, (Texture.Width / 4), Texture.Height);

            CurrentHp = BaseHp = 3;
            Damage = 4;

            
            //_pathFinding = new Pathfinding();
            //_pathFinding.Initialise(this, Otter.Instance);
        }
        public override void Update(GameTime gameTime)
        {
            
            CurrentAnimation.Update(gameTime);
            Move(gameTime);
        }

        
        public override void Draw(Sprites spriteBatch)
        {
            if (State is State.Dead)
            {
                
            }
            else CurrentAnimation.Draw(spriteBatch, Position, SpriteEffects.None, 1f);
        }
        private Random rng;
        public override void Move(GameTime gameTime)
        {
            _velocity = new Vector2(_velocity.X * gameTime.ElapsedGameTime.Milliseconds, _velocity.Y *gameTime.ElapsedGameTime.Milliseconds);
            Position += _velocity;
            HitBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        
    }
}
