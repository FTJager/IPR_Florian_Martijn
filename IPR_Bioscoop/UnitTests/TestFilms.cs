using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class TestFilms
    {
        [TestMethod]
        public void testFilm()
        {
            Assert.IsNotNull(new Server.Film("title", 10, "desc", 20));
            Assert.IsNull(new Server.Film());
        }
    }
}
