using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace XamUiTest.Pages
{
    public class AdminPage : BasePage
    {
        Query AdminUser, AdminPass, Login;

        public AdminPage(IApp app, string pageTitle) : base(app, pageTitle)
        {
            AdminUser = x => x.Marked("AdminUser");
            AdminPass = x => x.Marked("AdminPass");
            Login = x => x.Marked("Login");
        }

        public void EnterUsername(string username)
        {
            App.Tap(AdminUser);
            App.EnterText(username);
            App.DismissKeyboard();
            App.Screenshot("username entered");
        }

        public void EnterPassword(string password)
        {
            App.Tap(AdminPass);
            App.EnterText(password);
            App.DismissKeyboard();
            App.Screenshot("password entered");
        }

        public void TapLogin()
        {
            App.Tap(Login);
            App.Screenshot("Admin login button tapped");
        }

        public void EnterAdminCredentials(string username, string password)
        { 
            this.EnterUsername(username);
            this.EnterPassword(password);
            this.TapLogin();
            App.WaitForElement(x => x.Marked("Continue"));
            App.Tap(x => x.Marked("Continue"));
            
        }
    }
}
