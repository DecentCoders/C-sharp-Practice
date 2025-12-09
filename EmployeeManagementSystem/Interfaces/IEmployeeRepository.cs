using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Interfaces;

public interface IEmployeeRepository
{
    int GetNextAvailableId();
    IEnumerable<Employee> GetAllEmployees();
    Employee? GetEmployeeById(int id);
    IEnumerable<Employee> SearchEmployeesByName(string searchTerm);
    void AddEmployee(Employee employee);
    bool UpdateEmployee(Employee employee);
    bool DeleteEmployee(int id);
    bool EmployeeExists(int id);
}