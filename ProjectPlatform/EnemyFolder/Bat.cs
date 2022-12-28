using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Animations;
using OtterlyAdventure.Graphics;
using OtterlyAdventure.Interface;
using OtterlyAdventure.PathFinding;
using OtterlyAdventure.Mapfolder;
using OtterlyAdventure.OtterFolder;

//https://github.com/roy-t/AStar

namespace OtterlyAdventure.EnemyFolder
{
    internal class Bat : TrackingEnemy
    {
        //flying tracking enemy. will start flying to you when you are 15 tiles away, regardless of walls. Once detected, it will keep tracking

        public static Texture2D Texture;
        private Vector2 _velocity;
        private float _attackRate; 
        private float _attackTimer;
        public override Rectangle HitBox
        {
            get => new((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            set => throw new NotSupportedException();
        }
        public Bat(Vector2 position): base(position,150)
        {
            _velocity = Vector2.Zero;
            Animations = new List<Animation> { new(Texture, 4, 10) };

            Position = position;

            CurrentHp = BaseHp = 3;
            CanDamage = true;
            Damage = 4;
            Speed = 0.1f;
            _attackRate = 0.5f / 1000f; //0.5 times per second
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
                    _velocity = new Vector2(0, 0.2f * gameTime.ElapsedGameTime.Milliseconds);
                    IsFacingLeft = false;
                }
                if (_timer >= 4000) Remove = true;
                
                Position += _velocity;
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
            base.Update(gameTime);
            CurrentAnimation.Update(gameTime);
            Move(gameTime);
        }

        internal override void Chase()
        {
            var otterPos = Otter.Instance.Position;
            var angle = Math.Atan2(otterPos.Y - Position.Y, otterPos.X - Position.X);
            _velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Speed;

        }

        internal override void Return()
        {
            var spawnPos = SpawnPosition;
            var angle = Math.Atan2(spawnPos.Y - Position.Y, spawnPos.X - Position.X);
            _velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Speed;
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
            _velocity = new Vector2(_velocity.X * gameTime.ElapsedGameTime.Milliseconds, _velocity.Y *gameTime.ElapsedGameTime.Milliseconds);
            Position += _velocity;
        }
        public override int Attack()
        {
            var attack = base.Attack();
            if (attack > 0) _attackTimer = 0;
            return attack;
        }


    }
}
