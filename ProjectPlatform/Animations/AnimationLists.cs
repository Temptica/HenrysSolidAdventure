using System.Collections.Generic;
using System.Linq;
using HenrySolidAdventure.Characters;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Animations
{
    internal class FrameList<T> : List<T> where T: Frame
    {

        public int AnimationIndex;
        public Frame CurrentFrame => this[AnimationIndex];

        public void ResetIndex()
        {
            AnimationIndex = 0;
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
                animations.IsFinished = false;
            }
        }
        public void Update(State state, GameTime gameTime)
        {
            if (_currentAnimation != state)
            {
                ResetAnimations();
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
