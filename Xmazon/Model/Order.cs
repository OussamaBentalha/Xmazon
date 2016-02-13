using System;
using System.Collections.Generic;

namespace Xmazon
{
	public class Order
	{
		public Order ()
		{
		}

		public string Uid { get; set; }

		public List<Product> Product_Cart { get; set; }
	}
}

