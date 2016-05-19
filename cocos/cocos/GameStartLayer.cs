using System;
using System.Collections.Generic;
using CocosSharp;

namespace GoneBananas
{
    public class GameStartLayer : CCLayerColor
    {
        public GameStartLayer() : base()
        {
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = (touches, ccevent) => Window.DefaultDirector.ReplaceScene(GameLayer.GameScene(Window));
            AddEventListener(touchListener, this);

            Color = new CCColor3B(50,50,50);
            Opacity = 255;
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            string hi = ((GoneBananasApplicationDelegate)Window.Application.ApplicationDelegate).LoadText("hi.txt");
            var label = new CCLabel($"Hello, {hi}!", "arial", 22)
            {
                Position = VisibleBoundsWorldspace.Center,
                Color = CCColor3B.Green,
                HorizontalAlignment = CCTextAlignment.Center,
                VerticalAlignment = CCVerticalTextAlignment.Center,
                AnchorPoint = CCPoint.AnchorMiddle
            };

            AddChild(label);
        }

        public static CCScene GameStartLayerScene(CCWindow mainWindow)
        {
            var scene = new CCScene(mainWindow);
            var layer = new GameStartLayer();

            scene.AddChild(layer);

            return scene;
        }
    }
}
