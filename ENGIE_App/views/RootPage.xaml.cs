using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ENGIE_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootPage : MasterDetailPage
    {
        /// <summary>
        /// Sets behaviour for the menu
        /// </summary>
        public RootPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;
            this.IsGestureEnabled = false;
        }
        public void enableGesture()
        {
            this.IsGestureEnabled = true;
        }
        public void disableGesture()
        {
            this.IsGestureEnabled = false;
        }
    }
}