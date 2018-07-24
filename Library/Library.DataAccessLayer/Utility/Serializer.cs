using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Library.DataAccessLayer.Utility
{
	public static class Serializer
	{
		public static void SerializeArchive(ArchiveModel model, string path)
		{
			string json = JsonConvert.SerializeObject(model);
			using (StreamWriter sw = new StreamWriter(path))
			{
				sw.WriteLine(json);
			}
		}

		public static ArchiveModel DeserializeArchive(string path)
		{
			FileInfo file = new FileInfo(path);
			string json = null;
			using (var reader = file.OpenText())
			{
				json = reader.ReadLine();
			}
			if (json != null)
			{
				ArchiveModel archive = new ArchiveModel();

				archive = JsonConvert.DeserializeObject<ArchiveModel>(json);
				return archive;
			}
			return new ArchiveModel();
		}

		public static ArchiveModel LoadFromDb()
		{
			ArchiveModel model = new ArchiveModel();
			using (var context = new librarydbEntities())
			{
				var authors = context.authors.ToList();
				var books = context.books.ToList();
				var clients = context.clients.ToList();
				var offers = context.offers.ToList();
				var requests = context.requests.ToList();
				foreach (var book in books)
				{
					BookModel bm = BookModelFromEntity(book);
					model.books.Add(bm.isbn, bm);
					model.amounts[bm.isbn] = (int)book.amount;
				}
				foreach (var client in clients)
				{
					ClientModel cm = new ClientModel
					{
						birth_year = client.age,
						balance = client.balance,
						card = new ClientCardModel
						{
							completedOffers = client.offers.Where(off => off.status == "Completed").Select(off => off.id).ToList(),
							pendingOffers = client.offers.Where(off => off.status == "Pending").Select(off => off.id).ToList(),
							id = client.clientId,
							requests = client.requests.Select(req => req.idRequests).ToList()
						},
						login = client.login,
						password = client.password,
						name = client.name,
						sex = "male"
					};
					model.clients.Add(cm.card.id, cm);
				}
				foreach (var request in requests)
				{
					BookRequestModel br = new BookRequestModel
					{
						id = request.idRequests,
						book = model.books[request.book.isbn],
						client = model.clients[request.client_id]
					};
					model.requests.Add(br);
				}
				foreach (var offer in offers)
				{
					OfferModel off = new OfferModel
					{
						id = offer.id,
						returnDate = Convert.ToDateTime(offer.deadline),
						status = (OfferStatus)Enum.Parse(typeof(OfferStatus), offer.status),
						book = model.books[offer.book.isbn],
						client = model.clients[offer.client_id]
					};
					if (off.status == OfferStatus.Completed)
					{
						model.completedOffers.Add(off.id, off);
					}
					else if (off.status == OfferStatus.Pending)
					{
						model.pendingOffers.Add(off.id, off);
					}
				}
			}
			return model;
		}

		public static void AddClient(ClientModel client)
		{
			using (var context = new librarydbEntities())
			{
				context.clients.Add(new client
				{
					name = client.name,
					age = (int)client.birth_year,
					balance = client.balance,
					login = client.login,
					password = client.password,
				});
				context.SaveChanges();
			}
		}

		public static void AddBook(BookModel book)
		{
			using (var context = new librarydbEntities())
			{
				var am = book.author;
				author auth = context.authors.Single(au => au.name == am.name && au.birth_year == am.age);
				if (auth == null)
				{
					auth = new author
					{
						birth_year = am.age,
						name = am.name,
						bio = "Bio"
					};
					context.authors.Add(auth);
					context.SaveChanges();
				}
				context.books.Add(new book
				{
					amount = 1,
					name = book.name,
					author = auth,
					depositCost = book.depositCost,
					description = book.description,
					isbn = book.isbn,
					rentCost = book.rentCost,
					year = book.year,
					jenres = string.Join(",", book.jenres),
					pages = book.pages
				});
				context.SaveChanges();
			}
		}

		public static void AddRequest(BookRequestModel request)
		{
			using (var context = new librarydbEntities())
			{
				context.requests.Add(new request
				{
					book = context.books.Single(bk => bk.isbn == request.book.isbn),
					client = context.clients.Single(cl => cl.clientId == request.client.card.id),
				});
				context.SaveChanges();
			}
		}

		public static void AddOffer(OfferModel offer)
		{
			using (var context = new librarydbEntities())
			{
				context.offers.Add(new offer
				{
					status = offer.status.ToString(),
					deadline = Convert.ToString(offer.returnDate),
					book = context.books.Single(bk => bk.isbn == offer.book.isbn),
					client = context.clients.Single(cl => cl.clientId == offer.client.card.id),
				});
				context.SaveChanges();
			}
		}

		public static void CompleteOffer(OfferModel offer)
		{
			using (var context = new librarydbEntities())
			{
				var off = context.offers.Single(of => of.id == offer.id);
				off.status = "Completed";
				context.SaveChanges();
			}
		}

		public static void IncreaseAmount(string isbn, int delta)
		{
			using (var context = new librarydbEntities())
			{
				book b = context.books.Single(bk => bk.isbn == isbn);
				b.amount += delta;
				context.SaveChanges();
			}
		}

		public static void RemoveBook(string isbn)
		{
			using (var context = new librarydbEntities())
			{
				book b = context.books.Single(bk => bk.isbn == isbn);
				context.books.Remove(b);
				context.SaveChanges();
			}
		}

		public static void ChangeBook(BookModel bm, int amount)
		{
			using (var context = new librarydbEntities())
			{
				book b = context.books.Single(bk => bk.isbn == bm.isbn);
				var am = bm.author;
				author auth = context.authors.Single(au => au.name == am.name && au.birth_year == am.age);
				if (auth == null)
				{
					auth = new author
					{
						birth_year = am.age,
						name = am.name,
						bio = "Bio"
					};
					context.authors.Add(auth);
					context.SaveChanges();
				}
				b.name = bm.name;
				b.author = auth;
				b.amount = amount;
				b.depositCost = bm.depositCost;
				b.description = bm.description;
				b.isbn = bm.isbn;
				b.rentCost = bm.rentCost;
				b.year = bm.year;
				b.jenres = string.Join(",", bm.jenres);
				b.pages = bm.pages;
				context.SaveChanges();
			}
		}

		private static BookModel BookModelFromEntity(book book)
		{
			BookModel bm = new BookModel
			{
				author = new PersonModel
				{
					age = book.author.birth_year,
					name = book.author.name,
					sex = "male"
				},
				name = book.name,
				description = book.description,
				isbn = book.isbn,
				jenres = book.jenres.Split(',').Select(s => (BookJenre)Enum.Parse(typeof(BookJenre), s)).ToList(),
				depositCost = book.depositCost,
				rentCost = book.rentCost,
				year = book.year,
				pages = book.pages,
				dbId = book.idBook
			};
			return bm;
		}
	}
}
