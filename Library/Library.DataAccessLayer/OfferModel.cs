using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataAccessLayer
{
	public class OfferModel
	{
		public ClientModel client;
		public DateTime returnDate;
		public BookModel book;
		public OfferStatus status;
		public static int curId = 0;
		public int id;
	}
}
