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
        private async void SenesteNytLabel_Tapped(object sender, EventArgs e)
        {
            if (selectedCategory != "SenesteNyt")
            {
                selectedCategory = "SenesteNyt";
                SendMessage(selectedCategory);
            }
            UpdateButtonAppearance(SenesteNytFrame);
        }

        private async void IndlandLabel_Tapped(object sender, EventArgs e)
        {
            if (selectedCategory != "Indland")
            {
                selectedCategory = "Indland";
                SendMessage(selectedCategory);
            }
            UpdateButtonAppearance(IndlandFrame);
        }

        private async void UdlandLabel_Tapped(object sender, EventArgs e)
        {
            if (selectedCategory != "Udland")
            {
                selectedCategory = "Udland";
                SendMessage(selectedCategory);
            }
            UpdateButtonAppearance(UdlandFrame);
        }

        private void SendMessage(string category)
        {
            MessagingCenter.Send(this, "CategorySelected", category);
        }
        private void FilterButton_Clicked(object sender, EventArgs e)
        {
            filterOptions.IsVisible = !filterOptions.IsVisible;
        }

        private void UpdateButtonAppearance(Frame currentFrame)
        {
            lastSelectedFrame.BackgroundColor = mainColor;

            currentFrame.BackgroundColor = selectedColor;
            lastSelectedFrame = currentFrame;
        }
    }
}
