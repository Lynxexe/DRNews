using DRNews.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using System.Xml.Linq;
using System.Net.Http;
using HtmlAgilityPack;

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
        public async Task<List<NewsItem>> GetNewsAsync(string? category, string filter)
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

            if (!string.IsNullOrEmpty(filter))
            {

                string[] filters = filter.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(f => f.Trim())
                                 .ToArray();
                foreach (var filterString in filters)
                {
                    if (!string.IsNullOrEmpty(filterString))
                    {
                        items = items.Where(x => !x.Title.Contains(filterString, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                }
            }
            items = await FormatNewsDatesAsync(items);
            items = items.OrderByDescending(item => DateTime.Parse(item.DateObject)).ToList();

            return items;
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
        public async Task<List<NewsItem>> UpdateNewsFeed(List<NewsItem> currentItems, string selectedCategory, string Filter)
        {
            var latestNews = await GetNewsAsync(selectedCategory, Filter);
            var returnItems = currentItems;
            foreach (var news in latestNews)
            {
                var existingItem = returnItems.FirstOrDefault(item => item.Link == news.Link);
                if (existingItem != null)
                {
                    if (existingItem.Title != news.Title)
                        existingItem.Title = news.Title;

                    if (existingItem.Date != news.Date)
                        existingItem.Date = news.Date;

                    if (existingItem.Image != news.Image)
                        existingItem.Image = news.Image;

                    if (existingItem.DateObject != news.DateObject)
                        existingItem.DateObject = news.DateObject;
                }
                else
                {
                    returnItems.Add(news);
                }
            }
            returnItems = returnItems.OrderByDescending(item => DateTime.Parse(item.DateObject)).ToList();
            returnItems = await FormatNewsDatesAsync(returnItems);

            return returnItems;
        }
        public async Task<string> GetNewsItemHtmlAsync(string newsItemLink)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var htmlContent = await httpClient.GetStringAsync(newsItemLink);
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);
                    var item = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'hydra-latest-news-page__short-news-item') and contains(@class, 'dre-variables')]");

                    if (item != null)
                    {
                        var paragraphs = item.SelectNodes(".//p");

                        if (paragraphs != null)
                        {
                            string content = "";
                            foreach (var paragraph in paragraphs)
                            {
                                content += paragraph.InnerText + "\n";
                            }
                            return content;
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
