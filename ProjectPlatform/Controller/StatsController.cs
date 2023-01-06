using System;
using Microsoft.Xna.Framework;

namespace HenrySolidAdventure.Controller
{
    internal class StatsController
    {
        //make signleton

        private static StatsController _uInstance;
        public static StatsController Instance => _uInstance ??= new StatsController();
        private static Stats Stats { get; set; }
        
        private StatsController()
        {
            Reset();
        }

        public static void UpdatePlayTime(GameTime gameTime)
        {
            Stats.PlayTime += gameTime.ElapsedGameTime;
        }

        public static void AddRegainedHealth(int regained)
        {
            Stats.HealthRegained += regained;
        }

        public static void AddHealthLost(int lost)
        {
            Stats.HealthLost += lost;
        }

        public static void AddKill()
        {
            Stats.Kills++;
        }
        
        public static void AddCoin()
        {
            Stats.TotalCoins++;
            Stats.SavedCoins++;
        }

        public static void RemoveCoins(int coins)
        {
            Stats.SavedCoins -= coins;
        }
        public static void AddDamageDealt(int damage)
        {
            Stats.TotalDamage += damage;
        }

        public static void AddBlock()
        {
            Stats.Blocks++;
        }

        public static string GetStats()
        {
            return Stats.ToString();
        }

        public static void Reset()
        {
            Stats = new Stats();
        }
    }
    internal class Stats
    {
        public int TotalCoins { get; set; }
        public int SavedCoins { get; set; }//coins that remain
        public int Kills { get; set; }
        public int HealthLost { get; set; }
        public int HealthRegained { get; set; }
        public TimeSpan PlayTime { get; set; }
        public int TotalDamage { get; set; }
        public int Blocks { get; set; }

        public override string ToString()
        {
            return $"STATS\nTotal Coins:{TotalCoins}\nSaved Coins:{SavedCoins}\nKills:{Kills}\nHealth Lost:\n" +
                   $"{HealthLost}\nHealth Regained:\n{HealthRegained}\nPlay Time:\n" +
                   $"{PlayTime.Minutes + PlayTime.Hours*60}:{PlayTime.Seconds}.{PlayTime.Milliseconds}\n" +
                   $"Total Damage: {TotalDamage}\nBlocks: {Blocks}";
        }
    }
}
