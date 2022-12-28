using Microsoft.Xna.Framework;
using OtterlyAdventure.Animations;

namespace OtterlyAdventure.Characters.Traps
{
    internal abstract class Trap: IAnimatable
    {
        public AnimationList<Animation> Animations { get; set; }
        public Rectangle HitBox { get; set; }
        public Vector2 Position { get; set; }
    }
}
