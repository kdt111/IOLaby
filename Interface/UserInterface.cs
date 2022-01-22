using System;
using System.Collections.Generic;
using System.Text;
using IOLaby.Controllers;
using IOLaby.Data.Users;
using IOLaby;

namespace IOLaby.Interface
{
	public class UserInterface
	{
		public void ClearScreen()
		{
			Console.Clear();
		}

		public BaseUser LoginScreen(UserController userController)
		{
			string login, password;

			Console.WriteLine("Logowanie");
			Console.Write("Login: ");
			login = Console.ReadLine();
			Console.Write("Hasło: ");
			password = Console.ReadLine();

			return userController.ValidateUser(login, password);
		}

		public void MainLoop()
		{
			while(true)
			{
				PrintMenu();
				ConsoleKey key = Console.ReadKey(true).Key;
				switch (key)
				{
					case ConsoleKey.D1:
						Application.DisplayGroupInterface();
						break;
					case ConsoleKey.D2:
						Application.DisplayCalendar();
						break;
					case ConsoleKey.D3:
						Application.ModifyLessonInterface();
						break;
					case ConsoleKey.Escape: // Wyście z pętli - wyjście z aplikacji
						return;
				}
			}
		}

		private void PrintMenu()
		{
			Console.WriteLine("1 - Wyświetl grupy");
			Console.WriteLine("2 - Wyświetl terminarz");
			Console.WriteLine("3 - Zmodyfikuj lekcje");
			Console.WriteLine("[ESC] - Wyjście z aplikacji");
		}
	}
}
