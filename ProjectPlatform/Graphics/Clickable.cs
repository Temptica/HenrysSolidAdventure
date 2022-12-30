using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using OtterlyAdventure.Controller;

namespace OtterlyAdventure.Graphics
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
