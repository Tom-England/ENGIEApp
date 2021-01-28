﻿using Android.App;
using Android.Support.V7.App;
using ENGIE_App.Droid;

[Activity(Label = "Mobile App Name", Icon = "@mipmap/icon", Theme = "@style/splashscreen", MainLauncher = false, NoHistory = true)]
public class SplashActivity : AppCompatActivity
{
    protected override void OnResume()
    {
        base.OnResume();
        StartActivity(typeof(MainActivity));
    }
}