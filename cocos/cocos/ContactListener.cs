using System;
using Box2D.Collision;
using Box2D.Dynamics;
using Box2D.Dynamics.Contacts;
using cocos;
using Box2D.Common;

namespace GoneBananas
{
    internal class ContactListener : b2ContactListener
    {
        public ContactListener()
        {
        }

        public override void BeginContact(b2Contact contact)
        {
            b2Body bodyA = contact.FixtureA.Body;
            b2Body bodyB = contact.FixtureB.Body;
            if ((bodyA.UserData is Ball) && (bodyB.UserData is Wall))
            {
                var temp = bodyA;
                bodyA = bodyB;
                bodyB = temp;
            }
            if ((bodyA.UserData is Wall) && (bodyB.UserData is Ball))
            {
                var dir = bodyB.LinearVelocity.x > 0 ? 1 : -1;
                bodyA.ApplyLinearImpulse(new b2Vec2(dir*15.0f, 0.0f), bodyA.WorldCenter);
                bodyB.ApplyLinearImpulse(new b2Vec2(dir*-15.0f, 0.0f), bodyB.WorldCenter);
                ((Ball)bodyB.UserData).Sprite.Color = new CocosSharp.CCColor3B(255, 0, 0);
            }

            base.BeginContact(contact);
        }

        public override void EndContact(b2Contact contact)
        {
            b2Body bodyA = contact.FixtureA.Body;
            b2Body bodyB = contact.FixtureB.Body;
            if ((bodyA.UserData is Ball) && (bodyB.UserData is Wall))
            {
                var temp = bodyA;
                bodyA = bodyB;
                bodyB = temp;
            }
            if ((bodyA.UserData is Wall) && (bodyB.UserData is Ball))
            {
                ((Ball)bodyB.UserData).Sprite.Color = new CocosSharp.CCColor3B(0, 255, 0);
            }

            base.EndContact(contact);
        }

        public override void PostSolve(b2Contact contact, ref b2ContactImpulse impulse)
        {
            //throw new NotImplementedException();
        }

        public override void PreSolve(b2Contact contact, b2Manifold oldManifold)
        {
            //throw new NotImplementedException();
        }
    }
}