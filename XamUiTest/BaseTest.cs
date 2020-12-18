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
        readonly Platform _platform;

        protected BaseTest(Platform platform) => _platform = platform;

        protected IApp app { get; private set; }
        protected Pages.LoginPage LoginPage { get; private set; }

        [SetUp]
        public virtual void TestSetup()
        {
            app = AppInitializer.StartApp(_platform);
            LoginPage = new Pages.LoginPage(app, "Welcome");
        }
    }
}
