﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using OtterlyAdventure.Animations;
using OtterlyAdventure.Interface;

namespace OtterlyAdventure.EnemyFolder.Traps
{
    internal abstract class Trap: IAnimatable
    {
        public AnimationList<Animation> Animations { get; set; }
        public Rectangle HitBox { get; set; }
        public Vector2 Position { get; set; }
    }
}
