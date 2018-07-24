using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Library.UI.Tools
{
	[ValueConversion(typeof(int), typeof(string))]
	public class PageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((int)value + 1).ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool res = Int32.TryParse(value.ToString(), out int page);
			if (!res) return null;
			return page - 1;
		}
	}
}
