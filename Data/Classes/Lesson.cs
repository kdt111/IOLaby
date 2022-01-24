using System;
using System.Collections.Generic;
using System.Text;
using IOLaby.Data.Users;

namespace IOLaby.Data.Classes
{
	public class Lesson
	{
		public string Topic { get; set; }
		public List<Atendence> AtendanceList { get; set; }
		public DateTime LessonDate { get; set; }
		public int LessonId { get; private set; }

		public Lesson(string topic, string date, bool withoutId = false)
		{
			Topic = topic;
			SetLessonDate(date);
			AtendanceList = new List<Atendence>();
			if (withoutId)
				LessonId = -1;
			else
				LessonId = Database.NextLessonId;
		}

		public void SetLessonDate(string date)
		{
			Boolean valid = DateTime.TryParse(date, out DateTime lessonDate);
            if (valid)
            {
				LessonDate = lessonDate;
            }
		}

		public void MarkAtendence(Student student, bool wasPresent)
		{
			Atendence atendence = AtendanceList.Find(atd => atd.Student.UserId == student.UserId);
			if (atendence == null)
				AtendanceList.Add(new Atendence(student, wasPresent));
			else
				atendence.WasPresent = wasPresent;
		}
	}
}
