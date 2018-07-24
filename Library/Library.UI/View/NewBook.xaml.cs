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
	/// Interaction logic for NewBook.xaml
	/// </summary>
	public partial class NewBook : Window
	{
		private ApplicationViewModel viewModel;
		public NewBook(ref ApplicationViewModel vm)
		{
			viewModel = vm;
			DataContext = viewModel;
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (!Int32.TryParse(AgeBox.Text, out int x))
				{
					MessageBox.Show("Wrong birth year");
				}
				if (!Int32.TryParse(YearBox.Text, out int x1))
				{
					MessageBox.Show("Wrong book year");
				}
				if (!Int32.TryParse(PagesBox.Text, out int x2))
				{
					MessageBox.Show("Wrong pages amount");
				}
				if (!Int32.TryParse(DepositBox.Text, out int x3))
				{
					MessageBox.Show("Wrong deposit cost");
				}
				if (!Int32.TryParse(RentBox.Text, out int x4))
				{
					MessageBox.Show("Wrong rent cost");
				}
				viewModel.CheckNewBook();
				if (!viewModel.edit)
				{
					if (viewModel.Add()) MessageBox.Show("Success");
					else MessageBox.Show("Book already exists");
				}
				else
				{
					viewModel.Edit();
					MessageBox.Show("Success");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + " Please, repeat");
			}
		}
	}
}
