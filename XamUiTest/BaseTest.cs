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

        protected IApp App { get; private set; }
        protected Pages.LoginPage Loginpage { get; private set; }

        [SetUp]
        public virtual void TestSetup()
        {
            App = AppInitializer.StartApp(_platform);
            Loginpage = new Pages.LoginPage(App, "Welcome");
        }
    }
}
