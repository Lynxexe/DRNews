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
        Task<List<NewsItem>> GetNewsAsync();
    }
}
