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
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern long GetKeyboardLayoutName(
          StringBuilder pwszKLID);
        private static KeyboardState keyboard => Keyboard.GetState();
        internal static bool LeftInput { get; private set; }
        internal static bool RightInput { get; private set; }
        internal static bool DodgeInput { get; private set; }
        internal static bool JumpInput { get; private set; }
        internal static bool ShiftInput { get; private set; }
        internal static bool InteractInput { get; private set; }
        internal static bool Attack { get; private set; }
        internal static bool ExitInput { get; private set; }
#if DEBUG
        internal static bool NextInput { get; private set; }
        internal static bool PreviousInput { get; private set; }
#endif
        internal static void Update()
        {
            StringBuilder name = new StringBuilder(9);
            GetKeyboardLayoutName(name);
            string layout = name.ToString();

            //get arrow inputs first
            LeftInput = keyboard.IsKeyDown(Keys.Left);
            RightInput = keyboard.IsKeyDown(Keys.Right);
            JumpInput = keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.Space);


            if (layout == "00000409")//querty
            {
                LeftInput = LeftInput? LeftInput: keyboard.IsKeyDown(Keys.A);
                Attack = keyboard.IsKeyDown(Keys.Q);
                JumpInput = JumpInput ? JumpInput : keyboard.IsKeyDown(Keys.W);

            }
            else//azerty
            {
                LeftInput = LeftInput ? LeftInput : keyboard.IsKeyDown(Keys.Q);
                Attack = keyboard.IsKeyDown(Keys.A);
                JumpInput = JumpInput ? JumpInput : keyboard.IsKeyDown(Keys.W);
            }
            //non-layout dependant

            RightInput = RightInput ? RightInput : keyboard.IsKeyDown(Keys.D);
            InteractInput = keyboard.IsKeyDown(Keys.E) || keyboard.IsKeyDown(Keys.Enter);
            DodgeInput = DodgeInput ? keyboard.IsKeyDown(Keys.S) : DodgeInput;

            ShiftInput = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);
            InteractInput = keyboard.IsKeyDown(Keys.E) || keyboard.IsKeyDown(Keys.Enter);
            ExitInput = keyboard.IsKeyDown(Keys.Escape);
#if DEBUG
            NextInput = keyboard.IsKeyDown(Keys.N) && keyboard.IsKeyDown(Keys.LeftControl);
            PreviousInput = keyboard.IsKeyDown(Keys.B) && keyboard.IsKeyDown(Keys.LeftControl);
#endif
        }        
    }
}
