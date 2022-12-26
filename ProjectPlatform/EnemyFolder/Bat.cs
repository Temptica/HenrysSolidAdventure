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
using ProjectPlatform.OtterFolder;

//https://github.com/roy-t/AStar

namespace ProjectPlatform.EnemyFolder
{
    internal class Bat : TrackingEnemy, IGameObject
    {
        //flying tracking enemy. will start flying to you when you are 15 tiles away, regardless of walls. Once detected, it will keep tracking

        public static Texture2D Texture;
        Vector2 _velocity;
        public override Rectangle HitBox
        {
            get => new((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            set => throw new NotSupportedException();
        }
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
            _deadTexture = new Rectangle((Texture.Width / 4) * 2, 0, (Texture.Width / 4), Texture.Height);

            CurrentHp = BaseHp = 3;
            Damage = 4;

            
            //_pathFinding = new Pathfinding();
            //_pathFinding.Initialise(this, Otter.Instance);
        }

        private float timer = 0;
        private float rotation = 0;
        public override void Update(GameTime gameTime)
        {
            if (IsDead) State = State.Dead;
            if (State is State.Dead)
            {
                if (timer == 0)
                {
                    _velocity = new Vector2(0, 0.2f * gameTime.ElapsedGameTime.Milliseconds);
                }
                if (timer >= 4000) Remove = true;
                
                Position += _velocity;
                timer+= gameTime.ElapsedGameTime.Milliseconds;
                //rotate so it goes downwards
                if (rotation <= (Math.PI / 2))
                {
                    rotation += 0.01f * gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    rotation = (float)Math.PI / 2;
                }
                return;
            }
            CurrentAnimation.Update(gameTime);
            Move(gameTime);
        }

        
        public override void Draw(Sprites spriteBatch)
        {
            CurrentAnimation.Draw(spriteBatch, Position, State is State.Dead?SpriteEffects.FlipHorizontally: SpriteEffects.None, 1f,rotation);
        }
        private Random rng;
        public virtual void Move(GameTime gameTime)
        {
            _velocity = new Vector2(_velocity.X * gameTime.ElapsedGameTime.Milliseconds, _velocity.Y *gameTime.ElapsedGameTime.Milliseconds);
            Position += _velocity;
        }

        
    }
}
