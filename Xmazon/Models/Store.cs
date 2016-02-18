using System;
using System.Json;

namespace Xmazon
{
	public class Store
	{
		public string uid { get; set; }
		public string name { get; set; }

		public Store (string uid, string name)
		{
			this.uid = uid;
			this.name = name;
		}

		public static Store Deserialize (JsonValue jsonStore)
		{
			return new Store (jsonStore ["uid"], jsonStore ["name"]);
		}
	}
}

