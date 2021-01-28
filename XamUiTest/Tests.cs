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
        [Category("Login")]
        /* This tests the important EnterValidLoginCredentials method from Pages.LoginPage
         * Not only does this confirm the user can login successfully,
         * but it also tests logic used in many other tests to reduce code duplication
        */
        public void UserCanLogin()
        {
            // Arrange and Act done in LoginPage.EnterValidLoginCredentials
            LoginPage.EnterValidLoginCredentials();
            AppResult[] results = App.WaitForElement(c => c.Marked("HomePage"));
            App.Screenshot("Home page shown after login");

            // Assert
            Assert.IsTrue(results.Any());
        }

        // test for no first name
        [TestCase("", "Bloggs", "joe@bloggs.com", "+447777777777")]
        // test for no surname
        [TestCase("Joe", "", "joe@bloggs.com", "+447777777777")]
        // test for no email
        [TestCase("Joe", "Bloggs", "", "+447777777777")]
        // test for invalid email
        [TestCase("Joe", "Bloggs", "joe", "+447777777777")]
        // test for another invalid email
        [TestCase("Joe", "Bloggs", "joe@blogg", "+447777777777")]
        // test for no phone number
        [TestCase("Joe", "Bloggs", "joe@bloggs.com", "")]
        // test for phone number with invalid characters
        [TestCase("Joe", "Bloggs", "joe@bloggs.com", "+44777abc")]
        [Category("Login")]
        public void IncorrectUserDetailsValidated(string firstName, string lastName, string email, string phone)
        {
            // Arrange
            // done via testcases to avoid repitition

            // Act
            LoginPage.EnterFirstName(firstName);
            LoginPage.EnterLastName(lastName);
            LoginPage.EnterEmail(email);
            LoginPage.EnterPhone(phone);
            LoginPage.TapLoginButton();

            App.WaitForElement(x => x.Marked("Continue"));
            App.Tap(x => x.Marked("Continue"));
            AppResult[] results = App.Query(c => c.Marked("HomePage"));
            App.Screenshot("Home page shown after login");

            // Assert
            // assert false if login is successful, since all include invalid credentials
            Assert.IsFalse(results.Any());
        }

        // test for phone number beginning 07
        [TestCase("Joe", "Bloggs", "joe@bloggs.com", "07777777777")]
        // test for phone number with space
        [TestCase("Joe", "Bloggs", "joe@bloggs.com", "07777 777777")]
        // test for email with . in first segment
        [TestCase("Joe", "Bloggs", "joe.mail@bloggs.com", "+447777777777")]
        [Category("Login")]
        public void CorrectUserDetailsValidated(string firstName, string lastName, string email, string phone)
        {
            // Arrange
            // done via testcases to avoid repitition

            // Act
            LoginPage.EnterFirstName(firstName);
            LoginPage.EnterLastName(lastName);
            LoginPage.EnterEmail(email);
            LoginPage.EnterPhone(phone);
            LoginPage.TapLoginButton();

            App.WaitForElement(x => x.Marked("Continue"));
            App.Tap(x => x.Marked("Continue"));
            AppResult[] results = App.Query(c => c.Marked("HomePage"));
            App.Screenshot("Home page shown after login");

            // Assert
            // assert true if login is successful, since all credentials must have been accepted
            Assert.IsTrue(results.Any());
        }

        [Test]
        [Category("Navigation")]
        public void MenuButtonOpensMenu()
        {
            // Arrange
            // done in LoginPage.EnterValidLoginCredentials to avoid repition of login information

            // Act
            LoginPage.EnterValidLoginCredentials();
            MainPage.TapMenuButton();
            AppResult[] results = App.WaitForElement(c => c.Marked("MenuPage"));
            App.Screenshot("Menu page shown after menu button pressed");

            // Assert
            Assert.IsTrue(results.Any());
        }

        [Test]
        [Category("Navigation")]
        public void ScanPageCanBeAccessed()
        {
            // Arrange
            // done in LoginPage.EnterValidLoginCredentials to avoid repition of login information

            // Act
            LoginPage.EnterValidLoginCredentials();
            MainPage.TapMenuButton();
            MenuPage.TapScanPageButton();
            AppResult[] results = App.WaitForElement(c => c.Marked("ScanPage"));
            App.Screenshot("Scan page shown after scan asset button in menu pressed");

            // Assert
            Assert.IsTrue(results.Any());
        }

        [Test]
        [Category("Navigation")]
        public void RecentFormsPageCanBeAccessed()
        {
            // Arrange
            // done in LoginPage.EnterValidLoginCredentials to avoid repition of login information

            // Act
            LoginPage.EnterValidLoginCredentials();
            MainPage.TapMenuButton();
            MenuPage.TapRecentPageButton();
            AppResult[] results = App.WaitForElement(c => c.Marked("RecentPage"));
            App.Screenshot("Recently scanned forms page shown after recent forms button in menu pressed");

            // Assert
            Assert.IsTrue(results.Any());
        }

        [Test]
        [Category("Navigation")]
        public void HelpPageCanBeAccessed()
        {
            // Arrange
            // done in LoginPage.EnterValidLoginCredentials to avoid repition of login information

            // Act
            LoginPage.EnterValidLoginCredentials();
            MainPage.TapMenuButton();
            MenuPage.TapHelpPageButton();
            AppResult[] results = App.WaitForElement(c => c.Marked("HelpPage"));
            App.Screenshot("Help page shown after help button in menu pressed");

            // Assert
            Assert.IsTrue(results.Any());
        }

        [Test]
        [Category("Navigation")]
        public void LogoutReturnsToLoginPage()
        {
            // Arrange
            // done in LoginPage.EnterValidLoginCredentials to avoid repition of login information

            // Act
            LoginPage.EnterValidLoginCredentials();
            MainPage.TapMenuButton();
            MenuPage.TapLogoutButton();
            AppResult[] results = App.WaitForElement(c => c.Marked("LoginPage"));
            App.Screenshot("Login page shown after logout button in menu pressed");

            // Assert
            Assert.IsTrue(results.Any());
        }

        [Test]
        [Category("Admin")]
        public void AdminCanLogin()
        {
            // Act
            const string username = "admin";
            const string password = "admin";
   
            // Arrange
            LoginPage.TapAdminButton();
            AdminPage.EnterUsername(username);
            AdminPage.EnterPassword(password);
            AdminPage.TapLogin();
            App.WaitForElement(x => x.Marked("Continue"));
            App.Tap(x => x.Marked("Continue"));
            AppResult[] results = App.WaitForElement(c => c.Marked("AdminOptionsPage"));
            App.Screenshot("Admin options page shown after admin logs in");

            // Assert
            Assert.IsTrue(results.Any());
        }
    }
}