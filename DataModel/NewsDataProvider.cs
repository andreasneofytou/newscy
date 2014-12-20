using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.DataModel
{
  public class NewsDataProvider
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public List<NewsDataCategory> Categories { get; set; }
    public string ImageUrl { get; set; }
    public string Url { get; set; }
    public bool IsEnabled { get; set; }

    public NewsDataProvider() { }
  }
}
