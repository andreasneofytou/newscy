using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.DataModel
{
  public class NewsDataArticle : IComparable
  {
    public string Title { get; set; }
    public string Author { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }

    private DateTime _pubDate;
    public DateTime PubDate
    {
      get { if (_pubDate == null) return DateTime.Now; else return _pubDate; }
      set { this._pubDate = value; }
    }
    public string Link { set; get; }
    public string Source { get; set; }

    public NewsDataArticle(string title, string author, string category, string description, string imageUrl, DateTime pubDate, string link, string source)
    {
      this.Title = title;
      this.Author = author;
      this.Category = category;
      this.Description = description;
      this.ImageUrl = imageUrl;
      this.PubDate = pubDate;
      this.Link = link;
      this.Source = source;
    }

    public NewsDataArticle(XElement article)
    {
      var temp = article.FirstNode;
      while (temp != null)
      {
        SetElementValue((XElement)temp);
        temp = temp.NextNode;
      }
    }

    public NewsDataArticle() { }

    private void SetElementValue(XElement element)
    {
      var name = element.Name.ToString();
      switch (name)
      {
        case "title":
          Title = element.Value;
          break;
        case "link":
          Link = element.Value;
          break;
        case "creator":
          Author = element.Value;
          break;
        case "content":
          Description = RemoveHtmlTags(element.Value);
          break;
        case "description":
          ExtractImage(element.Value);
          Description = RemoveHtmlTags(element.Value);
          break;
        case "author":
          Author = element.Value;
          break;
        case "category":
          Category = element.Value;
          break;
        case "enclosure":
          ImageUrl = element.Attribute("url").Value;
          break;
        case "pubDate":
          string time = element.Value;
          if (time != null && !time.Equals(string.Empty))
          {
            try
            {
              PubDate = DateTime.Parse(time);
            }
            catch
            {
              PubDate = DateTime.Now;
            }
          }
          break;
        case "guid":
          if (element.Value.Contains(".jpg"))
          {
            ImageUrl = element.Value;
          }
          break;
        default:
          if (name.Contains("creator"))
          {
            Author = element.Value;
          }
          else if (name.Contains("content"))
          {
            Description = RemoveHtmlTags(element.Value);
          }
          break;
      }
    }

    private bool ExtractImage(string description)
    {
      if (description.Contains("<img") && description.Contains("src=\"http://dialogos"))
      {
        var f = description.IndexOf("src=\"");
        var b = description.IndexOf("g\"");
        if (f > 1 && b > 1)
        {
          ImageUrl = description.Substring(f + 5, b - 4 - f);
          return true;
        }
      }
      return false;
    }

    private string RemoveHtmlTags(string source)
    {
      string results = source;

      results = results.Replace("&amp;", "&");
      results = results.Replace("&lt;", "<");
      results = results.Replace("&gt;", "<");
      results = results.Replace("&nbsp;", " ");
      results = results.Replace("&quot;", "\"");
      results = results.Replace("&euro;", "€");
      results = results.Replace("&ndash;", "-");
      results = results.Replace("&laquo;", "«");
      results = results.Replace("&raquo;", "»");
      results = results.Replace("&copy;", "©  ");
      results = results.Replace("&pound;", "£");
      results = results.Replace("&#39;", "\'");
      results = results.Replace("&rsquo;", "’");
      results = results.Replace("&lsquo;", "‘");
      results = results.Replace("&rdquo;", "”");
      results = results.Replace("&ldquo;", "“");
      results = results.Replace("&micro;", "μ");
      results = results.Replace("&middot;", "·");
      results = results.Replace("&frac12;", "½");
      results = results.Replace("&dec;", "°");
      results = results.Replace("&hellip;", "…");
      results = results.Replace("&#8216;", "‘");
      results = results.Replace("&#8217", "’");
      results = results.Replace("\t", "");
      results = results.Replace("<br />\n<br />", "\n");
      results = results.Replace("<br />", "");
      results = Regex.Replace(results, "<.*?>", "");

      return results;
    }

    public override string ToString()
    {
      return this.Title;
    }

    public int CompareTo(object obj)
    {
      if (obj is NewsDataArticle)
      {
        var a = this.PubDate > (obj as NewsDataArticle).PubDate;  // compare user names
        return a ? 1 : 0;
      }

      throw new ArgumentException("");
    }

  }
}
