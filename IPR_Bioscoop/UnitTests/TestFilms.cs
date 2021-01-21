using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class TestFilms
    {
        private Server.Film film;

        [TestInitialize]
        public void initTest()
        {
            film = new Server.Film("test", 10, "TestFilm", 100);
        }

        [TestMethod()]
        public void testFilm()
        {
            Assert.IsNotNull(film);
            //Assert.IsNull(new Server.Film());
        }

        [TestMethod]
        public void testGetTitle()
        {
            string testTitle = "test";
            Assert.AreEqual(testTitle, film.getTitle);
        }


    }
}
