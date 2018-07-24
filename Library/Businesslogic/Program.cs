using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.BusinessLogic
{
	class Program
	{
		static void Main(string[] args)
		{
			Client cl = new Client("a", "b", "c", 2, "sd");
			cl.ReplenishBalance(100);
			Library.GetBooks(new List<Book>(new Book[] {
				new Book(new Person("dfg", 567, "Dfg")) {Name = "ggg", DepositCost = 10, RentCost = 5, Year = 2018, Isbn = "dfgdfg546", Pages = 569}
			}));
			cl.AskFor("ggg");
			cl.ReturnBook(0);
		}
	}
}
