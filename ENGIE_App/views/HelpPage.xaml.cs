using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ENGIE_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpPage : ContentPage
    {
        public ObservableCollection<HelpItem> VideoItems { get; set; }
        private int gridSize;

        /// <summary>
        /// Initializer for the help page, disables original navigation options and adds videos to the page
        /// </summary>
        public HelpPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();

            gridSize = 0; // Used to track the last row of the grid so we can add to it easily

            // Stores each HelpItem object
            VideoItems = new ObservableCollection<HelpItem>();
            VideoItems.Add(new HelpItem { Url = "https://www.youtube.com/embed/0MQVlKo0duw", Title = "Tutorial 1", Description = "How to scan asset QR code and submit a form" });
            VideoItems.Add(new HelpItem { Url = "https://www.youtube.com/embed/DULDin5A5Mk", Title = "Test video two", Description = "Second video for testing" });
            VideoItems.Add(new HelpItem { Url = "https://www.youtube.com/embed/Sgc-K-3GhDs", Title = "Test video three", Description = "Third video for testing" });

            // Loops through the help items, adds them to the grid then increments the current last row value
            foreach (HelpItem item in VideoItems)
            {
                addHelpItemToGrid(item);
                gridSize+=2;
            }
        }

        /// <summary>
        ///  Function adds a row to the grid then creates the UI elements,
        ///  inserts the appropriate information from the HelpItem object
        ///  then adds it to the grids children   
        /// </summary>
        /// <param name="item"></param>
        private void addHelpItemToGrid(HelpItem item)
        {
            // Adds a row to the grid
            HelpGrid.RowDefinitions.Add(new RowDefinition());
            
            // Creates UI elements
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
                Text = item.Title,
                TextColor = Color.FromHex("#444")
            };
            var descLabel = new Label
            {
                FontSize = 10,
                FontFamily = "NormalFont",
                Text = item.Description,
                TextColor = Color.FromHex("#444")
            };

            // Adds labels to stacklayout
            tagStack.Children.Add(titleLabel);
            tagStack.Children.Add(descLabel);

            // Adds stacklayout and video to grid
            HelpGrid.Children.Add(video, 0, gridSize);
            HelpGrid.Children.Add(tagStack, 0, gridSize+1);
        }

        /// <summary>
        ///  Function adds a row to the grid then creates the UI elements,
        ///  inserts the appropriate information from the HelpItem object
        ///  then adds it to the grids children   
        /// </summary>
        /// <param name="item"></param>
        private void addHelpItemToGrid(HelpItem item)
        {
            // Adds a row to the grid
            HelpGrid.RowDefinitions.Add(new RowDefinition());
            
            // Creates UI elements
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
                Text = item.Title,
                TextColor = Color.FromHex("#444")
            };
            var descLabel = new Label
            {
                FontSize = 10,
                FontFamily = "NormalFont",
                Text = item.Description,
                TextColor = Color.FromHex("#444")
            };

            // Adds labels to stacklayout
            tagStack.Children.Add(titleLabel);
            tagStack.Children.Add(descLabel);

            // Adds stacklayout and video to grid
            HelpGrid.Children.Add(video, 0, gridSize);
            HelpGrid.Children.Add(tagStack, 0, gridSize+1);
        }
    }

    /// <summary>
    /// Helper class to store information about each grid row
    /// </summary>
    public class HelpItem
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Helper class to store information about each grid row
    /// </summary>
    public class HelpItem
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}