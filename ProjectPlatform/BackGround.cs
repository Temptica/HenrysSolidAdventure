using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ProjectPlatform
{
    internal class BackGround
    {
        Texture2D[] Backgrounds;
        float[] scroll;
        float scale;
        float scrollSpeed = 0.05f;
        

        public BackGround(Texture2D[] backgrounds)
        {
            Backgrounds = backgrounds;
            scroll = new float[backgrounds.Length];
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 ScreenSize)
        {
            if(scale == 0) scale = ScreenSize.X / Backgrounds[0].Width;
            for (int i = 0; i<Backgrounds.Length; i++)
            {
                spriteBatch.Draw(Backgrounds[i], new Vector2(scroll[i], 0), new Rectangle(0, 0, Backgrounds[i].Width, Backgrounds[i].Height), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);//scrolled background
                spriteBatch.Draw(Backgrounds[i], new Vector2((-Backgrounds[i].Width * scale) + scroll[i], 0), new Rectangle(0, 0, Backgrounds[i].Width, Backgrounds[i].Height), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f); //replace another background next to the other one
            }
            
        }
        
        public void Update(GameTime time)
        {
            //scroll all backgrounds
            for (int i = 0; i < scroll.Length; i++)
            {
                scroll[i] += (float)(scrollSpeed * time.ElapsedGameTime.TotalMilliseconds)*(i+1);
                if (scroll[i] >= Backgrounds[0].Width * scale)
                {
                    scroll[i] -= Backgrounds[0].Width * scale;
                }
            }
        }

        public void Reset()
        {
            scroll = new float[scroll.Length];
        }
    }
}
