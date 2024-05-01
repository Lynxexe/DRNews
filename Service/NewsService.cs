using DRNews.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DRNews.Service
{
    internal class NewsService : INewsService
    {
        
        private string GetFeedUrlForCategory(string category)
        {
            switch (category)
            {
                case "SenesteNyt":
                    return "https://www.dr.dk/nyheder/service/feeds/allenyheder";
                case "Indland":
                    return "https://www.dr.dk/nyheder/service/feeds/indland";
                case "Udland":
                    return "https://www.dr.dk/nyheder/service/feeds/udland";
                default:
                    throw new ArgumentException("Invalid category");
            }
        }
        public async Task<List<NewsItem>> GetNewsAsync(string? category)
        {
            var httpClient = new HttpClient();
            var feedUrl = GetFeedUrlForCategory(category);

            var result = await httpClient.GetStringAsync(feedUrl);

            var items = new List<NewsItem>();
            XNamespace mediaNameSpace = "http://search.yahoo.com/mrss/";

            var document = XDocument.Parse(result);
            var news = document.Descendants("item")
                               .Select(item => new NewsItem
                               {
                                   Title = (string)item.Element("title"),
                                   Link = (string)item.Element("link"),
                                   Date = (string)item.Element("pubDate"),
                                   Image = (string)item.Descendants(mediaNameSpace + "content").FirstOrDefault()?.Attribute("url")?.Value,
                                   DateObject = (string)item.Element("pubDate"),
                               });
            items.AddRange(news);

            // Format dates
            items = await FormatNewsDatesAsync(items);
            items = items.OrderByDescending(item => DateTime.Parse(item.DateObject)).ToList();

            return items; // Return the list of news items
        }
        public async Task<List<NewsItem>> FormatNewsDatesAsync(List<NewsItem> items)
        {
            return await Task.Run(() =>
            {
                return items.Select(item =>
                {
                    DateTime newsDate = DateTime.Parse(item.DateObject);
                    TimeSpan difference = DateTime.UtcNow - newsDate;
                    string formattedDate;
                    if (difference.TotalSeconds >= 0)
                    {
                        if (difference.TotalDays >= 1)
                        {
                            formattedDate = $"{(int)difference.TotalDays} days ago";
                        }
                        else if (difference.TotalHours >= 1)
                        {
                            formattedDate = $"{(int)difference.TotalHours} hours ago";
                        }
                        else if (difference.TotalMinutes >= 1)
                        {
                            formattedDate = $"{(int)difference.TotalMinutes} minutes ago";
                        }
                        else
                        {
                            formattedDate = $"{(int)difference.TotalSeconds} seconds ago";
                        }
                    }
                    else
                    {
                        formattedDate = "just now";
                    }
                    item.Date = formattedDate;
                    return item;
                }).ToList();
            });
        }
    }
    }
