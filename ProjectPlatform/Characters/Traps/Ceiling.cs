using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace OtterlyAdventure.Characters.Traps
{
    internal class Ceiling:Trap
    {
        public static Dictionary<TrapTier, Texture2D> Textures { get; set; }
        public static Dictionary<TrapTier, Texture2D> SpawnTextures { get; set; }

        public override bool CheckDamage()
        {
            throw new NotImplementedException();
        }
    }
}
