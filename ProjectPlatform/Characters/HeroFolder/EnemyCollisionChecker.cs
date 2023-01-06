using System.Linq;
using HenrySolidAdventure.Characters.Enemies;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.MapFolder;

namespace HenrySolidAdventure.Characters.HeroFolder;

internal static class EnemyCollisionChecker
{
    private static Hero Hero => Hero.Instance;

    internal static void CheckEnemies()
    {
        if (Map.Instance.Enemies.Count <= 0) return;
        int damage;
        var enemy1 = Map.Instance.Enemies.FirstOrDefault(e => e is Boss);
        if (enemy1 is Boss boss && boss.CurrentAttack is not null && CollisionHelper.PixelBasedHit(Hero, boss.CurrentAttack)) //get the boiss if exists and check hit with the attack if exist
        {
            if (Hero.Attack()) boss.CurrentAttack.GetDamage(Hero.Damage);
            else
            {
                damage = boss.CurrentAttack.Attack();
                if (damage > 0 && Hero.State != State.Hit && Hero.CanGetDamage)
                {
                    Hero.IsHit = true;
                    if (Hero.State is not State.Block && Hero.State is not State.BlockHit) Hero.Health -= damage;
                    DiscordRichPresence.Instance.UpdateHealth();
                }
            }
        }
        foreach (var enemy in Map.Instance.Enemies.Where(enemy => enemy.State is not State.Dead and not State.Hit).Where(enemy => CollisionHelper.PixelBasedHit(Hero, enemy)))
        {
            if (Hero.Attack())
            {
                if (!enemy.GetDamage(Hero.Damage)) { continue; }

                StatsController.Instance.AddKill();
                Hero.Coins += 1;
                var coin = new Coin(enemy.Position);
                Map.Instance.Coins.Add(coin);
                coin.Collect();
                continue;
            }
            damage = enemy.Attack();
            if (damage > 0 && Hero.State != State.Hit && Hero.CanGetDamage)
            {
                Hero.IsHit = true;
                if (Hero.State is not State.Block && Hero.State is not State.BlockHit)
                    Hero.Health -= damage;
                else StatsController.Instance.AddBlock();
                DiscordRichPresence.Instance.UpdateHealth();
            }
        }
    }
}