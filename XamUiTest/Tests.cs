﻿using System;
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
        /* This tests the important EnterValidLoginCredentials method from Pages.LoginPage
         * Not only does this confirm the user can login successfully,
         * but it also tests logic used in many other tests to reduce code duplication
        */
        public void UserCanLogin()
        {
            // Arrange and Act done in LoginPage.EnterValidLoginCredentials
            LoginPage.EnterValidLoginCredentials();
            AppResult[] results = App.WaitForElement(c => c.Marked("Home"));
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
        // test for phone number too short
        [TestCase("Joe", "Bloggs", "joe@bloggs.com", "+4479")]
        // test for phone number with invalid characters
        [TestCase("Joe", "Bloggs", "joe@bloggs.com", "+44777abc")]
        public void UserDetailsValidated(string firstName, string lastName, string email, string phone)
        {
            // Arrange
            // done via testcases to avoid repitition

            // Act
            LoginPage.EnterFirstName(firstName);
            LoginPage.EnterLastName(lastName);
            LoginPage.EnterEmail(email);
            LoginPage.EnterPhone(phone);
            App.DismissKeyboard();
            LoginPage.TapLoginButton();

            App.WaitForElement(x => x.Marked("Continue"));
            App.Tap(x => x.Marked("Continue"));
            AppResult[] results = App.Query(c => c.Marked("Home"));
            App.Screenshot("Home page shown after login");

            // Assert
            // assert false if login is successful, since all include invalid credentials
            Assert.IsFalse(results.Any());
        }

        [Test]
        public void MenuButtonOpensMenu()
        {
            // Arrange
            // done in LoginPage.EnterValidLoginCredentials to avoid repition of login information

            // Act
            App.Repl();
            LoginPage.EnterValidLoginCredentials();
            MainPage.TapMenuButton();
            // TODO must be a better way to check current page
            AppResult[] results = App.WaitForElement(c => c.Marked("Logout"));
            App.Screenshot("Menu page shown after menu button pressed");

            // Assert
            Assert.IsTrue(results.Any());
        }

        [Test]
        public void ScanPageCanBeAccessed()
        {
            // Arrange
            // done in LoginPage.EnterValidLoginCredentials to avoid repition of login information

            // Act
            LoginPage.EnterValidLoginCredentials();
            MainPage.TapMenuButton();
            MenuPage.TapScanPageButton();
            // TODO also needs overhaul
            //AppResult[] results = App.WaitForElement(c => c.Marked("Logout"));
            App.Screenshot("Scan page shown after scan asset button in menu pressed");

            // Assert
            //Assert.IsTrue(results.Any());
        }
    }
}