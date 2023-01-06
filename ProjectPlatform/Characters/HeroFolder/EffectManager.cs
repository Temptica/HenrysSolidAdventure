using System.Collections.Generic;
using HenrySolidAdventure.Shop;
using Microsoft.Xna.Framework;

namespace HenrySolidAdventure.Characters.HeroFolder;

internal class EffectManager
{
    //singleton
    private static EffectManager _instance;
    public static EffectManager Instance => _instance ??= new EffectManager();

    private Hero Hero => Hero.Instance;
    public Dictionary<PotionType, float> Effects { get; set; } //timer per potion

    private EffectManager()
    {
        Effects = new Dictionary<PotionType, float>();
    }
    internal void CheckPotions(GameTime gameTime)
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
                    Hero.Gravity = Hero.BaseGravity;
                    break;
                case PotionType.Invis:
                    Hero.IsInvisible = false;
                    break;
                case PotionType.Damage:
                    Hero.Damage = Hero.BaseDamage;
                    break;
                case PotionType.Jump:
                    Hero.JumpForce = Hero.BaseJumpForce;
                    break;
                case PotionType.Speed:
                    Hero.WalkSpeed = Hero.BaseWalkSpeed;
                    break;
                case PotionType.Undying:
                    Hero.CanGetDamage = true;
                    break;
            }

            Effects.Remove(potion);
        }
    }
}