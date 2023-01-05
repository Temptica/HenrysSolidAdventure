using System.Text;
using Microsoft.Xna.Framework.Input;

namespace HenrySolidAdventure.Controller
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
        internal static bool InventoryInput { get; private set; }
        internal static bool Attack { get; private set; }
        internal static bool Block { get; private set; }
        internal static bool ExitInput { get; private set; }
#if DEBUG
        internal static bool NextInput { get; private set; }
        internal static bool PreviousInput { get; private set; }
        public static bool DeadInput { get; internal set; }
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
            DeadInput = keyboard.IsKeyDown(Keys.OemBackslash) && keyboard.IsKeyDown(Keys.K);

            if (layout == "00000409")//querty
            {
                LeftInput = LeftInput ? LeftInput : keyboard.IsKeyDown(Keys.A);
                Attack = keyboard.IsKeyDown(Keys.Q);
                JumpInput = JumpInput ? JumpInput : keyboard.IsKeyDown(Keys.W);

            }
            else//azerty
            {
                LeftInput = LeftInput ? LeftInput : keyboard.IsKeyDown(Keys.Q);
                Attack = keyboard.IsKeyDown(Keys.A);
                JumpInput = JumpInput ? JumpInput : keyboard.IsKeyDown(Keys.Z);
            }
            //non-layout dependant

            RightInput = RightInput ? RightInput : keyboard.IsKeyDown(Keys.D);
            InteractInput = keyboard.IsKeyDown(Keys.F) || keyboard.IsKeyDown(Keys.Enter);
            DodgeInput = keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down) ;
            Block = keyboard.IsKeyDown(Keys.E);
            ShiftInput = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);
            ExitInput = keyboard.IsKeyDown(Keys.Escape);
            InventoryInput = keyboard.IsKeyDown(Keys.I);
        }
    }
}
