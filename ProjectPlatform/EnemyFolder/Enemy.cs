using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Animations;
using ProjectPlatform.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectPlatform.Interface;

namespace ProjectPlatform.EnemyFolder
{
    internal abstract class Enemy:IAnimatable, IGameObject
    {
        public static List<Enemy> Enemies;
        internal Enemy()
        {
            Enemies ??= new List<Enemy>();
            Enemies.Add(this);
        }
        public int BaseHp { get; set; }
        public int Damage { get; set; }
        public float CurrentHp { get; set; }
        
        public List<Animation> Animations { get; set; }
        public Animation CurrentAnimation => Animations.First(/*animation => animation.State == State*/);
        public Rectangle HitBox { get; set; }
        public Vector2 Position { get; set; }
        internal State State { get; set; }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(Sprites spriteBatch);
        public abstract void Move(GameTime gameTime);

        public bool GetDamage(float i)
        {
            CurrentHp -= i;
            if (!(CurrentHp < 0)) return false;
            State = State.Dead;
            return true;
        }

        public int Attack()
        {
            return Damage;
        }
    }
}
