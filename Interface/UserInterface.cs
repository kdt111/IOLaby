using System;
using System.Collections.Generic;
using System.Text;
using IOLaby.Controllers;
using IOLaby.Data.Users;

namespace IOLaby.Interface
{
	class UserInterface
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
					case ConsoleKey.Escape: // Wyście z pętli - wyjście z aplikacji
						return;
				}
			}
		}

		private void PrintMenu()
		{
			Console.WriteLine("[ESC] - Wyjście z aplikacji");
		}
	}
}
