using System;
using System.Collections.Generic;
using System.Text;

namespace IOLaby.Data.Users
{
	public abstract class BaseUser
	{
		public int UserId { get; private set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		
		public string Email { get; set; }

		public BaseUser(string login, string password, string firstName, string lastName, string email)
		{
			Login = login;
			Password = password;
			FirstName = firstName;
			LastName = lastName;
			UserId = Database.NextUserId;
			Email = email;
		}
	}
}
