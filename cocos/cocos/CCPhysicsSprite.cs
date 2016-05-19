﻿using System;
using Box2D.Common;
using Box2D.Dynamics;
using CocosSharp;

namespace GoneBananas
{
    public class CCPhysicsSprite : CCSprite
    {
        readonly float ptmRatio;

        public CCPhysicsSprite(CCTexture2D f, CCRect r, float ptmRatio) : base(f,r)
        {
            this.ptmRatio = ptmRatio;
        }

        public b2Body PhysicsBody { get; set; }

        public void UpdateBallTransform()
        {
            if (PhysicsBody != null)
            {
                b2Vec2 pos = PhysicsBody.Position;

                float x = pos.x * ptmRatio;
                float y = pos.y * ptmRatio;
                
                Position = new CCPoint(x, y);
                Rotation = -PhysicsBody.Angle * 57.3f;
            }
        }
    }
}