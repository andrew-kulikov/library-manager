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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Library.UI.ViewModel;

namespace Library.UI.View
{
	/// <summary>
	/// Interaction logic for ClientStats.xaml
	/// </summary>
	public partial class ClientStats : Page
	{
		private ApplicationViewModel viewModel;
		public ClientStats(ref ApplicationViewModel vm)
		{
			viewModel = vm;
			InitializeComponent();
			DataContext = vm;
		}
	}
}
