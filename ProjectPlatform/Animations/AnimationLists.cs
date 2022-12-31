using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Characters;
using OtterlyAdventure.Graphics;

namespace OtterlyAdventure.Animations
{
    internal class FrameList<T> : List<T> where T: Frame
    {

        private int _animationIndex;
        public Frame CurrentFrame => this[_animationIndex];

        public void ResetIndex()
        {
            _animationIndex = 0;
        }
    }

    internal class AnimationList<T> : List<T> where T : Animation
    {
        private State _currentAnimation;
        public Animation CurrentAnimation => this.FirstOrDefault(ani => ani.State == _currentAnimation) ?? this.First();
        public void ResetAnimations()
        {
            foreach (var animations in this)
            {
                animations.Frames.ResetIndex();
            }
        }
        public void Update(State state, GameTime gameTime)
        {
            if (_currentAnimation != state)
            {
                CurrentAnimation.Frames.ResetIndex();
                _currentAnimation = state;
            }
            CurrentAnimation.Update(gameTime);
        }

        public void Draw(Sprites sprites, Vector2 position, SpriteEffects effect, float scale, float rotation = 0, Color color = default)
        {
            CurrentAnimation.Draw(sprites, position, effect, scale, rotation, color);
        }
        
    }
}
