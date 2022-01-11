using System;
using System.Collections.Generic;
using System.Text;

namespace IOLaby.Data.Users
{
	public class Teacher : BaseUser
	{
		public Teacher(string login, string password, string firstName, string lastName, string email) : base(login, password, firstName, lastName, email) { }
	}
}
