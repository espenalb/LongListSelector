using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Zedge.Adapters
{
    /// <summary>
    /// Utility method to convert bool in ViewModel to a System.Windows.Visibility.
    /// 
    /// Stolen from http://invokeit.wordpress.com/2013/08/04/super-duper-all-in-one-visibilityconverter-for-wpdev/
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        public enum Mode
        {
            Default,
            Inverted,
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility returnVisibility = Visibility.Visible;
            Mode mode = Mode.Default;
            try
            {
                if (parameter != null)
                    mode = (Mode)Enum.Parse(typeof(Mode), (string)parameter, true);
            }
            catch
            {
                mode = Mode.Default;
            }

            if (value == null)
            {
                returnVisibility = Visibility.Collapsed;
            }
            else if (value is bool)
            {
                bool bVal = (bool)value;
                if (!bVal)
                    returnVisibility = Visibility.Collapsed;
            }
            else if (value is string)
            {
                string itemVal = value as String;

                if (String.IsNullOrWhiteSpace(itemVal))
                    returnVisibility = Visibility.Collapsed;
            }
            else if (value is IList)
            {
                IList objectList = value as IList;
                if (objectList == null || objectList.Count == 0)
                    returnVisibility = Visibility.Collapsed;
            }

            if (mode == Mode.Inverted)
                return returnVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            else
                return returnVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
