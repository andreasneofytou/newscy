using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Web.Http;

namespace News.DataModel
{
  public sealed class NewsDataSource
  {
    private static NewsDataSource _newsDataSource = new NewsDataSource();

    private static NewsDataProvider Philelefhteros = new NewsDataProvider
    {
      Name = "Phileleftheros",
      Description = "Ενημέρωση για πολιτική, οικονομία, αθλητισμό, ποδόσφαιρο, χρηματιστήριο, ψυχαγωγία από τον Φιλελεύθερο Κύπρου.",
      ImageUrl = "/Assets/Phileleftheros.png",
      Categories = new List<NewsDataCategory> 
      {
        new NewsDataCategory(NewsDataCategory.TOP_STORIES, NewsDataCategory.TOP_STORIES_EL, "Top Stories", "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=399" ),
        new NewsDataCategory(NewsDataCategory.WORLD, NewsDataCategory.WORLD_EL, "News from around the world", "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=5" ),
        new NewsDataCategory(NewsDataCategory.CYPRUS, NewsDataCategory.CYPRUS_EL, "News about Cyprus", "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=199" ),
        new NewsDataCategory(NewsDataCategory.GREECE, NewsDataCategory.GREECE_EL, "News about Greece", "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=2" ),
        new NewsDataCategory(NewsDataCategory.ECONOMY, NewsDataCategory.ECONOMY_EL, "Economy", "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=168" ),
        new NewsDataCategory(NewsDataCategory.TECHNOLOGY, NewsDataCategory.TECHNOLOGY_EL, "News about technology", "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=47" ),
        new NewsDataCategory(NewsDataCategory.WEIRD, NewsDataCategory.WEIRD_EL, "Weird news from around the globe", "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=192" ),
        new NewsDataCategory(NewsDataCategory.RECIPES, NewsDataCategory.RECIPES_EL, "Greek and Cypriot recipes", "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=139" ),
        new NewsDataCategory(NewsDataCategory.CULTURE, NewsDataCategory.CULTURE_EL, "Culture", "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=7" ),
      },
      IsEnabled = false,
      Url = "http://philenews.com"      
    };

    private static NewsDataProvider Dialogos = new NewsDataProvider
    {
      Name = "Dialogos",
      Description = "Η ΔΙΑΛΟΓΟΣ Media Group είναι ένα σύγχρονο δημοσιογραφικό δίκτυο που έχει ως πρωταρχικό στόχο την ενημέρωση των πολιτών, τόσο διαδικτυακά όσο και έντυπα και ραδιοφωνικά. " 
                  + "Βασικό χαρακτηριστικό της ιστοσελίδας μας, πέραν της δέσμευσής μας για έγκυρη και άμεση ενημέρωση, είναι η διαδραστικότητα μεταξύ του μέσου και των επισκεπτών του. Τα σχόλια, "
                  + "οι απόψεις και οι εισηγήσεις σας είναι συστατικό κομμάτι της dialogos.com.cy. ",
      ImageUrl = "/Assets/dialogos.jpg",
      Categories = new List<NewsDataCategory> 
      {
        new NewsDataCategory(NewsDataCategory.TOP_STORIES, NewsDataCategory.TOP_STORIES_EL, "Top Stories", "http://dialogos.com.cy/feed/" ),
        new NewsDataCategory(NewsDataCategory.WORLD, NewsDataCategory.WORLD_EL, "News from around the world", "http://dialogos.com.cy/blog/category/eidhseis/idisis-diethni/feed/" ),
        new NewsDataCategory(NewsDataCategory.CYPRUS, NewsDataCategory.CYPRUS_EL, "News about Cyprus", "http://dialogos.com.cy/blog/category/eidhseis/koinonia/feed" ),
        new NewsDataCategory(NewsDataCategory.ECONOMY, NewsDataCategory.ECONOMY_EL, "Economy", "http://dialogos.com.cy/blog/category/eidhseis/oikonimia/feed/" ),
        new NewsDataCategory(NewsDataCategory.SPORTS, NewsDataCategory.SPORTS_EL, "Sports", "http://dialogos.com.cy/blog/category/eidhseis/athlitismos/feed/" ),
        new NewsDataCategory(NewsDataCategory.TECHNOLOGY, NewsDataCategory.TECHNOLOGY_EL, "News about technology", "http://dialogos.com.cy/blog/category/eidhseis/epistimi-tehnologia/feed/" ),
        new NewsDataCategory(NewsDataCategory.INTERVIEWS, NewsDataCategory.INTERVIEWS_EL, "Interviews", "http://dialogos.com.cy/blog/category/interviews/feed/" ),
        new NewsDataCategory(NewsDataCategory.ABSURD, NewsDataCategory.ABSURD_EL, "Absurd Stuff", "http://dialogos.com.cy/blog/category/%cf%80%ce%b1%cf%81%ce%ac%ce%bb%ce%bf%ce%b3%ce%bf%cf%82/feed/" ),
        new NewsDataCategory(NewsDataCategory.CULTURE, NewsDataCategory.CULTURE_EL, "Culture", "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=7" ),
        new NewsDataCategory(NewsDataCategory.ENVIRONMENT, NewsDataCategory.ENVIRONMENT_EL, "Environment", "http://dialogos.com.cy/blog/category/oikologos/perivalon/feed/")
      },
      IsEnabled = false,
      Url = "http://dialogos.com.cy"
    };

    private static NewsDataProvider Politis = new NewsDataProvider
    {
      Name = "Politis",
      Description = "Ημερήσια πρωινή εφημερίδα",
      ImageUrl = "/Assets/politis.jpg",
      Categories = new List<NewsDataCategory> 
      {
        new NewsDataCategory(NewsDataCategory.TOP_STORIES, NewsDataCategory.TOP_STORIES_EL, "Top Stories", "http://www.politis-news.com/rss/news.xml " ),
        new NewsDataCategory(NewsDataCategory.WORLD, NewsDataCategory.WORLD_EL, "News from around the world", "http://www.politis-news.com/rss/news_int.xml" ),
        new NewsDataCategory(NewsDataCategory.CYPRUS, NewsDataCategory.CYPRUS_EL, "News about Cyprus", "http://www.politis-news.com/rss/news_cyprus.xml" ),
        new NewsDataCategory(NewsDataCategory.GREECE, NewsDataCategory.GREECE_EL, "News about Greece", "http://www.politis-news.com/rss/news_greece.xml" ),
        new NewsDataCategory(NewsDataCategory.ECONOMY, NewsDataCategory.ECONOMY_EL, "Economy", "http://www.politis-news.com/rss/news_economy.xml" ),
        new NewsDataCategory(NewsDataCategory.SPORTS, NewsDataCategory.SPORTS_EL, "Sports", "http://www.politis-sports.com/rss/newsrss.xml" ),
        new NewsDataCategory(NewsDataCategory.TECHNOLOGY, NewsDataCategory.TECHNOLOGY_EL, "News about technology", "http://www.politis-news.com/rss/news_tech.xml" ),
        new NewsDataCategory(NewsDataCategory.WEIRD, NewsDataCategory.WEIRD_EL, "Weird news from around the globe", "http://www.politis-news.com/rss/news_odd.xml" ),
        new NewsDataCategory(NewsDataCategory.CULTURE, NewsDataCategory.CULTURE_EL, "Culture", "http://www.politis-news.com/rss/lifestyle.xml" ),
      },
      IsEnabled = false,
      Url = "http://www.politis-news.com"
    };

    private static NewsDataProvider Offsite = new NewsDataProvider
    {
      Name = "Offsite",
      Description = "Καθημερινή ηλεκτρονική εφημερίδα με νέα από Κύπρο, Ελλάδα και τον κόσμο.",
      ImageUrl = "/Assets/offsite.jpg",
      Categories = new List<NewsDataCategory> 
      {
        new NewsDataCategory(NewsDataCategory.TOP_STORIES, NewsDataCategory.TOP_STORIES_EL, "Top Stories", "http://offsite.com.cy/feed/" ),
        new NewsDataCategory(NewsDataCategory.WORLD, NewsDataCategory.WORLD_EL, "News from around the world", "http://offsite.com.cy/category/offsite/kosmos/feed/" ),
        new NewsDataCategory(NewsDataCategory.CYPRUS, NewsDataCategory.CYPRUS_EL, "News about Cyprus", "http://offsite.com.cy/category/offsite/kypros/topika/feed/" ),
        new NewsDataCategory(NewsDataCategory.GREECE, NewsDataCategory.GREECE_EL, "News about Greece", "http://offsite.com.cy/category/offsite/ellada/feed/" ),
        new NewsDataCategory(NewsDataCategory.ECONOMY, NewsDataCategory.ECONOMY_EL, "Economy", "http://offsite.com.cy/category/offsite/kypros/oikonomia/feed/" ),
        new NewsDataCategory(NewsDataCategory.SPORTS, NewsDataCategory.SPORTS_EL, "Sports", "http://offsite.com.cy/category/offsite/athlitismos/feed/" ),
        new NewsDataCategory(NewsDataCategory.TECHNOLOGY, NewsDataCategory.TECHNOLOGY_EL, "News about technology", "http://offsite.com.cy/category/offsite/texnologia/feed/" ),
        new NewsDataCategory(NewsDataCategory.ENVIRONMENT, NewsDataCategory.ENVIRONMENT_EL, "Environment", "http://offsite.com.cy/category/offsite/oikologia/feed/")
      },
      IsEnabled = false,
      Url = "http://offsite.com.cy"
    };

    private static List<NewsDataProvider> _dataPoviders = new List<NewsDataProvider>() { Philelefhteros, Dialogos, Offsite };
    public static List<NewsDataProvider> NewsProviders { get { return _dataPoviders; } }


    private ObservableCollection<NewsDataCategory> _categories = new ObservableCollection<NewsDataCategory>();
    public ObservableCollection<NewsDataCategory> Categories
    {
      get { return this._categories; }
    }

    public static async Task<NewsDataCategory> GetCategoryAsync(string title)
    {
      await _newsDataSource.GetNewsDataAsync();
      // Simple linear search is acceptable for small data sets
      var matches = _newsDataSource.Categories.Where((group) => group.Title.Equals(title));
      if (matches.Count() == 1) return matches.First();
      return null;
    }

    public static async Task<NewsDataArticle> GetArticleAsync(string title)
    {
      await _newsDataSource.GetNewsDataAsync();
      // Simple linear search is acceptable for small data sets
      var matches = _newsDataSource.Categories.SelectMany(group => group.Articles).Where((item) => item.Title.Equals(title));
      if (matches.Count() == 1) return matches.First();
      return null;
    }

    public static async Task<IEnumerable<NewsDataCategory>> GetCategoriesAsync()
    {
      await _newsDataSource.GetNewsDataAsync();

      return _newsDataSource.Categories;
    }

    public static async Task<IEnumerable<NewsDataCategory>> RefreshCategoriesAsync()
    {
      await NewsDataSource.DownloadSources(false);
      await _newsDataSource.RetrieveDataAsync();

      return _newsDataSource.Categories;
    }

    private async Task GetNewsDataAsync()
    {
      await EnsureArticleDataLoaded();
    }

    private static async Task EnsureArticleDataLoaded()
    {
      if (_newsDataSource == null || _newsDataSource.Categories.Count == 0)
        await _newsDataSource.RetrieveDataAsync();

      return;
    }

    public async Task RetrieveDataAsync()
    {
      var categories = await ReadArticles(false);
      if (categories != null && categories.Count > 0)
      {
        _categories = categories;
      }
      else
      {
        await RefreshCategoriesAsync();
      }
    }

    public static async Task DownloadSources(bool includeDisabled)
    {
      foreach (var provider in NewsDataSource.NewsProviders)
      {
        if(!includeDisabled && !provider.IsEnabled)
        {
          continue;
        }
        foreach(var category in provider.Categories)
        {
          List<NewsDataArticle> artcles = await DownloadAricles(category.Url);
          category.Articles = artcles;
        }
        await SerializeProvider(provider);
      }
    }

    public static async Task SaveSources()
    {
      foreach(var provider in NewsProviders)
      {
        await SerializeProvider(provider);
      }
    }

    public static async Task LoadSources()
    {
      for (int index = 0; index < NewsProviders.Count; index++)
      {
        var temp = await DeserializeProvider(NewsProviders.ElementAt(index).Name);
        if (temp != null)
        {
          NewsProviders[index] = temp;
        }
      }
    }

    private static async Task SerializeProvider(NewsDataProvider provider)
    {
      try
      {
        StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        Stream baseStream = await localFolder.OpenStreamForWriteAsync(provider.Name, CreationCollisionOption.ReplaceExisting);

        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(NewsDataProvider));
        serializer.WriteObject(baseStream, provider);
        baseStream.Dispose();
      }
      catch
      {
      }
    }

    private static async Task<NewsDataProvider> DeserializeProvider(string filename)
    {
      try
      {
        StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        Stream baseStream = await localFolder.OpenStreamForReadAsync(filename);
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(NewsDataProvider));
        NewsDataProvider provider = serializer.ReadObject(baseStream) as NewsDataProvider;
        baseStream.Dispose();
        return provider;
      }
      catch
      {
        return null;
      }
    }

    private static async Task<List<NewsDataArticle>> DownloadAricles(string categoryUrl)
    {
      HttpClient client = new HttpClient();
      List<NewsDataArticle> list = new List<NewsDataArticle>();

      using (HttpResponseMessage response = await client.GetAsync(new Uri(categoryUrl.ToString(), UriKind.Absolute)))
      {
        if (response.IsSuccessStatusCode)
        {
          string contentxml = await response.Content.ReadAsStringAsync();
          string re = @"[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD\u10000-\u10FFFF]";
          contentxml = Regex.Replace(contentxml, re, ""); 
          XElement elem = XElement.Parse(contentxml);
          list =
            (
              from i in elem.DescendantsAndSelf("item")
              select new NewsDataArticle(i)
            ).ToList();
        }
      }
      return list;
    }

    public static async Task<ObservableCollection<NewsDataCategory>> ReadArticles(bool includedDisabled)
    {
      ObservableCollection<NewsDataCategory> categories = new ObservableCollection<NewsDataCategory>();
      foreach (var provider in NewsProviders)
      {
        if(!includedDisabled && !provider.IsEnabled)
        {
          continue;
        }

        NewsDataProvider newsProvider = await DeserializeProvider(provider.Name);
        if(newsProvider == null)
        {
          continue;
        }
        var providerCategories = newsProvider.Categories;
        foreach(var cat in providerCategories)
        {
          var tempList = categories.Where(x => x.Title.Equals(cat.Title)).ToList();
          var temp2 = (tempList.Count > 0) ? tempList[0] : null;
          if(temp2 != null)
          {
            temp2.Articles.AddRange(cat.Articles);
            temp2.Articles.Sort();
          }
          else
          {
            cat.Articles.Sort();
            categories.Add(cat);
          }
        }
      }
      return categories;
    }

    public static async Task<bool> CanReadArticleFiles()
    {
      StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
      StorageFile file;
      bool foundAny = false;
      foreach (var provider in NewsProviders)
      {
        try
        {
          file = await localFolder.GetFileAsync(provider.Name);
          foundAny = true;
        }
        catch (FileNotFoundException)
        {
        }
      }
      return foundAny;
    }
  }
}
