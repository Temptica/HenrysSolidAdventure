using System.Collections.Generic;
using HenrySolidAdventure.Characters.Enemies.Roaming;
using HenrySolidAdventure.Characters.Enemies.Tracking;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Characters.Enemies
{
    internal static class EnemyInitialiser
    {
        public static void LoadTextures(ContentManager content) //simply loads ion all the enemy textures and Traps
        {
            Bat.Texture = content.Load<Texture2D>("Enemies/Bat");
            Slime.Texture = content.Load<Texture2D>("Enemies/slime-Sheet");
            Dictionary<State, Texture2D> skeletonTextures = new()
            {
                { State.Idle, content.Load<Texture2D>("Enemies/Skeleton/Skeleton Idle") },
                { State.Dead, content.Load<Texture2D>("Enemies/Skeleton/Skeleton Dead") },
                { State.Hit, content.Load<Texture2D>("Enemies/Skeleton/Skeleton Hit") },
                { State.Attacking, content.Load<Texture2D>("Enemies/Skeleton/Skeleton Attack") },
                { State.Walking, content.Load<Texture2D>("Enemies/Skeleton/Skeleton Walk") },
                { State.Other, content.Load<Texture2D>("Enemies/Skeleton/Skeleton React") }
            };
            Skeleton.Textures = skeletonTextures;
            Boss.Texture = content.Load<Texture2D>("Enemies/Wizard_Boss");
            Traps.TrapTextureLoader.LoadTextures(content);
        }
    }
}
