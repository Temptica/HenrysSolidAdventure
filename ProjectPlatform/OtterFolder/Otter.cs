using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Animations;
using ProjectPlatform.Controller;
using ProjectPlatform.EnemyFolder;
using ProjectPlatform.Graphics;
using ProjectPlatform.Interface;
using ProjectPlatform.Mapfolder;

namespace ProjectPlatform.OtterFolder
{
    //TODO: Conditionbar, Healthbar, stats upgrades, coin collection
    internal enum State { Idle, Walking, Running, Jumping, Attacking, Sleeping, Dead, Hit, Other }
    internal class Otter : IAnimatable, IGameObject
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
        public Rectangle HitBox
        {
            get => GetHitBox();
            set => throw new NotImplementedException();
        }
        public int Health { get; private set; }
        public float HealthPercentage => (float)Math.Round(Health / (double)MaxHealth * 100, 0);
        public int MaxHealth { get; private set; }
        public int MaxCondition { get; private set; }
        public int Condition { get; set; }
        public int Damage { get; set; }
        public int AttackRange { get; set; }
        public State State { get; set; }
        public float Gravity { get; private set; }
        public float Scale { get; set; }
        public float Coins { get; private set; }


        #endregion
        #region private variables
        private Vector2 _velocity;
        private bool _canJump;
        private bool _canWalk;
        private bool _lookingLeft;
        private bool _IsWalking;
        private bool _IsRunning;
        private bool _IsJumping;
        private bool _IsAttacking;
        private bool _IsSleeping;
        private bool _IsDead;
        private bool _IsHit;
        #endregion
        //signleton 
        private static Otter _instance;
        private bool _canAttack = true;
        public static Otter Instance => _instance ??= new Otter();

        private Otter()
        {
        }
        public void Initialise(Texture2D otter, Vector2 position, float gravity, float scale)
        {
            Texture = otter;
            Position = position;
            Gravity = gravity;
            Scale = scale;
            Animations = new List<Animation>()
            {
                new(Texture, State, 6, Texture.Width/6, Texture.Height,0, 0,4, scale)
            };
            Health = MaxHealth = 20;
            Damage = 7;
        }

        public void Update(GameTime gameTime)
        {
            Map map = Map.Instance;
            SetState();
            CurrentAnimation.Update(gameTime);//update the animation
            MoveUpdate(gameTime, map);
            CheckCoins();
            CheckEnemies();
            _velocity.X = 0;
        }

        private void SetState()
        {
            if (State is State.Dead || _IsDead || State is State.Hit && _IsHit) return;
            if (State is State.Attacking)
            {
                if (!CurrentAnimation.IsFinished) return;
                _IsAttacking = false;
            }
            else
            {
                _IsAttacking = InputController.Attack;
            }


            if (_IsAttacking && _canAttack) State = State.Attacking;
            else if (_IsJumping) State = State.Jumping;
            else if (_IsRunning) State = State.Running;
            else if (_IsWalking) State = State.Walking;
            else State = State.Idle;
        }

        private void CheckEnemies()
        {
            if (Enemy.Enemies.Count <= 0) return;
            foreach (var enemy in Enemy.Enemies.Where(enemy => enemy.State is not State.Dead or State.Hit).Where(enemy => OtterCollision.PixelBasedHit(this, enemy)))
            {
                if (State == State.Attacking)
                {
                    if (!enemy.GetDamage(Damage)) continue;
                    Coins += 1;
                    var coin = new Coin(enemy.Position);
                    Map.Instance.Coins.Add(coin);
                    coin.Collect();
                }
                else
                {
                    Health -= enemy.Attack();
                }
            }
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
            if (!_canWalk)
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
                    _IsRunning = true;

                }
                else
                {
                    if (_velocity.X < -WalkSpeed)
                        _velocity.X = -WalkSpeed;
                    _IsRunning = false;
                }
                _lookingLeft = true;
                _IsWalking = true;
            }
            else if (InputController.RightInput)//right
            {
                _velocity.X += (float)(XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                if (InputController.ShiftInput)
                {
                    _velocity.X += (float)(XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                    if (_velocity.X > RunningSpeed)
                        _velocity.X = RunningSpeed;
                    _IsRunning = true;
                }
                else
                {
                    if (_velocity.X > WalkSpeed)
                        _velocity.X = WalkSpeed;
                    _IsRunning = false;
                }
                _IsWalking = true;
                _lookingLeft = false;
            }
            else
            {
                _velocity.X = 0;
                _IsWalking = false;
                _IsRunning = false;
            }

            if (InputController.JumpInput && _canJump) //jump
            {
                _velocity.Y = -JumpForce;
                _canJump = false;
            }
            else if (!_canJump)
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
                    nextPosition = new(nextPosition.X, tile.HitBox.Bottom + 1);
                }
                else
                {
                    _velocity.Y += (float)(Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
                    nextPosition = new Vector2(nextPosition.X, nextPosition.Y + _velocity.Y * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                }
                nextHitBox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox.Width, HitBox.Height);
                _canJump = false;
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
                    _canJump = true;
                    nextHitBox = new Rectangle((int)result.X, (int)result.Y, nextHitBox.Width, nextHitBox.Height);
                }
                else
                {
                    _canJump = false;
                    var newY = _velocity.Y + (float)(Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
                    _velocity.Y = newY > MaxYVelocity ? MaxYVelocity : newY;

                    nextPosition = new Vector2(nextPosition.X,
                        nextPosition.Y + _velocity.Y * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                    nextHitBox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox.Width,
                        HitBox.Height);
                }
            }
            Position = new(nextHitBox.X, nextHitBox.Y);

        }

        public Vector2 OnGround(Rectangle hitbox)
        {
            var groundBox = new Rectangle(hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height + 10);
            //checks if any tile in map.currentMap has collision with the bottom of the otter if so set otter to the tile height            
            var tile = OtterCollision.OtterGroundHit(groundBox, Map.Instance.FrontMap);
            if (tile == -1) return Vector2.Zero;
            //set the otter position so the otter is on the tile
            return new Vector2(hitbox.X, tile - hitbox.Height - CurrentAnimation.CurrentFrame.HitBox.Y * Scale);
        }

        public void Draw(Sprites spriteBatch)
        {
            CurrentAnimation.Draw(spriteBatch, Position, _lookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Scale);
        }

        public void SetWalk(bool canWalk)
        {
            _canWalk = canWalk;
        }
        private Rectangle GetHitBox()
        {
            if (_lookingLeft)
            {//invert hitbox
                //return new((int)(Position.X + CurrentAnimation.CurrentFrame.HitBox.X * Scale- CurrentAnimation.CurrentFrame.HitBox.Width * Scale)
                return new((int)(Position.X + CurrentAnimation.CurrentFrame.HitBox.X * Scale), (int)(Position.Y + CurrentAnimation.CurrentFrame.HitBox.Y * Scale), (int)(CurrentAnimation.CurrentFrame.HitBox.Width * Scale), (int)(CurrentAnimation.CurrentFrame.HitBox.Height * Scale));

            }
            return new((int)(Position.X + CurrentAnimation.CurrentFrame.HitBox.X * Scale), (int)(Position.Y + CurrentAnimation.CurrentFrame.HitBox.Y * Scale), (int)(CurrentAnimation.CurrentFrame.HitBox.Width * Scale), (int)(CurrentAnimation.CurrentFrame.HitBox.Height * Scale));
        }
    }

}
