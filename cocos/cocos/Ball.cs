using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using CocosSharp;
using GoneBananas;

namespace cocos
{
    class Ball : BodyNode
    {
        const int PTM_RATIO = 32;

        b2World theWorld;

        public Ball(b2World world, CCPoint position, CCTexture2D texture)
        {
            theWorld = world;
            sprite = new CCPhysicsSprite(new CCSprite("ball").Texture, new CCRect(0, 0, 32, 32), PTM_RATIO);
            //sprite.Color = new CCColor3B(0, 255, 0);

            AddChild(sprite);

            CCPoint p = position;

            sprite.Position = new CCPoint(p.X, p.Y);

            var def = new b2BodyDef();
            def.position = new b2Vec2(p.X / PTM_RATIO, p.Y / PTM_RATIO);
            def.linearVelocity = new b2Vec2(10.0f, 0.0f);
            def.type = b2BodyType.b2_dynamicBody;
            body = theWorld.CreateBody(def);
            body.GravityScale = 7f;
            body.UserData = this;

            var triangle = new b2PolygonShape();
            //            circle.Radius = 1;
            triangle.Set(new b2Vec2[3] { new b2Vec2(-.5f, -.5f), new b2Vec2(.5f, -.2f), new b2Vec2(-.2f, .5f)}, 3);

            var fd = new b2FixtureDef();
            fd.shape = triangle;
            fd.density = 2f;
            fd.restitution = 0.3f;
            fd.friction = .5f;
            body.CreateFixture(fd);

            body.ApplyLinearImpulse(new b2Vec2(0f, 10f), body.WorldCenter);
            sprite.PhysicsBody = body;

            AddChild(sprite);
        }

        public void JumpLeft()
        {
            sprite.PhysicsBody.LinearVelocity = new b2Vec2(-16f, 0f);
            sprite.PhysicsBody.ApplyLinearImpulse(new b2Vec2(0f, 10f), sprite.PhysicsBody.WorldCenter);
        }

        public void JumpRight()
        {
            sprite.PhysicsBody.LinearVelocity = new b2Vec2(16f, 0f);
            sprite.PhysicsBody.ApplyLinearImpulse(new b2Vec2(0f, 10f), sprite.PhysicsBody.WorldCenter);
        }

        public void Update()
        {
            sprite.UpdateBallTransform();
        }
    }
}
