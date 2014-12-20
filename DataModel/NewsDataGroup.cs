using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.DataModel
{  
  public class NewsDataGroup
  {
    public string Title { get; set; }
    public string GreekTitle { get; set; }
    public string Description { get; set; }
    public string ImagePath { set; get; }
    public ObservableCollection<NewsDataArticle> Articles { get; set; }

    public override string ToString()
    {
      return this.Title;
    }

    public NewsDataGroup()
    {
      Articles = new ObservableCollection<NewsDataArticle>();
    }

    public IEnumerable<NewsDataArticle> TopArticles
    {
      set { }
      get { return this.Articles.Take(15); }
    }
  }
}
