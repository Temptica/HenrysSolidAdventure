using System.Collections.Generic;
using System.Linq;
using HenrySolidAdventure.Graphics.Clickables;
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
                Mouse.SetCursor(MouseCursor.Arrow);//automatically set cursor accordingly
                return null;
            }
            Mouse.SetCursor(MouseCursor.Hand);
            return result;
            
        }
        public static bool CheckHit(IClickable clickable)
        {
            return clickable.HitBox.Contains(MouseController.GetScreenPosition(Game1.Screen));
        }
    }
}
