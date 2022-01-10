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
		private static int nextClassIndex = 0;

		public static int NextLessonId => nextLessonIndex++;
		public static int NextUserId => nextUserIndex++;
		public static int NextClassId => nextClassIndex++;

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
			// l.AtendanceList = lesson.AtendanceList;
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
	
		public Dictionary<GradeGroup, Grade> GetUserGrades(int userId, int classGroupId)
		{
			Dictionary<GradeGroup, Grade> returnValue = new Dictionary<GradeGroup, Grade>();
			BaseUser user = FindUser(userId);
			ClassGroup classGroup = GetGroup(user, classGroupId);
			if (user == null || classGroup == null)
				return returnValue;

			foreach(GradeGroup gradeGroup in classGroup.GradeGroupList)
			{
				foreach(Grade grade in gradeGroup.GradeList)
				{
					if (grade.Student.UserId == userId)
					{
						returnValue.Add(gradeGroup, grade);
						break;
					}
				}
			}

			return returnValue;
		}

		public Dictionary<Lesson, Atendence> GetUserAtendances(int userId, int classGroupId)
		{
			Dictionary<Lesson, Atendence> returnValue = new Dictionary<Lesson, Atendence>();
			BaseUser user = FindUser(userId);
			ClassGroup classGroup = GetGroup(user, classGroupId);
			if (user == null || classGroup == null)
				return returnValue;

			foreach(Lesson lesson in classGroup.LessonList)
            {
				foreach(Atendence atendence in lesson.AtendanceList)
                {
					if(atendence.Student.UserId == userId)
                    {
						returnValue.Add(lesson, atendence);
						break;
					}
                }
                
            }
			return returnValue;
		}

		public BaseUser ValidateUser(string login, string password)
		{
			BaseUser user = UserList.Find(u => u.Login == login && u.Password == password);
			return user;
		}
	
		private void GenerateData()
		{
			Student student_1 = new Student("u1", "haslo", "Jan", "Kowalski", "test@test.com");
			Student student_2 = new Student("u2", "haslo", "Michał", "Skiba", "michal.skiba@example.com");
			Student student_3 = new Student("u3", "haslo", "Damian", "Raczkowski", "damian.raczkowski@example.com");
			Teacher teacher_1 = new Teacher("n1", "haslo", "Tomasz", "Babczyńśki", "tomasz.babczynski@example.com");
			Teacher teacher_2 = new Teacher("n2", "haslo", "Zofia", "Kruczkiewicz", "zofia.kruczkiewicz@example.com");
			Teacher teacher_3 = new Teacher("n3", "haslo", "Olgierd", "Unold", "olgierd.unold@example.com");
			UserList.AddRange(new List<Student> { student_1, student_2, student_3});
			UserList.AddRange(new List<Teacher> { teacher_1, teacher_2, teacher_3 });

			ClassGroup class_group_1 = new ClassGroup("class_group_1");
			class_group_1.Teacher = teacher_1;
			class_group_1.StudentList.AddRange(new List<Student> { student_1, student_2, student_3 });
			GradeGroup gradeGroup_1_1 = new GradeGroup(5, "ocena z lab1_1", class_group_1);
			gradeGroup_1_1.AddGrade(new Grade(4, student_1));
			gradeGroup_1_1.AddGrade(new Grade(5, student_2));
			gradeGroup_1_1.AddGrade(new Grade(3, student_3));
			GradeGroup gradeGroup_1_2 = new GradeGroup(6, "ocena z lab1_2", class_group_1);
			gradeGroup_1_2.AddGrade(new Grade(3, student_1));
			gradeGroup_1_2.AddGrade(new Grade(2, student_2));
			gradeGroup_1_2.AddGrade(new Grade(5, student_3));
			class_group_1.GradeGroupList.AddRange(new List<GradeGroup> { gradeGroup_1_1, gradeGroup_1_2 });
			Lesson lesson_1_1 = new Lesson("lesson_1_1 topic", "04.01.2022");
			lesson_1_1.MarkAtendence(student_1, true);
			lesson_1_1.MarkAtendence(student_2, true);
			lesson_1_1.MarkAtendence(student_3, true);
			Lesson lesson_1_2 = new Lesson("lesson_1_2 topic", "11.01.2022");
			lesson_1_2.MarkAtendence(student_1, false);
			lesson_1_2.MarkAtendence(student_2, true);
			lesson_1_2.MarkAtendence(student_3, true);
			class_group_1.LessonList.AddRange(new List<Lesson> { lesson_1_1, lesson_1_2 });

			ClassGroup class_group_2 = new ClassGroup("class_group_2");
			class_group_2.Teacher = teacher_2;
			class_group_2.StudentList.AddRange(new List<Student> { student_1, student_2 });
			GradeGroup gradeGroup_2_1 = new GradeGroup(3, "ocena z lab2_1", class_group_2);
			gradeGroup_2_1.AddGrade(new Grade(5, student_1));
			gradeGroup_2_1.AddGrade(new Grade(5, student_2));
			GradeGroup gradeGroup_2_2 = new GradeGroup(3, "ocena z lab2_2", class_group_2);
			gradeGroup_2_2.AddGrade(new Grade(5, student_1));
			gradeGroup_2_2.AddGrade(new Grade(5, student_2));
			class_group_2.GradeGroupList.AddRange(new List<GradeGroup> { gradeGroup_2_1, gradeGroup_2_2 });
			Lesson lesson_2_1 = new Lesson("lesson_2_1 topic", "03.01.2022");
			lesson_2_1.MarkAtendence(student_1, true);
			lesson_2_1.MarkAtendence(student_2, true);
			Lesson lesson_2_2 = new Lesson("lesson_2_2 topic", "10.01.2022");
			lesson_2_2.MarkAtendence(student_1, false);
			lesson_2_2.MarkAtendence(student_2, false);
			class_group_2.LessonList.AddRange(new List<Lesson> { lesson_2_1, lesson_2_2 });

			ClassGroup class_group_3 = new ClassGroup("class_group_3");
			class_group_3.Teacher = teacher_3;
			class_group_3.StudentList.AddRange(new List<Student> { student_1, student_3 });
			GradeGroup gradeGroup_3_1 = new GradeGroup(2, "ocena z lab3_1", class_group_3);
			gradeGroup_3_1.AddGrade(new Grade(3, student_1));
			gradeGroup_3_1.AddGrade(new Grade(4, student_3));
			GradeGroup gradeGroup_3_2 = new GradeGroup(10, "ocena z lab3_2", class_group_3);
			gradeGroup_3_2.AddGrade(new Grade(4, student_1));
			gradeGroup_3_2.AddGrade(new Grade(3, student_3));
			class_group_3.GradeGroupList.AddRange(new List<GradeGroup> { gradeGroup_3_1, gradeGroup_3_2 });
			Lesson lesson_3_1 = new Lesson("lesson_3_1 topic", "05.01.2022");
			lesson_3_1.MarkAtendence(student_1, true);
			lesson_3_1.MarkAtendence(student_3, false);
			Lesson lesson_3_2 = new Lesson("lesson_3_2 topic", "12.01.2022");
			lesson_3_2.MarkAtendence(student_1, false);
			lesson_3_2.MarkAtendence(student_3, true);
			class_group_3.LessonList.AddRange(new List<Lesson> { lesson_3_1, lesson_3_2 });

			ClassGroupList.AddRange(new List<ClassGroup> { class_group_1, class_group_2, class_group_3 });
		}
	}
}
