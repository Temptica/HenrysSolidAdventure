using HenrySolidAdventure.Controller;
using Microsoft.Xna.Framework;

namespace HenrySolidAdventure.Graphics
{
    internal abstract class Clickable
    {
        public abstract Rectangle HitBox { get; protected set; }
        public bool CheckHit(Screen screen) //-1 = no hit / 0 hit no click / 1 hit and click
        {
            return HitBox.Contains(MouseController.GetScreenPosition(screen));
        }
    }
}
