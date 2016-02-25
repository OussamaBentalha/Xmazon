using System;
using System.Json;

namespace Xmazon
{
	public class CartProduct
	{
		public string uid { get; set; }
		public Product product { get; set; }
		public int quantity { get; set; }

		public CartProduct (string uid, Product product, int quantity)
		{
			this.uid = uid;
			this.product = product;
			this.quantity = quantity;
		}

		public static CartProduct Deserialize (JsonValue jsonCartProduct)
		{
			return new CartProduct (
				jsonCartProduct ["uid"],
				Product.Deserialize(jsonCartProduct ["product"]),
				(int)jsonCartProduct["quantity"]);
		}
	}
}

