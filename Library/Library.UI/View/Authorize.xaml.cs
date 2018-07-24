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
	/// Interaction logic for Authorize.xaml
	/// </summary>
	public partial class Authorize : Window
	{
		private AuthorizeViewModel viewModel;
		public Authorize()
		{
			viewModel = new AuthorizeViewModel();
			DataContext = viewModel;
			InitializeComponent();
			MainFrame.Content = new AuthPage(ref viewModel);
		}
	}
}
