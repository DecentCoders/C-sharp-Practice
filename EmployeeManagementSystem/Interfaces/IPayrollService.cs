using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Interfaces;

public interface IPayrollService
{
    decimal CalculateMonthlyGrossPay(Employee employee);
    decimal CalculateAnnualGrossPay(Employee employee);
    decimal CalculateTaxDeduction(Employee employee);
    decimal CalculateNetMonthlyPay(Employee employee);
    string GeneratePayrollReport(Employee employee);
    IEnumerable<string> GenerateAllEmployeesPayrollReport();
}