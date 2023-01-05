using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HenrySolidAdventure.Controller
{
    internal static class ClickableChecker
    {
        public static IClickable CheckHits(IEnumerable<IClickable> clickables)
        {
            var result = clickables.FirstOrDefault(CheckHit);
            if (result == default)
            {
                Mouse.SetCursor(MouseCursor.Arrow);
                return null;
            }
            Mouse.SetCursor(MouseCursor.Hand);
            return result;
            
        }
        public static bool CheckHit(IClickable clickable) //-1 = no hit / 0 hit no click / 1 hit and click
        {
            return clickable.HitBox.Contains(MouseController.GetScreenPosition(Game1._screen));
        }
    }
}
