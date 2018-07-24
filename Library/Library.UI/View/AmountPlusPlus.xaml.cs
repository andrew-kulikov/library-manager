using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Library.UI.ViewModel;

namespace Library.UI.View
{
	/// <summary>
	/// Interaction logic for AmountPlusPlus.xaml
	/// </summary>
	public partial class AmountPlusPlus : Window
	{
		private ApplicationViewModel vm;
		public AmountPlusPlus(ref ApplicationViewModel vm)
		{
			this.vm = vm;
			InitializeComponent();
			DataContext = vm;
		}

		private void Amount_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (!Int32.TryParse(amount.Text, out int kek))
				{
					MessageBox.Show("Wrong input");
					return;
				}
				
				vm.IncreaseAmount(kek);
			}
		}
	}
}
