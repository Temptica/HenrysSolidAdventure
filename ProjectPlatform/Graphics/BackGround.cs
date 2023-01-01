using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Graphics
{
    internal class BackGround
    {//from same bundle as tileset, decoration and nature
        Texture2D[] Backgrounds;
        float[] scroll;
        float scale;
        float scrollSpeed = 0.05f;


        //singleton
        private static BackGround instance;
        public static BackGround Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BackGround();
                }
                return instance;
            }
        }
        private BackGround()
        {
        }
        public void Initialise(ContentManager content)
        {
            var BackgroundTextures = new Texture2D[3];
            BackgroundTextures[0] = content.Load<Texture2D>("Background/background_layer_1");
            BackgroundTextures[1] = content.Load<Texture2D>("Background/background_layer_2");
            BackgroundTextures[2] = content.Load<Texture2D>("Background/background_layer_3");
            Backgrounds = BackgroundTextures;
            scroll = new float[Backgrounds.Length];
        }

        public void Draw(Sprites spriteBatch, Vector2 ScreenSize = default)
        {
            if (ScreenSize != default)
            {
                scale = ScreenSize.X / Backgrounds[0].Width;
            }
            for (int i = 0; i < Backgrounds.Length; i++)
            {
                spriteBatch.Draw(Backgrounds[i], new Vector2(scroll[i], 0), new Rectangle(0, 0, Backgrounds[i].Width, Backgrounds[i].Height), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);//scrolled background
                spriteBatch.Draw(Backgrounds[i], new Vector2(-Backgrounds[i].Width * scale + scroll[i], 0), new Rectangle(0, 0, Backgrounds[i].Width, Backgrounds[i].Height), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f); //replace another background next to the other one
            }

        }

        public void Update(GameTime time)
        {
            //scroll all backgrounds
            for (int i = 0; i < scroll.Length; i++)
            {
                scroll[i] += (float)(scrollSpeed * time.ElapsedGameTime.TotalMilliseconds) * (i + 1);
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
