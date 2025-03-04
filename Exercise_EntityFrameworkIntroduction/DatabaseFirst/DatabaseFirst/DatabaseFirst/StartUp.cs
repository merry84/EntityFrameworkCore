
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Linq;
using System.Text;
namespace SoftUni.Data
{

    public class StartUp
    {
        public static void Main(string[] args)
        {


            SoftUniContext dbContext = new SoftUniContext();
            string result = RemoveTown(dbContext);

            Console.WriteLine(result);
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
        //6.Adding a New Address and Updating Employee
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
        //7.Employees and Projects
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {


            StringBuilder sb = new StringBuilder();

            var employeesWithProjects = context
                .Employees
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    ManagerFirstName = e.Manager == null ?
                        null : e.Manager.FirstName,
                    ManagerLastName = e.Manager == null ?
                        null : e.Manager.LastName,
                    Projects = e.EmployeesProjects
                        .Select(ep => ep.Project)
                        .Where(p => p.StartDate.Year >= 2001 &&
                                    p.StartDate.Year <= 2003)
                        .Select(p => new
                        {
                            ProjectName = p.Name,
                            p.StartDate,
                            p.EndDate
                        })
                        .ToArray()
                })
                .Take(10)
                .ToArray();

            foreach (var e in employeesWithProjects)
            {
                sb
                    .AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");
                foreach (var p in e.Projects)
                {
                    string startDateFormatted = p.StartDate
                        .ToString("M/d/yyyy h:mm:ss tt");
                    string endDateFormatted = p.EndDate.HasValue ?
                        p.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished";
                    sb
                        .AppendLine($"--{p.ProjectName} - {startDateFormatted} - {endDateFormatted}");
                }
            }

            return sb.ToString().TrimEnd();
        }
        //8.	Addresses by Town
        public static string GetAddressesByTown(SoftUniContext context)
        {

            StringBuilder sb = new();
            var adrresses = context.Addresses

                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                    Text = a.AddressText,
                    TownName = a.Town.Name,
                    EmplCount = a.Employees.Count
                })
                .ToArray();
            foreach (var adr in adrresses)
            {

                sb.AppendLine($"{adr.Text}, {adr.TownName} - {adr.EmplCount} employees");
            }

            return sb.ToString().TrimEnd();
        }
        //9.Employee 147
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new();
            var employee = context.Employees
                .Find(147);

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            sb.AppendLine(string.Join(Environment.NewLine, employee!
                .EmployeesProjects
                .OrderBy(p => p.Project.Name)
                .Select(p => p.Project.Name)));

            return sb.ToString().TrimEnd();
        }
        //10. Departments with More Than 5 Employees
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new();
            var department = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name,
                    ManagerFirstname = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees
                     .OrderBy(e => e.FirstName)
                     .ThenBy(e => e.LastName)
                     .Select(e => new
                     {
                         e.FirstName,
                         e.LastName,
                         e.JobTitle
                     })
                     .ToList()

                })
                 .ToList();


            foreach (var dep in department)
            {
                sb.AppendLine($"{dep.Name} - {dep.ManagerFirstname}  {dep.ManagerLastName}");

                foreach (var empl in dep.Employees)
                {
                    sb.AppendLine($"{empl.FirstName} {empl.LastName} - {empl.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }
        //11.Find Latest 10 Projects
        public static string GetLatestProjects(SoftUniContext context)
        {

            StringBuilder sb = new();

            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })

                .Take(10)
                .OrderBy(p => p.Name)
                .ToArray();
            foreach (var pr in projects)
            {
                sb.AppendLine($"{pr.Name}");

                sb.AppendLine($"{pr.Description}");
                sb.AppendLine($"{pr.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");

            }

            return sb.ToString().TrimEnd();
        }
        //12.Increase Salaries

        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new();


            string[] departmentsToIncrease = { "Engineering", "Tool Design", "Marketing", "Information Services" };

            var employees = context.Employees
                .Where(e => departmentsToIncrease.Contains(e.Department.Name))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList(); // Материализираме резултата

            foreach (var employee in employees)
            {
                employee.Salary *= 1.12m;
            }

            context.SaveChanges();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }
        //13.Find Employees by First Name Starting with "Sa"
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new();
            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            foreach (var empl in employees)
            {
                sb.AppendLine($"{empl.FirstName} {empl.LastName} - {empl.JobTitle} - (${empl.Salary:f2})");
            }
            return sb.ToString().TrimEnd();
        }
        //14.	Delete Project by Id
        public static string DeleteProjectById(SoftUniContext context)
        {


            const int deleteProjectId = 2;

            var employeeProjectsDelete = context.EmployeesProjects
                .Where(ep => ep.ProjectId == deleteProjectId)
                .ToList();

            context.EmployeesProjects.RemoveRange(employeeProjectsDelete);

            var deleteProject = context.Projects.Find(deleteProjectId);

            if (deleteProject != null)
            {
                context.Projects.Remove(deleteProject);
            }

            context.SaveChanges();

            string[] projectNames = context.Projects
                .Select(p => p.Name)
                .Take(10)
                .ToArray();

            return string.Join(Environment.NewLine, projectNames);

        }
        //
        public static string RemoveTown(SoftUniContext context)
        {

            var addressIdsToRemove = context.Addresses
                           .Where(a => a.Town.Name == "Seattle")
                           .Select(a => a.AddressId)
                           .ToList();

            var employees = context.Employees
                .ToList();

            foreach (var id in addressIdsToRemove)
            {
                foreach (var emp in employees)
                {
                    emp.AddressId = null;
                }
            }

            var addressesToRemove = context.Addresses
                .Where(a => a.Town.Name == "Seattle")
                .ToList();

            foreach (var address in addressesToRemove)
            {
                context.Addresses.Remove(address);
            }

            var townToRemove = context.Towns
                .FirstOrDefault(t => t.Name == "Seattle");

            context.Towns.Remove(townToRemove);

            context.SaveChanges();

            return $"{addressesToRemove.Count} addresses in Seattle were deleted";
        }
    }
}



