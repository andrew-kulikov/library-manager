using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DataAccessLayer;

namespace Library.BusinessLogic
{
	public class Client : Account
	{
		private ClientCard card;
		private decimal balance;

		public Client(string login, string password, string name, int age, string sex) : 
			base(login, password, name, age, sex)
		{
			balance = 0;
		}
		public Client() 
		{

		}
		public void Init(ClientModel model)
		{
			name = model.name;
			age = model.birth_year;
			sex = model.sex;
			login = model.login;
			password = model.password;
			card = new ClientCard(model.card);
			balance = model.balance;
		}

		public decimal Balance => balance;
		public ClientCard Card {
			get => card;
			set => card = value;
		}
		
		public bool AskFor(string isbn)
		{
			Book book = Library.Find(isbn);
			BookRequest request = new BookRequest(this, book);
			Offer responce = Library.Responce(request);
			if (responce != null)
			{
				card.AddRequest(request);
				Deal(responce);
				return true;
			}
			return false;
		}
		public void Deal(Offer offer)
		{
			if (offer != null)
			{
				balance -= offer.GetBook().DepositCost;
				card.AddOffer(offer);
			}
		}
		public void ReturnBook(int offerId)
		{
			Offer offer;
			try
			{
				offer = Archive.FindPendingOffer(offerId);
			}
			catch (Exception e)
			{
				return;
			}
			Library.CloseOffer(offer);
			card.Return(offer);
			if (DateTime.Now <= offer.ReturnDate)
			{
				ReplenishBalance(offer.GetBook().DepositCost);
				balance -= offer.GetBook().RentCost;
			}
		}
		public void ReplenishBalance(int sum)
		{
			balance += sum;
		}
		public ClientModel GetModel()
		{
			ClientModel model = new ClientModel();
			model.balance = balance;
			model.birth_year = age;
			model.card = card.GetModel();
			model.login = login;
			model.password = password;
			model.sex = sex;
			model.name = name;
			return model;
		}
	}
}
