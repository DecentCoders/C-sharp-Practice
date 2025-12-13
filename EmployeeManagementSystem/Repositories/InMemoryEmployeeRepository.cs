using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.Enums;

namespace EmployeeManagementSystem.Repositories;

public class InMemoryEmployeeRepository : IEmployeeRepository
{
    private readonly List<Employee> _employees = new();
    private int _nextId = 1;

    public InMemoryEmployeeRepository()
    {
        var defaultEmployee = new Employee(
            id: _nextId++, 
            firstName: "Wang",
            lastName: "Haochen",
            department: Department.Engineering, 
            salary: 20000.00m, 
            hireDate: new DateTime(2020, 10, 20), 
            initialVacationDays: 15 
        );

        _employees.Add(defaultEmployee);
            var defaultEmployee2 = new Employee(
        id: _nextId++, 
        firstName: "Hridoy",
        lastName: "Hawladar",
        department: Department.Finance,  
        salary: 25000.00m,  
        hireDate: new DateTime(2021, 5, 15),  
        initialVacationDays: 20  
    );
    _employees.Add(defaultEmployee2);
    }

    public int GetNextAvailableId() => _nextId++;

    public IEnumerable<Employee> GetAllEmployees() => _employees.OrderBy(e => e.Id).ToList();

    public Employee? GetEmployeeById(int id) => _employees.FirstOrDefault(e => e.Id == id);

    public IEnumerable<Employee> SearchEmployeesByName(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return Enumerable.Empty<Employee>();
        var lowerTerm = searchTerm.Trim().ToLower();
        return _employees
            .Where(e => e.FirstName.ToLower().Contains(lowerTerm) || e.LastName.ToLower().Contains(lowerTerm))
            .OrderBy(e => e.Id)
            .ToList();
    }

    public void AddEmployee(Employee employee)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee));
        if (EmployeeExists(employee.Id)) throw new InvalidOperationException($"ID {employee.Id} already exists.");
        _employees.Add(employee);
    }

    public bool UpdateEmployee(Employee employee)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee));
        var existing = GetEmployeeById(employee.Id);
        if (existing == null) return false;

        existing.FirstName = employee.FirstName;
        existing.LastName = employee.LastName;
        existing.Department = employee.Department;
        existing.Salary = employee.Salary;
        existing.HireDate = employee.HireDate;
        return true;
    }

    public bool DeleteEmployee(int id)
    {
        var employee = GetEmployeeById(id);
        if (employee == null) return false;
        _employees.Remove(employee);
        return true;
    }

    public bool EmployeeExists(int id) => _employees.Any(e => e.Id == id);
}