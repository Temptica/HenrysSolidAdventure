using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Graphics
{
    public sealed class Screen : IDisposable
    {//from https://www.youtube.com/watch?v=yUSB_wAVtE8 and own previous project

        private bool _isDisposed;
        private readonly Game _game;
        private readonly RenderTarget2D _target;
        private bool _isSet;

        public int Width => _target.Width;

        public int Height => _target.Height;

        public Screen(Game game, int width, int height)
        {

            _game = game ?? throw new ArgumentNullException("game");

            _target = new RenderTarget2D(this._game.GraphicsDevice, width, height);
            _isSet = false;
        }

        public void Dispose()
        {
            if(_isDisposed)
            {
                return;
            }

            _target?.Dispose();
            _isDisposed = true;
        }

        public void Set()
        {
            if (_isSet)
            {
                throw new Exception("Render target is already set.");
            }

            _game.GraphicsDevice.SetRenderTarget(_target);
            _isSet = true;
        }

        public void UnSet()
        {
            if (!_isSet)
            {
                throw new Exception("Render target is not set.");
            }

            _game.GraphicsDevice.SetRenderTarget(null);
            _isSet = false;
        }

        public void Present(Sprites sprites, bool textureFiltering = true)
        {
            if(sprites is null)
            {
                throw new ArgumentNullException("sprites");
            }
            
            _game.GraphicsDevice.Clear(Color.Black);

            var destinationRectangle = CalculateDestinationRectangle();

            sprites.Begin(null,textureFiltering);
            sprites.Draw(_target, null, destinationRectangle, Color.White);
            sprites.End();
        }
        
        internal Rectangle CalculateDestinationRectangle()
        {
            var backBufferBounds = _game.GraphicsDevice.PresentationParameters.Bounds;
            var backBufferAspectRatio = backBufferBounds.Width / backBufferBounds.Height;
            var screenAspectRatio = Width / Height;

            var rx = 0f;
            var ry = 0f;
            float rw = backBufferBounds.Width;
            float rh = backBufferBounds.Height;

            if(backBufferAspectRatio > screenAspectRatio)
            {
                rw = rh * screenAspectRatio;
                rx = (backBufferBounds.Width - rw) / 2f;
            }
            else if(backBufferAspectRatio < screenAspectRatio)
            {
                rh = rw / screenAspectRatio;
                ry = (backBufferBounds.Height - rh) / 2f;
            }
            Rectangle result = new ((int)rx, (int)ry, (int)rw, (int)rh);
            return result;
        }

        
    }
}
