using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.DataModel
{
  class NewsDataProvider
  {
    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<NewsDataCategory> Categories { get; private set; }
    public string ImageUrl { get; private set; }
    public string Url { get; private set; }
    public bool IsEnebled { get; set; }
  }
}
