using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DataAccessLayer;

namespace Library.BusinessLogic
{
	public class BookRequest
	{
		private static int curId;
		private Client client;
		private Book book;
		private int id;

		public BookRequest(Client client, Book book)
		{
			this.client = client;
			this.book = book;
			id = curId;
			curId++;
		}
		public BookRequest(BookRequestModel model)
		{
			client = Archive.Clients[model.client.card.id];
			book = new Book(model.book);
			id = model.id;
			curId = BookRequestModel.curId;
		}

		public static int CurId
		{
			get => curId;
			set => curId = value;
		}
		public int Id => id;
		public Client GetClient() => client;
		public Book GetBook() => book;

		public BookRequestModel GetModel()
		{
			BookRequestModel model = new BookRequestModel();
			model.id = id;
			model.client = client.GetModel();
			model.book = book.GetModel();
			BookRequestModel.curId = curId;
			return model;
		}
	}
}
