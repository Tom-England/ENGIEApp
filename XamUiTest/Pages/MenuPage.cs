using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace XamUiTest.Pages
{
    public class MenuPage : BasePage
    {
        Query HomePageButton, ScanPageButton, RecentPageButton, HelpPageButton, LogoutButton;

        public MenuPage(IApp app, string pageTitle) : base(app, pageTitle)
        {
            HomePageButton = x => x.Marked("Home");
            ScanPageButton = x => x.Marked("Scan Asset");
            RecentPageButton = x => x.Marked("Recently Submitted Forms");
            HelpPageButton = x => x.Marked("Help");
            LogoutButton = x => x.Marked("Logout");
        }

        public void TapHomePageButton()
        {
            App.Tap(HomePageButton);
        }

        public void TapScanPageButton()
        {
            App.Tap(ScanPageButton);
        }
        public void TapRecentPageButton()
        {
            App.Tap(RecentPageButton);
        }

        public void TapHelpPageButton()
        {
            App.Tap(HelpPageButton);
        }

        public void TapLogoutButton()
        {
            App.Tap(LogoutButton);
        }

    }
}
