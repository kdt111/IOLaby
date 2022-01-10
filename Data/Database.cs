using System;
using System.Collections.Generic;
using System.Text;
using IOLaby.Data.Classes;
using IOLaby.Data.Users;

namespace IOLaby.Data
{
	class Database
	{
		private static int nextLessonIndex = 0;
		private static int nextUserIndex = 0;

		public static int NextLessonId => nextLessonIndex++;
		public static int NextUserId => nextUserIndex++;

		public List<ClassGroup> ClassGroupList { get; private set; }
		public List<BaseUser> UserList { get; private set; }

		public Database()
		{
			ClassGroupList = new List<ClassGroup>();
			UserList = new List<BaseUser>();

			GenerateData();
		}

		public ClassGroup GetGroup(BaseUser user, int groupId)
		{
			return ClassGroupList.Find(cg => cg.ContainsStudent(user) && cg.ClassId == groupId);
		}

		public List<Atendence> GetAtendence(ClassGroup classGroup)
		{
			List<Atendence> returnVal = new List<Atendence>();
			foreach (Lesson lesson in classGroup.LessonList)
				returnVal.AddRange(lesson.AtendanceList);
			return returnVal;
		}

		public Lesson GetLessonData(Teacher teacher, int lessonId)
		{
			foreach(ClassGroup classGroup in ClassGroupList)
			{
				if (classGroup.Teacher.UserId != teacher.UserId)
					continue;

				foreach(Lesson lesson in classGroup.LessonList)
				{
					if (lesson.LessonId == lessonId)
						return lesson;
				}
			}
			return null;
		}

		public Lesson GetLesson(int lessonId)
		{
			foreach (ClassGroup classGroup in ClassGroupList)
			{
				foreach (Lesson lesson in classGroup.LessonList)
				{
					if (lesson.LessonId == lessonId)
						return lesson;
				}
			}
			return null;
		}

		public bool ModifyLesson(int lessonId, Lesson lesson)
		{
			Lesson l = GetLesson(lessonId);
			if (l == null)
				return false;
			l.LessonDate = lesson.LessonDate;
			l.Topic = lesson.Topic;
			l.AtendanceList = lesson.AtendanceList;
			return true;
		}

		public BaseUser FindUser(int userId)
		{
			return UserList.Find(u => u.UserId == userId);
		}

		public List<ClassGroup> GetUserClasses(int userId)
		{
			List<ClassGroup> returnVal = new List<ClassGroup>();
			BaseUser user = FindUser(userId);
			if (user == null)
				return returnVal;
			
			foreach(ClassGroup cg in ClassGroupList)
			{
				if(user is Teacher)
				{
					if (cg.Teacher.UserId == userId)
						returnVal.Add(cg);
				}
				else
				{
					if (cg.ContainsStudent(user))
						returnVal.Add(cg);
				}
			}

			return returnVal;
		}
	
		public Dictionary<GradeGroup, Grade> GetUserGrades(int userId)
		{
			Dictionary<GradeGroup, Grade> returnValue = new Dictionary<GradeGroup, Grade>();
			BaseUser user = FindUser(userId);
			if (user == null)
				return returnValue;

			foreach(ClassGroup cg in GetUserClasses(userId))
			{
				foreach(GradeGroup gg in cg.GradeGroupList)
				{
					foreach(Grade grade in gg.GradeList)
					{
						if (grade.Student.UserId == userId)
						{
							returnValue.Add(gg, grade);
							break;
						}
					}
				}
			}

			return returnValue;
		}

		public Dictionary<ClassGroup, List<Atendence>> GetUserAtendances(int userId)
		{
			Dictionary<ClassGroup, List<Atendence>> returnValue = new Dictionary<ClassGroup, List<Atendence>>();
			BaseUser user = FindUser(userId);
			if (user == null)
				return returnValue;

			return returnValue;
		}
	
		public BaseUser ValidateUser(string login, string password)
		{
			BaseUser user = UserList.Find(u => u.Login == login && u.Password == password);
			return user;
		}
	
		private void GenerateData()
		{
			UserList.Add(new Student("uczenLogin", "haslo", "Jan", "Kowalski", "test@test.com"));
		}
	}
}
