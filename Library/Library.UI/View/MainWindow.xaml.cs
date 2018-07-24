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
using Library.UI;
using Library.UI.ViewModel;


namespace Library.UI.View
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ApplicationViewModel viewModel;
		public MainWindow()
		{
			viewModel = new ApplicationViewModel();
			DataContext = viewModel;
			viewModel.Refresh();
			InitializeComponent();
			if (viewModel.CheckAdmin())
			{
				MainFrame.Content = new AdminBooksPage(ref viewModel);
				editButton.Visibility = Visibility.Visible;
				getButton.Visibility = Visibility.Collapsed;
				increaseAmountButton.Visibility = Visibility.Visible;
				//AddBookField.Visibility = Visibility.Visible;
			}
			else
			{
				//AddBookField.Visibility = Visibility.Collapsed;
				addBook.Visibility = Visibility.Collapsed;
				MainFrame.Content = new ClientPage(ref viewModel);
				getButton.Visibility = Visibility.Visible;
				editButton.Visibility = Visibility.Collapsed;
			}
			settingsButton.ContextMenu.IsEnabled = false;
			infoButton.ContextMenu.IsEnabled = false;
		}

		private void ListBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (viewModel.CheckAdmin())
			{
				MainFrame.Content = new AdminBooksPage(ref viewModel);
				editButton.Visibility = Visibility.Visible;
				getButton.Visibility = Visibility.Collapsed;
				increaseAmountButton.Visibility = Visibility.Visible;
			}
			else
			{
				MainFrame.Content = new ClientPage(ref viewModel);
				getButton.Visibility = Visibility.Visible;
				editButton.Visibility = Visibility.Collapsed;
			}
			viewModel.Refresh();
			headerName.Text = "Books";
			infoPanel.Visibility = Visibility.Visible;
			searchField.Visibility = Visibility.Visible;
		}
		private void ListBoxItem1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (viewModel.CheckAdmin())
			{
				MainFrame.Content = new AdminPendingOffersPage(ref viewModel);
			}
			else
			{
				MainFrame.Content = new PendingOfferPage(ref viewModel);
			}
			headerName.Text = "Pending Offers";
			infoPanel.Visibility = Visibility.Collapsed;
			getButton.Visibility = Visibility.Collapsed;
			editButton.Visibility = Visibility.Collapsed;
			searchField.Visibility = Visibility.Collapsed;
			increaseAmountButton.Visibility = Visibility.Collapsed;
		}
		private void ListBoxItem2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (viewModel.CheckAdmin())
			{
				MainFrame.Content = new AdminClosedOffersPage(ref viewModel);
			}
			else
			{
				MainFrame.Content = new ClosedOffersPage(ref viewModel);
			}
			headerName.Text = "Closed Offers";
			infoPanel.Visibility = Visibility.Collapsed;
			getButton.Visibility = Visibility.Collapsed;
			editButton.Visibility = Visibility.Collapsed;
			searchField.Visibility = Visibility.Collapsed;
			increaseAmountButton.Visibility = Visibility.Collapsed;
		}
		private void AddBook_Click(object sender, RoutedEventArgs e)
		{
			NewBook newbook = new NewBook(ref viewModel);
			newbook.ShowDialog();
			settingsButton.ContextMenu.IsEnabled = false;
		}
		private void ButtonClose_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
		{
			ButtonOpenMenu.Visibility = Visibility.Collapsed;
			ButtonCloseMenu.Visibility = Visibility.Visible;
		}

		private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
		{
			ButtonOpenMenu.Visibility = Visibility.Visible;
			ButtonCloseMenu.Visibility = Visibility.Collapsed;
		}
		private void SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			Button btn = sender as Button;
			if (btn.ContextMenu.IsEnabled)
			{
				btn.ContextMenu.IsEnabled = false;
			}
			else
			{
				btn.ContextMenu.IsEnabled = true;
				btn.ContextMenu.PlacementTarget = (sender as Button);
				btn.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
				btn.ContextMenu.IsOpen = true;
			}
		}

		private void InfoButton_Click(object sender, RoutedEventArgs e)
		{
			Button btn = sender as Button;
			if (btn.ContextMenu.IsEnabled)
			{
				btn.ContextMenu.IsEnabled = false;
			}
			else
			{
				btn.ContextMenu.IsEnabled = true;
				btn.ContextMenu.PlacementTarget = (sender as Button);
				btn.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
				btn.ContextMenu.IsOpen = true;
			}
		}
		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			viewModel.NewBook = new BookViewModel(new BusinessLogic.Book(viewModel.SelectedBook.SBook.GetModel()));
			viewModel.edit = true;
			NewBook newBook = new NewBook(ref viewModel);
			newBook.ShowDialog();
		}

		private void Search_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			viewModel.Find();
		}

		private void Home_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			viewModel.TmpBookName = "";
			viewModel.Refresh();
			viewModel.RefreshPages();
		}

		private void LoadButton_Click(object sender, RoutedEventArgs e)
		{
			viewModel.LoadFromFile();
			settingsButton.ContextMenu.IsEnabled = false;
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			viewModel.SaveToFile();
			settingsButton.ContextMenu.IsEnabled = false;
		}

		private void SignOutButton_Click(object sender, RoutedEventArgs e)
		{
			Authorize window = new Authorize();
			window.Show();
			Close();
		}

		private void searchField_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				viewModel.Find();
			}
		}

		private void listViewItem5_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (viewModel.CheckAdmin())
			{
				MainFrame.Content = new AdminStatsPage(ref viewModel);
			}
			else
			{
				MainFrame.Content = new ClientStats(ref viewModel);
			}
			headerName.Text = "Stats";
			infoPanel.Visibility = Visibility.Collapsed;
			getButton.Visibility = Visibility.Collapsed;
			editButton.Visibility = Visibility.Collapsed;
			searchField.Visibility = Visibility.Collapsed;
		}

		private void AddMoney_Click(object sender, RoutedEventArgs e)
		{
			Miner mine = new Miner(ref viewModel);
			mine.ShowDialog();
		}
		private void IncreaseAmountButton_Click(object sender, RoutedEventArgs e)
		{
			AmountPlusPlus amountWindow = new AmountPlusPlus(ref viewModel);
			amountWindow.ShowDialog();
		}
		
	}
}
