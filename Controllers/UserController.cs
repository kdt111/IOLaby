using System;
using System.Collections.Generic;
using System.Text;
using IOLaby.Data;
using IOLaby.Data.Users;

namespace IOLaby.Controllers
{
	public class UserController
	{
		private Database database;

		public UserController(Database database)
		{
			this.database = database;
		}

		public BaseUser GetUser(int userId)
		{
			return database.FindUser(userId);
		}

		public BaseUser ValidateUser(string login, string password)
		{
			return database.ValidateUser(login, password);
		}
	}
}
