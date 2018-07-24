using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataAccessLayer
{
	[Serializable]
	public class ClientModel
	{
		public string name;
		public long birth_year;
		public string sex;
		public string login;
		public string password;
		public ClientCardModel card;
		public decimal balance;
	}
}
