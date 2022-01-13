using System;
using System.Collections.Generic;
using System.Text;
using IOLaby.Data.Users;

namespace IOLaby.Data.Classes
{
	public class Atendence
	{
		public Student Student { get; set; }
		public bool WasPresent { get; set; }

		public Atendence(Student student, bool wasPreset)
		{
			Student = student;
			WasPresent = wasPreset;
		}
	}
}
