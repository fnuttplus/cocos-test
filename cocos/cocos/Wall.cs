using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using CocosSharp;
using GoneBananas;

namespace cocos
{
    class Wall : BodyNode
    {
        const float PTM_RATIO = 32;

        b2World theWorld;

        public Wall(b2World world, CCPoint position, CCTexture2D texture, CCSize size, Settings.Color color)
        {
            theWorld = world;
            int idx = (CCRandom.Float_0_1() > .5 ? 0 : 1);
            int idy = (CCRandom.Float_0_1() > .5 ? 0 : 1);
            sprite = new CCPhysicsSprite(null, new CCRect(32 * idx, 64 * idy, 32, 64), PTM_RATIO);
            sprite.Color = new CCColor3B((byte)color.r, (byte)color.g, (byte)color.b);
            sprite.ContentSize = size;

            AddChild(sprite);

            CCPoint p = position;

            sprite.Position = new CCPoint(p.X, p.Y);

            var def = new b2BodyDef();
            def.position = new b2Vec2(p.X / PTM_RATIO, p.Y / PTM_RATIO);
            def.linearVelocity = new b2Vec2(0.0f, -1.0f);
            def.type = b2BodyType.b2_dynamicBody;
            body = theWorld.CreateBody(def);
            body.UserData = this;

            var circle = new b2PolygonShape();
            circle.SetAsBox(size.Width/(PTM_RATIO*2), size.Height/(2*PTM_RATIO));

            var fd = new b2FixtureDef();
            fd.shape = circle;
            fd.density = 3f;
            fd.restitution = 0.3f;
            fd.friction = .5f;
            body.CreateFixture(fd);

            sprite.PhysicsBody = body;
        }

        public void Update()
        {
            sprite.UpdateBallTransform();
        }


    }
}
