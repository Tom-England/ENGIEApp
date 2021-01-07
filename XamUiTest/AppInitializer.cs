using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XamUiTest
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp.Android.InstalledApp("com.companyname.engie_app").StartApp();
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}
