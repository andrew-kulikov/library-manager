using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DataAccessLayer;

namespace Library.BusinessLogic
{
	public class Book
	{
		private List<BookJenre> jenres;
		private string name = "";
		private string isbn = "";
		private int pages;
		private Person author;
		private int year;
		//private List<Offer> offers;
		private int depositCost;
		private int rentCost;
		private string description = "";

		public Book(Person author)
		{
			this.author = author;
			Genres = new List<BookJenre>();
			//offers = new List<Offer>();
		}
		public Book(BookModel model)
		{
			jenres = new List<BookJenre>();
			foreach (var jenre in model.jenres)
			{
				jenres.Add((BookJenre)((int)jenre));
			}
			name = model.name;
			isbn = model.isbn;
			pages = model.pages;
			author = new Person(model.author);
			year = model.year;
			depositCost = model.depositCost;
			rentCost = model.rentCost;
			description = model.description;
		}

		public List<BookJenre> Genres
		{
			get => jenres;
			set => jenres = value;
		}
		public string Name
		{
			get => name;
			set => name = value;
		}
		public string Isbn
		{
			get => isbn;
			set => isbn = value;
		}
		public int Pages
		{
			get => pages;
			set => pages = value;
		}
		public Person Author
		{
			get => author;
			set => author = value;
		}
		public int Year
		{
			get => year;
			set => year = value;
		}
		public int DepositCost
		{
			get => depositCost;
			set => depositCost = value;
		}
		public int RentCost
		{
			get => rentCost;
			set => rentCost = value;
		}
		public string Description
		{
			get => description;
			set => description = value;
		}

		public BookModel GetModel()
		{
			BookModel model = new BookModel();
			model.author = author.GetModel();
			model.depositCost = depositCost;
			model.description = description;
			model.rentCost = rentCost;
			model.pages = pages;
			model.name = name;
			model.jenres = new List<DataAccessLayer.BookJenre>();
			foreach (var jenre in jenres)
			{
				model.jenres.Add((DataAccessLayer.BookJenre)(int)jenre);
			}
			model.isbn = isbn;
			model.year = year;
			return model;
		}
	}
}
