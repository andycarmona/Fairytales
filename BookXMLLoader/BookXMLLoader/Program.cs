using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookXMLLoader
{
    using System.IO;
    using System.Xml.Serialization;

    

    class Program
    {
        static void Main(string[] args)
        {
            var serializer = new XmlSerializer(typeof(book));
            var stream = new FileStream("C:/Users/andresc/Desktop/FairyTales/fairytales/examplebook/book.xml", FileMode.Open);
            var container = serializer.Deserialize(stream) as book;
            stream.Close();
        }
    }
}
