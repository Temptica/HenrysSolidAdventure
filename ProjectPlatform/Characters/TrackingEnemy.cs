﻿using Microsoft.Xna.Framework;
using OtterlyAdventure.OtterFolder;

namespace OtterlyAdventure.Characters
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
            
            if (Vector2.Distance(Otter.Instance.Position, Position) <= Range)
            {
                Chase();
            }
            else if(Position != SpawnPosition)
            { 
                Return();
            }
            base.Update(gameTime);
        }

        internal abstract void Chase();

        internal abstract void Return();
    }
}