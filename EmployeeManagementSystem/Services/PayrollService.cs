using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Services;

public class PayrollService : IPayrollService
{
    // Fixed tax rates (can be modified)
    private const decimal FederalTaxRate = 0.22m;
    private const decimal StateTaxRate = 0.05m;
    private readonly IEmployeeRepository _repo;

    // Dependency Injection
    public PayrollService(IEmployeeRepository repository)
    {
        _repo = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public decimal CalculateAnnualGrossPay(Employee employee) => employee.Salary;

    public decimal CalculateMonthlyGrossPay(Employee employee) => CalculateAnnualGrossPay(employee) / 12;

    public decimal CalculateTaxDeduction(Employee employee)
    {
        var annualGross = CalculateAnnualGrossPay(employee);
        return annualGross * (FederalTaxRate + StateTaxRate);
    }

    public decimal CalculateNetMonthlyPay(Employee employee)
    {
        var annualNet = CalculateAnnualGrossPay(employee) - CalculateTaxDeduction(employee);
        return annualNet / 12;
    }

    public string GeneratePayrollReport(Employee employee)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee));

        var report = new System.Text.StringBuilder();
        report.AppendLine("=== PAYROLL REPORT ===");
        report.AppendLine($"Employee: {employee.GetFullName()} (ID: {employee.Id})");
        report.AppendLine($"Department: {employee.Department} | Tenure: {employee.GetYearsOfService()} Years");
        report.AppendLine();
        report.AppendLine($"Annual Gross:  ${CalculateAnnualGrossPay(employee),12:N2}");
        report.AppendLine($"Annual Taxes:  ${CalculateTaxDeduction(employee),12:N2}");
        report.AppendLine($"Annual Net:    ${CalculateAnnualGrossPay(employee) - CalculateTaxDeduction(employee),12:N2}");
        report.AppendLine();
        report.AppendLine($"Monthly Gross: ${CalculateMonthlyGrossPay(employee),12:N2}");
        report.AppendLine($"Monthly Net:   ${CalculateNetMonthlyPay(employee),12:N2}");
        report.AppendLine($"Pay Period: {DateTime.Now:MMMM yyyy}");
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        report.AppendLine("======================");
        Console.ResetColor();
        return report.ToString();
    }

    public IEnumerable<string> GenerateAllEmployeesPayrollReport()
    {
        var employees = _repo.GetAllEmployees();
        return employees.Any() 
            ? employees.Select(GeneratePayrollReport) 
            : new List<string> { "No employees to generate payroll for." };
    }
}