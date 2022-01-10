using System;
using System.Collections.Generic;
using System.Text;
using IOLaby.Data;
using IOLaby.Data.Classes;

namespace IOLaby.Controllers
{
	class ClassesController
	{
		private Database database;

		public ClassesController(Database database)
		{
			this.database = database;
		}

		public Lesson GetLesson(int lessonId)
		{
			return database.GetLesson(lessonId);
		}

		public bool ModifyLesson(int lessonId, Lesson lesson)
		{
			return database.ModifyLesson(lessonId, lesson);
		}

		public List<ClassGroup> GetUserClasses(int userId)
		{
			return database.GetUserClasses(userId);
		}

		public Dictionary<GradeGroup, Grade> GetUserGrades(int userId)
		{
			return database.GetUserGrades(userId);
		}

		public Dictionary<ClassGroup, List<Atendence>> GetUserAtendances(int userId)
		{
			return database.GetUserAtendances(userId);
		}
	}
}
