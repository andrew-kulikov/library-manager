using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DataAccessLayer.Utility;
using Library.DataAccessLayer;
using System.Diagnostics;

namespace Library.BusinessLogic
{
	public static class Library
	{
		private static string name;
		private static int curID;
		private static Admin admin;
		private static Account authorizedUser;
		private static List<Department> departments;
		private static InquiryOffice inquiryOffice;
		private static Management management;
		private static List<Account> users;

		static Library()
		{
			name = "Super puper lib";
			admin = new Admin(name, "Vasya", 18, "male");
			departments = new List<Department>();
			users = new List<Account>();
			departments.Add(new Department("admin"));
			//--
			inquiryOffice = new InquiryOffice();
			management = new Management();
		}

		public static int Id { get => curID; set => curID = value; }
		public static string Name => name;
		public static Account AuthorizedUser => authorizedUser;
		public static List<Department> Departments => departments;
		public static List<Account> Users => users;
		public static TimeSpan MaxRentTime => new TimeSpan(365, 0, 0, 0);

		public static void Register(Client client)
		{
			ClientCard card = new ClientCard(++curID);
			client.Card = card;
			client.ReplenishBalance(100);
			authorizedUser = client as Account;
			users.Add(client as Account);
			Archive.AddClient(client);
			Serializer.AddClient(client.GetModel());
		}
		public static bool AddMoney(int sum)
		{
			if ((authorizedUser as Client) == null) return false;
			(authorizedUser as Client)?.ReplenishBalance(sum);
			return true;
		}
		public static bool LoginExists(string login)
		{
			foreach (var client in Archive.Clients)
			{
				if (client.Value.Login == login)
				{
					return true;
				}
			}
			return false;
		}
		public static bool Authorize(string login, string password)
		{
			if (login == "admin" && admin.CheckPassword(password))
			{
				authorizedUser = admin;
				return true;
			}
			foreach (var user in Archive.Clients)
			{
				Client client = user.Value as Client;
				if (client != null && client.Login == login && client.CheckPassword(password))
				{
					authorizedUser = client as Account;
					return true;
				}
			}
			return false;
		}
		public static List<Offer> ClientPendingOffers()
		{
			Client client = authorizedUser as Client;
			if (client != null)
			{
				return inquiryOffice.ClientPendingOffers(client);
			}
			return null;
		}
		public static List<Offer> ClientClosedOffers()
		{
			Client client = authorizedUser as Client;
			if (client != null)
			{
				return inquiryOffice.ClientClosedOffers(client);
			}
			return null;
		}
		public static void LoadArchive(string path)
		{
			curID++;
			Archive.LoadFrom(Serializer.LoadFromDb());
		}
		public static void SaveArchive(string path)
		{
			Serializer.SerializeArchive(Archive.GetModel(), path);
		}
		public static void NewDepartment(string name, List<Employee> employees)
		{
			departments.Add(admin.CreateDepartment(name, employees));
		}
		public static void AddBook(Book book)
		{
			admin.GetBooks(new List<Book>(new Book[] { book }));
		}
		public static Offer Responce(BookRequest request)
		{
			Archive.AddRequest(request);
			Serializer.AddRequest(request.GetModel());
			Client client = request.GetClient();
			if (client == null || client.Card == null)
			{
				//throw new Exception("Client not registered");
				Register(client);
			}
			Offer responce;
			try
			{
				responce = management.Responce(request);
			}
			catch (Exception e)
			{
				Debug.Write(e.Message);
				return null;
			}
			responce.SetPending();
			Archive.AddPendingOffer(responce);
			Serializer.AddOffer(responce.GetModel());
			return responce;
		}
		public static Book Find(string bookName)
		{
			return inquiryOffice.FindBook(bookName);
		}
		public static void CloseOffer(Offer offer)
		{
			management.CloseOffer(offer);
			Serializer.CompleteOffer(offer.GetModel());
		}
		public static void GetBooks(List<Book> books)
		{
			admin.GetBooks(books);
		}
		public static void IncreaseAmount(Book book, int delta)
		{
			if (Archive.Amounts[book.Isbn] + delta > 1000000) throw new Exception("Cannot place so many books");
			Archive.Amounts[book.Isbn] += delta;
			Serializer.IncreaseAmount(book.Isbn, delta);
		}
		public static string GetInfo()
		{
			string info = "";
			info += "Library name: " + name + "\n";
			info += "Authorized user: " + authorizedUser.Name + "\n";
			return info;
		}
	}
}
