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

    private static NewsCategory world = new NewsCategory("World", "Κόσμος", "News from around the world", new List<string> { "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=5" });
    private static NewsCategory cyprus = new NewsCategory("Cyprus", "Κύπρος", "News about Cyprus", new List<string> { "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=199" });
    private static NewsCategory greece = new NewsCategory("Greece", "Ελλάδα", "News about Greece", new List<string> { "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=2" });
    private static NewsCategory economy = new NewsCategory("Economy", "Οικονομία", "Economy", new List<string> { "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=168" });
    private static NewsCategory technology = new NewsCategory("Technology", "Τεχνολογία", "News about technology", new List<string> { "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=47" });
    private static NewsCategory weird = new NewsCategory("Weird", "Παράξενα", "Weird news from around the globe", new List<string> { "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=192" });
    private static NewsCategory recipes = new NewsCategory("Recipes", "Συνταγές", "Greek and Cypriot recipes", new List<string> { "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=139" });
    private static NewsCategory topStories = new NewsCategory("Top Stories", "Κορυφαία", "Top Stories", new List<string> { "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=399", "http://dialogos.com.cy/feed/" });
    private static NewsCategory culture = new NewsCategory("Culture", "Κουλτούρα", "Culture", new List<string> { "http://www.philenews.com/Publications/RssModule/rss.aspx?CategoryId=7" });
    private static NewsCategory sports = new NewsCategory("Sports", "Αθλητικά", "Sports", new List<string> { "http://dialogos.com.cy/blog/category/eidhseis/athlitismos/feed/" });

    
    public static List<NewsCategory> Categories = new List<NewsCategory> { topStories, world, cyprus, greece, economy, sports, technology, culture, weird, recipes };

    private ObservableCollection<NewsDataGroup> _groups = new ObservableCollection<NewsDataGroup>();
    public ObservableCollection<NewsDataGroup> Groups
    {
      get { return this._groups; }
    }

    public static async Task<NewsDataGroup> GetGroupAsync(string title)
    {
      await _newsDataSource.GetNewsDataAsync();
      // Simple linear search is acceptable for small data sets
      var matches = _newsDataSource.Groups.Where((group) => group.Title.Equals(title));
      if (matches.Count() == 1) return matches.First();
      return null;
    }

    public static async Task<NewsDataArticle> GetItemAsync(string title)
    {
      await _newsDataSource.GetNewsDataAsync();
      // Simple linear search is acceptable for small data sets
      var matches = _newsDataSource.Groups.SelectMany(group => group.Articles).Where((item) => item.Title.Equals(title));
      if (matches.Count() == 1) return matches.First();
      return null;
    }

    public static async Task<IEnumerable<NewsDataGroup>> GetGroupsAsync()
    {
      await _newsDataSource.GetNewsDataAsync();

      return _newsDataSource.Groups;
    }

    public static async Task<IEnumerable<NewsDataGroup>> RefreshGroupsAsync()
    {
      await NewsDataSource.DownloadAricles();
      await _newsDataSource.RetrieveDataAsync();

      return _newsDataSource.Groups;
    }

    private async Task GetNewsDataAsync()
    {
      await EnsureArticleDataLoaded();
    }

    private static async Task EnsureArticleDataLoaded()
    {
      if (_newsDataSource == null || _newsDataSource.Groups.Count == 0)
        await _newsDataSource.RetrieveDataAsync();

      return;
    }

    public async Task RetrieveDataAsync()
    {
      var groups = await RetrieveArticles();
      if(groups != null || groups.Count > 0)
      {
        _groups = groups;
      }
    }

    public static async Task DownloadAricles()
    {
      foreach (var cat in Categories)
      {
        List<NewsDataArticle> articlesList = new List<NewsDataArticle>();
        NewsDataGroup group = new NewsDataGroup();
        group.Title = cat.Title;
        group.GreekTitle = cat.GreekTitle;
        group.Description = cat.Description;
        foreach(var source in cat.Urls)
        {
          articlesList.AddRange( await ReadAricles(source) as List<NewsDataArticle>);
        }
        articlesList.Sort();
        foreach (var article in articlesList)
        {
          group.Articles.Add(article);
        }
        SerializeArticlesGroup(group);
      }
    }

    private static async Task<List<NewsDataArticle>> ReadAricles(string categoryUrl)
    {
      HttpClient client = new HttpClient();
      List<NewsDataArticle> list = new List<NewsDataArticle>();

      using (HttpResponseMessage response = await client.GetAsync(new Uri(categoryUrl.ToString(), UriKind.Absolute)))
      {
        if (response.IsSuccessStatusCode)
        {
          String contentxml = await response.Content.ReadAsStringAsync();
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

    public static async Task<ObservableCollection<NewsDataGroup>> RetrieveArticles()
    {
      bool x = await CanReadArticleFiles();
      ObservableCollection<NewsDataGroup> articleGroups = new ObservableCollection<NewsDataGroup>();
      if (x)
      {
        foreach (var cat in Categories)
        {
          NewsDataGroup group = await ReadArticlesGroupFromLocalStorage(cat.Title);
          articleGroups.Add(group);
        }
      }
      return articleGroups;
    }

    public static async Task<bool> CanReadArticleFiles()
    {
      StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
      StorageFile file;
      try
      {
        foreach (var cat in Categories)
          file = await localFolder.GetFileAsync(cat.Title);
      }
      catch (FileNotFoundException)
      {
        return false;
      }
      return true;
    }

    private static async void SerializeArticlesGroup(NewsDataGroup articleDataGroup)
    {
      StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
      Stream baseStream = await localFolder.OpenStreamForWriteAsync(articleDataGroup.Title, CreationCollisionOption.ReplaceExisting);

      DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(NewsDataGroup));
      serializer.WriteObject(baseStream, articleDataGroup);
      baseStream.Dispose();
    }

    private static async Task<NewsDataGroup> ReadArticlesGroupFromLocalStorage(string filename)
    {
      StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
      Stream baseStream = await localFolder.OpenStreamForReadAsync(filename);
      DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(NewsDataGroup));
      NewsDataGroup articlesGroup = serializer.ReadObject(baseStream) as NewsDataGroup;
      baseStream.Dispose();
      return articlesGroup;
    }
  }
}
