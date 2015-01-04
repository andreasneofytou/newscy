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
      SetTemplateForRes();
      if (!CanDownloadSources())
      {
        MessageDialog message = new MessageDialog("Παρακαλώ επιλέξτε πηγές");
        await message.ShowAsync();

        if (!Frame.Navigate(typeof(SourcesPage)))
        {
          throw new Exception("NavigationFailedExceptionMessage");
        }
      }
      else if (defaultViewModel.Count < 1)
      {
        LoadingBar.Visibility = Visibility.Visible;
        LoadingBar.IsEnabled = true;
        var allGroups = await NewsDataSource.GetCategoriesAsync();
        defaultViewModel.Clear();
        this.defaultViewModel["AllCategories"] = allGroups;
        LoadingBar.Visibility = Visibility.Collapsed;
        LoadingBar.IsEnabled = false;
      }
    }

    private void SetTemplateForRes()
    {
      switch (ResolutionHelper.CurrentResolution)
      {
        case Resolutions.FULL_HD:
          MoreTiles.ContentTemplate = (DataTemplate)this.Resources["TileItemTemplateFullHD"];
          break;
        case Resolutions.HD:
          MoreTiles.ContentTemplate = (DataTemplate)this.Resources["TileItemTemplateHD"];
          break;
        case Resolutions.HD16:
          MoreTiles.ContentTemplate = (DataTemplate)this.Resources["TileItemTemplateHD16"];
          break;
        case Resolutions.WVGA:
          MoreTiles.ContentTemplate = (DataTemplate)this.Resources["TileItemTemplateWVGA"];
          break;
        case Resolutions.WXGA:
          MoreTiles.ContentTemplate = (DataTemplate)this.Resources["TileItemTemplateWXGA"];
          break;
        default:
          MoreTiles.ContentTemplate = (DataTemplate)this.Resources["TileItemTemplateWXGA"];
          break;
      }
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

    private bool CanDownloadSources()
    {
      return NewsDataSource.NewsProviders.Where(x => x.IsEnabled == true).Count() != 0;
    }

    private async Task RefreshArticles()
    {

      if (!CanDownloadSources())
      {
        
        if (!Frame.Navigate(typeof(SourcesPage)))
        {
          throw new Exception("NavigationFailedExceptionMessage");
        }
      }
      else if (NetworkInterface.GetIsNetworkAvailable())
      {
        LoadingBar.Visibility = Visibility.Visible;
        LoadingBar.IsEnabled = true;
        var allGroups = await NewsDataSource.RefreshCategoriesAsync();
        defaultViewModel.Clear();
        this.defaultViewModel["AllCategories"] = allGroups;
        await NewsDataSource.SaveSources();
        LoadingBar.Visibility = Visibility.Collapsed;
        LoadingBar.IsEnabled = false;
      }
      else
      {
        MessageDialog message = new MessageDialog("No network available");
        await message.ShowAsync();
      }
    }

    private void AppBarButton_SourceClick(object sender, RoutedEventArgs e)
    {
      if (!Frame.Navigate(typeof(SourcesPage)))
      {
        throw new Exception("NavigationFailedExceptionMessage");
      }
    }
  }
}
