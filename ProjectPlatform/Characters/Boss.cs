using System;
using HenrySolidAdventure.Animations;
using HenrySolidAdventure.Characters.Traps;
using HenrySolidAdventure.Graphics;
using HenrySolidAdventure.Mapfolder;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace HenrySolidAdventure.Characters
{
    internal class Boss:Enemy
    {//https://luizmelo.itch.io/evil-wizard-2
        public static Texture2D Texture { get; set; }
        private const float Gravity = 0.0001f;
        private readonly HealthBar _healthBar;
        public Trap CurrentAttack { get; private set; }
        private float _meleAttackTimer;
        private float _specialAttackTimer;
        private const float MeleTime = 3000f;
        private const float SpecialAttackTime = 10000f;
        private int _wormHP;
        private int _moveCounter;
        
        public Boss(Vector2 position)
        {
            Position = position;
            var width = Texture.Width / 8;
            var height = Texture.Height / 8;
            Animations = new AnimationList<Animation>()
            {
                new(Texture, State.Attacking, 16, width, height, 0, 0, 8),
                new(Texture, State.Dead, 7, width, height, height * 2, 0, 5),
                new(Texture, State.Idle, 8, width, height, height * 4, 0, 8),
                new(Texture, State.Walking, 8, width, height, height * 6, 0, 10),
                new(Texture, State.Hit, 3, width, height, Texture.Height - height, 0, 4)
            };
            _healthBar = new HealthBar(Position);
            Health = BaseHp = 50;
            _wormHP = 25;
            CanDamage = true;
            Damage = 5;
            _specialAttackTimer = 5000f;
            _rng = new Random();

        }

        private readonly Random _rng;
        public override void Update(GameTime gameTime)
        {
            CanDamage = true;
            int tier = 0;
            if (Health < BaseHp * 0.5f)
            {
                if (_moveCounter < 1)
                {
                    Hero.Instance.Position = Map.Instance.Spawn;
                    MapLoader.ReloadEnemies();
                    _moveCounter++;
                }
                tier = 1;
            }
            if (Health < BaseHp * 0.25f)
            {
                if (_moveCounter < 2 && _moveCounter > 0)
                {
                    Hero.Instance.Position = Map.Instance.Spawn;
                    MapLoader.ReloadEnemies();
                    _moveCounter++;
                }
                tier = 2;
            }
            if (_meleAttackTimer < MeleTime)
            {
                _meleAttackTimer += gameTime.ElapsedGameTime.Milliseconds;
            }
            if (_specialAttackTimer < SpecialAttackTime)
            {
                _specialAttackTimer += gameTime.ElapsedGameTime.Milliseconds * ((tier+1)*0.5f +0.5f);
            }
            if (Health <= 0)
            {
                CurrentAnimation.Update(gameTime);
                if (State != State.Dead) State = State.Dead;
                else if (Animations.CurrentAnimation.IsFinished)
                {
                    Game1.SetState(GameState.Win);
                }
                return;
            }
            base.Update(gameTime);

            if (CurrentAttack is not null)
            {
                CurrentAttack.Update(gameTime);
                if (CurrentAttack.IsFinished)
                {
                    if (CurrentAttack is Worm)
                    {
                        _wormHP = CurrentAttack.Health;
                    }
                    CurrentAttack = null;
                }
            }

            //look at otter
            IsFacingLeft = Hero.Instance.HitBox.Center.X < HitBox.Right;
            _healthBar.SetHealth(HealthPercentage);
            _healthBar.Position = IsFacingLeft ? new Vector2(HitBox.Center.X - 50, Position.Y) : new Vector2(HitBox.Center.X, Position.Y);


            if (IsHit)
            {
                if (State is not State.Hit)
                {
                    State = State.Hit;
                    return;
                }
                
            }

            if (State is State.Hit)
            {
                if (!CurrentAnimation.IsFinished) return;
                IsHit = false;
            }
            if (State is State.Attacking)
            {
                if (!CurrentAnimation.IsFinished) return;
                IsAttacking = false;
                State = State.Idle;
                _meleAttackTimer = 0;
            }

            //if otter is 200 pixels away from boss, use worm attack
            //if otter is 300 pixels away use Magic attack
            //if boss is half HP, use tier 2. if 25% then use tier 3
            
            var distance = Vector2.Distance(Hero.Instance.HitBox.Center.ToVector2(), HitBox.Center.ToVector2());
            IsAttacking = distance < 100 && _meleAttackTimer >= MeleTime;
            
            if (distance < 400 && CurrentAttack is null && _specialAttackTimer >= SpecialAttackTime) //distance smaller than 400 and no attack 
            {
                //position = left side of character and on the ground height of teh boss
                var position = new Vector2(Hero.Instance.HitBox.Left, HitBox.Bottom - Worm.Textures[(TrapTier)tier].Height);
                CurrentAttack = new Worm(position, (TrapTier)tier, IsFacingLeft, _wormHP);
                _specialAttackTimer = 2000f;
            }
            if (CurrentAttack is null && (distance > 400 || (distance <=400 && _rng.NextSingle()>0.90f)) && _specialAttackTimer >= SpecialAttackTime) // (if further than 400 OR if closer than 400 but with a 10 % chance) && time has run out then use an attack
            {
                //check if Hero is on a flat tile
                CurrentAttack = new Magic(Hero.Instance.HitBox.Center.ToVector2(), (TrapTier)tier, IsFacingLeft);
                _specialAttackTimer = 5000f;//takes less energy so less time
            }
            

            if (IsAttacking)
            {
                State = State.Attacking;
            }
        }

        public override void Draw(Sprites sprites, SpriteBatch spriteBatch)
        {
            
            base.Draw(sprites, spriteBatch);
            _healthBar.Draw(sprites);
            CurrentAttack?.Draw(sprites, spriteBatch);
            //todo
        }
        public Vector2 OnGround(Rectangle hitbox)
        {
            var groundBox = new Rectangle(hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height);
            //checks if any tile in map.currentMap has collision with the bottom of the otter if so set otter to the tile height            
            var tile = CollisionHelper.GroundHit(groundBox, Map.Instance.FrontMap);
            if (tile == -1) return Vector2.Zero;
            //set the otter position so the otter is on the tile
            return new Vector2(hitbox.X, tile - hitbox.Height - CurrentAnimation.CurrentFrame.HitBox.Y);
        }
        
        public override bool CheckDamage()
        {
            return State is State.Attacking && Animations.CurrentAnimation.CurrentFrameIndex is > 3 and < 8 or > 12 and < 16;
        }
    }
}
