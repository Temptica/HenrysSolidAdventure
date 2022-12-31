using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace OtterlyAdventure.Characters.Traps
{
    internal static class TrapTextureLoader
    {
        public static void LoadTextures(ContentManager content)
        {
            Ceiling.Textures = new Dictionary<TrapTier, Texture2D>
            {
                { TrapTier.One, content.Load<Texture2D>("Enemies/Traps/Ceiling Trap/Ceiling Trap - level 1") },
                { TrapTier.Two, content.Load<Texture2D>("Enemies/Traps/Ceiling Trap/Ceiling Trap - level 2") },
                { TrapTier.Three, content.Load<Texture2D>("Enemies/Traps/Ceiling Trap/Ceiling Trap - level 3") }
            };
            Ceiling.SpawnTextures = new Dictionary<TrapTier, Texture2D>
            {
                {
                    TrapTier.One,
                    content.Load<Texture2D>("Enemies/Traps/Ceiling Trap/Ceiling Trap - level 1 - Transition")
                },
                {
                    TrapTier.Two,
                    content.Load<Texture2D>("Enemies/Traps/Ceiling Trap/Ceiling Trap - level 2 - Transition")
                },
                {
                    TrapTier.Three,
                    content.Load<Texture2D>("Enemies/Traps/Ceiling Trap/Ceiling Trap - level 3 - Transition")
                }
            };
            
            Fire.Textures = new Dictionary<TrapTier, Texture2D>
            {
                { TrapTier.One, content.Load<Texture2D>("Enemies/Traps/Fire Trap/Fire Trap - level 1") },
                { TrapTier.Two, content.Load<Texture2D>("Enemies/Traps/Fire Trap/Fire Trap - level 2") },
                { TrapTier.Three, content.Load<Texture2D>("Enemies/Traps/Fire Trap/Fire Trap - level 3") }
            };
            Fire.SpawnTextures = new Dictionary<TrapTier, Texture2D>
            {
                { TrapTier.One, content.Load<Texture2D>("Enemies/Traps/Fire Trap/Fire Trap - level 1 - Transition") },
                { TrapTier.Two, content.Load<Texture2D>("Enemies/Traps/Fire Trap/Fire Trap - level 2 - Transition") },
                { TrapTier.Three, content.Load<Texture2D>("Enemies/Traps/Fire Trap/Fire Trap - level 3 - Transition") }
            };

            Saw.Textures = new Dictionary<TrapTier, Texture2D>
            {
                { TrapTier.One, content.Load<Texture2D>("Enemies/Traps/Saw Trap/Saw Trap - level 1") },
                { TrapTier.Two, content.Load<Texture2D>("Enemies/Traps/Saw Trap/Saw Trap - level 2") },
                { TrapTier.Three, content.Load<Texture2D>("Enemies/Traps/Saw Trap/Saw Trap - level 3") }
            };
            Saw.SpawnTextures = new Dictionary<TrapTier, Texture2D>
            {
                { TrapTier.One, content.Load<Texture2D>("Enemies/Traps/Saw Trap/Saw Trap - level 1 - Transition") },
                { TrapTier.Two, content.Load<Texture2D>("Enemies/Traps/Saw Trap/Saw Trap - level 2 - Transition") },
                { TrapTier.Three, content.Load<Texture2D>("Enemies/Traps/Saw Trap/Saw Trap - level 3 - Transition") }
            };

            Toxic.Textures = new Dictionary<TrapTier, Texture2D>
            {
                { TrapTier.One, content.Load<Texture2D>("Enemies/Traps/Toxic Trap/Toxic Trap - level 1") },
                { TrapTier.Two, content.Load<Texture2D>("Enemies/Traps/Toxic Trap/Toxic Trap - level 2") },
                { TrapTier.Three, content.Load<Texture2D>("Enemies/Traps/Toxic Trap/Toxic Trap - level 3") }
            };
            Toxic.SpawnTextures = new Dictionary<TrapTier, Texture2D>
            {
                { TrapTier.One, content.Load<Texture2D>("Enemies/Traps/Toxic Trap/Toxic Trap - level 1 - Transition") },
                { TrapTier.Two, content.Load<Texture2D>("Enemies/Traps/Toxic Trap/Toxic Trap - level 2 - Transition") },
                {
                    TrapTier.Three,
                    content.Load<Texture2D>("Enemies/Traps/Toxic Trap/Toxic Trap - level 3 - Transition")
                }
            };
            //worm class with Sandworm as name
            Worm.Textures = new Dictionary<TrapTier, Texture2D>
            {
                { TrapTier.One, content.Load<Texture2D>("Enemies/Traps/Sandworm Trap/Sandworm Trap - level 1") },
                { TrapTier.Two, content.Load<Texture2D>("Enemies/Traps/Sandworm Trap/Sandworm Trap - level 2") },
                { TrapTier.Three, content.Load<Texture2D>("Enemies/Traps/Sandworm Trap/Sandworm Trap - level 3") }
            };
            Worm.SpawnTextures = new Dictionary<TrapTier, Texture2D>
            {
                {
                    TrapTier.One,
                    content.Load<Texture2D>("Enemies/Traps/Sandworm Trap/Sandworm Trap - level 1 - Transition")
                },
                {
                    TrapTier.Two,
                    content.Load<Texture2D>("Enemies/Traps/Sandworm Trap/Sandworm Trap - level 2 - Transition")
                }
            };
        }
    }
}
