using System;
using System.Collections.Generic;
using System.Text;
using IOLaby.Data.Users;

namespace IOLaby.Data.Classes
{
	public class Grade
	{
		public int Value { get; set;}
		public Student Student { get; set; }

		public Grade(int value, Student student)
		{
			Value = value;
			Student = student;
		}
	}
}
