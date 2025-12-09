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
        if (employee == null) return "=== VACATION REPORT ===\nEmployee not found.\n=======================";

        var report = new System.Text.StringBuilder();
        report.AppendLine("=== VACATION REPORT ===");
        report.AppendLine($"Employee: {employee.GetFullName()} (ID: {employee.Id})");
        report.AppendLine($"Department: {employee.Department} | Tenure: {employee.GetYearsOfService()} Years");
        report.AppendLine();
        report.AppendLine($"Total Allocated: {employee.VacationDaysAvailable,5}");
        report.AppendLine($"Days Used:       {employee.VacationDaysUsed,5}");
        report.AppendLine($"Remaining:       {employee.GetRemainingVacationDays(),5}");
        report.AppendLine("=======================");
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