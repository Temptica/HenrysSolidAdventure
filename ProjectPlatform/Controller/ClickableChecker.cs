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
        public static Clickable CheckHits(IEnumerable<Clickable> clickables, Screen screen)
        {
            var result = clickables.FirstOrDefault(c => c.CheckHit(screen));
            if (result == default)
            {
                Mouse.SetCursor(MouseCursor.Hand);
                return null;
            }
            Mouse.SetCursor(MouseCursor.Arrow);
            return result;

        }
    }
}
