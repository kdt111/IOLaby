using System;
using System.Collections.Generic;
using System.Text;
using IOLaby.Data;
using IOLaby.Data.Classes;

namespace IOLaby.Controllers
{
	public class ClassesController
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

		public Dictionary<GradeGroup, Grade> GetUserGrades(int userId, int classGroupId)
		{
			return database.GetUserGrades(userId, classGroupId);
		}
		
		public Dictionary<Lesson, Atendence> GetUserAtendances(int userId, int classGroupId)
		{
			return database.GetUserAtendances(userId, classGroupId);
		}
	}
}
