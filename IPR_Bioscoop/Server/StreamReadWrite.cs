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

        public static void Write(List<Film> films)
        {
            using(Stream stream = File.Open(fileName, FileMode.Append))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, films);
            }
        }

        public static List<Film> Read()
        {
            List<Film> filmList = new List<Film>();
            using (Stream stream = File.Open(fileName, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                filmList = (List<Film>)binaryFormatter.Deserialize(stream);
            }

            return filmList;
        }
    }
}
