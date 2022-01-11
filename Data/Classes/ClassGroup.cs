using System;
using System.Collections.Generic;
using System.Text;
using IOLaby.Data.Users;

namespace IOLaby.Data.Classes
{
	public class ClassGroup
	{
		public int ClassId { get; set; }
		public string ClassName { get; set; }
		public Teacher Teacher { get; set; }
		public List<Student> StudentList { get; set; }
		public List<GradeGroup> GradeGroupList { get; set; }
		public List<Lesson> LessonList { get; set; }

		public ClassGroup(string className)
		{
			ClassId = Database.NextClassId;
			StudentList = new List<Student>();
			GradeGroupList = new List<GradeGroup>();
			LessonList = new List<Lesson>();
			this.ClassName = className;
		}

		public bool ContainsStudent(BaseUser student)
		{
			return StudentList.Find(s => s.UserId == student.UserId) != null;
		}
	}
}
