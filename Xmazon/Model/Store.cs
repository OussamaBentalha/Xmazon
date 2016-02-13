using System;

namespace Xmazon
{
	public class Store
	{
		public Store ()
		{
		}

		public Store(string uid, string name)
		{
			this.Uid = uid;
			this.Name = name;
		}

		public string Uid { get; set; }

		public string Name { get; set; }
	}
}

