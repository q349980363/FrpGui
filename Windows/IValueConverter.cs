using IniFile;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FrpGui
{

    [ValueConversion(typeof(PropertyValue), typeof(string))]
    class PropertyValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = (PropertyValue)value;
            return item.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = (string)value;
            return (PropertyValue)(item);
        }
    }
}
