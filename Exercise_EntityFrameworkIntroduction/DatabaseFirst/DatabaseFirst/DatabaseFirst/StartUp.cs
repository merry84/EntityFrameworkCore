using SoftUni.Data;
using SoftUni.Models;
using System.Text;

namespace SoftUni
{


    public class StartUp
    {
        public static void Main(string[] args)
        {
            using SoftUniContext dbContext = new();
            dbContext.Database.EnsureCreated();



            using (var context = new SoftUniContext())
            {
                var result = AddNewAddressToEmployee(context);

                Console.WriteLine(result);
            }
        }
        //03. Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new();

            var emploeyes = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary

                }).ToArray();

            foreach (var empl in emploeyes)
            {
                sb.AppendLine($"{empl.FirstName} {empl.LastName} {empl.MiddleName} {empl.JobTitle} {empl.Salary:F2}");
            }
            return sb.ToString().TrimEnd();
        }

        //4.Employees with Salary Over 50 000;
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {

            StringBuilder sb = new();
            var employees = context.Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ToArray();
            foreach (var empl in employees)
            {

                sb.AppendLine($"{empl.FirstName} - {empl.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        //5.Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new();
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .Where(e => e.DepartmentName == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToArray();

            foreach (var empl in employees)
            {
                sb.AppendLine($"{empl.FirstName} {empl.LastName} from {empl.DepartmentName} - ${empl.Salary:f2}");
            }
            return sb.ToString().TrimEnd();

        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var address = new Address//create
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };
            var employee = context.Employees.First(e => e.LastName == "Nakov");//search

            address.Employees.Add(employee);//add employee with address
            employee.Address = address;

            context.SaveChanges();//SAVE!!!!

            var employeesAdrress = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => e.Address.AddressText)
                .Take(10)
                .ToArray();


            StringBuilder sb = new();

            foreach (var adrress in employeesAdrress)
            {
                sb.AppendLine(adrress);
            }
            return sb.ToString().TrimEnd();
        }
    }
}
