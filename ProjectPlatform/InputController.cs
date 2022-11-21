using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace ProjectPlatform
{
    internal static class InputController
    {
        private static KeyboardState keyboard => Keyboard.GetState();
        internal static bool LeftInput { get; private set; }
        internal static bool RightInput { get; private set; }
        internal static bool JumpInput { get; private set; }
        internal static bool ShiftInput { get; private set; }
        internal static bool InteractInput { get; private set; }
        internal static bool ExitInput { get; private set; }
        internal static void Update()
        {
            LeftInput = keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.Q);
            RightInput = keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D);
            JumpInput = keyboard.IsKeyDown(Keys.Space) || keyboard.IsKeyDown(Keys.Up);
            ShiftInput = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);
            InteractInput = keyboard.IsKeyDown(Keys.E) || keyboard.IsKeyDown(Keys.Enter);
            ExitInput = keyboard.IsKeyDown(Keys.Escape);

        }
        
    }
}
