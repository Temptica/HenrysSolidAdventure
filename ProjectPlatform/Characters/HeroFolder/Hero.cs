using System;
using System.Linq;
using HenrySolidAdventure.Animations;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using HenrySolidAdventure.MapFolder;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using HenrySolidAdventure.Shop;
using HenrySolidAdventure.Characters.Enemies;

namespace HenrySolidAdventure.Characters.HeroFolder
{
    //TODO: Conditionbar, Healthbar, stats upgrades, coin collection
    internal class Hero : Character
    {
        #region Consts

        public const float BaseJumpForce = 0.30f;
        public const float MaxYVelocity = 0.5f;
        public const float BaseWalkSpeed = 0.18f;
        public const float RollingSpeed = 0.25f;
        public const float XAcceleration = 0.03f;
        private const float AttackWaitTime = 1500f;//1.5 seconds
        public const int BaseDamage = 3;
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
        public bool IsInvisible { get; set; }
        public bool CanJump { get; set; }
        public bool IsRolling { get; set; }
        public bool IsJumping { get; set; }
        public bool IsFalling { get; set; }
        #endregion

        #region fields
        //it's a field trip ;)
        public bool CanWalk;

        private static Hero _instance;
        private bool _canAttack = true;
        private bool _isBlocking;
        private float _attackTimer;
        private float _lastTextureHeight;
        private bool _canBlock;
        private readonly EffectManager _effectManager; //manages the potion's effects + duration

        #endregion

        public static Hero Instance => _instance ??= new Hero();

        private Hero()//use last texture height to correct position
        {
            Inventory = new Inventory();
            _effectManager = EffectManager.Instance;
            CanWalk = true;
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
                new(Texture, State.Rolling, 9, width, height, height*7, width,18, scale),
            };
            Reset(); //making sure we have no unwanted values
            _lastTextureHeight = Animations.CurrentAnimation.CurrentFrame.HitBox.Height;
            Damage = BaseDamage;
            CanGetDamage = true;
        }

        public override void Update(GameTime gameTime) //update while playing the levels
        {
            if (Health <= 0 && !IsDead)
            {
                IsDead = true;
                AudioController.Instance.PlayEffect(SoundEffects.Die);
            }
            _isBlocking = InputController.Block && _canBlock;
            IsRolling = InputController.ShiftInput;

            SetState(gameTime);

            Animations.Update(State, gameTime);
            if (_lastTextureHeight != Animations.CurrentAnimation.CurrentFrame.HitBox.Height)
            {
                var difference = _lastTextureHeight - Animations.CurrentAnimation.CurrentFrame.HitBox.Height;
                Position = new Vector2(Position.X, Position.Y + difference);
                _lastTextureHeight = Animations.CurrentAnimation.CurrentFrame.HitBox.Height;
            }
            if (State is State.Dead) return;
            HeroMovement.MoveUpdate(gameTime, Map.Instance);
            _effectManager.CheckPotions(gameTime);
            CheckCoins();
            Inventory.Update();
            EnemyCollisionChecker.CheckEnemies();
            Velocity.X = 0;
        }
        internal void MenuUpdate(GameTime gameTime, float leftBound, float rightBound, float bottomBound) //update when in menu to let you play on top of the button
        {
            SetState(gameTime);
            Animations.Update(State, gameTime);//update the animation
            HeroMovement.MoveUpdate(gameTime, leftBound, rightBound, bottomBound);
            Velocity.X = 0;
        }

        private void SetState(GameTime gameTime)
        {
            _isBlocking = InputController.Block && _canBlock;
            CanWalk = false;
            CanJump = false;
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
                if (!Animations.CurrentAnimation.IsFinished) return;
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
                    Random rng = new ();
                    SoundEffects effect = rng.Next(0, 3) switch
                    {
                        0 => SoundEffects.Hurt1,
                        1 => SoundEffects.Hurt2,
                        _ => SoundEffects.Hurt3
                    };
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

            if (IsFalling)
            {
                State = State.Falling;
                CanWalk = true;
                _canBlock = false;
                return;
            }

            if (IsJumping)
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
            CanJump = true;

            if (IsRolling) State = State.Rolling;
            else if (IsWalking) State = State.Walking;
            else State = State.Idle;
        }

        private void CheckCoins()
        {
            var collected = Map.Instance.Coins?.FirstOrDefault(coin => coin.HitBox.Intersects(HitBox))?.Collect();
            if (collected == true)
            {
                Coins++;
            }
        }

        public override void Draw(Sprites sprites, SpriteBatch spriteBatch)
        {
            Animations.Draw(sprites, Position, IsFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Scale, 0f, Color.White);
            Inventory.Draw(sprites, spriteBatch);

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
            IsJumping = false;
            IsRolling = false;
            IsWalking = false;
            Inventory = new Inventory();
        }

        public bool Attack()
        {
            int frameI = Animations.CurrentAnimation.CurrentFrameIndex; //makes code more readable
            return State switch
            {
                State.Attacking => frameI is > 1 and < 5,
                State.Attacking2 => frameI is > 0 and < 4,
                State.Attacking3 => frameI is > 0 and < 5,
                _ => false
            };
        }
    }

}
