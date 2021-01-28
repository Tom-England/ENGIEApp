using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace XamUiTest.Pages
{
    public class AdminOptionsPage : BasePage
    {
        Query DestinationEmail, SetDesEmailButton, NewAdminPass, NewAdminUser, CreateAdminButton;

        public AdminOptionsPage(IApp app, string pageTitle) : base(app, pageTitle)
        {
            DestinationEmail = x => x.Marked("DestinationEmail");
            SetDesEmailButton = x => x.Marked("SetDesEmailButton");
            NewAdminPass = x => x.Marked("NewAdminPass");
            NewAdminUser = x => x.Marked("NewAdminUser");
            CreateAdminButton = x => x.Marked("CreateAdminButton");
        }

        public void EnterDestinationEmail(string email)
        {
            App.Tap(DestinationEmail);
            App.EnterText(email);
            App.DismissKeyboard();
            App.Screenshot("destination email entered");
        }

        public void EnterNewUsername(string username)
        {
            App.Tap(NewAdminUser);
            App.EnterText(username);
            App.DismissKeyboard();
            App.Screenshot("New admin username entered");
        }

        public void EnterNewPassword(string password)
        {
            App.Tap(NewAdminPass);
            App.EnterText(password);
            App.DismissKeyboard();
            App.Screenshot("New admin password entered");
        }

        public void TapSetDesEmailButton()
        {
            App.Tap(SetDesEmailButton);
            App.Screenshot("button pressed to set destination email");
        }

        public void TapCreateAdminButton()
        {
            App.Tap(CreateAdminButton);
            App.Screenshot("button pressed to create new admin");
        }

       
    }
}
