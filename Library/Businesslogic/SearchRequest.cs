using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.BusinessLogic
{
	public class SearchRequest
	{
		private int minRentCost;
		private int maxRentCost;
		private int minDepositCost;
		private int maxDepositCost;
		private bool sortByDate;
		private bool sortByPrice;
		private bool sortByName;
		private int minDate;
		private int maxDate;

		public SearchRequest()
		{

		}

		public int MinRentCost
		{
			get => minRentCost;
			set => minRentCost = value;
		}
		public int MaxRentCost
		{
			get => maxRentCost;
			set => maxRentCost = value;
		}
		public int MinDepositCost
		{
			get => minDepositCost;
			set => minDepositCost = value;
		}
		public int MaxDepositCost
		{
			get => maxDepositCost;
			set => maxDepositCost = value;
		}
		public bool SortByDate
		{
			get => sortByDate;
			set => sortByDate = value;
		}
		public bool SortByPrice
		{
			get => sortByPrice;
			set => sortByPrice = value;
		}
		public bool SortByName
		{
			get => sortByName;
			set => sortByName = value;
  		}
		public int MinDate
		{
			get => minDate;
			set => minDate = value;
		}
		public int MaxDate
		{
			get => maxDate;
			set => maxDate = value;
		}
	}
}
