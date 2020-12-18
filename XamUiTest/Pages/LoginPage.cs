using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace XamUiTest.Pages
{
    public class LoginPage : BasePage
    {
        Query FirstNameText, LastNameText, EmailText, PhoneText, LoginButton;

        public LoginPage(IApp app, string pageTitle) : base(app, pageTitle)
        {
            FirstNameText = x => x.Marked("FirstName");
            LastNameText = x => x.Marked("LastName");
            EmailText = x => x.Marked("Email");
            PhoneText = x => x.Marked("Phone");
            LoginButton = x => x.Marked("Login");
        }

        public void EnterFirstName(string firstName)
        {
            App.Tap(FirstNameText);
            App.EnterText(firstName);
            App.Screenshot("First name entered");
        }

        public void EnterLastName(string lastName)
        {
            App.Tap(LastNameText);
            App.EnterText(lastName);
            App.Screenshot("Last name entered");
        }

        public void EnterEmail(string email)
        {
            App.Tap(EmailText);
            App.EnterText(email);
            App.Screenshot("Email entered");
        }

        public void EnterPhone(string phone)
        {
            App.Tap(PhoneText);
            App.EnterText(phone);
            App.Screenshot("Phone entered");
        }

        public void TapLoginButton()
        {
            App.Tap(LoginButton);
            App.Screenshot("Login button tapped");
        }
    }
}
