using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.BusinessLogic
{
	public class Employee : Account
	{
		protected Department department;
		protected string position;
		protected int id;

		public Employee(
			Department department, string position, int id, string login, 
			string password, string name, int age, string sex) :
			base(login, password, name, age, sex)
		{
			this.department = department;
			this.position = position;
			this.id = id;
		}

	}
}
