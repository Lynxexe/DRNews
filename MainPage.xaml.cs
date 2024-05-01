using DRNews.Components.Pages;
using DRNews.Service;
using DRNews.Model;
using Microsoft.AspNetCore.Components;

namespace DRNews
{
    public partial class MainPage : ContentPage
    {
        private static string selectedCategory = "SenesteNyt";
        private Frame lastSelectedFrame;
        private Color mainColor = Color.FromHex("#1976D2");
        private Color selectedColor = Color.FromHex("#FFA500");
        private static INewsService NewsService;
        private Message message;
        public MainPage(INewsService newsService)
        {

            InitializeComponent();
            NewsService = newsService;
            lastSelectedFrame = SenesteNytFrame;
            SenesteNytFrame.BackgroundColor = selectedColor;
            message = new Message() { Category=selectedCategory, Filter=""};
            filterSearchBar.TextChanged += FilterSearchBar_TextChanged;
        }
        private async void SenesteNytLabel_Tapped(object sender, EventArgs e)
        {
            if (selectedCategory != "SenesteNyt")
            {
                selectedCategory = "SenesteNyt";
                message = new Message() { Category = selectedCategory, Filter = filterSearchBar.Text };
                SendMessage(message);
            }
            UpdateButtonAppearance(SenesteNytFrame);
        }

        private async void IndlandLabel_Tapped(object sender, EventArgs e)
        {
            if (selectedCategory != "Indland")
            {
                selectedCategory = "Indland";
                message = new Message() { Category = selectedCategory, Filter = filterSearchBar.Text };
                SendMessage(message);
            }
            UpdateButtonAppearance(IndlandFrame);
        }

        private async void UdlandLabel_Tapped(object sender, EventArgs e)
        {
            if (selectedCategory != "Udland")
            {
                selectedCategory = "Udland";
                message = new Message() { Category = selectedCategory, Filter = filterSearchBar.Text };
                SendMessage(message);
            }
            UpdateButtonAppearance(UdlandFrame);
        }

        private void SendMessage(Message message)
        {
            MessagingCenter.Send(this, "CategorySelected", message);
        }
        private void FilterButton_Clicked(object sender, EventArgs e)
        {
            filterOptions.IsVisible = !filterOptions.IsVisible;
        }
        private void FilterSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            message.Filter = e.NewTextValue;
            SendMessage(message);
        }
        private void UpdateButtonAppearance(Frame currentFrame)
        {
            lastSelectedFrame.BackgroundColor = mainColor;

            currentFrame.BackgroundColor = selectedColor;
            lastSelectedFrame = currentFrame;
        }
    }
}
