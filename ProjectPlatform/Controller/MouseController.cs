using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ProjectPlatform.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectPlatform.Controller
{
    internal static class MouseController
    {
        public static Vector2 Position => Mouse.GetState().Position.ToVector2();
        public static bool IsLeftClicked => Mouse.GetState().LeftButton == ButtonState.Pressed;
        public static bool IsRightClicked => Mouse.GetState().RightButton == ButtonState.Pressed;
        public static Vector2 GetScreenPosition(Screen screen)
        {
            Rectangle screenDestination = screen.CalculateDestinationRectangle();
            float sx = Position.X - screenDestination.X;
            float sy = Position.Y - screenDestination.Y;

            sx /= screenDestination.Width;
            sy /= screenDestination.Height;

            sx *= screen.Width;
            sy *= screen.Height;

            return new Vector2(sx, sy);

        }
    }
}
