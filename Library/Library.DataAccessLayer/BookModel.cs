using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataAccessLayer
{
	public class BookModel
	{
		public List<BookJenre> jenres;
		public string name;
		public string isbn;
		public int pages;
		public PersonModel author;
		public int year;
		public int depositCost;
		public int rentCost;
		public string description;
		public int dbId;
	}
}
