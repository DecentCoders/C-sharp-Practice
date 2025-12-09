namespace EmployeeManagementSystem.Interfaces;

public interface IVacationService
{
    bool AddVacationDays(int employeeId, int daysToAdd);
    bool UseVacationDays(int employeeId, int daysToUse);
    string GenerateVacationReport(int employeeId);
    IEnumerable<string> GenerateAllEmployeesVacationReport();
    int GetRemainingVacationDays(int employeeId);
}