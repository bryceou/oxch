using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace ConTest
{

    public class Student
    {
        public string Field1 { get; set; }
        public DateTime? Field2 { get; set; }

    }
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Staff.xml";//add comment
            XDocument doc = XDocument.Load(filePath);
            IEnumerable<XElement> staffs = doc.Descendants("Staff");
            foreach (XElement staff in staffs)
            {
                staff.Element("Name").ReplaceWith(new XElement("Name", new XCData(staff.Element("Name").Value)));
            }
            doc.Save(filePath);

       
            Console.WriteLine("Complete");
            Console.Read();
        }
    }
}
