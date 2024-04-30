using DRNews.Components.Pages;
using DRNews.Service;
using Microsoft.AspNetCore.Components;

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
        private void SenesteNytLabel_Tapped(object sender, EventArgs e)
        {
            if (selectedCategory != "SenesteNyt")
            {
                selectedCategory = "SenesteNyt";

            }

            UpdateButtonAppearance(SenesteNytFrame);
        }

        private void IndlandLabel_Tapped(object sender, EventArgs e)
        {
            if (selectedCategory != "Indland")
            {
                selectedCategory = "Indland";
            }
            UpdateButtonAppearance(IndlandFrame);

        }

        private void UdlandLabel_Tapped(object sender, EventArgs e)
        {
            if (selectedCategory != "Udland")
            {
                selectedCategory = "Udland";
            }
            UpdateButtonAppearance(UdlandFrame);
        }
        //private void Frame_Tapped(object sender, EventArgs e)
        //{
        //    var tappedFrame = sender as Frame;
        //    UpdateButtonAppearance(tappedFrame);
        //}
        private void UpdateButtonAppearance(Frame currentFrame)
        {
            lastSelectedFrame.BackgroundColor = mainColor;

            currentFrame.BackgroundColor = selectedColor;
            lastSelectedFrame = currentFrame;
        }
    }
}
