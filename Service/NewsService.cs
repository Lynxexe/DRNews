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
        public async Task<List<NewsItem>> GetNewsAsync()
        {
            var httpClient = new HttpClient();
            var feedUrls = new List<string>
    {
        "https://www.dr.dk/nyheder/service/feeds/senestenyt",
        "https://www.dr.dk/nyheder/service/feeds/indland",
        "https://www.dr.dk/nyheder/service/feeds/udland",
        // Add other feed URLs here...
    };

            var tasks = feedUrls.Select(url => httpClient.GetStringAsync(url));
            var results = await Task.WhenAll(tasks);

            var items = new List<NewsItem>();
            XNamespace mediaNameSpace = "http://search.yahoo.com/mrss/";
            foreach (var result in results)
            {
                var document = XDocument.Parse(result);
                Console.WriteLine(result);
                var news = document.Descendants("item")
                                    .Select(item => new NewsItem
                                    {
                                        Title = (string)item.Element("title"),
                                        Link = (string)item.Element("link"),
                                        Date = (string)item.Element("pubDate"),
                                        Image = (string)item.Descendants(mediaNameSpace + "content").FirstOrDefault()?.Attribute("url")?.Value
                                    });
                items.AddRange(news);
            }

            // Format dates
            items = items.Select(item =>
            {
                // Parse the date string into a DateTime object
                DateTime newsDate = DateTime.Parse(item.Date);
                // Calculate the time difference
                TimeSpan difference = DateTime.UtcNow - newsDate;
                string formattedDate;
                if (difference.TotalSeconds >= 0)
                {
                    if (difference.TotalDays >= 1)
                    {
                        // If the difference is more than or equal to 1 day, display it in days
                        formattedDate = $"{(int)difference.TotalDays} days ago";
                    }
                    else if (difference.TotalHours >= 1)
                    {
                        // If the difference is less than 1 day but more than or equal to 1 hour, display it in hours
                        formattedDate = $"{(int)difference.TotalHours} hours ago";
                    }
                    else if (difference.TotalMinutes >= 1)
                    {
                        // If the difference is less than 1 hour but more than or equal to 1 minute, display it in minutes
                        formattedDate = $"{(int)difference.TotalMinutes} minutes ago";
                    }
                    else
                    {
                        // Otherwise, display it in seconds
                        formattedDate = $"{(int)difference.TotalSeconds} seconds ago";
                    }
                }
                else
                {
                    formattedDate = "just now";
                }
                // Update the Date property with the formatted date
                item.Date = formattedDate;
                return item;
            }).ToList();

            return items; // Return the list of news items
        }
    }
    }
