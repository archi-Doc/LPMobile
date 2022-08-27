// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using System;
using System.Collections.ObjectModel;
using Tinyhand;

namespace Arc.Views;

public static class Converters
{
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

    public static ulong CultureStringToId(string culture) => culture switch
    {
        "en" => LPMobile.Hashed.Language.En,
        "ja" => LPMobile.Hashed.Language.Ja,
        _ => 0,
    };
}

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

public class CultureListToStringConverter : IValueConverter
{// Culture to String
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is List<int> list)
        {
            var stringList = list.Select(x => HashedString.Get(Converters.CultureIndexToId(x))).ToList();
            /*var collection = new ObservableCollection<string>();
            foreach (var x in list)
            {
                collection.Add(HashedString.Get(x));
            }*/

            return stringList;
        }

        return Array.Empty<string>();
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class CultureToStringConverter : IValueConverter
{// Culture to String
    public static CultureToStringConverter Instance { get; } = new();

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is ulong id)
        {
            return HashedString.Get(id);
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is string st)
        {
            return Converters.CultureStringToId(st);
        }

        return 0;
    }
}

/*public class CultureToStringConverter : IValueConverter
{// Culture to String
    public static CultureToStringConverter Instance { get; } = new();

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is string st)
        {
            switch (st)
            {
                case "en": // eglish
                    return HashedString.Get("Language.En");
                case "ja": // japanese
                    return HashedString.Get("Language.Ja");

                default: // default = english
                    return HashedString.Get("Language.En");
            }
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is string st)
        {
            if (st == HashedString.Get("Language.En"))
            {
                return "en";
            }
            else if (st == HashedString.Get("Language.Ja"))
            {
                return "ja";
            }
        }

        return string.Empty;
    }
}*/

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
}

public class InverseBoolConverter : IValueConverter
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

        return !b;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        bool b;

        if (value is bool b2)
        {
            b = b2;
        }
        else
        {
            b = false;
        }

        return !b;
    }
}*/
