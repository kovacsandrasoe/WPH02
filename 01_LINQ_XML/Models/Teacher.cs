using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _01_LINQ_XML.Models
{
    class Teacher
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Dept { get; set; }

        public string Rank { get; set; }

        public int Floor { get; set; }

        public int Room { get; set; }

        public static List<Teacher> ImportFromXML(string xml_file_name)
        {
            List<Teacher> collection = new List<Teacher>();

            XDocument xdoc = XDocument.Load(xml_file_name);
            foreach (XElement item in xdoc.Descendants("person"))
            {
                collection.Add(new Teacher()
                {
                    Name = item.Element("name").Value,
                    Email = item.Element("email").Value,
                    Dept = item.Element("dept").Value,
                    Rank = item.Element("rank").Value,
                    Floor = int.Parse(item.Element("room").Value.Split('.')[1]),
                    Room = int.Parse(item.Element("room").Value.Split('.')[2])
                });
            }

            return collection;
        }
    }



}
