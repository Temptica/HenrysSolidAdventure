using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using ProjectPlatform.Mapfolder;
using ProjectPlatform.Animations;
using ProjectPlatform.Graphics;

namespace ProjectPlatform
{
    //TODO: Conditionbar, Healthbar, stats upgrades, coin collection
    internal enum State { Idle, Walking, Running, Jumping, Attacking, Sleeping, Dead, Other }
    internal class Otter
    {
        #region Consts
        private const float JumpForce = 0.35f;
        private const float MaxYVelocity = 0.5f;
        private const float WalkSpeed = 0.1f;
        private const float RunningSpeed = 0.2f;
        private const float XAcceleration = 0.03f;
        #endregion

        #region properities
        public Vector2 Position { get; set; }
        public static Texture2D Texture { get; set; }
        public List<Animation> Animations { get; set; }
        public Animation CurrentAnimation => /*Animations?.Where(a => a.AnimationState == State).FirstOrDefault();*/ Animations[0];
        //revert hitbox if moving left
        public Rectangle HitBox => GetHitBox();
        public int CurrentSpriteX { get; set; }
        public int MoveSpeed { get; set; }
        public int Health { get; set; }
        public int MaxHeath{ get; private set; }
        public int MaxCondition { get; private set; }
        public int Condition { get; set; }
        public int Damage { get; set; }
        public int AttackRange { get; set; }
        public State State { get; set; }
        public float Gravity { get; }        
        public float Scale { get; set; }
        public float Coins { get; private set; }
        

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
                new(Texture, "idle", 6, Texture.Width/6, Texture.Height,0, 4, scale)
            };
        }

        public void Update(GameTime gameTime)
        {
            Map map = Map.Instance;
            CurrentAnimation.Update(gameTime);//update the animation
            MoveUpdate(gameTime, map);
            CheckCoins();
            _velocity.X = 0;
        }
        
        private void CheckCoins()
        {
            var collected = Map.Instance.Coins?.FirstOrDefault(coin => coin.HitBox.Intersects(HitBox))?.Collect();
            if (collected == true)
            {
                Coins++;
            }
        }

        public void MoveUpdate(GameTime gameTime, Map map)
        {
            if (!canWalk)
            {
                _velocity.X = 0;
            }
            else if (InputController.LeftInput) //left
            {
                _velocity.X -= (float)(XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                if (InputController.ShiftInput)
                {
                    _velocity.X -= (float)(XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                    if (_velocity.X < -RunningSpeed)
                        _velocity.X = -RunningSpeed;
                    State = State.Running;
                }
                else
                {
                    if (_velocity.X < -WalkSpeed)
                        _velocity.X = -WalkSpeed;
                    State = State.Walking;
                }
                LookingLeft = true;
            }
            else if (InputController.RightInput)//right
            {
                _velocity.X += (float)(XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                if (InputController.ShiftInput)
                {
                    _velocity.X += (float)(XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                    if (_velocity.X > RunningSpeed)
                        _velocity.X = RunningSpeed;
                    State = State.Running;
                }
                else
                {
                    if (_velocity.X > WalkSpeed)
                        _velocity.X = WalkSpeed;
                    State = State.Walking;
                }
                LookingLeft = false;
            }
            else
            {
                _velocity.X = 0;
            }

            if (InputController.JumpInput && canJump) //jump
            {
                _velocity.Y = -JumpForce;
                canJump = false;
                State = State.Jumping;
            }
            else if (canJump)
            {
                State = State.Idle;
            }
            else
            {
                var newY = _velocity.Y + (float)(Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
                _velocity.Y = newY > MaxYVelocity ? MaxYVelocity : newY;
            }
            float velocityXDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * _velocity.X);

            var nextPosition = Position;
            if (velocityXDelta != 0)
            {
                nextPosition.X += velocityXDelta;
            }
            if (_velocity.Y != 0)
            {
                nextPosition.Y += _velocity.Y;
            }
            var nextHitBox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox.Width, HitBox.Height);

            if (_velocity.Y < 0)//going up
            {
                var tile = OtterCollision.OtterTopHit(nextHitBox, map.FrontMap);
                if (tile != null)
                {
                    _velocity.Y = 0f;
                    nextPosition = new(nextPosition.X, tile.HitBox.Bottom);
                }
                else
                {
                    _velocity.Y += (float)(Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
                    nextPosition = new Vector2(nextPosition.X, nextPosition.Y + _velocity.Y * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                }
                nextHitBox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox.Width, HitBox.Height);
                canJump = false;
            }

            if (_velocity.X > 0)//to right
            {
                //make new hitbox that will check if there will be a collision

                var tile = OtterCollision.OtterRightHit(nextHitBox, map.FrontMap);
                if (tile is not null)
                {
                    nextPosition = new Vector2(tile.Position.X - HitBox.Width - 1, nextHitBox.Y);
                    nextHitBox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox.Width, HitBox.Height);
                    _velocity.X = 0;
                }
                else if (OtterCollision.LeavingRightMapBorder(nextHitBox, Map.Instance.ScreenRectangle.Right))
                {
                    MapLoader.LoadNextMap(Map.Instance.ScreenRectangle.Height);
                    Position = Map.Instance.Spawn;
                    _velocity = Vector2.Zero;
                    return;
                }


            }
            else if (_velocity.X < 0)
            {

                var tile = OtterCollision.OtterLeftHit(nextHitBox, map.FrontMap);
                if (tile is not null)
                {
                    nextPosition = new Vector2(tile.Position.X + tile.HitBox.Width + 1, nextHitBox.Y);
                    nextHitBox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox.Width, HitBox.Height);
                    _velocity.X = 0;
                }
                else if (OtterCollision.LeavingLeftMapBorder(nextHitBox, Map.Instance.ScreenRectangle.Left))

                {
                    MapLoader.LoadPreviousMap(Map.Instance.ScreenRectangle.Height);
                    Position = new Vector2(Map.Instance.ScreenRectangle.Right - HitBox.Width - 1, Position.Y);
                    _velocity = Vector2.Zero;
                    return;
                }
            }
            if (_velocity.Y >= 0) //jump/falling mechanism
            {
                var result = OnGround(nextHitBox);
                if (result != Vector2.Zero)
                {
                    _velocity.Y = 0f;
                    canJump = true;
                    nextPosition = new((int)result.X, (int)result.Y);
                    nextHitBox = new Rectangle((int)result.X, (int)result.Y, nextHitBox.Width, nextHitBox.Height);
                }
                else
                {
                    canJump = false;
                    var newY = _velocity.Y + (float)(Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
                    _velocity.Y = newY > MaxYVelocity ? MaxYVelocity : newY;

                    nextPosition = new Vector2(nextPosition.X,
                        nextPosition.Y + _velocity.Y * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                    nextHitBox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox.Width,
                        HitBox.Height);
                }
            }


            if (_velocity.Y != 0) State = State.Jumping;
            else if (_velocity.X == 0 && _velocity.Y == 0) State = State.Idle;
            Position = new(nextHitBox.X, nextHitBox.Y);
        }
        
        public Vector2 OnGround(Rectangle hitbox)
        {
            var groundBox = new Rectangle(hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height + 10);
            //checks if any tile in map.currentMap has collision with the bottom of the otter if so set otter to the tile height            
            var tile = OtterCollision.OtterGroundHit(groundBox, Map.Instance.FrontMap);
            if (tile ==-1) return Vector2.Zero;
            //set the otter position so the otter is on the tile
            return new Vector2(hitbox.X, tile- hitbox.Height- CurrentAnimation.CurrentFrame.HitBox.Y*Scale);
        }

        public void Draw(Sprites spriteBatch)
        {
            CurrentAnimation.Draw(spriteBatch, Position, LookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Scale);
        }

        public void SetWalk(bool canWalk)
        {
            this.canWalk = canWalk;
        }
        private Rectangle GetHitBox()
        {
            if (LookingLeft)
            {//invert hitbox
                //return new((int)(Position.X + CurrentAnimation.CurrentFrame.HitBox.X * Scale- CurrentAnimation.CurrentFrame.HitBox.Width * Scale)
            return new((int)(Position.X + CurrentAnimation.CurrentFrame.HitBox.X * Scale), (int)(Position.Y + CurrentAnimation.CurrentFrame.HitBox.Y * Scale), (int)(CurrentAnimation.CurrentFrame.HitBox.Width * Scale), (int)(CurrentAnimation.CurrentFrame.HitBox.Height * Scale));

            }
            return new((int)(Position.X + CurrentAnimation.CurrentFrame.HitBox.X * Scale), (int)(Position.Y + CurrentAnimation.CurrentFrame.HitBox.Y * Scale), (int)(CurrentAnimation.CurrentFrame.HitBox.Width * Scale), (int)(CurrentAnimation.CurrentFrame.HitBox.Height * Scale));
        }
    }
    
}
