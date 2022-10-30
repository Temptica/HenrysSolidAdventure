using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPlatform
{
    internal enum State { Idle, Walking, Running, Jumping, Attacking, Sleeping, Dead }
    internal class Otter:IMoveable
    {
        #region Consts
        private const int Height = 200;
        private const int Width = 200;
        private const float JumpForce = 0.5f;
        private const float MaxYVelocity = 10f;
        private const float WalkSpeed = 1f;
        private const float RunningSpeed = 2f;
        #endregion

        #region properities


        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Rectangle { get; set; }
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

        #endregion

        #region private variables
        private int _currentSpriteX;
        private double _time;
        private Vector2 _velocity;
        private bool canJump;
        private bool canWalk;
        #endregion

        public Otter(Texture2D otter, Vector2 position, float gravity)
        {
            Texture = otter;
            Position = position;
            Gravity = gravity;
        }

        
        public void Update(GameTime gameTime)
        {
            if (OnGround() && _velocity.Y >= 0)
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
            if (State == State.Idle)
            {
                _time += gameTime.ElapsedGameTime.TotalMilliseconds; 

                if (_time > 250) //1000/4
                {
                    _currentSpriteX += Width;
                    if (_currentSpriteX >= Texture.Width)
                        _currentSpriteX = 0;
                    _time = 0;
                }

            }

            if (!canWalk)
            {
                _velocity.X = 0;
            }
            Position += _velocity*(float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void MoveLeft()
        {
            _velocity.X = -WalkSpeed;
        }

        public void MoveRight()
        {
            _velocity.X = WalkSpeed;
        }

        public void Jump()
        {
            if(canJump)_velocity = new Vector2(_velocity.X, -JumpForce);
        }

        public bool OnGround()
        {
            return Position.Y >= 950;
        }

        public void Draw(SpriteBatch spriteBatch, float scale = 0.5f)
        {
            spriteBatch.Draw(Texture, Position
                ,new Rectangle(_currentSpriteX,0,Height,Width)
                ,Color.White,0,Vector2.Zero, scale, 
                _velocity.X < 0? SpriteEffects.FlipHorizontally:SpriteEffects.None//if moving to the left, then flip
                ,0f);
            _velocity.X = 0;
        }

        public void SetCanWalk(bool canWalk)
        {
            this.canWalk = canWalk;
        }
    }
}
