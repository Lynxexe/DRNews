using DRNews.Service;

namespace DRNews
{
    public partial class MainPage : ContentPage
    {
        private static string selectedCategory = "SenesteNyt";
        private Frame lastSelectedFrame;
        private Color mainColor = Color.FromHex("#1976D2"); // Main color
        private Color selectedColor = Color.FromHex("#FFA500");
        private static INewsService NewsService;
        public MainPage(INewsService newsService)
        {

            InitializeComponent();
            NewsService = newsService;
            lastSelectedFrame = SenesteNytFrame;
            SenesteNytFrame.BackgroundColor = selectedColor;
        }
        private void Frame_Tapped(object sender, EventArgs e)
        {
            var tappedFrame = sender as Frame;
            UpdateButtonAppearance(tappedFrame);
        }

        private void UpdateButtonAppearance(Frame currentFrame)
        {
            lastSelectedFrame.BackgroundColor = mainColor;

            currentFrame.BackgroundColor = selectedColor;
            lastSelectedFrame = currentFrame;
        }

        private async void FilterButton_Clicked(object sender, EventArgs e)
        {
            filterOptions.IsVisible = !filterOptions.IsVisible;
            filterSearchBar.Text = ""; // Clear text when showing filter options
        }

    }
}
