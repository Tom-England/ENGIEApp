using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ENGIE_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpPage : ContentPage
    {
        public ObservableCollection<HelpItem> VideoItems { get; set; }
        private int gridSize;
        public HelpPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();

            gridSize = 0;

            VideoItems = new ObservableCollection<HelpItem>();
            VideoItems.Add(new HelpItem { Url = "https://www.youtube.com/embed/q0qkBMuSHXY", Title = "Test video one", Description = "Test video until real ones are made" });
            VideoItems.Add(new HelpItem { Url = "https://www.youtube.com/embed/DULDin5A5Mk", Title = "Test video two", Description = "Second video for testing" });
            VideoItems.Add(new HelpItem { Url = "https://www.youtube.com/embed/Sgc-K-3GhDs", Title = "Test video three", Description = "Third video for testing" });

            foreach (HelpItem item in VideoItems)
            {
                addHelpItemToGrid(item);
                gridSize++;
            }

           
            //helpTable.ItemsSource = VideoItems;
        }

        public void addHelpItemToGrid(HelpItem item)
        {
            HelpGrid.RowDefinitions.Add(new RowDefinition());
            var video = new WebView
            {
                Source = item.Url,
                HorizontalOptions = LayoutOptions.Fill,
                HeightRequest = 150
            };
            var tagStack = new StackLayout { HorizontalOptions = LayoutOptions.Fill };
            var titleLabel = new Label
            {
                FontSize = 15,
                FontFamily = "NormalFont",
                Text = item.Title
            };
            var descLabel = new Label
            {
                FontSize = 10,
                FontFamily = "NormalFont",
                Text = item.Description
            };
            tagStack.Children.Add(titleLabel);
            tagStack.Children.Add(descLabel);
            HelpGrid.Children.Add(video, 0, gridSize);
            HelpGrid.Children.Add(tagStack, 1, gridSize);
        }
    }

    public class HelpItem
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}