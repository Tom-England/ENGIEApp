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
            LoginPage.TapLoginButton();

            LoginPage.WaitForPageToLoad();

            app.WaitForElement(x => x.Marked("Continue"));
            app.Tap(x => x.Marked("Continue"));
            AppResult[] results = app.WaitForElement(c => c.Marked("MenuPage"));
            app.Screenshot("Menu");



            //Assert
            Assert.IsTrue(results.Any());
        }

    }
}