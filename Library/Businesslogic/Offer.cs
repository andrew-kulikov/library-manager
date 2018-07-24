using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DataAccessLayer;

namespace Library.BusinessLogic
{
	public class Offer
	{
		private Client client;
		private DateTime returnDate;
		private Book book;
		private OfferStatus status;
		private static int curId = 0;
		private int id;

		public Offer(Client client, Book book)
		{
			this.client = client;
			this.book = book;
			status = OfferStatus.Processed;
			id = ++curId;
			returnDate = DateTime.Now + Library.MaxRentTime;
		}
		public Offer(OfferModel model)
		{
			client = Archive.Clients[model.client.card.id];
			client.Init(model.client);
			returnDate = model.returnDate;
			book = new Book(model.book);
			status = (OfferStatus)((int)model.status);
			if (OfferModel.curId > curId) curId = OfferModel.curId;
			id = model.id;
		}

		public static int CurId { get => curId; set => curId = value; }
		public OfferStatus Status => status;
		public DateTime ReturnDate => returnDate;
		public Client Client => client;
		public int Id => id;
		public Book Book => book;

		public Client GetClient() => client;
		public Book GetBook() => book;
		public void SetPending()
		{
			status = OfferStatus.Pending;
		}		
		public void Close()
		{
			status = OfferStatus.Completed;
		}
		public OfferModel GetModel()
		{
			OfferModel model = new OfferModel();
			model.id = id;
			OfferModel.curId = curId;
			model.returnDate = returnDate;
			model.status = (DataAccessLayer.OfferStatus)(int)status;
			model.client = client.GetModel();
			model.book = book.GetModel();
			return model;
		}
	}
}
