using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.BusinessLogic
{
	public class Account : Person
	{
		protected string login;
		protected string password;

		public Account(string login, string password, string name, int age, string sex) : base(name, age, sex)
		{
			this.login = login;
			this.password = password;
		}
		public Account()
		{

		}

		public string Login => login;

		public void ChangePassword(string oldPassword, string newPassword)
		{
			if (oldPassword == password)
			{
				password = newPassword;
			}
		}
		public bool CheckPassword(string password)
		{
			return this.password == password;
		}
	}
}
