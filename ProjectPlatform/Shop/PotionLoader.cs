using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Shop
{
    internal static class PotionLoader
    {
        private static Texture2D _textures;
        private static Dictionary<PotionType, Rectangle> _hitBoxes;
        public const int Width = 120;
        public static void Initialise(ContentManager content)
        {
            _textures = content.Load<Texture2D>("Items/Potions");
            _hitBoxes = new Dictionary<PotionType, Rectangle>();
            for (int i = 0; i < 7; i++)
            {
                _hitBoxes.Add((PotionType)i, new Rectangle(Width * i, 0, Width, Width));
            }
        }

        public static Potion[] MakePotions(int count)
        {
            
            Random rng = new();
            var potions = new Potion[count];
            potions[0] = new Potion(_textures, _hitBoxes[PotionType.Healing], PotionType.Healing);
            for (int i = 1; i < count; i++)
            {
                var value = rng.NextSingle() * 115;
                if (value < 20)
                {
                    potions[i] = new Potion(_textures, _hitBoxes[PotionType.Floating],PotionType.Floating);
                }
                else if (value < 40)
                {
                    potions[i] = new Potion(_textures, _hitBoxes[PotionType.Healing], PotionType.Healing);
                }
                else if (value < 60)
                {
                    potions[i] = new Potion(_textures, _hitBoxes[PotionType.Damage], PotionType.Damage);
                }
                else if (value < 80)
                {
                    potions[i] = new Potion(_textures, _hitBoxes[PotionType.Speed], PotionType.Speed);
                }
                else if (value < 100)
                {
                    potions[i] = new Potion(_textures, _hitBoxes[PotionType.Jump], PotionType.Jump);
                    
                }
                else if (value < 110)
                {
                    potions[i] = new Potion(_textures, _hitBoxes[PotionType.Invis], PotionType.Invis);
                }
                else
                {
                    potions[i] = new Potion(_textures, _hitBoxes[PotionType.Undying], PotionType.Undying);
                }
            }

            return potions;
        }

        public static Potion MakeHealingPotion()
        {
            return new Potion(_textures, _hitBoxes[PotionType.Healing], PotionType.Healing);
        }
    }
}
