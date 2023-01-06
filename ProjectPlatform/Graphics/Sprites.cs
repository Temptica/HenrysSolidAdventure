using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Graphics
{
    public sealed class Sprites : IDisposable
    {//from https://www.youtube.com/watch?v=yUSB_wAVtE8 and own previous project
        private bool _isDisposed;
        private readonly Game _game;
        private readonly SpriteBatch _sprites;
        private readonly BasicEffect _effect;
        private FlatTransform _flatTransform;
        public Sprites(Game game)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));

            _isDisposed = false;

            _sprites = new SpriteBatch(game.GraphicsDevice);

            _effect = new BasicEffect(game.GraphicsDevice)
            {
                FogEnabled = false,
                TextureEnabled = true,
                LightingEnabled = false,
                VertexColorEnabled = true
            };
            _flatTransform = new FlatTransform();
        }

        public void Dispose()
        {
            if(_isDisposed)
            {
                return;
            }

            _effect?.Dispose();
            _sprites?.Dispose();
            _isDisposed = true;
        }

        public void Begin(Camera camera, bool isTextureFileteringEnabled)
        {
            if (camera is null)
            {
                var vp = _game.GraphicsDevice.Viewport;
                _effect.Projection = Matrix.CreateOrthographicOffCenter(0, vp.Width, vp.Height,0,  0f, 1f);
                _effect.View = Matrix.Identity;
            }
            else
            {
                camera.UpdateMatrices();
                _flatTransform = new FlatTransform(camera.Position, 0, 1f);
                _effect.View = camera.View;
                _effect.Projection = camera.Projection;
            }
            _sprites.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointWrap, rasterizerState: RasterizerState.CullNone, effect: _effect);
        }

        public void End()
        {
            _sprites.End();
        }

        public void Draw(Texture2D texture, Vector2 origin, Vector2 position, Color color)
        {
            _sprites.Draw(texture, Util.Transform(position, _flatTransform), null, color, 0f, origin, 1f, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, Rectangle? sourceRectangle, Vector2 origin, Vector2 position, float rotation, Vector2 scale, Color color)
        {
            _sprites.Draw(texture, Util.Transform(position, _flatTransform), sourceRectangle, color, rotation, origin, scale, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, Rectangle? sourceRectangle, Rectangle destinationRectangle, Color color)
        {
            _sprites.Draw(texture, destinationRectangle, sourceRectangle, color, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            _sprites.Draw(texture, destinationRectangle, color);
        }

        public void Draw(Texture2D background, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, float layerDepth)
        {
            _sprites.Draw(background, Util.Transform(position, _flatTransform), sourceRectangle, color, rotation, origin, scale, effect, layerDepth);
        }
    }
}
