using NUnit.Framework;
using IOLaby.Data;
using IOLaby.Data.Users;
using IOLaby.Data.Classes;
using Moq;
using System.Collections.Generic;

namespace IOLabyTests
{
	public class Tests
	{
		private Database database;

		[SetUp]
		public void Setup()
		{
			database = new Database();
		}

        // Michal's tests
        [Test]
        public void TestGetLessonData()
        {
            // success scenario
            ClassGroup classGroup = database.ClassGroupList[0];
            Lesson lesson = classGroup.LessonList[0];
            Assert.AreEqual(lesson, database.GetLessonData(classGroup.Teacher, lesson.LessonId));

            // wrong teacher scenario
            ClassGroup wrongClassGroup = database.ClassGroupList[1];
            Assert.IsNull(database.GetLessonData(wrongClassGroup.Teacher, lesson.LessonId));

            // wrong lesson id scenario
            Assert.IsNull(database.GetLessonData(classGroup.Teacher, 2137));
        }
        [Test]
		public void TestGetUserAtendances()
        {
			// variables setup
			Student student = (Student)database.UserList[0];
			Student nullStudent = null;
			ClassGroup classGroup = database.ClassGroupList[0];
			ClassGroup nullClassGroup = null;

			// success scenario
			// mock setup
			var mock_1 = new Mock<Database>();
			mock_1.Setup(db => db.FindUser(student.UserId)).Returns(student);
			mock_1.Setup(db => db.GetGroup(student, classGroup.ClassId)).Returns(classGroup);
			mock_1.CallBase = true;
			Database mockedDatabase_1 = mock_1.Object;

			// test

			Dictionary<Lesson, Atendence> studentAtendences_1 = mockedDatabase_1.GetUserAtendances(student.UserId, classGroup.ClassId);
			Assert.AreEqual(2, studentAtendences_1.Count);

			// wrong student id scenario
			// mock setup
			var mock_2 = new Mock<Database>();
			mock_2.Setup(db => db.FindUser(student.UserId)).Returns(nullStudent);
			mock_2.CallBase = true;
			Database mockedDatabase_2 = mock_2.Object;

			// test
			Dictionary<Lesson, Atendence> studentAtendences_2 = mockedDatabase_2.GetUserAtendances(student.UserId, classGroup.ClassId);
            Assert.AreEqual(0, studentAtendences_2.Count);

			// wrong group id scenario
			var mock_3 = new Mock<Database>();
			mock_3.Setup(db => db.FindUser(student.UserId)).Returns(student);
			mock_3.Setup(db => db.GetGroup(student, classGroup.ClassId)).Returns(nullClassGroup);
			mock_3.CallBase = true;
			Database mockedDatabase_3 = mock_3.Object;

			// test
			Dictionary<Lesson, Atendence> studentAtendences_3 = mockedDatabase_3.GetUserAtendances(student.UserId, classGroup.ClassId);
			Assert.AreEqual(0, studentAtendences_3.Count);
		}

		// Damian's Tests
		[Test]
		public void TestGetLesson()
        {
			// setup variables for testing
			Lesson lesson = database.ClassGroupList[0].LessonList[0];
			
			// situation with success
			Assert.AreEqual(lesson, database.GetLesson(0));
			// null scenarios - no lesson
			Assert.IsNull(database.GetLesson(2137), "Found an instance of the lesson which is not included in a list");
			// null scenario - when index is lover than  0
			Assert.IsNull(database.GetLesson(-1), "Returned a lesson for index <0");

        }
		[Test]
		public void TestGetUserGrades()
        {
			//setup 
			Student student = (Student)database.UserList[0];
			Student nullStudent = null;
			ClassGroup classGroup = database.ClassGroupList[0];
			ClassGroup nullClassGroup = null;


			//success 
			//mock
			var mock1 = new Mock<Database>();
			mock1.Setup(db => db.FindUser(student.UserId)).Returns(student);
			mock1.Setup(db => db.GetGroup(student, classGroup.ClassId)).Returns(classGroup);
			mock1.CallBase = true;
			Database mockedDatabase1 = mock1.Object;

			// test
			Dictionary<GradeGroup, Grade> studentGrades1 = mockedDatabase1.GetUserGrades(student.UserId, classGroup.ClassId);
			Assert.AreEqual(2, studentGrades1.Count);
			//wrongId
				//wrongStudent 
			//setup
			var mock2 = new Mock<Database>();
			mock2.Setup(db => db.FindUser(student.UserId)).Returns(nullStudent);
			mock2.CallBase = true;
			Database mockedDatabase2 = mock2.Object;

			// test
			Dictionary<GradeGroup, Grade> studentGrades2 = mockedDatabase2.GetUserGrades(student.UserId, classGroup.ClassId);
			Assert.AreEqual(0, studentGrades2.Count);
				//wrongclass
			//setup
			var mock3 = new Mock<Database>();
			mock3.Setup(db => db.FindUser(student.UserId)).Returns(student);
			mock3.Setup(db => db.GetGroup(student, classGroup.ClassId)).Returns(nullClassGroup);
			mock3.CallBase = true;
			Database mockedDatabase_3 = mock3.Object;

			//test
			Dictionary<GradeGroup, Grade> studentGrades3 = mockedDatabase2.GetUserGrades(student.UserId, classGroup.ClassId);
			Assert.AreEqual(0, studentGrades3.Count);


			// wrong id and classGroup
			//setup
			var mock4 = new Mock<Database>();
			mock4.Setup(db => db.FindUser(student.UserId)).Returns(nullStudent);
			mock4.Setup(db => db.GetGroup(student, classGroup.ClassId)).Returns(nullClassGroup);
			mock4.CallBase = true;
			Database mockedDatabase4 = mock4.Object;

			//test
			Dictionary<GradeGroup, Grade> studentGrades4 = mockedDatabase2.GetUserGrades(student.UserId, classGroup.ClassId);
			Assert.AreEqual(0, studentGrades4.Count);

		}

		// Jan's Tests
		[Test]
		public void TestValidateUser()
		{
			BaseUser user = database.UserList[0];

			// Success
			Assert.AreEqual(user, database.ValidateUser(user.Login, user.Password));

			// Wrong credentials
			Assert.IsNull(database.ValidateUser(user.Login, "wrong-password"));
			Assert.IsNull(database.ValidateUser("wrong-login", user.Password));

			// No user
			Assert.IsNull(database.ValidateUser("wrong-login", "wrong-password"));

			// Wrong character case
			Assert.IsNull(database.ValidateUser(user.Login.ToUpper(), user.Password));
			Assert.IsNull(database.ValidateUser(user.Login, user.Password.ToUpper()));
		}
		[Test]
		public void TestGetUserClasses()
		{
			Student student = (Student)database.UserList[0];
			Teacher teacher = (Teacher)database.UserList[3];

			Student nullStudent = null;
			Teacher nullTeacher = null;

			// Success student
			Mock<Database> mock1 = new Mock<Database>();
			mock1.Setup(db => db.FindUser(student.UserId)).Returns(student);
			mock1.CallBase = true;

			List<ClassGroup> groups = mock1.Object.GetUserClasses(student.UserId);
			Assert.AreEqual(3, groups.Count);

			// Success teacher
			Mock<Database> mock2 = new Mock<Database>();
			mock2.Setup(db => db.FindUser(teacher.UserId)).Returns(teacher);
			mock2.CallBase = true;

			groups = mock2.Object.GetUserClasses(teacher.UserId);
			Assert.AreEqual(1, groups.Count);

			// Wrong student id
			Mock<Database> mock3 = new Mock<Database>();
			mock3.Setup(db => db.FindUser(student.UserId)).Returns(nullStudent);
			mock3.CallBase = true;

			groups = mock3.Object.GetUserClasses(student.UserId);
			Assert.AreEqual(0, groups.Count);

			// Wrong teacher id
			Mock<Database> mock4 = new Mock<Database>();
			mock4.Setup(db => db.FindUser(teacher.UserId)).Returns(nullTeacher);
			mock4.CallBase = true;

			groups = mock4.Object.GetUserClasses(teacher.UserId);
			Assert.AreEqual(0, groups.Count);
		}
	}
}