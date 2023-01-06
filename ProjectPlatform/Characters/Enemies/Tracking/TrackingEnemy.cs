using HenrySolidAdventure.Characters.HeroFolder;
using Microsoft.Xna.Framework;

namespace HenrySolidAdventure.Characters.Enemies.Tracking
{
    internal abstract class TrackingEnemy : Enemy
    {
        public Vector2 SpawnPosition { get; }
        public float Range { get; }
        public TrackingEnemy(Vector2 spawnPosition, float range)
        {
            SpawnPosition = spawnPosition;
            Range = range;
        }
        public override void Update(GameTime gameTime)
        {

            if (!Hero.Instance.IsInvisible && Vector2.Distance(Hero.Instance.Position, Position) <= Range)
            {
                Chase(Hero.Instance.HitBox.Center.ToVector2());
            }
            else if (Vector2.Distance(Position, SpawnPosition) > 20)
            {
                Chase(SpawnPosition);
            }
            else
            {
                Velocity = Vector2.Zero;
            }
            base.Update(gameTime);
        }

        internal abstract void Chase(Vector2 position);

    }
}
