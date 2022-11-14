using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using ProjectPlatform.Mapfolder;
using ProjectPlatform.Animations;

namespace ProjectPlatform
{
    internal enum State { Idle, Walking, Running, Jumping, Attacking, Sleeping, Dead }
    internal class Otter
    {
        #region Consts
        private const float JumpForce = 0.5f;
        private const float MaxYVelocity = 10f;
        private const float WalkSpeed = 0.4f;
        private const float RunningSpeed = 1f;
        #endregion

        #region properities
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public List<Animation> Animations { get; set; }
        public Animation CurrentAnimation => /*Animations?.Where(a => a.AnimationState == State).FirstOrDefault();*/ Animations[0];
        //revert hitbox if moving left
        public Rectangle HitBox => new Rectangle((int)Position.X + CurrentAnimation.CurrentFrame.HitBox.X, (int)Position.Y + CurrentAnimation.CurrentFrame.HitBox.Y, CurrentAnimation.CurrentFrame.HitBox.Width, CurrentAnimation.CurrentFrame.HitBox.Height);
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

        private double _time;
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
            Scale = scale*2;
            Animations = new List<Animation>()
            {
                new(Texture, State.Idle, 6, 200, 200,0, 4, scale)
            };
        }

        public void Update(GameTime gameTime)
        {
            if (_velocity.Y >= 0)
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
                canJump = false;
                _velocity.Y += (float)(Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            //UpdateAnimation(gameTime);
            CurrentAnimation.Update(gameTime);

            if (!canWalk)
            {
                _velocity.X = 0;
            }
            Position += _velocity*(float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_velocity.Y != 0) State = State.Jumping;
            else if (_velocity.X != 0) State = _velocity.X> WalkSpeed ? State.Running:State.Walking;
            else State = State.Idle;
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
            Map map = Map.GetInstance();
            
            //checks if any tile in map.currentMap has collision with the bottom of the otter if so set otter to the tile height            
            var tile = OtterCollision.OtterGroundHit(this, map.FrontMap);

            if(tile is not null)
            {
                Position = new(Position.X, tile.HitBox.Top - (HitBox.Height/Scale) + 2);
                return true;
            }
            return false;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentAnimation.Draw(spriteBatch, Position, LookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            _velocity.X = 0;
        }

        public void SetCanWalk(bool canWalk)
        {
            this.canWalk = canWalk;
        }
    }
    
}
