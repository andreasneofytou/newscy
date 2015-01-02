using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.Xaml;

namespace News.Common
{
  public enum Resolutions { WVGA, WXGA, HD, HD16, FULL_HD, UNKNOWN };

  public static class ResolutionHelper
  {
    private static bool IsWvga
    {
      get
      {
        return DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel == 1.2;
      }
    }

    private static bool IsWxga
    {
      get
      {
        return DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel == 2.0;
      }
    }

    private static bool IsHD
    {
      get
      {
        return DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel == 1.8;
      }
    }

    private static bool IsHD16
    {
      get
      {
        return DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel == 1.6;
      }
    }

    private static bool IsFullHD
    {
      get
      {
        return DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel == 2.2;
      }
    }

    public static Resolutions CurrentResolution
    {
      get
      {
        if (IsWvga) return Resolutions.WVGA;
        else if (IsWxga) return Resolutions.WXGA;
        else if (IsHD) return Resolutions.HD;
        else if (IsFullHD) return Resolutions.FULL_HD;
        else if (IsHD16) return Resolutions.HD16;
        else return Resolutions.UNKNOWN;
      }
    }
  }
}
