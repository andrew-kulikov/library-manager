using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.BusinessLogic
{
	public class Department
	{
		protected List<Employee> staff;
		protected string name;

		public Department(string name)
		{
			staff = new List<Employee>();
			this.name = name;
		}

		public List<Employee> Staff => staff;
		public string Name => name;

		public Employee FindEmployee(string name)
		{
			return staff.Find(em => em.Name == name);
		}
		public string GetInfo()
		{
			throw new NotImplementedException();
		}
		public void AddEmployee(Employee employee)
		{
			staff.Add(employee);
		}
	}
}
