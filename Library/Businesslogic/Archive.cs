using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DataAccessLayer;
using Library.DataAccessLayer.Utility;

namespace Library.BusinessLogic
{
	public static class Archive
	{
		private static Dictionary<string, Book> books;
		private static Dictionary<int, Offer> pendingOffers;
		private static Dictionary<int, Offer> completedOffers;
		private static Dictionary<int, Client> clients;
		private static Dictionary<string, int> amounts;
		private static List<BookRequest> requests;

		static Archive()
		{
			books = new Dictionary<string, Book>();
			amounts = new Dictionary<string, int>();
			pendingOffers = new Dictionary<int, Offer>();
			completedOffers = new Dictionary<int, Offer>();
			clients = new Dictionary<int, Client>();
			requests = new List<BookRequest>();
		}

		public static Dictionary<string, Book> Books => books;
		public static Dictionary<int, Offer> PendingOffers => pendingOffers;
		public static Dictionary<int, Offer> CompletedOffers => completedOffers;
		public static Dictionary<int, Client> Clients => clients;
		public static Dictionary<string, int> Amounts => amounts;
		public static List<BookRequest> Requests => requests;

		public static void SetAmount(string key, int am)
		{
			amounts[key] = am;
		}
		public static bool AddBook(Book book)
		{
			if (!books.ContainsKey(book.Isbn))
			{
				books.Add(book.Isbn, book);
				amounts[book.Isbn] = 1;
				Serializer.AddBook(book.GetModel());
				return true;
			}
			else
			{
				amounts[book.Isbn]++;
				Serializer.IncreaseAmount(book.Isbn, 1);
				return false;
			}
		}
		public static void RemoveBook(Book book)
		{
			books.Remove(book.Isbn);
			Serializer.RemoveBook(book.Isbn);
		}
		public static void ChangeBook(Book book)
		{
			books[book.Isbn] = book;
			Serializer.ChangeBook(book.GetModel(), Archive.amounts[book.Isbn]);
		}
		public static void AddRequest(BookRequest request)
		{
			requests.Add(request);
		}
		public static void AddPendingOffer(Offer offer)
		{
			Amounts[offer.GetBook().Isbn]--;
			pendingOffers.Add(offer.Id, offer);
		}
		public static void ComleteOffer(Offer offer)
		{
			Amounts[offer.GetBook().Isbn]++;
			pendingOffers.Remove(offer.Id);
			completedOffers.Add(offer.Id, offer);
		}
		public static void AddClient(Client client)
		{
			clients.Add(client.Card.Id, client);
		} 
		public static Offer FindPendingOffer(int id)
		{
			return pendingOffers[id];
			//return pendingOffers.First(off => off.Value.GetBook().Equals(book) && off.Value.GetClient().Equals(client));
		} 
		public static void LoadFrom(ArchiveModel model)
		{
			books = new Dictionary<string, Book>();
			amounts = new Dictionary<string, int>();
			pendingOffers = new Dictionary<int, Offer>();
			completedOffers = new Dictionary<int, Offer>();
			clients = new Dictionary<int, Client>();
			requests = new List<BookRequest>();
			int offerId = 0;
			foreach (var client in model.clients)
			{
				Client c = new Client();
				c.Init(client.Value);
				clients.Add(client.Key, c);
				Library.Id = Math.Max(client.Key, Library.Id);
			}
			foreach (var off in model.pendingOffers)
			{
				offerId = Math.Max(Offer.CurId, off.Key);
				pendingOffers.Add(off.Key, new Offer(off.Value));
			}
			foreach (var off in model.completedOffers)
			{
				offerId = Math.Max(offerId, off.Key);
				completedOffers.Add(off.Key, new Offer(off.Value));
			}
			foreach (var book in model.books)
			{
				books.Add(book.Key, new Book(book.Value));
			}
			amounts = model.amounts;
			Offer.CurId = offerId;
			offerId = 0;
			foreach (var request in model.requests)
			{
				offerId = Math.Max(offerId, request.id);
				requests.Add(new BookRequest(request));
			}
			
			BookRequest.CurId = offerId + 1;
		}
		public static ArchiveModel GetModel()
		{
			ArchiveModel model = new ArchiveModel();
			foreach (var client in clients)
			{
				model.clients.Add(client.Key, client.Value.GetModel());
			}
			foreach (var off in pendingOffers)
			{
				model.pendingOffers.Add(off.Key, off.Value.GetModel());
			}
			foreach (var off in completedOffers)
			{
				model.completedOffers.Add(off.Key, off.Value.GetModel());
			}
			foreach (var book in books)
			{
				model.books.Add(book.Key, book.Value.GetModel());
			}
			model.amounts = amounts;
			foreach (var request in requests)
			{
				model.requests.Add(request.GetModel());
			}
			return model;
		}
	}
}
