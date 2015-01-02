using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.DataModel
{
  public class NewsDataCategory
  {
    public const string CYPRUS = "Cyprus";
    public const string GREECE = "Greece";
    public const string WORLD = "World";
    public const string ECONOMY = "Economy";
    public const string TECHNOLOGY = "Technology";
    public const string WEIRD = "Weird";
    public const string RECIPES = "Recipes";
    public const string TOP_STORIES = "Top Stories";
    public const string CULTURE = "Culture";
    public const string SPORTS = "Sports";
    public const string INTERVIEWS = "Interviews";
    public const string ABSURD = "Absurd";
    public const string ENVIRONMENT = "Environment";
    public const string POLITICS = "Politics";
    public const string SCIENCE = "Science";

    public const string CYPRUS_EL = "Κύπρος";
    public const string GREECE_EL = "Ελλάδα";
    public const string WORLD_EL = "Διεθνή";
    public const string ECONOMY_EL = "Οικονομία";
    public const string TECHNOLOGY_EL = "Τεχνολογία";
    public const string WEIRD_EL = "Παράξενα";
    public const string RECIPES_EL = "Συνταγές";
    public const string TOP_STORIES_EL = "Κορυφαία";
    public const string CULTURE_EL = "Κουλτούρα";
    public const string SPORTS_EL = "Αθλητικά";
    public const string INTERVIEWS_EL = "Συνεντεύξεις";
    public const string ABSURD_EL = "Παράλογος";
    public const string ENVIRONMENT_EL = "Περιβάλον";
    public const string POLITICS_EL = "Πολιτική";
    public const string SCIENCE_EL = "Επιστήμη";
    
    public string Title { get; set; }
    public string GreekTitle { get; set; }
    public string Url { get; set; }
    public string Description { get; set; }
    public List<NewsDataArticle> Articles { get; set; }
    public IEnumerable<NewsDataArticle> TopArticles
    {
      set { } 
      get 
      { 
        if(Articles == null)
        {
          return null;
        }
        else if (Articles.Count >14)
        {
          return this.Articles.Take(15);
        }
        else
        {
          return this.Articles;
        }
      }
    }

    public NewsDataCategory(string title, string greekTitle, string description, string url)
    {
      this.Title = title;
      this.GreekTitle = greekTitle;
      this.Url = url;
      this.Description = description;
    }

    public NewsDataCategory() { }
  }
}
