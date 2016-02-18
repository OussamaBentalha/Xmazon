using System;

namespace Xmazon
{
	public class User
	{	
		public User ()
		{
		}

		public User (string username, string password, string email, string firstname, string lastname, string birthday)
		{
			this.Username = username;
			this.Password = password;
			this.Email = email;
			this.Firstname = lastname;
			this.Lastname = lastname;
			this.Birthdate = birthday;
		}

		public string Username { get; set; }

		public string Password { get; set; }

		public string Email { get; set; }

		public string Firstname { get; set; }

		public string Lastname { get; set; }

		public string Birthdate { get; set; }
	}
}

