using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookXMLLoader
{
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    

    class Program
    {
        static void Main(string[] args)
        {
           var serializer = new XmlSerializer(typeof(book));
           var stream = new FileStream("bookNoMetadata2.xml", FileMode.Open);
           var container = serializer.Deserialize(stream) as book;
           
         stream.Close();
            

        }
    }
}
