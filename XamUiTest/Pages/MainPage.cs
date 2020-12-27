using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace XamUiTest.Pages
{
    public class MainPage : BasePage
    {
        Query MenuButton;

        public MainPage(IApp app, string pageTitle) : base(app, pageTitle)
        {
            MenuButton = x => x.Marked("MenuButton");
        }

        public void TapMenuButton()
        {
            App.Tap(MenuButton);
        }


    }
}
