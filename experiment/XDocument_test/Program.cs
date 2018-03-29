using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Xml.Linq;

namespace XDocument_test
{
    class Program
    {
        private static Assembly _assembly;
        private static Stream _xmlStream;
        static void Main(string[] args)
        {
            _assembly = Assembly.GetExecutingAssembly();
            _xmlStream = _assembly.GetManifestResourceStream("XDocument_test.Employees.xml");
            XDocument xDocument = XDocument.Load(_xmlStream);
            IEnumerable<XElement> employees = xDocument.Elements();
            foreach (var employee in employees)
            {
                Console.WriteLine(employee);
            }


            Console.ReadLine();
        }
    }
}
