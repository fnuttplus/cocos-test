using System;
using System.Collections.Generic;
using CocosDenshion;
using CocosSharp;
using System.Linq;

using Box2D.Common;
using Box2D.Dynamics;
using Box2D.Collision.Shapes;
using cocos;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Newtonsoft.Json;

namespace GoneBananas
{
    public class GameLayer : CCLayerColor
    {
        const float MONKEY_SPEED = 350.0f;
        const float GAME_DURATION = 60.0f; // game ends after 60 seconds or when the monkey hits a ball, whichever comes first
        const int MAX_NUM_BALLS = 10;

        // point to meter ratio for physics
        const int PTM_RATIO = 32;

        float elapsedTime;
                
        // physics world
        b2World world;
        
        // balls sprite batch

        CCSpriteBatchNode wallsBatch;
        CCTexture2D wallTexture;
        Ball ball;
        CCSprite grass;
        List<Wall> walls;
        CCSprite ballsprite;
        Settings settings;

        public GameLayer ()
        {

            var touchListener = new CCEventListenerTouchAllAtOnce ();
            touchListener.OnTouchesEnded = OnTouchesEnded;

            AddEventListener (touchListener, this);
            Color = new CCColor3B (CCColor4B.White);
            Opacity = 255;

            AddGrass();
            walls = new List<Wall>();

            wallsBatch = new CCSpriteBatchNode("walls", 100);
            wallTexture = wallsBatch.Texture;
            AddChild(wallsBatch, 1, 1);

            ballsprite = new CCSprite("ball");

            StartScheduling();

//            CCSimpleAudioEngine.SharedEngine.PlayBackgroundMusic ("Sounds/backgroundMusic", true);
        }

        void StartScheduling()
        {
            Schedule (t => {
#if DEBUG
                ReadSettings();
#endif
                elapsedTime += t;
                if (ShouldEndGame ()) {
                    EndGame ();
                }
                AddWall ();
            }, .5f);

            Schedule (t => {
                CheckCollision();
                world.Step(t, 8, 1);
                foreach (Wall wall in walls)
                {
                    wall.Update();
                }

                if (ball.Sprite.PhysicsBody.Position.x < 0f)
                {
                    ball.Sprite.PhysicsBody.LinearVelocity = new b2Vec2(5.0f, 0.0f);
                } else if (ball.Sprite.PhysicsBody.Position.x * PTM_RATIO > ContentSize.Width)
                {
                    ball.Sprite.PhysicsBody.LinearVelocity = new b2Vec2(-5.0f, 0.0f);
                }
                ball.Sprite.UpdateBallTransform();

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                {
                    ((GoneBananasApplicationDelegate)Window.Application.ApplicationDelegate).BackButton();
                    // Add your stuff here
                    CCLog.Log("back button pressed");

                }
            });
        }

        void ReadSettings()
        {
            string strSettings = ((GoneBananasApplicationDelegate)Window.Application.ApplicationDelegate).LoadText("settings.txt");
            settings = JsonConvert.DeserializeObject<Settings>(strSettings);
        }

        CCPoint GetRandomPosition (CCSize spriteSize)
        {
            float rnd = CCRandom.Float_0_1 ();
            float randomX = (rnd > 0.5) 
                ? VisibleBoundsWorldspace.Size.Width - spriteSize.Width * 2 
                : spriteSize.Width * 2;

            return new CCPoint (randomX, VisibleBoundsWorldspace.Size.Height - spriteSize.Height / 2);
        }
        
        void CheckCollision ()
        {

        }

        void EndGame ()
        {
            // Stop scheduled events as we transition to game over scene
            UnscheduleAll();

            var gameOverScene = GameOverLayer.SceneWithScore (Window, 0);
            var transitionToGameOver = new CCTransitionMoveInR (0.3f, gameOverScene);

            Director.ReplaceScene (transitionToGameOver);
        }

        void Explode (CCPoint pt)
        {
            var explosion = new CCParticleExplosion(pt); //TODO: manage "better" for performance when "many" particles
            
            explosion.TotalParticles = 10;
            explosion.AutoRemoveOnFinish = true;
            AddChild (explosion);
        }

        bool ShouldEndGame ()
        {
            return false;
            return elapsedTime > GAME_DURATION;
        }

        void OnTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
        {
            CCPoint location = WorldToScreenspace(touches[0].LocationOnScreen);
            if (location.X < ball.Sprite.Position.X) ball.JumpLeft();
            else ball.JumpRight();
            Explode(location);
        }

        protected override void AddedToScene ()
        {
            base.AddedToScene ();
            Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.NoBorder;
            grass.Position = VisibleBoundsWorldspace.Center;

            var b = VisibleBoundsWorldspace;
        }

        void InitPhysics ()
        {
            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            var gravity = new b2Vec2 (0.0f, -10.0f);
            world = new b2World (gravity);

            world.SetAllowSleeping (true);
            world.SetContinuousPhysics (true);

            var def = new b2BodyDef ();
            def.allowSleep = true;
            def.position = b2Vec2.Zero;
            def.type = b2BodyType.b2_staticBody;
            b2Body groundBody = world.CreateBody (def);
            groundBody.SetActive (true);

            b2EdgeShape groundBox = new b2EdgeShape ();
            groundBox.Set (b2Vec2.Zero, new b2Vec2 (s.Width / PTM_RATIO, 0));
            b2FixtureDef fd = new b2FixtureDef ();
            fd.shape = groundBox;
            groundBody.CreateFixture (fd);

            var contactlistener = new ContactListener();
            world.SetContactListener(contactlistener);
        }

        void AddBall()
        {
            ball = new Ball(world, VisibleBoundsWorldspace.Center, wallTexture);
            AddChild(ball);
        }

        void AddWall()
        {
            CCSize size = new CCSize(32f, CCRandom.Next(50,128));
            var wall = new Wall(world, GetRandomPosition(size), wallTexture, size, settings.color);
            walls.Add(wall);
            AddChild(wall);
        }

        void AddGrass()
        {
            grass = new CCSprite("grass");
            AddChild(grass);
        }


        public override void OnEnter ()
        {
            base.OnEnter ();
            ReadSettings();
            InitPhysics();
            AddBall();

        }

        public static CCScene GameScene (CCWindow mainWindow)
        {
            var scene = new CCScene (mainWindow);
            var layer = new GameLayer ();
			
            scene.AddChild (layer);

            return scene;
        }
    }
}