using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.BusinessLogic
{
	public class InquiryOffice : Department
	{
		public InquiryOffice() : base("InquiryOffice")
		{

		}
		public List<Book> FindBooks(SearchRequest request)
		{
			throw new NotImplementedException();
		}
		public List<Offer> FindOffers(SearchRequest request)
		{
			throw new NotImplementedException();
		}
		public List<BookRequest> FindRequests(SearchRequest request)
		{
			throw new NotImplementedException();
		}
		public List<Client> FindClients(SearchRequest request)
		{
			throw new NotImplementedException();
		}
		public bool IsClient(Client client)
		{
			return Archive.Clients[client.Card.Id] != null;
		}
		public Book FindBook(string isbn)
		{
			return Archive.Books[isbn];
		}
		public List<Offer> ClientPendingOffers(Client client)
		{
			List<Offer> offs = new List<Offer>();
			foreach (var id in client.Card.PendingOffers)
			{
				offs.Add(Archive.PendingOffers[id]);
			}
			return offs;
		}
		public List<Offer> ClientClosedOffers(Client client)
		{
			List<Offer> offs = new List<Offer>();
			foreach (var id in client.Card.CompletedOffers)
			{
				offs.Add(Archive.CompletedOffers[id]);
			}
			return offs;
		}
	}
}
