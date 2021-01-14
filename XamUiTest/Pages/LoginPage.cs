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
            App.DismissKeyboard();
            App.Screenshot("First name entered");
        }

        public void EnterLastName(string lastName)
        {
            App.Tap(LastNameText);
            App.EnterText(lastName);
            App.DismissKeyboard();
            App.Screenshot("Last name entered");
        }

        public void EnterEmail(string email)
        {
            App.Tap(EmailText);
            App.EnterText(email);
            App.DismissKeyboard();
            App.Screenshot("Email entered");
        }

        public void EnterPhone(string phone)
        {
            App.Tap(PhoneText);
            App.EnterText(phone);
            App.DismissKeyboard();
            App.Screenshot("Phone entered");
        }

        public void TapLoginButton()
        {
            App.Tap(LoginButton);
            App.Screenshot("Login button tapped");
        }

        // method to enter valid login credentials to reduce code duplication in all other tests that would require a login to run
        public void EnterValidLoginCredentials()
        {
            const string firstName = "Joe";
            const string lastName = "Bloggs";
            const string email = "joe@bloggs.com";
            const string phone = "+447777777777";

            this.EnterFirstName(firstName);
            this.EnterLastName(lastName);
            this.EnterEmail(email);
            this.EnterPhone(phone);
            this.TapLoginButton();
            App.WaitForElement(x => x.Marked("Continue"));
            App.Tap(x => x.Marked("Continue"));
            App.WaitForElement(c => c.Marked("Home"));
        }
    }
}
