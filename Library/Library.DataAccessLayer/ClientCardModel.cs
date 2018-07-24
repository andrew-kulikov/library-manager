using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataAccessLayer
{
	[Serializable]
	public class ClientCardModel
	{
		public int id;
		public List<int> requests;
		public List<int> completedOffers;
		public List<int> pendingOffers;
		public bool hasBook;
	}
}
