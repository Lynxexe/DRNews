using DRNews.Service;

namespace DRNews
{
    public partial class MainPage : ContentPage
    {
        private INewsService NewsService;
        public MainPage(INewsService _newsservice)
        {
            InitializeComponent();
             NewsService = _newsservice;
        }
    }
}
