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
	/// Interaction logic for AuthPage.xaml
	/// </summary>
	public partial class AuthPage : Page
	{
		private AuthorizeViewModel viewModel;
		public AuthPage(ref AuthorizeViewModel vm)
		{
			viewModel = vm;
			DataContext = viewModel;
			InitializeComponent();
		}

		private void Login(object sender, RoutedEventArgs e)
		{
			viewModel.Password = passwordBox.Password;
			if (viewModel.Authorize())
			{
				MainWindow window = new MainWindow();
				window.Show();
				Window.GetWindow(this).Close();
			}
			else
			{
				MessageBox.Show("Incorrect input");
			}
		}
		private void Register(object sender, RoutedEventArgs e)
		{
			SignIn page = new SignIn(ref viewModel);
			NavigationService.Navigate(page);
		}
	}
}
