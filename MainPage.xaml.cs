namespace DRNews
{
    public partial class MainPage : ContentPage
    {
        private static string selectedCategory = "SenesteNyt";
        private Frame lastSelectedFrame;
        private Color mainColor = Color.FromHex("#1976D2"); // Main color
        private Color selectedColor = Color.FromHex("#FFA500");
        public MainPage()
        {
            InitializeComponent();

        }
    }
}
