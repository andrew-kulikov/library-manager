using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Library.UI;
using Library.BusinessLogic;

namespace Library.UI.ViewModel
{
	public class BookViewModel : INotifyPropertyChanged
	{
		private Book book;

		public BookViewModel(Book book)
		{
			this.book = book;
			OnPropertyChanged("Genres");
		}

		public Book SBook
		{
			get => book;
			set
			{
				book = value;
				OnPropertyChanged();
			}
		}
		public string Genres
		{
			get
			{
				string s = "";
				foreach (var genre in book.Genres)
				{
					s += genre + " ";
				}
				return s;
			}
		}
		public string Price => "Deposit: " + book.DepositCost.ToString() + "$, Rent: " + book.RentCost + "$";
		public string Name
		{
			get => book.Name;
			set
			{
				book.Name = value;
			}
		}
		public int Amount => BusinessLogic.Archive.Amounts[book.Isbn]; 
		public string ImagePath
		{
			get
			{
				string directory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "Images");
				if (File.Exists(directory + "//" + book.Name + ".jpg"))
				{
					return directory + "//" + book.Name + ".jpg";
				}
				else
				{
					return directory + "//default.jpg";
				}
			}
		}
		public void OnPropertyChanged([CallerMemberName]string prop = "") =>
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
