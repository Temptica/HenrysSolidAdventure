using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Animations;

namespace OtterlyAdventure.Characters.Traps
{
    public enum TrapTier{One,Two,Three}
    internal abstract class Trap:Enemy
    {
        public TrapTier Tier { get; set; }
        

    }
}
