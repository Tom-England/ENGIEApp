using System;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XamUiTest
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests : BaseTest
    {
        public Tests(Platform platform) : base(platform)
        {
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
            LoginPage.EnterFirstName(firstName);
            LoginPage.EnterLastName(lastName);
            LoginPage.EnterEmail(email);
            LoginPage.EnterPhone(phone);
            App.DismissKeyboard();
            LoginPage.TapLoginButton();

            App.WaitForElement(x => x.Marked("Continue"));
            App.Tap(x => x.Marked("Continue"));
            // TODO is there a more elegant way to check which page you are on with new page object architecture?
            AppResult[] results = App.WaitForElement(c => c.Marked("Home"));
            App.Screenshot("Home page shown after login");

            //Assert
            Assert.IsTrue(results.Any());
        }

    }
}