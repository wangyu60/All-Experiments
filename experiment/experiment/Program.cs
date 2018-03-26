using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Reflection;

namespace experiment
{
    class Program
    {
        static Assembly _assembly;
        static Stream _xmlStream;
        //StreamReader _textStreamReader;

        static void Main(string[] args)
        {
            _assembly = Assembly.GetExecutingAssembly();
            _xmlStream = _assembly.GetManifestResourceStream("experiment.Employees.xml");
            XElement xElement = XElement.Load(_xmlStream);
            IEnumerable<XElement> employees = xElement.Elements();

            //SimpleRead(employees);
            //PrintNameOnly(employees);
            //List_Names_with_ID(employees);
            //Female_Only(xElement);
            //Home_phone_nos(xElement);
            //In_Alta_city(xElement);
            //All_zip_codes(xElement);
            Apply_Sorting(xElement);

            Console.ReadLine();
        }

        private static void Apply_Sorting(XElement xElement)
        {
            IEnumerable<string> codes = from code in xElement.Elements("Employee")
                                        let zip = (string)code.Element("Address").Element("Zip")
                                        orderby zip
                                        select zip;
            Console.WriteLine("List and Sort all Zip Codes");

            foreach (string zp in codes)
                Console.WriteLine(zp);
        }

        //Find Nested Elements (using Descendants Axis)
        private static void All_zip_codes(XElement xElement)
        {
            Console.WriteLine("List all zip codes");
            foreach (var zip in xElement.Descendants("Zip"))
            {
                Console.WriteLine((string)zip);
            }
        }

        private static void In_Alta_city(XElement xElement)
        {
            var in_Alta = from employee in xElement.Elements("Employee")
                where (string)employee.Element("Address").Element("City") == "Alta"
                select employee;
            Console.WriteLine("List all employees living in Alta city");
            foreach (var employee in in_Alta)
            {
                Console.WriteLine(employee);
            }
        }

        private static void Home_phone_nos(XElement xElement)
        {
            var homeNumber = from phoneNm in xElement.Elements("Employee")
                let phoneEle = phoneNm.Element("Phone")
                where phoneEle != null && (string) phoneEle.Attribute("Type") == "Home"
                select phoneNm;

            Console.WriteLine("List all home numbers");
            foreach (var number in homeNumber)
            {
                Console.WriteLine(number.Element("Phone").Value);
            }
        }

        private static void Female_Only(XElement employees)
        {
            var name = from nm in employees.Elements("Employee")
                       where (string) nm.Element("Sex") == "Female"
                       select nm;
            Console.WriteLine("Details of female employees:");
            foreach (var xElement in name)
            {
                Console.WriteLine(xElement);
            }
        }

        private static void List_Names_with_ID(IEnumerable<XElement> employees)
        {
            foreach (var employee in employees)
            {
                var name = employee.Element("Name");
                if (name != null)
                    Console.WriteLine($"{name.Value} has Employee ID {employee.Element("EmpId").Value}");
            }
        }

        private static void PrintNameOnly(IEnumerable<XElement> employees)
        {
            foreach (var employee in employees)
            {
                var xElement = employee.Element("Name");
                if (xElement != null)
                    Console.WriteLine(xElement.Value);
            }
        }

        private static void SimpleRead(IEnumerable<XElement> employees)
        {
            foreach (var employee in employees)
            {
                Console.WriteLine(employee);
            }
        }
    }
}
