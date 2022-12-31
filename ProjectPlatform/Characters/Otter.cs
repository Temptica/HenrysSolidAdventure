using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Animations;
using OtterlyAdventure.Controller;
using OtterlyAdventure.Graphics;
using OtterlyAdventure.Mapfolder;
using OtterlyAdventure.OtterFolder;

namespace OtterlyAdventure.Characters
{
    //TODO: Conditionbar, Healthbar, stats upgrades, coin collection
    internal enum State { Idle, Walking, Running, Jumping, Attacking, Sleeping, Dead, Hit, Other }
    internal class Otter : Character
    {
        #region Consts
        private const float JumpForce = 0.35f;
        private const float MaxYVelocity = 0.5f;
        private const float WalkSpeed = 0.1f;
        private const float RunningSpeed = 0.2f;
        private const float XAcceleration = 0.03f;
        #endregion

        #region properities
        public static Texture2D Texture { get; set; }
        public Animation CurrentAnimation
        {
            get
            {
                var ani = Animations?.Where(a => a.State == State).FirstOrDefault();
                return ani ?? Animations?.FirstOrDefault();//if ani is default, it will take standard animation
            }
        }
        public int Health
        {
            get => _healt;
            private set => _healt = Util.Clamp(value, 0, MaxHealth);
        }

        public float HealthPercentage => (float)Math.Round(Health / (double)MaxHealth * 100, 0);
        public int MaxHealth { get; private set; }
        public int MaxCondition { get; private set; }
        public int Condition { get; set; }
        public State State { get; set; }
        public float Gravity { get; private set; }
        public float Scale { get; set; }
        public float Coins { get; private set; }
        #endregion
        //signleton 
        private static Otter _instance;
        private bool _canAttack = true;
        private bool _canJump;
        private bool _isRunning;
        private bool _canWalk;
        private bool _isJumping;
        private int _healt;
        public static Otter Instance => _instance ??= new Otter();

        private Otter()//use last texture height to correct position
        {
        }
        public void Initialise(Texture2D otter, Vector2 position, float gravity, float scale)
        {
            Texture = otter;
            Position = position;
            Gravity = gravity;
            Scale = scale;
            var widthHeight = Texture.Width / 5;//is squared
            Animations = new AnimationList<Animation>()//idea: textures for run, jump and attack from online, keep idle and walk from self drawn. make hit and dead myself
            {//0,0-4 + 
                new(Texture, State.Walking, 6, widthHeight, widthHeight,0, 0,6, scale),
                new(Texture, State.Idle, 9, widthHeight, widthHeight, widthHeight, widthHeight,4, scale),
                new(Texture, State.Jumping, 6, widthHeight, widthHeight,0, 0,6, scale),
                new(Texture, State.Running, 6,widthHeight, widthHeight,0, 0,12, scale),
                new(Texture, State.Attacking,9, widthHeight, widthHeight, widthHeight, widthHeight,4, scale)
            };
            Reset();
        }

        public override void Update(GameTime gameTime)
        {
            if (Health <= 0)
            {
                IsDead = true;
            }
            SetState();
            Animations.Update(State, gameTime);
            if (State is State.Dead) return;
            MoveUpdate(gameTime, Map.Instance);
            CheckCoins();
            CheckEnemies();
            Velocity.X = 0;
        }

        internal void MenuUpdate(GameTime gameTime, float leftBound, float rightBound, float bottomBound)
        {
            SetState();
            CurrentAnimation.Update(gameTime);//update the animation
            MoveUpdate(gameTime, leftBound, rightBound, bottomBound);
            Velocity.X = 0;
        }



        private void SetState()
        {
            if (State is State.Dead || IsDead)
            {
                if (CurrentAnimation.IsFinished)
                {
                    Game1.SetState(GameState.GameOver);
                }
                return;
            }

            if (State is State.Hit)
            {
                IsHit = !CurrentAnimation.IsFinished;
            }
            if (IsHit)
            {
                State = State.Hit;
                return;
            }
            if (State is State.Attacking)
            {
                if (!CurrentAnimation.IsFinished) return;
                IsAttacking = false;
                _canAttack = true;
            }
            else
            {
                IsAttacking = _canAttack && InputController.Attack;
            }

            if (IsAttacking)
            {
                State = State.Attacking;
                _canAttack = false;
            }
            else if (_isJumping) State = State.Jumping;
            else if (_isRunning) State = State.Running;
            else if (IsWalking) State = State.Walking;
            else State = State.Idle;
        }

        private void CheckEnemies()
        {
            if (Map.Instance.Enemies.Count <= 0) return;
            foreach (var enemy in Map.Instance.Enemies.Where(enemy => enemy.State is not State.Dead and not State.Hit).Where(enemy => CollisionHelper.PixelBasedHit(this, enemy)))
            {

                if (State == State.Attacking)
                {
                    _canAttack = false;
                    if (!enemy.GetDamage(Damage)) { continue; }
                    Coins += 1;
                    var coin = new Coin(enemy.Position);
                    Map.Instance.Coins.Add(coin);
                    coin.Collect();
                    continue;
                }
                var damage = enemy.Attack();
                if (damage > 0)
                {
                    IsHit = true;
                    Health -= damage;
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
        private void MoveUpdate(GameTime gameTime, float leftBound, float rightBound, float bottomBound)
        {
            GetVelocity(gameTime);

            float velocityXDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Velocity.X);

            var nextPosition = Position;
            if (velocityXDelta != 0)
            {
                nextPosition.X += velocityXDelta;
            }
            if (Velocity.Y != 0)
            {
                nextPosition.Y += (float)(Velocity.Y * gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            var nextHitBox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox.Width, HitBox.Height);

            if (Velocity.X > 0 && nextHitBox.Right > rightBound)
            {
                nextPosition.X = rightBound - HitBox.Width;
            }
            else if (Velocity.X < 0 && nextHitBox.Left < leftBound)
            {
                nextPosition.X = leftBound;
            }
            if (Velocity.Y > 0 && nextHitBox.Bottom > bottomBound)
            {
                nextPosition.Y = bottomBound - HitBox.Height;
                Velocity.Y = 0;
                _canJump = true;
            }
            else if (Velocity.Y < 0 && nextHitBox.Top < 0)
            {
                nextPosition.Y = 0;
            }
            Position = nextPosition;
        }
        public void MoveUpdate(GameTime gameTime, Map map)
        {
            GetVelocity(gameTime);

            float velocityXDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Velocity.X);

            var nextPosition = Position;
            if (velocityXDelta != 0)
            {
                nextPosition.X += velocityXDelta;
            }
            if (Velocity.Y != 0)
            {
                nextPosition.Y += Velocity.Y;
            }
            var nextHitBox = new Rectangle((int)Math.Round(nextPosition.X), (int)Math.Round(nextPosition.Y), HitBox.Width, HitBox.Height);



            if (Velocity.Y < 0)//going up
            {

                Velocity.Y += (float)(Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
                nextPosition = new Vector2(nextPosition.X, nextPosition.Y + Velocity.Y * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                nextHitBox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox.Width, HitBox.Height);
                var tile = CollisionHelper.OtterTopHit(nextHitBox, map.FrontMap);
                if (tile != null)
                {
                    Velocity.Y = 0f;
                    nextPosition = new(nextPosition.X, tile.HitBox.Bottom + 3);
                    nextHitBox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox.Width, HitBox.Height);
                }
                _canJump = false;
            }

            if (Velocity.X > 0)//to right
            {
                var tile = CollisionHelper.OtterRightHit(nextHitBox, map.FrontMap);
                if (tile is not null)
                {
                    nextPosition = new Vector2(tile.Position.X - HitBox.Width - 1, nextHitBox.Y);
                    nextHitBox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox.Width, HitBox.Height);
                    Velocity.X = 0;
                }
                else if (CollisionHelper.LeavingRightMapBorder(nextHitBox, Map.Instance.ScreenRectangle.Right))
                {
                    MapLoader.LoadNextMap(Map.Instance.ScreenRectangle.Height);
                    Position = Map.Instance.Spawn;
                    Velocity = Vector2.Zero;
                    return;
                }
            }
            else if (Velocity.X < 0)
            {
                var tile = CollisionHelper.OtterLeftHit(nextHitBox, map.FrontMap);
                if (tile is not null)
                {
                    nextPosition = new Vector2(tile.HitBox.Right + 1, nextHitBox.Y);
                    nextHitBox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox.Width, HitBox.Height);
                    Velocity.X = 0;
                }
                else if (CollisionHelper.LeavingLeftMapBorder(nextHitBox, Map.Instance.ScreenRectangle.Left))
                {
                    Velocity.X = 0;
                    nextPosition = new Vector2(1, nextHitBox.Y);
                    nextHitBox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox.Width, HitBox.Height);
                }
            }
            if (Velocity.Y >= 0) //falling mechanism
            {
                var result = OnGround(nextHitBox);
                if (result != Vector2.Zero)
                {
                    Velocity.Y = 0f;
                    _canJump = true;
                    nextHitBox = new Rectangle((int)result.X, (int)result.Y, nextHitBox.Width, nextHitBox.Height);
                }
                else
                {
                    if (CollisionHelper.LeavingBottomMapBorder(nextHitBox, Map.Instance.ScreenRectangle.Height))
                    {
                        Position = nextPosition = Map.Instance.Spawn;
                        Health -= MaxHealth / 4;
                        return;
                    }
                    _canJump = false;
                    var newY = Velocity.Y + (float)(Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
                    Velocity.Y = newY > MaxYVelocity ? MaxYVelocity : newY;

                    nextPosition = new Vector2(nextPosition.X,
                        nextPosition.Y + Velocity.Y * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                    nextHitBox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox.Width,
                        HitBox.Height);
                }
            }

            _isJumping = Velocity.Y != 0;
            Position = new(nextHitBox.X, nextHitBox.Y);

        }

        private void GetVelocity(GameTime gameTime)
        {
            if (!_canWalk)
            {
                Velocity.X = 0;
            }
            else if (InputController.LeftInput) //left
            {
                Velocity.X -= (float)(XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                if (InputController.ShiftInput)
                {
                    Velocity.X -= (float)(XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                    if (Velocity.X < -RunningSpeed)
                        Velocity.X = -RunningSpeed;
                    _isRunning = true;

                }
                else
                {
                    if (Velocity.X < -WalkSpeed)
                        Velocity.X = -WalkSpeed;
                    _isRunning = false;
                }
                IsFacingLeft = true;
                IsWalking = true;
            }
            else if (InputController.RightInput)//right
            {
                Velocity.X += (float)(XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                if (InputController.ShiftInput)
                {
                    Velocity.X += (float)(XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                    if (Velocity.X > RunningSpeed)
                        Velocity.X = RunningSpeed;
                    _isRunning = true;
                }
                else
                {
                    if (Velocity.X > WalkSpeed)
                        Velocity.X = WalkSpeed;
                    _isRunning = false;
                }
                IsWalking = true;
                IsFacingLeft = false;
            }
            else
            {
                Velocity.X = 0;
                IsWalking = false;
                _isRunning = false;
            }

            if (InputController.JumpInput && _canJump) //jump
            {
                Velocity.Y = -JumpForce;
                _canJump = false;
            }
            else if (!_canJump)
            {
                var newY = Velocity.Y + (float)(Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
                Velocity.Y = newY > MaxYVelocity ? MaxYVelocity : newY;
            }
        }

        public Vector2 OnGround(Rectangle hitbox)
        {
            var groundBox = new Rectangle(hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height + 10);
            //checks if any tile in map.currentMap has collision with the bottom of the otter if so set otter to the tile height            
            var tile = CollisionHelper.OtterGroundHit(groundBox, Map.Instance.FrontMap);
            if (tile == -1) return Vector2.Zero;
            //set the otter position so the otter is on the tile
            return new Vector2(hitbox.X, tile - hitbox.Height - CurrentAnimation.CurrentFrame.HitBox.Y * Scale);
        }


        public override void Draw(Sprites spriteBatch)
        {
            Color color = Color.White;
            if (State is State.Hit) color = Color.Red;
            else if (State is State.Attacking) color = Color.Yellow;
            else if (State is State.Dead) color = Color.Blue;

            spriteBatch.Draw(Game1._hitbox, new Vector2(HitBox.X, HitBox.Y), HitBox, Color.Green, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            Animations.Draw(spriteBatch, Position, IsFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Scale, 0f, color);


        }

        public void SetWalk(bool canWalk)
        {
            _canWalk = canWalk;
        }

        public void Reset()
        {
            Coins = 0;
            Health = MaxHealth = 20;
            Damage = 7;
            State = State.Idle;
            IsAttacking = false;
            IsDead = false;
            IsHit = false;
            _isJumping = false;
            _isRunning = false;
            IsWalking = false;
        }
    }

}
