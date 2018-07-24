using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DataAccessLayer;

namespace Library.BusinessLogic
{
	public class Person
	{
		protected string name;
		protected long age;
		protected string sex;

		public Person(string name, int age, string sex)
		{
			this.name = name;
			this.sex = sex;
			this.age = age;
		}
		public Person(PersonModel model)
		{
			name = model.name;
			age = model.age;
			sex = model.sex;
		}
		public Person()
		{

		}

		public string Name
		{
			get => name;
			set => name = value;
		}
		public long Age
		{
			get => age;
			set
			{
				if (value >= 0 && value < 100) age = value;
			}
		}
		public string Sex
		{
			get => sex;
			set => sex = value;
		}
		public PersonModel GetModel()
		{
			PersonModel model = new PersonModel();
			model.name = name;
			model.age = age;
			model.sex = sex;
			return model;
		}
	}
}
