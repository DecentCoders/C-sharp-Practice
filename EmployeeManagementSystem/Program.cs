using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Repositories;
using EmployeeManagementSystem.Services;
using EmployeeManagementSystem.UI;

namespace EmployeeManagementSystem;

class Program
{
    static void Main(string[] args)
    {
        // Dependency Injection (simple setup for console app)
        IEmployeeRepository repo = new InMemoryEmployeeRepository();
        IPayrollService payroll = new PayrollService(repo);
        IVacationService vacation = new VacationService(repo);

        // Launch UI
        var menu = new ConsoleMenu(repo, payroll, vacation);
        menu.Run();
    }
}