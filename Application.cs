using System;
using IOLaby.Interface;
using System.Collections.Generic;
using IOLaby.Data;
using IOLaby.Controllers;
using IOLaby.Data.Users;
using IOLaby.Data.Classes;

namespace IOLaby
{
	class Application
	{
		private static UserInterface userInterface = new UserInterface();
		private static Database database = new Database();

		private static UserController userController = new UserController(database);
		private static ClassesController classesController = new ClassesController(database);
		private static BaseUser user = null;

		static void Main(string[] args)
		{
			userInterface.ClearScreen();
			
			while(user == null)
			{
				user = userInterface.LoginScreen(userController);
				if (user == null)
					Console.WriteLine("Błędny login lub hasło");
				else
				{
					userInterface.ClearScreen();
					Console.WriteLine($"Witaj {user.FirstName} {user.LastName}");
				}
			}

			userInterface.MainLoop();
		}

		static public void DisplayGroup()
		{
			List<ClassGroup> userClasses = classesController.GetUserClasses(user.UserId);
			if (user is Student)
			{
				Console.WriteLine($"Grupy ucznia {user.FirstName} {user.LastName}:");
				foreach (ClassGroup userClass in userClasses)
				{
					Console.WriteLine($"Nazwa grupy: {userClass.ClassName}");
					Console.WriteLine($"\tNauczyciel: {userClass.Teacher.FirstName} {userClass.Teacher.LastName}");
					Console.WriteLine($"\tOceny:");
					Dictionary<GradeGroup, Grade> userGrades = classesController.GetUserGrades(user.UserId, userClass.ClassId);
					foreach (var item in userGrades)
					{
						Console.WriteLine($"\t\t Ocena {item.Value.Value}, waga {item.Key.GradeWeight} za '{item.Key.GradeGroupName}'");
					}
					Console.WriteLine($"\tObecności:");
					Dictionary<Lesson, Atendence> userAtendence = classesController.GetUserAtendances(user.UserId, userClass.ClassId);
					foreach (var item in userAtendence)
					{
						if (item.Value.WasPresent)
						{
							Console.WriteLine($"\t\tUżytkownik był obecny dnia {item.Key.LessonDate} na lekcji o temacie '{item.Key.Topic}'");
						}
						else
						{
							Console.WriteLine($"\t\tUżytkownik nie był obecny dnia {item.Key.LessonDate} na lekcji o temacie '{item.Key.Topic}'");
						}
					}
				}
			}
			else if (user is Teacher)
			{
				Console.WriteLine($"Grupy nauczyciela {user.FirstName} {user.LastName}:");
				foreach (ClassGroup userClass in userClasses)
				{
					Console.WriteLine($"Nazwa grupy: {userClass.ClassName}");
					Console.WriteLine($"\tUczniowie:");
					foreach (Student student in userClass.StudentList)
					{
						Console.WriteLine($"\t\t{student.FirstName} {student.LastName}");
					}
					Console.WriteLine($"\toceny:");
					foreach(GradeGroup gradeGroup in userClass.GradeGroupList)
                    {
						Console.WriteLine($"\t\t{gradeGroup.GradeGroupName}, waga {gradeGroup.GradeWeight}:");
						foreach(Grade grade in gradeGroup.GradeList)
                        {
							Console.WriteLine($"\t\t\t{grade.Student.FirstName} {grade.Student.LastName}, ocena {grade.Value}");
						}
					}
					Console.WriteLine($"\tLekcje:");
					foreach(Lesson lesson in userClass.LessonList)
                    {
						Console.WriteLine($"\t\tTemat {lesson.Topic}, data {lesson.LessonDate}, id {lesson.LessonId}");
						foreach(Atendence atendence in lesson.AtendanceList)
                        {
                            if (atendence.WasPresent)
                            {
								Console.WriteLine($"\t\t\t{atendence.Student.FirstName} {atendence.Student.LastName} był obecny");
							}
                            else
                            {
								Console.WriteLine($"\t\t\t{atendence.Student.FirstName} {atendence.Student.LastName} był nieobecny");
							}
                        }
					}
				}
			}
            else
            {
				Console.WriteLine("Nie masz dostępu do tej akcji");
            }
		}

		static public void DisplayCalendar()
        {
			List<ClassGroup> userClasses = classesController.GetUserClasses(user.UserId);
			List<KeyValuePair<string, DateTime>> calendarEntries = new List<KeyValuePair<string, DateTime>>();
			Console.WriteLine($"Kalendarz użytkownika {user.FirstName} {user.LastName}:");
			foreach (ClassGroup classGroup in userClasses)
			{
				foreach (Lesson lesson in classGroup.LessonList)
				{
					calendarEntries.Add(new KeyValuePair<string, DateTime>($"\tgrupa {classGroup.ClassName}, data {lesson.LessonDate}, temat {lesson.Topic}", lesson.LessonDate));
				}
			}
			calendarEntries.Sort((ce1, ce2) => ce1.Value.CompareTo(ce2.Value));
			foreach(var calendarEntry in calendarEntries)
            {
				Console.WriteLine(calendarEntry.Key);
            }
		}

		static public void ModifyLesson()
        {
			if(user is Teacher)
            {
				Console.Write("Podaj id lekcji do modyfikacji: ");
				int lessonid = Int16.Parse(Console.ReadLine());
				string topic, date;
				Console.Write("Podaj nowy temat lekcji: ");
				topic = Console.ReadLine();
				Console.Write("Podaj nową datę lekcji: ");
				date = Console.ReadLine();
				Lesson lesson = new Lesson(topic, date);
				bool status = classesController.ModifyLesson(lessonid, lesson);
                if (status)
                {
					Console.WriteLine("Operacja modyfikacji lekcji powiodła się");
				}
                else{
					Console.WriteLine("Operacja modyfikacji lekcji nie powiodła się");
				}
			}
            else
            {
				Console.WriteLine("Nie masz dostępu do tej akcji");
			}
        }
	}
}
