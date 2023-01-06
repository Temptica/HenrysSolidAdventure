using System;
using Microsoft.Xna.Framework;

namespace HenrySolidAdventure.Graphics
{
    public struct FlatTransform
    {//from https://www.youtube.com/watch?v=yUSB_wAVtE8 and own previous project
        public float PosX;
        public float PosY;
        public float CosScaleX;
        public float SinScaleX;
        public float CosScaleY;
        public float SinScaleY;

        public FlatTransform(Vector2 position, float angle, float scale) //helps to put items compared to the camera position on and off the screen
        {
            float sin = MathF.Sin(angle);
            float cos = MathF.Cos(angle);

            PosX = position.X;
            PosY = position.Y;
            CosScaleX = cos * scale;
            SinScaleX = sin * scale;
            CosScaleY = cos * scale;
            SinScaleY = sin * scale;
        }
    }
}
