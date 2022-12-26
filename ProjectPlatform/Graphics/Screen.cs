using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OtterlyAdventure.Graphics
{
    public sealed class Screen : IDisposable
    {//from https://www.youtube.com/watch?v=yUSB_wAVtE8 and own previous project

        private bool isDisposed;
        private Game game;
        private RenderTarget2D target;
        private bool isSet;

        public int Width
        {
            get { return target.Width; }
        }

        public int Height
        {
            get { return target.Height; }
        }

        public Screen(Game game, int width, int height)
        {

            this.game = game ?? throw new ArgumentNullException("game");

            target = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            isSet = false;
        }

        public void Dispose()
        {
            if(isDisposed)
            {
                return;
            }

            target?.Dispose();
            isDisposed = true;
        }

        public void Set()
        {
            if (isSet)
            {
                throw new Exception("Render target is already set.");
            }

            game.GraphicsDevice.SetRenderTarget(target);
            isSet = true;
        }

        public void UnSet()
        {
            if (!isSet)
            {
                throw new Exception("Render target is not set.");
            }

            game.GraphicsDevice.SetRenderTarget(null);
            isSet = false;
        }

        public void Present(Sprites sprites, bool textureFiltering = true)
        {
            if(sprites is null)
            {
                throw new ArgumentNullException("sprites");
            }
            
            game.GraphicsDevice.Clear(Color.Black);

            Rectangle destinationRectangle = CalculateDestinationRectangle();

            sprites.Begin(null,textureFiltering);
            sprites.Draw(target, null, destinationRectangle, Color.White);
            sprites.End();
        }
        
        internal Rectangle CalculateDestinationRectangle()
        {
            Rectangle backbufferBounds = game.GraphicsDevice.PresentationParameters.Bounds;
            int backbufferAspectRatio = backbufferBounds.Width / backbufferBounds.Height;
            int screenAspectRatio = Width / Height;

            float rx = 0f;
            float ry = 0f;
            float rw = backbufferBounds.Width;
            float rh = backbufferBounds.Height;

            if(backbufferAspectRatio > screenAspectRatio)
            {
                rw = rh * screenAspectRatio;
                rx = (backbufferBounds.Width - rw) / 2f;
            }
            else if(backbufferAspectRatio < screenAspectRatio)
            {
                rh = rw / screenAspectRatio;
                ry = (backbufferBounds.Height - rh) / 2f;
            }
            Rectangle result = new Rectangle((int)rx, (int)ry, (int)rw, (int)rh);
            return result;
        }

        
    }
}
