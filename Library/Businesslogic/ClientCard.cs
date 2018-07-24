using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DataAccessLayer;

namespace Library.BusinessLogic
{
	public class ClientCard
	{
		private int id;
		private List<int> requests;
		private List<int> completedOffers;
		private List<int> pendingOffers;


		public ClientCard(int id)
		{
			requests = new List<int>();
			completedOffers = new List<int>();
			pendingOffers = new List<int>();
			this.id = id;
		}
		public ClientCard(ClientCardModel model)
		{
			requests = model.requests;
			pendingOffers = model.pendingOffers;
			completedOffers = model.completedOffers;
			id = model.id;
		}

		public int Id => id;
		public List<int> Requests => requests;
		public List<int> CompletedOffers => completedOffers;
		public List<int> PendingOffers => pendingOffers;

		public void AddOffer(Offer offer)
		{
			pendingOffers.Add(offer.Id);
		}
		public void AddRequest(BookRequest request)
		{
			requests.Add(request.Id);
		}
		public void Return(Offer offer)
		{
			pendingOffers.Remove(offer.Id);
			completedOffers.Add(offer.Id);
		}
		public ClientCardModel GetModel()
		{
			ClientCardModel model = new ClientCardModel();
			model.id = id;
			model.completedOffers = completedOffers;
			model.pendingOffers = pendingOffers;
			model.requests = requests;
			return model;
		}
	}
}
