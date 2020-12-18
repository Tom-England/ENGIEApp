using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;

namespace XamUiTest
{
    public abstract class BaseTest
    {
        Platform platform;

        protected BaseTest(Platform platform)
        {
            this.platform = platform;
        }

        protected IApp App { get; private set; }
        protected Pages.LoginPage LoginPage { get; private set; }

        [SetUp]
        public virtual void TestSetup()
        {
            App = AppInitializer.StartApp(platform);
            LoginPage = new Pages.LoginPage(App, "Welcome");
        }
    }
}
