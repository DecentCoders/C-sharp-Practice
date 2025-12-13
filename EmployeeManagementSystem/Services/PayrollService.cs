using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Services;

public class PayrollService : IPayrollService
{
    private const decimal FederalTaxRate = 0.22m;
    private const decimal StateTaxRate = 0.05m;
    private readonly IEmployeeRepository _repo;

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
    report.AppendLine("\x1b[35m=== PAYROLL REPORT ===\x1b[0m"); // Magenta title
    report.AppendLine($"\x1b[32mEmployee: {employee.GetFullName()} (ID: {employee.Id})\x1b[0m"); 
    report.AppendLine($"Department: {employee.Department} | Tenure: {employee.GetYearsOfService()} Years");
    report.AppendLine();
    report.AppendLine($"\x1b[33mAnnual Gross:  ${CalculateAnnualGrossPay(employee),12:N2}\x1b[0m"); 
    report.AppendLine($"\x1b[31mAnnual Taxes:  ${CalculateTaxDeduction(employee),12:N2}\x1b[0m"); 
    report.AppendLine($"\x1b[32mAnnual Net:    ${CalculateAnnualGrossPay(employee) - CalculateTaxDeduction(employee),12:N2}\x1b[0m"); 
    report.AppendLine();
    report.AppendLine($"\x1b[33mMonthly Gross: ${CalculateMonthlyGrossPay(employee),12:N2}\x1b[0m"); 
    report.AppendLine($"\x1b[32mMonthly Net:   ${CalculateNetMonthlyPay(employee),12:N2}\x1b[0m"); 
    report.AppendLine($"Pay Period: {DateTime.Now:MMMM yyyy}");
    report.AppendLine("\x1b[35m=======================\x1b[0m"); 
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