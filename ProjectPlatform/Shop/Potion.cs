﻿using System;
using HenrySolidAdventure.Characters.HeroFolder;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using HenrySolidAdventure.Graphics.Clickables;
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
        private readonly EffectManager _effectManager;
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                HitBox = new Rectangle(Position.ToPoint(), new Point(PotionLoader.Width));
            }
        }
        public PotionType Type { get;  }
        public int Cost { get; }
        public string Description { get;}

        public const float Duration = 5000;
        private readonly Texture2D _texture;

        public Potion(Texture2D texture, Rectangle textureHitBox, PotionType type)
        {
            _effectManager = EffectManager.Instance;
            _texture = texture;
            _textureHitBox = textureHitBox;
            Type = type;
            Cost = type switch
            {
                PotionType.Floating => 5,
                PotionType.Healing => 4,
                PotionType.Invis => 8,
                PotionType.Damage => 9,
                PotionType.Jump => 7,
                PotionType.Speed => 6,
                PotionType.Undying => 20,
                _ => 0
            };

            Description = type switch
            {
                PotionType.Floating => "Reduces falling\nspeed",
                PotionType.Healing => "Heals you\nfor 5 HP",
                PotionType.Invis => "Makes you\ninvisible",
                PotionType.Damage => "Adds 3 extra\ndamage",
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
                if (_effectManager.Effects.ContainsKey(Type))
                {
                    _effectManager.Effects[Type] += Duration;
                    return;
                }
                
            }
            
            switch (Type)
            {
                case PotionType.Floating:
                    hero.Gravity -= 0.0002f;
                    _effectManager.Effects.Add(Type, Duration);
                    break;
                case PotionType.Healing:
                    hero.Health += 5;
                    StatsController.AddRegainedHealth(5);
                    DiscordRichPresence.Instance.UpdateHealth();
                    break;
                case PotionType.Invis:
                    hero.IsInvisible = true;
                    _effectManager.Effects.Add(Type, Duration*2);
                    break;
                case PotionType.Damage:
                    hero.Damage += 3;
                    _effectManager.Effects.Add(Type, Duration*2);
                    break;
                case PotionType.Jump:
                    hero.JumpForce += 0.05f;
                    _effectManager.Effects.Add(Type, Duration);
                    break;
                case PotionType.Speed:
                    hero.WalkSpeed += 0.15f;
                    _effectManager.Effects.Add(Type, Duration);
                    break;
                case PotionType.Undying:
                    hero.CanGetDamage = false;
                    _effectManager.Effects.Add(Type, Duration*1.5f);
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
