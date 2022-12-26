using System;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OtterlyAdventure.Graphics
{
    public sealed class Sprites : IDisposable
    {
        private bool isDisposed;
        private Game game;
        private SpriteBatch sprites;
        private BasicEffect effect;
        private FlatTransform flatTransform;
        public Sprites(Game game)
        {
            if(game is null)
            {
                throw new ArgumentNullException("game");
            }

            this.game = game;

            isDisposed = false;

            sprites = new SpriteBatch(game.GraphicsDevice);

            effect = new BasicEffect(game.GraphicsDevice);
            effect.FogEnabled = false;
            effect.TextureEnabled = true;
            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;
            flatTransform = new FlatTransform();
        }

        public void Dispose()
        {
            if(isDisposed)
            {
                return;
            }

            effect?.Dispose();
            sprites?.Dispose();
            isDisposed = true;
        }

        public void Begin(Camera camera, bool isTextureFileteringEnabled)
        {
            if (camera is null)
            {
                Viewport vp = game.GraphicsDevice.Viewport;
                effect.Projection = Matrix.CreateOrthographicOffCenter(0, vp.Width, 0, vp.Height, 0f, 1f);
                effect.View = Matrix.Identity;
            }
            else
            {
                camera.UpdateMatrices();
                flatTransform = new FlatTransform(camera.Position, 0, 1f);
                effect.View = camera.View;
                effect.Projection = camera.Projection;
            }
            sprites.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointWrap, rasterizerState: RasterizerState.CullNone, effect: effect);
        }

        public void End()
        {
            sprites.End();
        }

        public void Draw(Texture2D texture, Vector2 origin, Vector2 position, Color color)
        {
            sprites.Draw(texture, Util.Transform(position, flatTransform), null, color, 0f, origin, 1f, SpriteEffects.FlipVertically, 0f);
        }

        public void Draw(Texture2D texture, Rectangle? sourceRectangle, Vector2 origin, Vector2 position, float rotation, Vector2 scale, Color color)
        {
            sprites.Draw(texture, Util.Transform(position, flatTransform), sourceRectangle, color, rotation, origin, scale, SpriteEffects.FlipVertically, 0f);
        }

        public void Draw(Texture2D texture, Rectangle? sourceRectangle, Rectangle destinationRectangle, Color color)
        {
            sprites.Draw(texture, destinationRectangle, sourceRectangle, color, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            sprites.Draw(texture, destinationRectangle, color);
        }

        public void Draw(Texture2D background, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, float layerDepth)
        {
            sprites.Draw(background, Util.Transform(position, flatTransform), sourceRectangle, color, rotation, origin, scale, effect, layerDepth);
        }
    }
}
