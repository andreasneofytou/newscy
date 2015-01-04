using News.Common;
using News.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
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
  public sealed partial class SourcesPage : Page
  {
    private readonly NavigationHelper navigationHelper;
    private ObservableDictionary defaultViewModel = new ObservableDictionary();

    public SourcesPage()
    {
      this.InitializeComponent();

      this.navigationHelper = new NavigationHelper(this);
      this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
      this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
    }

    /// <summary>
    /// Invoked when this page is about to be displayed in a Frame.
    /// </summary>
    /// <param name="e">Event data that describes how this page was reached.
    /// This parameter is typically used to configure the page.</param>
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      this.navigationHelper.OnNavigatedTo(e);
    }
    /// <summary>
    /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
    /// </summary>
    public NavigationHelper NavigationHelper
    {
      get { return this.navigationHelper; }
    }

    /// <summary>
    /// Gets the view model for this <see cref="Page"/>.
    /// This can be changed to a strongly typed view model.
    /// </summary>
    public ObservableDictionary DefaultViewModel
    {
      get { return this.defaultViewModel; }
    }

    /// <summary>
    /// Populates the page with content passed during navigation.  Any saved state is also
    /// provided when recreating a page from a prior session.
    /// </summary>
    /// <param name="sender">
    /// The source of the event; typically <see cref="NavigationHelper"/>
    /// </param>
    /// <param name="e">Event data that provides both the navigation parameter passed to
    /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
    /// a dictionary of state preserved by this page during an earlier
    /// session.  The state will be null the first time a page is visited.</param>
    private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
    {
      DataContext = NewsDataSource.NewsProviders;
      SetTemplateForRes();      
    }

    private void SetTemplateForRes()
    {
      switch(ResolutionHelper.CurrentResolution)
      {
        case Resolutions.FULL_HD:
          SourceList.ItemTemplate = (DataTemplate)this.Resources["SourceListTemplateFullHD"];
          break;
        case Resolutions.HD:
          SourceList.ItemTemplate = (DataTemplate)this.Resources["SourceListTemplateHD"];
          break;
        case Resolutions.WVGA:
          SourceList.ItemTemplate = (DataTemplate)this.Resources["SourceListTemplateWVGA"];
          break;
        case Resolutions.WXGA:
          SourceList.ItemTemplate = (DataTemplate)this.Resources["SourceListTemplateWXGA"];
          break;
        case Resolutions.HD16:
          SourceList.ItemTemplate = (DataTemplate)this.Resources["SourceListTemplateHD16"];
          break;
        default:
          SourceList.ItemTemplate = (DataTemplate)this.Resources["SourceListTemplateWXGA"];
          break;
      }
    }

    /// <summary>
    /// Preserves state associated with this page in case the application is suspended or the
    /// page is discarded from the navigation cache.  Values must conform to the serialization
    /// requirements of <see cref="SuspensionManager.SessionState"/>.
    /// </summary>
    /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
    /// <param name="e">Event data that provides an empty dictionary to be populated with
    /// serializable state.</param>
    private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
    {
    }

    protected async override void OnNavigatedFrom(NavigationEventArgs e)
    {
      await NewsDataSource.SaveSources();
    }

  }
}
