using DRNews.Service;
using Microsoft.AspNetCore.Components;

namespace DRNews
{
    public partial class App : Application
    {
        public App(INewsService newsService)
        {
            InitializeComponent();
            MainPage = new MainPage(newsService);
        }
    }
}
