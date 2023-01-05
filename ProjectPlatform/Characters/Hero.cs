using System;
using System.Linq;
using HenrySolidAdventure.Animations;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using HenrySolidAdventure.Mapfolder;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using HenrySolidAdventure.Shop;

namespace HenrySolidAdventure.Characters
{
    //TODO: Conditionbar, Healthbar, stats upgrades, coin collection
    internal class Hero : Character
    {
        #region Consts
        private const float BaseJumpForce = 0.30f;
        private const float MaxYVelocity = 0.5f;
        private const float BaseWalkSpeed = 0.18f;
        private const float RollingSpeed = 0.22f;
        private const float XAcceleration = 0.03f;
        private const float AttackWaitTime = 1500f;//1.5 seconds
        private const int BaseDamage = 3;
        #endregion
        
        #region properities
        public static Texture2D Texture { get; set; }
        public State State { get; set; }
        public float BaseGravity { get; private set; }
        public float JumpForce { get; set; } = BaseJumpForce;
        public float WalkSpeed { get; set; } = BaseWalkSpeed;
        public bool CanGetDamage { get; set; } 
        public float Gravity { get; set; }
        public float Scale { get; set; }
        public float Coins { get; set; }
        public Inventory Inventory { get; private set; }
        public Dictionary<PotionType, float> Effects { get; set; } //timer per potion
        public bool IsInvisible { get; set; }
        #endregion
        //signleton 
        
        public bool CanWalk;
        
        private static Hero _instance;
        private bool _canAttack = true;
        private bool _canJump;
        private bool _isRolling;
        private bool _isJumping;
        private bool _isFalling;
        private int _health;
        private bool _isBlocking;
        private float _attackTimer;
        private float _lastTextureHeight;
        private bool _canBlock;
        

        public static Hero Instance => _instance ??= new Hero();

        private Hero()//use last texture height to correct position
        {
            Effects = new Dictionary<PotionType, float>();
            Inventory = new Inventory();
        }
        public void Initialise(Texture2D otter, Vector2 position, float gravity, float scale)
        {
            Texture = otter;
            Position = position;
            Gravity = BaseGravity = gravity;
            Scale = scale;
            var width = Texture.Width / 10;
            var height = Texture.Height / 9;
            Animations = new AnimationList<Animation>()
            {
                new(Texture, State.Idle, 8, width, height, 0, 0,6, scale),
                new(Texture, State.Walking, 10, width, height, 0, width*8,10, scale),
                new(Texture, State.Attacking, 6, width, height,height , width*8,10, scale),
                new(Texture, State.Attacking2, 6, width, height, height*2, width*4,8, scale),
                new(Texture, State.Attacking3, 8, width, height, height*3, 0,8, scale),
                new(Texture, State.Jumping, 3, width, height, height*3, width*8,6, scale),
                new(Texture, State.Falling, 3, width, height, height*4, width,6, scale),
                new(Texture, State.Hit, 3, width, height, height*4, width*5,6, scale),
                new(Texture, State.Dead, 8, width, height, height*4, width*8,6, scale),
                new(Texture, State.Block, 8, width, height, height*5, width*8,8, scale),
                new(Texture, State.BlockHit, 5, width, height, height*6, width*6,6, scale),
                new(Texture, State.Rolling, 9, width, height, height*7, width,12, scale),
            };
            Reset();
            _lastTextureHeight = Animations.CurrentAnimation.CurrentFrame.HitBox.Height;
            Damage = BaseDamage;
            CanGetDamage = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Health <= 0 && !IsDead)
            {
                IsDead = true;
                AudioController.Instance.PlayEffect(SoundEffects.Die);
            }
            _isBlocking = InputController.Block && _canBlock;
            _isRolling = InputController.ShiftInput;

            SetState(gameTime);
            
            Animations.Update(State, gameTime);
            if (_lastTextureHeight != Animations.CurrentAnimation.CurrentFrame.HitBox.Height)
            {
                var difference = _lastTextureHeight - Animations.CurrentAnimation.CurrentFrame.HitBox.Height;
                Position = new Vector2(Position.X, Position.Y + difference);
                _lastTextureHeight = Animations.CurrentAnimation.CurrentFrame.HitBox.Height;
            }
            if (State is State.Dead) return;
            MoveUpdate(gameTime, Map.Instance);
            CheckPotions(gameTime);
            CheckCoins();
            Inventory.Update();
            CheckEnemies();
            Velocity.X = 0;
        }

        private void CheckPotions(GameTime gameTime)
        {
            var remove = new List<PotionType>();
            foreach (var potion in Effects)
            {
                Effects[potion.Key] -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (Effects[potion.Key] <= 0)
                {
                    remove.Add(potion.Key);
                }
            }
            foreach (var potion in remove)
            {
                switch (potion)
                {
                    case PotionType.Floating:
                        Gravity = BaseGravity;
                        break;
                    case PotionType.Invis:
                        IsInvisible = false;
                        break;
                    case PotionType.Damage:
                        Damage = BaseDamage;
                        break;
                    case PotionType.Jump:
                        JumpForce = BaseJumpForce;
                        break;
                    case PotionType.Speed:
                        WalkSpeed = BaseWalkSpeed;
                        break;
                    case PotionType.Undying:
                        CanGetDamage = true;
                        break;
                }
                Effects.Remove(potion);
            }
        }

        internal void MenuUpdate(GameTime gameTime, float leftBound, float rightBound, float bottomBound)
        {
            SetState(gameTime);
            Animations.Update(State, gameTime);//update the animation
            MoveUpdate(gameTime, leftBound, rightBound, bottomBound);
            Velocity.X = 0;
        }


        private void SetState(GameTime gameTime)
        {
            _isBlocking = InputController.Block && _canBlock;
            CanWalk = false;
            _canJump = false;
            if (State is State.Dead || IsDead)
            {
                State = State.Dead;
                if (Animations.CurrentAnimation.IsFinished)
                {
                    Game1.SetState(GameState.GameOver);
                }
                return;
            }
            if (_attackTimer < AttackWaitTime && State is not State.Attacking or State.Attacking2 or State.Attacking3)
            {
                _attackTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                _canAttack = false;
            }
            else
            {
                _canAttack = true;
            }
            
            if (State is State.BlockHit)
            {
                if (!Animations.CurrentAnimation.IsFinished)return;
                IsHit = false;
            }
            if (State is State.Hit)
            {
                if (!Animations.CurrentAnimation.IsFinished) return;
                IsHit = false;
            }
            
            switch (IsHit)
            {
                case true when _isBlocking://is hit while blocking
                    State = State.BlockHit;
                    AudioController.Instance.PlayEffect(SoundEffects.ShieldBlock);
                    return;
                case true:
                    State = State.Hit;//is hit while not blocking
                    Random rng = new Random();
                    SoundEffects effect;
                    switch (rng.Next(0,3))
                    {
                        case 0:
                            effect = SoundEffects.Hurt1;
                            break;
                        case 1:
                            effect = SoundEffects.Hurt2;
                            break;
                        default:
                            effect = SoundEffects.Hurt3;
                            break;
                    }
                    AudioController.Instance.PlayEffect(effect);
                    return;
                case false when _isBlocking://is blocking while not being hit
                    State = State.Block;
                    return;
            }

            IsAttacking = _canAttack && InputController.Attack;
            if (State is State.Attacking or State.Attacking2 or State.Attacking3)
            {
                _attackTimer = 0;
                if (!Animations.CurrentAnimation.IsFinished) return;
                switch (State)
                {
                    //if still attacking, go to the next one
                    case State.Attacking when InputController.Attack:
                        State = State.Attacking2;
                        AudioController.Instance.PlayEffect(SoundEffects.Sword);
                        return;
                    case State.Attacking2 when InputController.Attack:
                        State = State.Attacking3;
                        AudioController.Instance.PlayEffect(SoundEffects.Sword);
                        return;
                }
                
                IsAttacking = false;
            }
            
            if (_isFalling)
            {
                State = State.Falling;
                CanWalk = true;
                _canBlock = false;
                return;
            }

            if (_isJumping)
            {
                State = State.Jumping;
                CanWalk = true;
                _canBlock = false;
                return;
            }

            _canBlock = true;
            if (State is State.Rolling)
            {
                CanWalk = true;
                if (!Animations.CurrentAnimation.IsFinished) return;
            }
            if (IsAttacking)
            {
                State = State.Attacking;
                AudioController.Instance.PlayEffect(SoundEffects.Sword);
                return;
            }
            CanWalk = true;
            _canJump = true;
            
            if (_isRolling) State = State.Rolling;
            else if (IsWalking) State = State.Walking;
            else State = State.Idle;
        }
        private void CheckEnemies()
        {
            if (Map.Instance.Enemies.Count <= 0) return;
            int damage;
            var enemy1 = Map.Instance.Enemies.FirstOrDefault(e => e is Boss);
            if (enemy1 is Boss boss && boss.CurrentAttack is not null && CollisionHelper.PixelBasedHit(this, boss.CurrentAttack)) //get the boiss if exists and check hit with the attack if exist
            {
                if (Attack()) boss.CurrentAttack.GetDamage(Damage);
                else
                {
                    damage = boss.CurrentAttack.Attack();
                    if (damage > 0 && State != State.Hit&&CanGetDamage) 
                    { 
                        IsHit = true; 
                        if (State is not State.Block && State is not State.BlockHit) Health -= damage;
                    }
                }
            }
            foreach (var enemy in Map.Instance.Enemies.Where(enemy => enemy.State is not State.Dead and not State.Hit).Where(enemy => CollisionHelper.PixelBasedHit(this, enemy)))
            {
                if (Attack())
                {
                    if (!enemy.GetDamage(Damage)) { continue; }

                    StatsController.Instance.AddKill();
                    Coins += 1;
                    var coin = new Coin(enemy.Position);
                    Map.Instance.Coins.Add(coin);
                    coin.Collect();
                    continue;
                }
                damage = enemy.Attack();
                if (damage > 0 && State != State.Hit && CanGetDamage)
                {
                    IsHit = true;
                    if(State is not State.Block && State is not State.BlockHit)
                        Health -= damage;
                    else StatsController.Instance.AddBlock();
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
        private void MoveUpdate(GameTime gameTime, float leftBound, float rightBound, float bottomBound)//main menu
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
            var nextHitBox = GetHitBox(nextPosition);

            if (Velocity.X > 0 && nextHitBox.Right > rightBound)
            {
                nextHitBox.X = (int)(rightBound - nextHitBox.Width);
            }
            else if (Velocity.X < 0 && nextHitBox.Left < leftBound)
            {
                nextHitBox.X = (int)leftBound;
            }
            if (Velocity.Y > 0 && nextHitBox.Bottom > bottomBound)
            {
                nextHitBox.Y = (int)(bottomBound - nextHitBox.Height);
                Velocity.Y = 0;
                
            }
            else if (Velocity.Y < 0 && nextHitBox.Top < 0)
            {
                nextHitBox.Y = 0;
            }
            _isJumping = Velocity.Y != 0;
            IsWalking = Velocity.X != 0;
            Position = GetPosition(nextHitBox);
        } 
        public void MoveUpdate(GameTime gameTime, Map map)//during game
        {
            GetVelocity(gameTime);

            float velocityXDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Velocity.X);
            var nextHitBox = GetHitBox(new Vector2(Position.X+ velocityXDelta, Position.Y +Velocity.Y));


            if (Velocity.Y < 0)//going up
            {
                _isJumping = true;
                Velocity.Y += (float)(Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
                nextHitBox = new Rectangle(nextHitBox.X, (int)(nextHitBox.Y+ Velocity.Y * (float)gameTime.ElapsedGameTime.TotalMilliseconds), HitBox.Width, HitBox.Height);
                var tile = CollisionHelper.TopHit(nextHitBox, map.FrontMap, IsFacingLeft);
                if (tile != null)
                {
                    Velocity.Y = 0f;
                    _isJumping = false;
                    nextHitBox = new Rectangle(nextHitBox.X, tile.HitBox.Bottom + 3, HitBox.Width, HitBox.Height);
                }
            }

            if (Velocity.X > 0)//to right
            {
                var tile = CollisionHelper.RightHit(nextHitBox, map.FrontMap);
                if (tile is not null)
                {
                    nextHitBox = new Rectangle((int)tile.Position.X - HitBox.Width - 1, (int)nextHitBox.Y, HitBox.Width, HitBox.Height);
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
                var tile = CollisionHelper.LeftHit(nextHitBox, map.FrontMap);
                if (tile is not null)
                {
                    
                    nextHitBox = new Rectangle((int)tile.HitBox.Right + 1, (int)nextHitBox.Y, HitBox.Width, HitBox.Height);
                    Velocity.X = 0;
                }
                else if (CollisionHelper.LeavingLeftMapBorder(nextHitBox, Map.Instance.ScreenRectangle.Left))
                {
                    Velocity.X = 0;
                    nextHitBox = new Rectangle((int)1, (int)nextHitBox.Y, HitBox.Width, HitBox.Height);
                }
            }
            if (Velocity.Y >= 0) //falling mechanism
            {
                var result = OnGround(nextHitBox);
                if (result != Vector2.Zero)
                {
                    Velocity.Y = 0f;
                    _isFalling = false;
                    nextHitBox = new Rectangle((int)result.X, (int)result.Y, nextHitBox.Width, nextHitBox.Height);
                }
                else
                {
                    if (CollisionHelper.LeavingBottomMapBorder(nextHitBox, Map.Instance.ScreenRectangle.Height))
                    {
                        Position = Map.Instance.Spawn;
                        Health -= BaseHp / 4;
                        _isFalling = false;
                        return;
                    }
                    _isFalling = true;
                    var newY = Velocity.Y + (float)(Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
                    Velocity.Y = newY > MaxYVelocity ? MaxYVelocity : newY;

                    nextHitBox = new Rectangle(nextHitBox.X, (int)(nextHitBox.Y + Velocity.Y * (float)gameTime.ElapsedGameTime.TotalMilliseconds), HitBox.Width,
                        HitBox.Height);
                }
            }

            
            _isJumping = Velocity.Y != 0;
            IsWalking = Velocity.X != 0;
            Position = GetPosition(nextHitBox);

        }

        private void GetVelocity(GameTime gameTime)
        {
            if (!CanWalk)
            {
                Velocity.X = 0;
            }
            else if (State is State.Rolling)
            {
                if (IsFacingLeft)
                {
                    Velocity.X -= (float)(XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                    if (Velocity.X < -RollingSpeed)
                        Velocity.X = -RollingSpeed;
                }
                else
                {
                    Velocity.X += (float)(XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                    if (Velocity.X > RollingSpeed)
                        Velocity.X = RollingSpeed;
                }
                
                
            }
            else if (InputController.LeftInput) //left
            {
                Velocity.X -= (float)(XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                if (Velocity.X < -WalkSpeed)
                    Velocity.X = -WalkSpeed;
                _isRolling = false;
                IsFacingLeft = true;
                IsWalking = true;
            }
            else if (InputController.RightInput)//right
            {
                Velocity.X += (float)(XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                if (Velocity.X > WalkSpeed)
                    Velocity.X = WalkSpeed;
                _isRolling = false;
                IsWalking = true;
                IsFacingLeft = false;
            }
            else
            {
                Velocity.X = 0;
                IsWalking = false;
                _isRolling = false;
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
            var groundBox = new Rectangle(hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height+5);
            //checks if any tile in map.currentMap has collision with the bottom of the otter if so set otter to the tile height            
            var tile = CollisionHelper.GroundHit(groundBox, Map.Instance.FrontMap, IsFacingLeft);
            if (tile == -1) return Vector2.Zero;
            //set the otter position so the otter is on the tile
            return new Vector2(hitbox.X, tile - hitbox.Height);
        }


        public override void Draw(Sprites sprites, SpriteBatch spriteBatch)
        {
            Animations.Draw(sprites, Position, IsFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Scale, 0f, Color.White);
            Inventory.Draw(sprites, spriteBatch);

        }

        public void SetWalk(bool canWalk)
        {
            CanWalk = canWalk;
        }

        public void Reset()
        {
            Coins = 0;
            Health = BaseHp = 20;
            Damage = 7;
            State = State.Idle;
            IsAttacking = false;
            IsDead = false;
            IsHit = false;
            _isJumping = false;
            _isRolling = false;
            IsWalking = false;
            Inventory = new Inventory();
        }

        public bool Attack()
        {
            int frameI = Animations.CurrentAnimation.CurrentFrameIndex; //makes code more readable
            switch (State)
            {
                case State.Attacking:
                    return frameI is > 1 and < 5;
                case State.Attacking2:
                    return frameI is > 0 and < 4;
                case State.Attacking3:
                    return frameI is > 0 and < 5;
                default:
                    return false;
            }
        }
    }

}
