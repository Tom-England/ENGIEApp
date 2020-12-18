using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XamUiTest
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void LoginPageIsDisplayed()
        {
            AppResult[] results = app.WaitForElement(x => x.Marked("Login"));
            app.Screenshot("Login page");

            Assert.IsTrue(results.Any());
        }

        [Test]
        public void UserCanLogin()
        {
            //Arrange
            const string firstName = "Joe";
            const string lastName = "Bloggs";
            const string email = "joe@bloggs.com";
            const string phone = "07777777777";

            //Act
            app.WaitForElement(x => x.Marked("Login"));
            app.Tap(x => x.Marked("FirstName"));
            app.EnterText(firstName);
            app.Tap(x => x.Marked("LastName"));
            app.EnterText(lastName);
            app.Tap(x => x.Marked("Email"));
            app.EnterText(email);
            app.Tap(x => x.Marked("Phone"));
            app.EnterText(phone);
            app.Tap(x => x.Marked("Login"));
            app.WaitForElement(x => x.Marked("Continue"));
            app.Tap(x => x.Marked("Continue"));
            AppResult[] results = app.WaitForElement(c => c.Marked("MenuPage"));
            app.Screenshot("Menu");

            //Assert
            Assert.IsTrue(results.Any());
        }

        [Test]
        public void MenuButtonDisplaysMenu()
        {
            app.WaitForElement(x => x.Marked("Login"));
            app.Tap(x => x.Marked("Login"));
            app.WaitForElement(x => x.Marked("Continue"));
            app.Tap(x => x.Marked("Continue"));
            app.WaitForElement(x => x.Marked("MenuButton"));
            app.Tap(x => x.Marked("MenuButton"));

            AppResult[] results = app.WaitForElement(c => c.Marked("MenuPage"));
            app.Screenshot("Menu");

            Assert.IsTrue(results.Any());
        }
            
    }
}
