using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.BusinessLogic;
using Library.UI;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Library.UI.Tools;
using System.Text.RegularExpressions;

namespace Library.UI.ViewModel
{
	public class AuthorizeViewModel : INotifyPropertyChanged
	{
		private string login;
		private string password;
		private RelayCommand authorizeCommand;
		private RelayCommand registerCommand;

		public AuthorizeViewModel()
		{
			BusinessLogic.Library.LoadArchive(@"D:\study\OOP\Library\Data\books_autosaved.json");
		}

		public string Login
		{
			get => login;
			set
			{
				login = value;
				OnPropertyChanged();
			}
		}
		public string Password
		{
			get => password;
			set
			{
				password = value;
				OnPropertyChanged();
			}
		}

		public RelayCommand AuthorizeCommand => authorizeCommand ??
			(authorizeCommand = new RelayCommand(obj =>
			{
				BusinessLogic.Library.Authorize(login, password);
			}));

		public bool Authorize()
		{
			return BusinessLogic.Library.Authorize(login, password);
		}
		public void Register(string name, int age, string sex, string login, string password)
		{
			if (age < 0 || age > 100) throw new Exception("Incorrect age");
			if (!Regex.IsMatch(login, @"^[a-zA-Z0-9]{1,40}$")) throw new Exception("Wrong login");
			if (!Regex.IsMatch(password, @"^[a-zA-Z0-9]{1,40}$")) throw new Exception("Wrong password");
			if (!BusinessLogic.Library.LoginExists(login))
			{
				BusinessLogic.Library.Register(new Client(login, password, name, age, sex));
			}
			else
			{
				throw new Exception("Login already exists");
			}
		}


		public void OnPropertyChanged([CallerMemberName]string prop = "") =>
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
