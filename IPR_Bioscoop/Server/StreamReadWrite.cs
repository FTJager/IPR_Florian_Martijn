using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server
{
    class StreamReadWrite
    {
        private static string fileName = Path.GetFullPath("Movies.xml");
        public StreamReadWrite()
        {

        }

        /// <summary>
        /// Writes a list of movies to a file
        /// </summary>
        /// <param name="films">List of movies to be saved</param>
        public static void Write(List<Film> films)
        {
            using(Stream stream = File.Open(fileName, FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, films);
                stream.Close();
            }
        }

        /// <summary>
        /// Reads a list of movies from a file
        /// </summary>
        /// <returns></returns>
        public static List<Film> Read()
        {
            List<Film> filmList = new List<Film>();
            using (Stream stream = File.Open(fileName, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                filmList = (List<Film>)binaryFormatter.Deserialize(stream);
                stream.Close();
            }

            return filmList;
        }

        /// <summary>
        /// Empties the file
        /// </summary>
        public static void ClearFile()
        {
            using(Stream stream = File.Open(fileName, FileMode.Create))
            {
                stream.Close();
            }
        }
    }
}
