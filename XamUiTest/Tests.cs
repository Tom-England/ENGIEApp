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

        // test for no first name
        [TestCase("", "Bloggs", "joe@bloggs.com", "07777777777")]
        // test for no surname
        [TestCase("Joe", "", "joe@bloggs.com", "07777777777")]
        // test for no email
        [TestCase("Joe", "Bloggs", "", "07777777777")]
        // test for invalid email
        [TestCase("Joe", "Bloggs", "joe", "07777777777")]
        // test for another invalid email
        [TestCase("Joe", "Bloggs", "joe@blogg", "07777777777")]
        // test for no phone number
        [TestCase("Joe", "Bloggs", "joe@bloggs.com", "")]
        // test for phone number too short
        [TestCase("Joe", "Bloggs", "joe@bloggs.com", "079")]
        // test for phone number with invalid characters
        [TestCase("Joe", "Bloggs", "joe@bloggs.com", "0777abc")]
        public void UserDetailsValidated(string firstName, string lastName, string email, string phone)
        {
            //Arrange
            // done via testcases to avoid repitition

            //Act
            LoginPage.EnterFirstName(firstName);
            LoginPage.EnterLastName(lastName);
            LoginPage.EnterEmail(email);
            LoginPage.EnterPhone(phone);
            App.DismissKeyboard();
            LoginPage.TapLoginButton();

            App.WaitForElement(x => x.Marked("Continue"));
            App.Tap(x => x.Marked("Continue"));
            AppResult[] results = App.WaitForElement(c => c.Marked("Home"));
            App.Screenshot("Home page shown after login");

            //Assert
            // assert false if login is successful, since all include invalid credentials
            Assert.IsFalse(results.Any());
        }

        public void MenuButtonOpensMenu()
        {
            //Act
            LoginPage.EnterValidLoginCredentials();
            MainPage.TapMenuButton();
            // TODO again, this is not an elegant way of testing and needs an update
            AppResult[] results = App.WaitForElement(c => c.Marked("Logout"));
            App.Screenshot("Menu page shown after menu button pressed");

            //Assert
            Assert.IsTrue(results.Any());
        }
    }
}