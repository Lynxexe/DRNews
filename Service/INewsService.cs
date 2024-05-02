using DRNews.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRNews.Service
{
    public interface INewsService
    {
        Task<List<NewsItem>> GetNewsAsync(string selectedCategory, string filter);
        Task<List<NewsItem>> FormatNewsDatesAsync(List<NewsItem> items);
        Task<List<NewsItem>> UpdateNewsFeed(List<NewsItem> currentItems, string selectedCategory, string Filter);
        Task<string> GetNewsItemHtmlAsync(string newsItemLink);
    }
}
