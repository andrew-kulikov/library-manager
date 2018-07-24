using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataAccessLayer
{
    public class ArchiveModel
    {
		public Dictionary<string, BookModel> books;
		public Dictionary<int, OfferModel> pendingOffers;
		public Dictionary<int, OfferModel> completedOffers;
		public Dictionary<int, ClientModel> clients;
		public Dictionary<string, int> amounts;
		public List<BookRequestModel> requests;

		public ArchiveModel()
		{
			books = new Dictionary<string, BookModel>();
			amounts = new Dictionary<string, int>();
			pendingOffers = new Dictionary<int, OfferModel>();
			completedOffers = new Dictionary<int, OfferModel>();
			clients = new Dictionary<int, ClientModel>();
			requests = new List<BookRequestModel>();
		}
	}
}
