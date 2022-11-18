using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using ProjectPlatform.Mapfolder;
using ProjectPlatform.Animations;

namespace ProjectPlatform
{
    //TODO: Contionbar, Healthbar, stats upgrades, coin collection
    internal enum State { Idle, Walking, Running, Jumping, Attacking, Sleeping, Dead }
    internal class Otter
    {
        #region Consts
        private const float JumpForce = 0.5f;
        private const float MaxYVelocity = 0.7f;
        private const float WalkSpeed = 0.2f;
        private const float RunningSpeed = 0.4f;
        private const float XAcceleration = 0.1f;
        #endregion

        #region properities
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public List<Animation> Animations { get; set; }
        public Animation CurrentAnimation => /*Animations?.Where(a => a.AnimationState == State).FirstOrDefault();*/ Animations[0];
        //revert hitbox if moving left
        public Rectangle HitBox => new((int)(Position.X + CurrentAnimation.CurrentFrame.HitBox.X*Scale), (int)(Position.Y + CurrentAnimation.CurrentFrame.HitBox.Y*Scale), (int)(CurrentAnimation.CurrentFrame.HitBox.Width*Scale), (int)(CurrentAnimation.CurrentFrame.HitBox.Height * Scale));
        public int CurrentSpriteX { get; set; }
        public int MoveSpeed { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public int AttackRange { get; set; }
        public State State { get; set; }
        public float Gravity { get; }
        private float _currentYVelocity;

        public float CurrentYVelocity
        {
            get => _currentYVelocity;
            set => _currentYVelocity = value>MaxYVelocity?MaxYVelocity:value;
        }
        public float Scale { get; set; }

        #endregion
        #region private variables
        private Vector2 _velocity;
        private bool canJump;
        private bool canWalk;
        private bool LookingLeft = false;
        
        #endregion

        public Otter(Texture2D otter, Vector2 position, float gravity, float scale)
        {
            Texture = otter;
            Position = position;
            Gravity = gravity;
            Scale = scale;
            Animations = new List<Animation>()
            {
                new(Texture, State.Idle, 6, 200, 200,0, 4, scale)
            };
        }

        public void Update(GameTime gameTime)
        {
            Map map = Map.GetInstance();
            if (_velocity.Y >= 0) //jump/falling mechanism
            {
                if (OnGround())
                {
                    _velocity.Y = 0f;
                    canJump = true;
                }
                else
                {
                    canJump = false;
                    var newY = _velocity.Y + (float)(Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
                    _velocity.Y = newY > MaxYVelocity ? MaxYVelocity : newY;
                }
            }
            else //still going up
            {
                var tile = OtterCollision.OtterTopHit(this, map.FrontMap);
                if (tile != null)
                {
                    _velocity.Y = 0f;
                    Position =  new (Position.X,tile.HitBox.Bottom +1);

                }
                else
                {
                    _velocity.Y += (float)(Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
                }
                canJump = false;                
            }

            

            if (!canWalk)
            {
                _velocity.X = 0;
            }

            if (_velocity.X > 0)
            {
                var tile = OtterCollision.OtterRightHit(this, map.FrontMap);
                if(tile is not null)
                {
                    Position = new(tile.Position.X - HitBox.Width-1, Position.Y);
                    _velocity.X = 0;
                }
            }
            else if (_velocity.X < 0)
            {
                var tile = OtterCollision.OtterLeftHit(this, map.FrontMap);
                if (tile is not null)
                {
                    Position = new(tile.Position.X + tile.HitBox.Width+1, Position.Y);
                    _velocity.X = 0;
                }
            }
            Position += _velocity*(float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_velocity.Y != 0) State = State.Jumping;
            else if (_velocity.X != 0) State = Math.Abs(_velocity.X)> WalkSpeed ? State.Running:State.Walking;
            else State = State.Idle;
            
            CurrentAnimation.Update(gameTime);//update the animation
        }

        public void MoveLeft(bool isRunning = false)
        {
            _velocity.X = isRunning? -RunningSpeed : -WalkSpeed;
            LookingLeft = true;
        }

        public void MoveRight(bool isRunning = false)
        {
            _velocity.X = isRunning ? RunningSpeed : WalkSpeed;
            LookingLeft = false;
        }

        public void Jump()
        {
            if (canJump) _velocity.Y = -JumpForce;
        }

        public bool OnGround()
        {
            //checks if any tile in map.currentMap has collision with the bottom of the otter if so set otter to the tile height            
            var tile = OtterCollision.OtterGroundHit(this, Map.GetInstance().FrontMap);
            if (tile ==-1) return false;
            //set the otter position so the otter is on the tile
            Position = new Vector2(Position.X, tile- HitBox.Height- CurrentAnimation.CurrentFrame.HitBox.Y*Scale+1);
            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentAnimation.Draw(spriteBatch, Position, LookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Scale);
            _velocity.X = 0;
        }

        public void SetCanWalk(bool canWalk)
        {
            this.canWalk = canWalk;
        }
    }
    
}
