using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using CocosSharp;
using Microsoft.Xna.Framework;
using GoneBananas;
using System.IO;

namespace GoneBananasAndroid
{
    [Activity(
        Label = "GoneBananas",
        AlwaysRetainTaskState = true,
        Icon = "@drawable/icon",
        Theme = "@android:style/Theme.NoTitleBar",
        ScreenOrientation = ScreenOrientation.Portrait,
        LaunchMode = LaunchMode.SingleInstance,
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)
    ]
    public class MainActivity : AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var application = new CCApplication();
//            application.ApplicationDelegate = new GoneBananasApplicationDelegate();
//            SetContentView(application.AndroidContentView);
//            application.StartGame();

            var gad = new GoneBananasApplicationDelegate();

            // assign the method here to the Action
            gad.BackButton = BackButton;
            gad.LoadText = LoadText;

            application.ApplicationDelegate = gad;
            CCLog.Logger = Console.WriteLine;
            SetContentView(application.AndroidContentView);
            application.StartGame();
        }

        // Called from PCL code
        void BackButton()
        {
            CCLog.Log("We are in the BackButton Action!!!");
            Finish();
            
        }

        public string LoadText(string filename)
        {
            var documentsPath = Android.OS.Environment.ExternalStorageDirectory;
            var filePath = Path.Combine(documentsPath.ToString(), filename);
            return File.ReadAllText(filePath);
        }

    }
}