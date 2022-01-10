using System;
using IOLaby.Interface;
using IOLaby.Data;
using IOLaby.Controllers;
using IOLaby.Data.Users;

namespace IOLaby
{
	class Application
	{
		static void Main(string[] args)
		{
			UserInterface userInterface = new UserInterface();
			Database database = new Database();

			UserController userController = new UserController(database);
			ClassesController classesController = new ClassesController(database);

			userInterface.ClearScreen();
			BaseUser user = null;
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
	}
}
