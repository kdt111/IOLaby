using NUnit.Framework;
using IOLaby.Data;

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

		[Test]
		public void Test1()
		{
			Assert.AreEqual(true, true);
		}

		// Michal tests

		[Test]
		public void TestGetLessonData()
        {
			Assert.AreEqual(1, 1);
        }
	}
}