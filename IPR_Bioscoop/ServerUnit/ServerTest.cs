using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ServerUnit
{
    [TestClass]
    public class ServerTest
    {
        public ClientHandling clientHandling;

        public void testSetup()
        {
            List<Film> testFilms = new List<Film>();

            testFilms.Add(new Film("testMovie", 120, "This is a test", 500));
            testFilms.Add(new Film("testMovie2", 90, "This is also a test", 650));
            testFilms[0].Date = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
            testFilms[1].Date = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));

            clientHandling = new ClientHandling(new TcpClient(), testFilms);
        }

        [TestMethod]
        public void TestMethod1()
        {
            testSetup();
            List<Film> testFilms = new List<Film>();

            testFilms.Add(new Film("testMovie", 120, "This is a test", 500));
            testFilms.Add(new Film("testMovie2", 90, "This is also a test", 650));
            testFilms[0].Date = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
            testFilms[1].Date = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));

            List<Film> expectedBefore = Server.Server.MakeFilmList();
            List<Film> expectedAfter = testFilms;

            Assert.AreEqual(expectedBefore, Server.Server.films);
            Server.Server.updateFilms(testFilms);
            Assert.AreEqual(expectedAfter, Server.Server.films);
        }
    }
}
