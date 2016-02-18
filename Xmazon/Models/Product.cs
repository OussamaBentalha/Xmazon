using System;
using System.Json;

namespace Xmazon
{
	public class Product
	{
		public string uid { get; set; }
		public string name { get; set; }
		public decimal price { get; set; }
		public bool available { get; set; }

		public Product (string uid, string name, decimal price, bool available)
		{
			this.uid = uid;
			this.name = name;
			this.price = price;
			this.available = available;
		}

		public static Product Deserialize (JsonValue jsonProduct)
		{
			return new Product (
				jsonProduct ["uid"],
				jsonProduct ["name"],
				(decimal) jsonProduct["price"],
				(bool) jsonProduct["available"]);
		}
	}
}

