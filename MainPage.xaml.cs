using News.Common;
using News.DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace News
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page
  {
    private readonly NavigationHelper navigationHelper;
    private ObservableDictionary defaultViewModel = new ObservableDictionary();
    //private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

    public ObservableDictionary DefaultViewModel
    {
      get { return this.defaultViewModel; }
    }

    /// <summary>
    /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
    /// </summary>
    public NavigationHelper NavigationHelper
    {
      get { return this.navigationHelper; }
    }
    
    public MainPage()
    {
      this.InitializeComponent();
      this.NavigationCacheMode = NavigationCacheMode.Required;

      this.navigationHelper = new NavigationHelper(this);
      this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
      this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
    }

    private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
    {
    }

    private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
    {
      await GetArticles();
      var allGroups = await NewsDataSource.GetGroupsAsync();
      this.defaultViewModel["AllGroups"] = allGroups;
      this.defaultViewModel["RestOfTheGroups"] = allGroups.Reverse().Take(allGroups.Count() - 1).Reverse();
    }

    /// <summary>
    /// Invoked when this page is about to be displayed in a Frame.
    /// </summary>
    /// <param name="e">Event data that describes how this page was reached.
    /// This parameter is typically used to configure the page.</param>
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      navigationHelper.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      this.navigationHelper.OnNavigatedFrom(e);
    }

    private void ArticlesList_articleClick(object sender, ItemClickEventArgs e)
    {
      if(!Frame.Navigate(typeof(ArticlePage), e.ClickedItem))
      {
        throw new Exception("NavigationFailedExceptionMessage");
      }
    }

    private async Task GetArticles()
    {
      bool x = await NewsDataSource.CanReadArticleFiles();
      if (!x)
      {
        await RefreshArticles();
      }
    }

    private void CategoryClick_categoryClick(object sender, ItemClickEventArgs e) 
    {
      if (!Frame.Navigate(typeof(CategoryPage), e.ClickedItem))
      {
        throw new Exception("NavigationFailedExceptionMessage");
      }
    }
    private async void AppBarButtonRefresh_Click(object sender, RoutedEventArgs e) 
    {
      await RefreshArticles();
    } 

    private async Task RefreshArticles()
    {
      if (NetworkInterface.GetIsNetworkAvailable())
      {
        Hub.IsEnabled = false;     
        LoadingRing.IsEnabled = true;
        LoadingRing.IsActive = true;
        LoadingRing.Visibility = Visibility.Visible;
        var allGroups = await NewsDataSource.RefreshGroupsAsync();
        defaultViewModel.Clear();
        this.defaultViewModel["AllGroups"] = allGroups;
        this.defaultViewModel["RestOfTheGroups"] = allGroups.Reverse().Take(allGroups.Count() - 1).Reverse();
        LoadingRing.IsEnabled = false;
        LoadingRing.Visibility = Visibility.Collapsed;
        Hub.Visibility = Visibility.Visible;
        Hub.IsEnabled = true;
      }
      else
      {
        MessageDialog message = new MessageDialog("No network available");
        await message.ShowAsync();
      }
    }
  }
}
