using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;

namespace XamUiTest
{
    public abstract class BasePage
    {
        private IAppDomainSetup app;
        private string pageTitle;

        protected BasePage(IApp app, string pageTitle)
        {
            App = app;
            Title = pageTitle;
        }

        protected BasePage(IAppDomainSetup app, string pageTitle)
        {
            this.app = app;
            this.pageTitle = pageTitle;
        }

        public string Title { get; }
        protected IApp App { get; }

        public virtual void WaitForPageToLoad() => App.WaitForElement(Title);
    }
}
