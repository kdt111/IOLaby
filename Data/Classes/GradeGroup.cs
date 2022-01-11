using System;
using System.Collections.Generic;
using System.Text;

namespace IOLaby.Data.Classes
{
	public class GradeGroup
	{
		public int GradeWeight { get; set; }
		public string GradeGroupName { get; set; }
		public ClassGroup ClassGroup { get; set; }
		public List<Grade> GradeList { get; private set; }

		public GradeGroup(int gradeWeight, string gradeGroupName, ClassGroup classGroup)
		{
			GradeWeight = gradeWeight;
			GradeGroupName = gradeGroupName;
			ClassGroup = classGroup;

			GradeList = new List<Grade>();
		}

		public void AddGrade(Grade grade)
		{
			Grade res = GradeList.Find(g => g.Student.UserId == grade.Student.UserId);
			if (res == null)
				GradeList.Add(grade);
			else
				res.Value = grade.Value;
		}
	}
}
