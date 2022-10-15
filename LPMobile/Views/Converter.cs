// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using System;
using System.Collections.ObjectModel;
using Tinyhand;

namespace Arc.Views;

public static class Converters
{
    public const int ScaleIndexMax = 10;

    public static List<int> ScaleIndexList { get; } = Enumerable.Range(0, ScaleIndexMax).ToList();

    public static double ScaleIndexToScale(int index) => index switch
    {
        0 => 0.50,
        1 => 0.67,
        2 => 0.80,
        3 => 0.90,
        4 => 1.00,
        5 => 1.10,
        6 => 1.25,
        7 => 1.50,
        8 => 1.75,
        9 => 2.00,
        _ => 1.00,
    };

    public static int ScaleToScaleIndex(double scale)
    {
        for (var i = 0; i < ScaleIndexMax; i++)
        {
            if (scale <= ScaleIndexToScale(i))
            {
                return i;
            }
        }

        return ScaleIndexMax - 1;
    }

    public const int CultureIndexMax = 2;

    public static List<int> CultureIndexList { get; } = Enumerable.Range(0, CultureIndexMax).ToList();

    public static int CultureStringToIndex(string culture) => culture switch
    {
        "en" => 0,
        "ja" => 1,
        _ => 0,
    };

    public static string CultureIndexToString(int index) => index switch
    {
        0 => "en",
        1 => "ja",
        _ => "en",
    };

    public static ulong CultureIndexToId(int index) => index switch
    {
        0 => LPMobile.Hashed.Language.En,
        1 => LPMobile.Hashed.Language.Ja,
        _ => LPMobile.Hashed.Language.En,
    };
}

public class ScaleListToStringConverter : IValueConverter
{// DisplayScaling to String
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is List<int> list)
        {
            var x = list.Select(x => $"{(int)(Converters.ScaleIndexToScale(x) * 100d)} %").ToList();
            return x;
        }

        return Array.Empty<string>();
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class CultureListToStringConverter : IValueConverter
{// Culture to String
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is List<int> list)
        {
            var stringList = list.Select(x => HashedString.Get(Converters.CultureIndexToId(x))).ToList();
            return stringList;
        }

        return Array.Empty<string>();
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/*public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        bool b = false;

        if (value is bool)
        {
            b = (bool)value;
        }
        else if (value is bool?)
        {
            var b2 = (bool?)value;
            b = b2.HasValue ? b2.Value : false;
        }

        if (parameter != null)
        { // Reverse conversion on any given parameter.
            b = !b;
        }

        return b ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        bool b;

        if (value is System.Windows.Visibility v)
        {
            b = v == System.Windows.Visibility.Visible;
        }
        else
        {
            b = false;
        }

        if (parameter != null)
        { // Reverse conversion on any given parameter.
            b = !b;
        }

        return b;
    }
}*/

/*public class DateTimeToStringConverter : IValueConverter
{// DateTime to String
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value != null && value is DateTime)
        {
            var dt = (DateTime)value;
            if (dt.Ticks == 0)
            {
                return string.Empty;
            }

            return dt.ToString(HashedString.Get("App.DateTime"));
        }

        return System.Windows.DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class DisplayScalingToStringConverter : IValueConverter
{// Display scaling to String
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is double d)
        {
            return (d * 100).ToString("F0") + "%";
        }
#if XAMARIN
        return null;;
#else
        return System.Windows.DependencyProperty.UnsetValue;
#endif
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        double d = 1;

        if (value is string st)
        {
            d = double.Parse(st.TrimEnd(new char[] { '%', ' ' })) / 100;
        }

        return d;
    }
}*/
