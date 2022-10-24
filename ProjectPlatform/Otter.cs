using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPlatform
{
    internal enum State { Idle, Walking, Running, Jumping, Attacking, Sleeping, Dead }
    internal class Otter : IMovable
    {
        #region Consts
        private const int Height = 200;
        private const int Width = 200;
        private const float jumpForce = 7f;
        private const float maxY = 10f;
        #endregion

        #region properities
        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Rectangle { get; set; }
        public int MoveSpeed { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public int AttackRange { get; set; }
        public State State { get; set; }
        public float Gravity { get; }
        #endregion
        
        #region private variables
        private int _currentX;
        private double _time;
        #endregion

        public Otter(Texture2D otter, Vector2 position, float gravity)
        {
            Texture = otter;
            Position = position;
            Gravity = gravity;
        }

        private float airtime;
        public void Update(GameTime gameTime)
        {
            var newVelocity = Velocity;
            if (!OnGround())
            {
                airtime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                var newY = (Gravity * airtime)+ Velocity.Y;
                newVelocity.Y += newY > maxY ? maxY : newY;//make falling smoother
                
            }
            else
            {
                airtime = 0f;
            }
            if (State == State.Idle)
            {
                _time += gameTime.ElapsedGameTime.TotalMilliseconds; 

                if (_time > 250) //1000/4
                {
                    _currentX += Width;
                    if (_currentX >= Texture.Width)
                        _currentX = 0;
                    _time = 0;
                }

            }

            Velocity = newVelocity;
            Position += newVelocity;
        }

        public void Jump()
        {
            Velocity = new Vector2(Velocity.X, -jumpForce) ;
            airtime = 0f;
        }

        public bool OnGround()
        {
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position,new Rectangle(_currentX,0,Height,Width)
                ,Color.White,0,Vector2.Zero,0.5f, 
                Velocity.X < 0? SpriteEffects.FlipHorizontally:SpriteEffects.None//if moving to the left, then flip
                ,0f);
        }
    }
}
