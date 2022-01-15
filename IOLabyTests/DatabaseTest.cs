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
	}
}