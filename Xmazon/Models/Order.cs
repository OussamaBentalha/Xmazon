using System;
using System.Json;

namespace Xmazon
{
	public class Order
	{
		public string uid { get; set; }
		public CartProduct[] cartProducts { get; set; }

		public Order (string uid, CartProduct[] cartProducts, int quantity)
		{
			this.uid = uid;
			this.cartProducts = cartProducts;
		}
	}
}

