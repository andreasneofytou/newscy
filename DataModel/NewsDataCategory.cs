using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.DataModel
{
  class NewsDataCategory
  {
    public string Title { get; set; }
    public string GreekTitle { get; set; }
    public string Url { get; set; }
    public string Description { get; set; }

    public NewsDataCategory(string title, string greekTitle, string description, string url)
    {
      this.Title = title;
      this.GreekTitle = greekTitle;
      this.Url = url;
      this.Description = description;
    }
  }
}
