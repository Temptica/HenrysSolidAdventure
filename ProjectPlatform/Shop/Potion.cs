﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HenrySolidAdventure.Characters;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Shop
{
    public enum PotionType{Floating,Healing,Invis,Damage,Jump,Speed,Undying}
    internal class Potion: IClickable
    {
        public Rectangle HitBox { get; set; }
        private readonly Rectangle _textureHitBox;
        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                HitBox = new Rectangle(Position.ToPoint(), new Point(PotionLoader.Width));
            }
        }
        public PotionType Type { get; private set; }
        public int Cost { get; private set; }
        public string Description { get; private set; }

        public const float Duration = 5000;
        private readonly Texture2D _texture;

        public Potion(Texture2D texture, Rectangle textureHitBox, PotionType type)
        {
            _texture = texture;
            _textureHitBox = textureHitBox;
            Type = type;
            Cost = type switch
            {
                PotionType.Floating => 4,
                PotionType.Healing => 5,
                PotionType.Invis => 12,
                PotionType.Damage => 10,
                PotionType.Jump => 8,
                PotionType.Speed => 7,
                PotionType.Undying => 25,
                _ => 0
            };

            Description = type switch
            {
                PotionType.Floating => "Reduces falling\nspeed",
                PotionType.Healing => "Heals you 5HP",
                PotionType.Invis => "Makes you\ninvisible",
                PotionType.Damage => "Adds 5 extra\ndamage",
                PotionType.Jump => "Increases jump\nforce",
                PotionType.Speed => "Increases speed",
                PotionType.Undying => "Makes you\ninvincible",
                _ => "none"
            };
        }

        public void Use()
        {
            var hero = Hero.Instance;
            
            if (Type is not PotionType.Healing)
            {
                if (hero.Effects.ContainsKey(Type))
                {
                    hero.Effects[Type] += Duration;
                    return;
                }
                
            }
            
            switch (Type)
            {
                case PotionType.Floating:
                    hero.Gravity -= 0.0002f;
                    hero.Effects.Add(Type, Duration);
                    break;
                case PotionType.Healing:
                    hero.Health += 5;
                    StatsController.Instance.AddRegainedHealth(5);
                    break;
                case PotionType.Invis:
                    hero.IsInvisible = true;
                    hero.Effects.Add(Type, Duration*2);
                    break;
                case PotionType.Damage:
                    hero.Damage += 3;
                    hero.Effects.Add(Type, Duration*2);
                    break;
                case PotionType.Jump:
                    hero.JumpForce += 0.05f;
                    hero.Effects.Add(Type, Duration);
                    break;
                case PotionType.Speed:
                    hero.WalkSpeed += 0.2f;
                    hero.Effects.Add(Type, Duration);
                    break;
                case PotionType.Undying:
                    hero.CanGetDamage = false;
                    hero.Effects.Add(Type, Duration*1.5f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public void Draw(Sprites sprites)
        {
            sprites.Draw(_texture, _textureHitBox, Vector2.One, Position, 0f,Vector2.One, Color.White);
        }

        
    }
}