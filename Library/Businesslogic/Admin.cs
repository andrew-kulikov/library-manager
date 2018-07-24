using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DataAccessLayer.Utility;
using Library.DataAccessLayer;

namespace Library.BusinessLogic
{
	public class Admin : Employee
	{
		private string library;

		public Admin(string library, string name, int age, string sex) : 
			base(new Department("admin"), "admin", 0, "admin", "admin", name, age, sex)
		{
			this.library = library;
		}

		public string LibraryName => library;

		public void RegsterClient(Client client)
		{
			if (client.Card != null)
			{
				Library.Register(client);
			}
		}
		public void GetBooks(List<Book> books)
		{
			foreach (var book in books)
			{
				Archive.AddBook(book);
			}
		}
		public Department CreateDepartment(string name, List<Employee> employees)
		{
			Department dep = new Department(name);
			foreach (var emp in employees)
			{
				dep.AddEmployee(emp);
			}
			return dep;
		}
		public void MoveEmployee(Employee employee, Department department)
		{

		}
		public void NewEmployee(Employee employee, Department department)
		{

		}
		public void LayOff(Employee employee)
		{

		}
	}
}
