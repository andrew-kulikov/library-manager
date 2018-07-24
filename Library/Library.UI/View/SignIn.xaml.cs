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
	/// Interaction logic for SignIn.xaml
	/// </summary>
	public partial class SignIn : Page
	{
		private AuthorizeViewModel viewModel;
		public SignIn(ref AuthorizeViewModel vm)
		{
			viewModel = vm;
			DataContext = viewModel;
			InitializeComponent();
		}

		private void Register(object sender, RoutedEventArgs e)
		{
			try
			{
				if (Name.Text == "")
				{
					throw new Exception("Name field is empty");
				}
				if (Age.Text == "")
				{
					throw new Exception("Birth year field is empty");
				}
				if (Sex.Text == "")
				{
					throw new Exception("Sex field is empty");
				}
				if (Login.Text == "")
				{
					throw new Exception("Login field is empty");
				}
				if (Password.Password == "")
				{
					throw new Exception("Password field is empty");
				}
				if (Password.Password != CheckPassword.Password)
				{
					throw new Exception("Wrong password");
				}
				viewModel.Register(Name.Text, Convert.ToInt32(Age.Text), Sex.Text, Login.Text, Password.Password);
				MessageBox.Show("Success");
				MainWindow window = new MainWindow();
				window.Show();
				Window.GetWindow(this).Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void Back_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new AuthPage(ref viewModel));
		}
	}
}
