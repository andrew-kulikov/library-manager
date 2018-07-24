using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Library.UI;
using Library.BusinessLogic;
using System.Collections.ObjectModel;
using Library.UI.Tools;
using System.Windows;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Library.UI.ViewModel
{
	public class ApplicationViewModel : INotifyPropertyChanged
	{
		private BookViewModel selectedBook;
		private Offer selectedOffer;
		private ObservableCollection<BookViewModel> books;
		private ObservableCollection<BookViewModel> curBookPage;
		private ObservableCollection<BookViewModel> nextBookPage;
		private ObservableCollection<BookViewModel> prevBookPage;
		private Client client;
		private int pageSize = 3;
		private int curPageNumber = 0;
		private RelayCommand nextPageCommand;
		private RelayCommand prevPageCommand;
		private RelayCommand firstPageCommand;
		private RelayCommand lastPageCommand;
		private RelayCommand offerCommand;
		private RelayCommand returnCommand;
		private RelayCommand addCommand;
		private Dictionary<string, bool> jenres;
		private BookViewModel newBook;
		private RelayCommand editCommand;
		private string tmpBookName = "";

		public ApplicationViewModel()
		{
			jenres = new Dictionary<string, bool>
			{
				["Detective"] = false,
				["Distopia"] = false,
				["Fantasy"] = false,
				["Classic"] = false,
				["ReferenceBook"] = false,
				["PopularScience"] = false
			};
			books = new ObservableCollection<BookViewModel>();
			foreach (var book in Archive.Books)
			{
				books.Add(new BookViewModel(book.Value));
			}
			LoadNextPage();
			newBook = new BookViewModel(new Book(new Person("", 0, "male")));
			nextBookPage = new ObservableCollection<BookViewModel>();
			prevBookPage = new ObservableCollection<BookViewModel>();
			curBookPage = new ObservableCollection<BookViewModel>(books.Take(pageSize).ToArray());
			client = Library.BusinessLogic.Library.AuthorizedUser as Library.BusinessLogic.Client;
		}

		public string UserName => BusinessLogic.Library.AuthorizedUser.Name + " (" + BusinessLogic.Library.AuthorizedUser.Login + ")";
		public string MoneyLeft => "Money left: " + ((BusinessLogic.Library.AuthorizedUser as Client)?.Balance.ToString() ?? "0");
		public bool edit = false;
		public Offer SelectedOffer
		{
			get => selectedOffer;
			set
			{
				selectedOffer = value;
				OnPropertyChanged();
			}
		}
		public string TmpBookName
		{
			get => tmpBookName;
			set
			{
				tmpBookName = value;
				OnPropertyChanged();
			}
		}
		public BookViewModel NewBook
		{
			get => newBook;
			set
			{
				newBook = value;
				OnPropertyChanged();
			}
		}
		public Client CurClient
		{
			get => client;
			set
			{
				client = value;
				OnPropertyChanged();
			}
		}
		public Dictionary<string, bool> Jenres
		{
			get => jenres;
			set
			{
				jenres = value;
				OnPropertyChanged();
			}
		}
		public BookViewModel SelectedBook
		{
			get => selectedBook;
			set
			{
				selectedBook = value;
				OnPropertyChanged();
			}
		}
		public ObservableCollection<BookViewModel> Books
		{
			get => books;
			set
			{
				books = value;
				OnPropertyChanged();
			}
		}
		public int CurrentPageNumber
		{
			get => curPageNumber;
			set
			{
				curPageNumber = value;
				OnPropertyChanged();
			}
		}
		public ObservableCollection<BookViewModel> CurBookPage
		{
			get => curBookPage;
			set
			{
				curBookPage = value;
				OnPropertyChanged();
			}
		}
		public ObservableCollection<Offer> PendingOffers => new ObservableCollection<Offer>(BusinessLogic.Library.ClientPendingOffers());
		public ObservableCollection<Offer> ClosedOffers => new ObservableCollection<Offer>(BusinessLogic.Library.ClientClosedOffers());
		public ObservableCollection<Offer> AdminPendingOffers => new ObservableCollection<Offer>(Archive.PendingOffers.Values);
		public ObservableCollection<Offer> AdminClosedOffers => new ObservableCollection<Offer>(Archive.CompletedOffers.Values);
		public int ClientClosedOfferAmount => client.Card.CompletedOffers.Count;
		public int ClientPendingOfferAmount => client.Card.PendingOffers.Count;
		public int ClientBooksRead => new SortedSet<string>(Library.BusinessLogic.Library.ClientClosedOffers().Select(off => off.Book.Isbn)).Count;
		public double MoneySpent
		{
			get
			{
				double sum = 0;
				foreach (var off in Library.BusinessLogic.Library.ClientClosedOffers())
				{
					sum += off.Book.RentCost;
				}
				foreach (var off in Library.BusinessLogic.Library.ClientPendingOffers())
				{
					sum += off.Book.DepositCost;
				}
				return sum;
			}
		}

		public int ClosedOfferAmount => Archive.CompletedOffers.Values.Count;
		public int PendingOfferAmount => Archive.PendingOffers.Values.Count;
		public int BooksRead => new SortedSet<string>(Archive.CompletedOffers.Values.Select(off => off.Book.Isbn)).Count;
		public double MoneySpentAll
		{
			get
			{
				double sum = 0;
				foreach (var off in Archive.CompletedOffers.Values)
				{
					sum += off.Book.RentCost;
				}
				foreach (var off in Archive.PendingOffers.Values)
				{
					sum += off.Book.DepositCost;
				}
				return sum;
			}
		}
		public string GoodUser
		{
			get
			{
				int maxBooks = 0;
				Client good = null;
				foreach (var cl in Archive.Clients)
				{
					if (cl.Value.Card.CompletedOffers.Count > maxBooks)
					{
						maxBooks = cl.Value.Card.CompletedOffers.Count;
						good = cl.Value;
					}
				}
				return good?.Name + "(" + good?.Login + ")"; 
			}
		}

		public void ParseJenres()
		{
			NewBook.SBook.Genres = new List<BookJenre>();
			foreach (var jenre in jenres)
			{
				if (jenre.Value == true)
				{
					NewBook.SBook.Genres.Add((BookJenre)Enum.Parse(typeof(BookJenre), jenre.Key));
				}
			}
		}
		public bool CheckAdmin()
		{
			return BusinessLogic.Library.AuthorizedUser is Admin;
		}
		public void Refresh()
		{
			Books = new ObservableCollection<BookViewModel>();
			foreach (var book in Archive.Books)
			{
				Books.Add(new BookViewModel(book.Value));
			}
			if (curBookPage.Count > 0) selectedBook = curBookPage[0];
			else selectedBook = null;
		}
		public void LoadNextPage()
		{
			Task t = Task.Factory.StartNew(() =>
			{
				nextBookPage = new ObservableCollection<BookViewModel>(
					books.Skip((curPageNumber + 1) * pageSize).Take(pageSize).ToArray());

			});
		}
		public void LoadPrevPage()
		{
			Task t = Task.Factory.StartNew(() =>
			{
				if (CurrentPageNumber <= 0)
				{
					prevBookPage = new ObservableCollection<BookViewModel>();
					CurrentPageNumber = 0;
					return;
				}
				prevBookPage = new ObservableCollection<BookViewModel>(
					books.Skip((curPageNumber - 1) * pageSize).Take(pageSize).ToArray());
			});
		}
		public RelayCommand NextPageCommand => nextPageCommand ?? (
			nextPageCommand = new RelayCommand(obj =>
			{

				if (nextBookPage == null || nextBookPage.Count == 0) return;
				CurrentPageNumber++;
				CurBookPage = new ObservableCollection<BookViewModel>(nextBookPage);
				try
				{
					SelectedBook = curBookPage[0];
				}
				catch
				{
					SelectedBook = null;
				}

				LoadNextPage();
				LoadPrevPage();
			}));
		public RelayCommand PrevPageCommand => prevPageCommand ?? (
			prevPageCommand = new RelayCommand(obj =>
			{

				if (prevBookPage == null || prevBookPage.Count == 0) return;
				CurrentPageNumber--;
				CurBookPage = new ObservableCollection<BookViewModel>(prevBookPage);
				try
				{
					SelectedBook = curBookPage[0];
				}
				catch
				{
					SelectedBook = null;
				}
				LoadNextPage();
				LoadPrevPage();
			}));
		public RelayCommand FirstPageCommand => firstPageCommand ?? (
			firstPageCommand = new RelayCommand(obj =>
			{
				curPageNumber = 0;

				CurBookPage = new ObservableCollection<BookViewModel>(books.Take(pageSize).ToArray());
				try
				{
					SelectedBook = curBookPage[0];
				}
				catch
				{
					SelectedBook = null;
				}


				LoadNextPage();
				LoadPrevPage();
			}));
		public RelayCommand LastPageCommand => lastPageCommand ?? (
			lastPageCommand = new RelayCommand(obj =>
			{
				CurrentPageNumber = (books.Count - 1) / pageSize;

				CurBookPage = new ObservableCollection<BookViewModel>(
					books.Skip(curPageNumber * pageSize).Take(pageSize).ToArray());
				try
				{
					SelectedBook = curBookPage[0];
				}
				catch
				{
					SelectedBook = null;
				}

				LoadNextPage();
				LoadPrevPage();
			}));
		public RelayCommand OfferCommand => offerCommand ?? (
			offerCommand = new RelayCommand(obj =>
			{
				if (BusinessLogic.Library.AuthorizedUser is Client)
				{
					bool res = (BusinessLogic.Library.AuthorizedUser as Client).AskFor(selectedBook.SBook.Isbn);
					if (res)
					{
						MessageBox.Show("Offer created successfully");
					}
					else
					{
						MessageBox.Show("Not enough money");
					}
					RefreshPages();
					OnPropertyChanged("MoneyLeft");
					OnPropertyChanged("AdminPendingOffers");
					OnPropertyChanged("AdminCompletedOffers");
				}
			}));
		public RelayCommand ReturnCommand => returnCommand ?? (
			returnCommand = new RelayCommand(obj =>
			{
				if (BusinessLogic.Library.AuthorizedUser is Client && selectedOffer != null)
				{
					(BusinessLogic.Library.AuthorizedUser as Client).ReturnBook(selectedOffer.Id);
					RefreshPages();
					OnPropertyChanged("PendingOffers");
					OnPropertyChanged("CompletedOffers");
					OnPropertyChanged("MoneyLeft");
					OnPropertyChanged("AdminPendingOffers");
					OnPropertyChanged("AdminCompletedOffers");
				}
			}));
		public RelayCommand AddCommand => addCommand ?? (
			addCommand = new RelayCommand(obj =>
			{
				if (!edit)
				{
					ParseJenres();
					if (Archive.AddBook(newBook.SBook))
					{
						Books.Add(newBook);
						OnPropertyChanged("Books");
					}
					else throw new Exception("Book already exists");
				}
			}));
		public bool Add()
		{
			if (!edit)
			{
				ParseJenres();
				if (Archive.AddBook(newBook.SBook))
				{
					Books.Add(newBook);
					OnPropertyChanged("Books");
					return true;
				}
				return false;
			}
			return false;
		}
		public void ShutupAndTakeMyMoney(int sum)
		{
			bool res = BusinessLogic.Library.AddMoney(sum);
			if (res)
			{
				MessageBox.Show("Ok");
			}
			else
			{
				MessageBox.Show("Admin cannot have money");
			}
		}
		public void IncreaseAmount(int delta)
		{
			try
			{
				BusinessLogic.Library.IncreaseAmount(SelectedBook.SBook, delta);
				MessageBox.Show("Ok");
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}
		private string GetFileName()
		{
			OpenFileDialog od = new OpenFileDialog();
			od.Filter = "Json|*.json";
			if (od.ShowDialog() == true)
			{
				return od.FileName;
			}
			return null;
		}
		public void CheckNewBook()
		{
			if (newBook.Name == null || newBook.Name == "" || !Regex.IsMatch(newBook.Name, @"^[a-zA-Z0-9,. ]*$")) throw new Exception("Wrong book name");
			if (newBook.SBook.Isbn == null ||
				newBook.SBook.Isbn == ""  ||
				!Regex.IsMatch(newBook.SBook.Isbn, @"^[-a-zA-z0-9]*$")) throw new Exception("Wrong isbn");
			if (!Regex.IsMatch(newBook.SBook.Author.Name, @"^[a-zA-Z ]*$")) throw new Exception("Wrong author name");
			if (newBook.SBook.Year > DateTime.Now.Year) throw new Exception("Wrong book year");
			if (newBook.SBook.Author.Age < -1000 || newBook.SBook.Author.Age > DateTime.Now.Year) throw new Exception("Wrong author birth date");
			if (newBook.SBook.Pages < 1) throw new Exception("Wrong pages amount");
			if (newBook.SBook.DepositCost < newBook.SBook.RentCost) throw new Exception("Deposit cost must be greater than rent");

		}
		public void Edit()
		{
			int amount = Archive.Amounts[selectedBook.SBook.Isbn];
			
			ParseJenres();
			Book newB = new Book(NewBook.SBook.GetModel());
			Archive.ChangeBook(newB);
			
			Books = new ObservableCollection<BookViewModel>();
			foreach (var book in Archive.Books)
			{
				Books.Add(new BookViewModel(book.Value));
			}
			SelectedBook = new BookViewModel(newB);
			RefreshPages();
			OnPropertyChanged("SelectedBook");
			OnPropertyChanged("Books");
			OnPropertyChanged("CurBookPage");
			edit = false;
		}
		public void RefreshPages()
		{
			
			CurBookPage = new ObservableCollection<BookViewModel>(books.Take(pageSize).ToArray());
			curPageNumber = 0;
			try
			{
				SelectedBook = curBookPage[0];
			}
			catch
			{
				SelectedBook = null;
			}

			LoadPrevPage();
			LoadNextPage();
		}
		public bool Find()
		{
			if (tmpBookName == "")
			{
				TmpBookName = "";
				Refresh();
				RefreshPages();
				return false;
			}
			Refresh();
			bool res = false;

			var newBooks = from book in books
						   where book.Name.ToLower().Contains(tmpBookName.ToLower())
						   select book;
			Books = new ObservableCollection<BookViewModel>(newBooks.ToArray());
			RefreshPages();
			return res;
		}
		public void LoadFromFile()
		{
			string name = GetFileName();
			if (name != null)
			{
				Library.BusinessLogic.Library.LoadArchive(name);
				Refresh();
				RefreshPages();
				MessageBox.Show("Success");
			}
		}
		public void SaveToFile()
		{
			SaveFileDialog sd = new SaveFileDialog();
			sd.Filter = "Json|*.json";
			if (sd.ShowDialog() == true)
			{
				string name = sd.FileName;
				Library.BusinessLogic.Library.SaveArchive(name);
				MessageBox.Show("Success");
			}
		}

		public void OnPropertyChanged([CallerMemberName]string prop = "") =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		public event PropertyChangedEventHandler PropertyChanged;
		~ApplicationViewModel()
		{
			BusinessLogic.Library.SaveArchive(@"D:\study\OOP\Library\Data\books_autosaved.json");
		}
	}
}
