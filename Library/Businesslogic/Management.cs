using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.BusinessLogic
{
	public class Management: Department
	{
		public Management(): base("Managenent")
		{
		}

		public Offer Responce(BookRequest request)
		{
			Client client = request.GetClient();
			Book book = request.GetBook();
			if (client.Balance < book.DepositCost)
			{
				throw new Exception("Not enough money");
			}
			if (Archive.Books[book.Isbn] == null || Archive.Amounts[book.Isbn] == 0)
			{
				throw new Exception("No such a book");
			}
			Offer responce = new Offer(client, book);
			return responce;
		}
		public void CloseOffer(Offer offer)
		{
			offer.Close();
			Archive.ComleteOffer(offer);
		}
	}
}
