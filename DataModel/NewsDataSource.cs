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
        new NewsDataCategory(NewsDataCategory.TOP_STORIES, NewsDataCategory.TOP_STORIES_EL, "Top Stories", "http://www.politis-news.com/rss/news.xml" ),
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

    private static NewsDataProvider IKypros = new NewsDataProvider
    {
      Name = "ikypros",
      Description = "Ηλεκτρονική Πύλη ikypros - Άμεση ενημέρωση για όσα συμβαίνουν στην Κύπρο και στον κόσμο.",
      ImageUrl = "/Assets/ikypros.jpg",
      Categories = new List<NewsDataCategory> 
      {
        new NewsDataCategory(NewsDataCategory.TOP_STORIES, NewsDataCategory.TOP_STORIES_EL, "Top Stories", "http://ikypros.com/?feed=rss2" ),
        new NewsDataCategory(NewsDataCategory.WORLD, NewsDataCategory.WORLD_EL, "News from around the world", "http://ikypros.com/?cat=31&feed=rss2" ),
        new NewsDataCategory(NewsDataCategory.CYPRUS, NewsDataCategory.CYPRUS_EL, "News about Cyprus", "http://ikypros.com/?cat=3&feed=rss2" ),
        new NewsDataCategory(NewsDataCategory.GREECE, NewsDataCategory.GREECE_EL, "News about Greece", "http://ikypros.com/?cat=30&feed=rss2" ),
        new NewsDataCategory(NewsDataCategory.SPORTS, NewsDataCategory.SPORTS_EL, "Sports", "http://ikypros.com/?cat=34&feed=rss2" ),
        new NewsDataCategory(NewsDataCategory.ENVIRONMENT, NewsDataCategory.ENVIRONMENT_EL, "Environment", "http://ikypros.com/?cat=9&feed=rss2")
      },
      IsEnabled = false,
      Url = "http://ikypros.com"
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


    private static NewsDataProvider NewsGr = new NewsDataProvider
    {
      Name = "news.gr",
      Description = "Ειδήσεις και Άμεση Ενημέρωση. Ειδήσεις τώρα, από την Ελλάδα και τον Κόσμο.",
      ImageUrl = "/Assets/newsgr.png",
      Categories = new List<NewsDataCategory> 
      {
        new NewsDataCategory(NewsDataCategory.TOP_STORIES, NewsDataCategory.TOP_STORIES_EL, "Top Stories", "http://www.news.gr/rss.ashx?colid=2" ),
        new NewsDataCategory(NewsDataCategory.WORLD, NewsDataCategory.WORLD_EL, "News from around the world", "http://www.news.gr/rss.ashx?catid=10" ),
        new NewsDataCategory(NewsDataCategory.GREECE, NewsDataCategory.GREECE_EL, "News about Greece", "http://www.news.gr/rss.ashx?catid=7" ),
        new NewsDataCategory(NewsDataCategory.POLITICS, NewsDataCategory.POLITICS_EL, "News about Politics", "http://www.news.gr/rss.ashx?catid=5" ),
        new NewsDataCategory(NewsDataCategory.ECONOMY, NewsDataCategory.ECONOMY_EL, "Economy", "http://www.news.gr/rss.ashx?catid=9" ),
        new NewsDataCategory(NewsDataCategory.SPORTS, NewsDataCategory.SPORTS_EL, "Sports", "http://www.news.gr/rss.ashx?catid=11" ),
        new NewsDataCategory(NewsDataCategory.TECHNOLOGY, NewsDataCategory.TECHNOLOGY_EL, "News about technology", "http://www.news.gr/rss.ashx?catid=12" ),
        new NewsDataCategory(NewsDataCategory.ENVIRONMENT, NewsDataCategory.ENVIRONMENT_EL, "Environment", "http://www.news.gr/rss.ashx?catid=17")
      },
      IsEnabled = false,
      Url = "http://news.gr"
    };

    private static NewsDataProvider Thema = new NewsDataProvider
    {
      Name = "Πρώτο θέμα",
      Description = "Όλες οι ειδήσεις και τελευταία γεγονότα από το ΠΡΩΤΟ ΘΕΜΑ!",
      ImageUrl = "/Assets/thema.png",
      Categories = new List<NewsDataCategory> 
      {
        new NewsDataCategory(NewsDataCategory.TOP_STORIES, NewsDataCategory.TOP_STORIES_EL, "Top Stories", "http://www.protothema.gr/rss/news/general/" ),
        new NewsDataCategory(NewsDataCategory.WORLD, NewsDataCategory.WORLD_EL, "News from around the world", "http://www.protothema.gr/rss/news/world" ),
        new NewsDataCategory(NewsDataCategory.GREECE, NewsDataCategory.GREECE_EL, "News about Greece", "http://www.protothema.gr/rss/news/greece" ),
        new NewsDataCategory(NewsDataCategory.POLITICS, NewsDataCategory.POLITICS_EL, "News about Politics", "http://www.protothema.gr/rss/news/politics" ),
        new NewsDataCategory(NewsDataCategory.ECONOMY, NewsDataCategory.ECONOMY_EL, "Economy", "http://www.protothema.gr/rss/news/economy" ),
        new NewsDataCategory(NewsDataCategory.SPORTS, NewsDataCategory.SPORTS_EL, "Sports", "http://www.protothema.gr/rss/news/sports" ),
        new NewsDataCategory(NewsDataCategory.TECHNOLOGY, NewsDataCategory.TECHNOLOGY_EL, "News about technology", "http://www.protothema.gr/rss/news/technology" ),
        new NewsDataCategory(NewsDataCategory.ENVIRONMENT, NewsDataCategory.ENVIRONMENT_EL, "Environment", "http://www.protothema.gr/rss/news/environmnent")
      },
      IsEnabled = false,
      Url = "http://news.gr"
    };

    private static NewsDataProvider InGr = new NewsDataProvider
    {
      Name = "In.gr",
      Description = "The leading news site in Greece",
      ImageUrl = "/Assets/ingr.png",
      Categories = new List<NewsDataCategory> 
      {
        new NewsDataCategory(NewsDataCategory.TOP_STORIES, NewsDataCategory.TOP_STORIES_EL, "Top Stories", "http://rss.in.gr/feed/news" ),
        new NewsDataCategory(NewsDataCategory.WORLD, NewsDataCategory.WORLD_EL, "News from around the world", "http://rss.in.gr/feed/news/world" ),
        new NewsDataCategory(NewsDataCategory.GREECE, NewsDataCategory.GREECE_EL, "News about Greece", "http://rss.in.gr/feed/news/greece" ),
        new NewsDataCategory(NewsDataCategory.ECONOMY, NewsDataCategory.ECONOMY_EL, "Economy", "http://rss.in.gr/feed/news/economy" ),
        new NewsDataCategory(NewsDataCategory.SPORTS, NewsDataCategory.SPORTS_EL, "Sports", "http://rss.in.gr/feed/sports" ),
        new NewsDataCategory(NewsDataCategory.TECHNOLOGY, NewsDataCategory.TECHNOLOGY_EL, "News about technology", "http://rss.in.gr/feed/technology/main" ),
        new NewsDataCategory(NewsDataCategory.SCIENCE, NewsDataCategory.SCIENCE_EL, "Science", "http://rss.in.gr/feed/news/science"),
        new NewsDataCategory(NewsDataCategory.CULTURE, NewsDataCategory.CULTURE_EL, "Culture", "http://rss.in.gr/feed/news/culture" ),
      },
      IsEnabled = false,
      Url = "http://news.gr"
    };

    private static NewsDataProvider Skai = new NewsDataProvider
    {
      Name = "Σκάι",
      Description = "Η επικαιρότητα από τον ΣΚΑΪ.",
      ImageUrl = "/Assets/skai.png",
      Categories = new List<NewsDataCategory> 
      {
        new NewsDataCategory(NewsDataCategory.TOP_STORIES, NewsDataCategory.TOP_STORIES_EL, "Top Stories", "http://feeds.feedburner.com/skai/Uulu" ),
        new NewsDataCategory(NewsDataCategory.WORLD, NewsDataCategory.WORLD_EL, "News from around the world", "http://feeds.feedburner.com/skai/aqOL" ),
        new NewsDataCategory(NewsDataCategory.GREECE, NewsDataCategory.GREECE_EL, "News about Greece", "http://feeds.feedburner.com/skai/PLwa" ),        
        new NewsDataCategory(NewsDataCategory.POLITICS, NewsDataCategory.POLITICS_EL, "News about Politics", "http://feeds.feedburner.com/skai/yinm" ),
        new NewsDataCategory(NewsDataCategory.ECONOMY, NewsDataCategory.ECONOMY_EL, "Economy", "http://feeds.feedburner.com/skai/oPUt" ),
        new NewsDataCategory(NewsDataCategory.SPORTS, NewsDataCategory.SPORTS_EL, "Sports", "http://feeds.feedburner.com/skai/TfmK" ),
        new NewsDataCategory(NewsDataCategory.TECHNOLOGY, NewsDataCategory.TECHNOLOGY_EL, "News about technology", "http://feeds.feedburner.com/skai/fqsg" ),
        new NewsDataCategory(NewsDataCategory.CULTURE, NewsDataCategory.CULTURE_EL, "Culture", "http://feeds.feedburner.com/skai/ppGl" ),        
        new NewsDataCategory(NewsDataCategory.ENVIRONMENT, NewsDataCategory.ENVIRONMENT_EL, "Environment", "http://feeds.feedburner.com/skai/jVWs"),
        new NewsDataCategory(NewsDataCategory.WEIRD, NewsDataCategory.WEIRD_EL, "Weird news from around the globe", "http://feeds.feedburner.com/skai/bpAR" ),
      },
      IsEnabled = false,
      Url = "http://skai.gr"
    };

    private static NewsDataProvider RealGr = new NewsDataProvider
    {
      Name = "Real.gr",
      Description = "Ειδήσεις από την Ελλάδα και τον Κόσμο, άμεση ενημέρωση για όλες τις εξελίξεις, Διασκέδαση – Ψυχαγωγία και Αθλητισμός.",
      ImageUrl = "/Assets/realgr.jpeg",
      Categories = new List<NewsDataCategory> 
      {
        new NewsDataCategory(NewsDataCategory.TOP_STORIES, NewsDataCategory.TOP_STORIES_EL, "Top Stories", "http://www.real.gr/Rss.aspx?pid=474" ),
        new NewsDataCategory(NewsDataCategory.WORLD, NewsDataCategory.WORLD_EL, "News from around the world", "http://www.real.gr/Rss.aspx?pid=148" ),
        new NewsDataCategory(NewsDataCategory.GREECE, NewsDataCategory.GREECE_EL, "News about Greece", "http://www.real.gr/Rss.aspx?pid=149" ),        
        new NewsDataCategory(NewsDataCategory.POLITICS, NewsDataCategory.POLITICS_EL, "News about Politics", "http://www.real.gr/Rss.aspx?pid=151" ),
        new NewsDataCategory(NewsDataCategory.ECONOMY, NewsDataCategory.ECONOMY_EL, "Economy", "http://www.real.gr/Rss.aspx?pid=150" ),
        new NewsDataCategory(NewsDataCategory.SPORTS, NewsDataCategory.SPORTS_EL, "Sports", "http://www.real.gr/Rss.aspx?pid=146" ),
        new NewsDataCategory(NewsDataCategory.CULTURE, NewsDataCategory.CULTURE_EL, "Culture", "http://www.real.gr/Rss.aspx?pid=144" ),        
        new NewsDataCategory(NewsDataCategory.ENVIRONMENT, NewsDataCategory.ENVIRONMENT_EL, "Environment", "http://www.real.gr/Rss.aspx?pid=147"),
      },
      IsEnabled = false,
      Url = "http://www.real.gr"
    };

    private static NewsDataProvider NewsBeast = new NewsDataProvider
    {
      Name = "NewsBeast",
      Description = "Το τέρας των ειδήσεων με ειδήσεις, media, ψυχαγωγία, αθλητικά, περιβάλλον, lifestyle, γυναίκα, αυτοκίνητο, χρηματιστήριο, ενημέρωση, ότι να ναι.",
      ImageUrl = "/Assets/newsbeast.jpg",
      Categories = new List<NewsDataCategory> 
      {
        new NewsDataCategory(NewsDataCategory.TOP_STORIES, NewsDataCategory.TOP_STORIES_EL, "Top Stories", "http://www.newsbeast.gr/feeds/home" ),
        new NewsDataCategory(NewsDataCategory.WORLD, NewsDataCategory.WORLD_EL, "News from around the world", "http://www.newsbeast.gr/feeds/world" ),
        new NewsDataCategory(NewsDataCategory.GREECE, NewsDataCategory.GREECE_EL, "News about Greece", "http://www.newsbeast.gr/feeds/greece" ),        
        new NewsDataCategory(NewsDataCategory.POLITICS, NewsDataCategory.POLITICS_EL, "News about Politics", "http://www.newsbeast.gr/feeds/politiki" ),
        new NewsDataCategory(NewsDataCategory.ECONOMY, NewsDataCategory.ECONOMY_EL, "Economy", "http://www.newsbeast.gr/feeds/financial" ),
        new NewsDataCategory(NewsDataCategory.SPORTS, NewsDataCategory.SPORTS_EL, "Sports", "http://www.newsbeast.gr/feeds/sports" ),
        new NewsDataCategory(NewsDataCategory.CULTURE, NewsDataCategory.CULTURE_EL, "Culture", "http://www.newsbeast.gr/feeds/society" ),        
        new NewsDataCategory(NewsDataCategory.ENVIRONMENT, NewsDataCategory.ENVIRONMENT_EL, "Environment", "http://www.newsbeast.gr/feeds/environment"),
        new NewsDataCategory(NewsDataCategory.TECHNOLOGY, NewsDataCategory.TECHNOLOGY_EL, "News about technology", "http://www.newsbeast.gr/feeds/technology" ),        
        new NewsDataCategory(NewsDataCategory.WEIRD, NewsDataCategory.WEIRD_EL, "Weird news from around the globe", "http://www.newsbeast.gr/feeds/weird" ),
      },
      IsEnabled = false,
      Url = "http://www.newsbeast.gr"
    };

    private static NewsDataProvider Enet = new NewsDataProvider
    {
      Name = "Ελευθεροτυπία",
      Description = "Ειδήσεις από την Ελλάδα και τον Κόσμο.",
      ImageUrl = "/Assets/enet.png",
      Categories = new List<NewsDataCategory> 
      {
        new NewsDataCategory(NewsDataCategory.TOP_STORIES, NewsDataCategory.TOP_STORIES_EL, "Top Stories", "http://www.enet.gr/rss?i=news.el.article" ),
        new NewsDataCategory(NewsDataCategory.WORLD, NewsDataCategory.WORLD_EL, "News from around the world", "http://www.enet.gr/rss?i=news.el.categories&c=die8nh" ),
        new NewsDataCategory(NewsDataCategory.GREECE, NewsDataCategory.GREECE_EL, "News about Greece", "http://www.enet.gr/rss?i=news.el.categories&c=ellada" ),        
        new NewsDataCategory(NewsDataCategory.POLITICS, NewsDataCategory.POLITICS_EL, "News about Politics", "http://www.enet.gr/rss?i=news.el.categories&c=politikh" ),
        new NewsDataCategory(NewsDataCategory.ECONOMY, NewsDataCategory.ECONOMY_EL, "Economy", "http://www.enet.gr/rss?i=news.el.categories&c=oikonomia" ),
        new NewsDataCategory(NewsDataCategory.SPORTS, NewsDataCategory.SPORTS_EL, "Sports", "http://www.enet.gr/rss?i=news.el.categories&c=a8lhtismos" ),
        new NewsDataCategory(NewsDataCategory.TECHNOLOGY, NewsDataCategory.TECHNOLOGY_EL, "News about technology", "http://www.enet.gr/rss?i=news.el.categories&c=episthmh--texnologia" ),        
      },
      IsEnabled = false,
      Url = "http://www.enet.gr"
    };

    private static List<NewsDataProvider> _dataPoviders = new List<NewsDataProvider>() { Philelefhteros, Dialogos, IKypros, Offsite, NewsGr, Enet, Thema, InGr, Skai, RealGr, NewsBeast };
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
      _newsDataSource._categories.Clear();
      await NewsDataSource.DownloadSources(false);
      //await _newsDataSource.RetrieveDataAsync();

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
          List<NewsDataArticle> artcles = await DownloadAricles(category.Url) as List<NewsDataArticle>;
          if (artcles.Count > 24)
          {
            var a = artcles.Take(25).ToList();
            category.Articles = a as List<NewsDataArticle>;

          }
          else
          {
            category.Articles = artcles;
          }
          if (_newsDataSource._categories.Where(x => x.Title.Equals(category.Title)).Count() > 0)
          {
            var a = _newsDataSource._categories.Where(x => x.Title.Equals(category.Title));
            if (a.ToList()[0].Articles != null)
            {
              a.ToList()[0].Articles.AddRange(category.Articles);
            }
            else a.ToList()[0].Articles = category.Articles;

          }
          else
          {
            _newsDataSource._categories.Add(category);
          }
        }
        
        //await SerializeProvider(provider);
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
            if (cat.Articles != null && cat.Articles.Count > 0)
            {
              cat.Articles.Sort();
            }
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
