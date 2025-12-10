using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Services;

public class VacationService : IVacationService
{
    private readonly IEmployeeRepository _repo;

    // Dependency Injection
    public VacationService(IEmployeeRepository repository)
    {
        _repo = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public bool AddVacationDays(int employeeId, int daysToAdd)
    {
        if (daysToAdd < 0 || daysToAdd > 365) throw new ArgumentException("Days: 0-365.");
        var employee = _repo.GetEmployeeById(employeeId);
        if (employee == null) return false;

        try
        {
            employee.AddVacationDays(daysToAdd);
            return true;
        }
        catch { return false; }
    }

    public bool UseVacationDays(int employeeId, int daysToUse)
    {
        if (daysToUse < 0) throw new ArgumentException("Days cannot be negative.");
        var employee = _repo.GetEmployeeById(employeeId);
        return employee?.UseVacationDays(daysToUse) ?? false;
    }

public string GenerateVacationReport(int employeeId)
{
    var employee = _repo.GetEmployeeById(employeeId);
    if (employee == null) 
        return "=== VACATION REPORT ===\nEmployee not found.\n=======================";

    var report = new System.Text.StringBuilder();
    // ANSI color codes: \x1b[38;5;<color>m for text, \x1b[0m to reset
    report.AppendLine("\x1b[36m=== VACATION REPORT ===\x1b[0m"); // Cyan title
    report.AppendLine($"\x1b[32mEmployee: {employee.GetFullName()} (ID: {employee.Id})\x1b[0m"); // Green name
    report.AppendLine($"Department: {employee.Department} | Tenure: {employee.GetYearsOfService()} Years");
    report.AppendLine();
    report.AppendLine($"\x1b[33mTotal Allocated: {employee.VacationDaysAvailable,5}\x1b[0m"); // Yellow
    report.AppendLine($"\x1b[31mDays Used:       {employee.VacationDaysUsed,5}\x1b[0m"); // Red
    report.AppendLine($"\x1b[32mRemaining:       {employee.GetRemainingVacationDays(),5}\x1b[0m"); // Green
    report.AppendLine("\x1b[36m=======================\x1b[0m"); // Cyan line
    return report.ToString();
}

    public IEnumerable<string> GenerateAllEmployeesVacationReport()
    {
        var employees = _repo.GetAllEmployees();
        return employees.Any() 
            ? employees.Select(e => GenerateVacationReport(e.Id)) 
            : new List<string> { "No employees to generate vacation reports for." };
    }

    public int GetRemainingVacationDays(int employeeId)
    {
        var employee = _repo.GetEmployeeById(employeeId);
        return employee?.GetRemainingVacationDays() ?? -1; // -1 = not found
    }
}