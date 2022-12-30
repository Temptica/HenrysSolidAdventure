using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Animations;
using OtterlyAdventure.Graphics;
using OtterlyAdventure.OtterFolder;

//https://github.com/roy-t/AStar

namespace OtterlyAdventure.Characters
{
    internal class Bat : TrackingEnemy
    {
        //flying tracking enemy. will start flying to you when you are 15 tiles away, regardless of walls. Once detected, it will keep tracking

        public static Texture2D Texture;
        private float _attackRate; 
        private float _attackTimer;
        public override Rectangle HitBox
        {
            get => new((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }
        public Bat(Vector2 position): base(position,150)
        {
            Velocity = Vector2.Zero;
            Animations = new AnimationList<Animation> { new(Texture, 4, 10) };

            Position = position;

            CurrentHp = BaseHp = 3;
            CanDamage = true;
            Damage = 4;
            Speed = 0.1f;
            _attackRate = 2000f; //0.5 times per second
        }

        private float _timer;
        private float _rotation;
        public override void Update(GameTime gameTime)
        {
            if (IsDead) State = State.Dead;
            if (State is State.Dead)
            {
                if (_timer == 0)
                {
                    Velocity = new Vector2(0, 0.2f * gameTime.ElapsedGameTime.Milliseconds);
                    IsFacingLeft = false;
                }
                if (_timer >= 4000) Remove = true;
                
                Position += Velocity;
                _timer+= gameTime.ElapsedGameTime.Milliseconds;
                //rotate so it goes downwards
                if (_rotation <= (Math.PI / 2))
                {
                    _rotation += 0.01f * gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    _rotation = (float)Math.PI / 2;
                }
                return;
            }
            if (_attackTimer <= _attackRate)
            {
                _attackTimer += gameTime.ElapsedGameTime.Milliseconds;
            }
            else {
                CanDamage = true;
                State = State.Attacking;
            }

            
            base.Update(gameTime);
            Move(gameTime);
        }

        internal override void Chase(Vector2 position)
        {
            var angle = Math.Atan2(position.Y - Position.Y, position.X - Position.X);
            //reduce speed if close by to avoid overshooting
            var tempSpeed = Vector2.Distance(Position, position) < 30 ? 0.05f : 0.1f;
            IsFacingLeft = Velocity.X < 0;
            Velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * tempSpeed;

        }


        public override void Draw(Sprites spriteBatch)
        {
            CurrentAnimation.Draw(spriteBatch, Position, State is State.Dead?SpriteEffects.FlipHorizontally: SpriteEffects.None, 1f,_rotation);
        }

        public override bool CheckDamage()
        {
            return _attackTimer >= _attackRate;
        }
        public virtual void Move(GameTime gameTime)
        {
            
            Position += Velocity*(float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        public override int Attack()
        {
            var attack = base.Attack();
            if (attack > 0) _attackTimer = 0;
            return attack;
        }


    }
}
