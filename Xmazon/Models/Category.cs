using System;
using System.Json;

namespace Xmazon
{
	public class Category
	{
		public string uid { get; set; }
		public string name { get; set; }

		public Category (string uid, string name)
		{
			this.uid = uid;
			this.name = name;
		}

		public static Category Deserialize (JsonValue jsonCategory)
		{
			return new Category (jsonCategory ["uid"], jsonCategory ["name"]);
		}
	}
}

