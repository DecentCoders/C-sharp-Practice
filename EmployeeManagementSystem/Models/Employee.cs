using EmployeeManagementSystem.Models.Enums;

namespace EmployeeManagementSystem.Models;

public class Employee
{
    private int _id;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private Department _department;
    private decimal _salary;
    private DateTime _hireDate;
    private int _vacationDaysAvailable;
    private int _vacationDaysUsed;

    public int Id 
    { 
        get => _id; 
        private set 
        {
            if (value <= 0) throw new ArgumentException("ID must be positive.");
            _id = value;
        }
    }

    public string FirstName 
    { 
        get => _firstName; 
        set 
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || value.Length > 50)
                throw new ArgumentException("First name: 2-50 non-empty characters.");
            _firstName = value.Trim();
        }
    }

    public string LastName 
    { 
        get => _lastName; 
        set 
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || value.Length > 50)
                throw new ArgumentException("Last name: 2-50 non-empty characters.");
            _lastName = value.Trim();
        }
    }

    public Department Department 
    { 
        get => _department; 
        set 
        {
            if (!Enum.IsDefined(typeof(Department), value))
                throw new ArgumentException("Invalid department.");
            _department = value;
        }
    }

    public decimal Salary 
    { 
        get => _salary; 
        set 
        {
            if (value < 10000 || value > 1000000)
                throw new ArgumentException("Salary: $10,000 - $1,000,000.");
            _salary = value;
        }
    }

public DateTime HireDate 
{ 
    get => _hireDate; 
    set 
    {
        if (value > DateTime.Now)
            throw new ArgumentException("Hire date cannot be in the future.");
        
        var companyEstablishmentDate = new DateTime(2020, 1, 1);
        if (value < companyEstablishmentDate)
            throw new ArgumentException("our company was established in 2020. Hire date cannot be before 2020.");
        
        _hireDate = value;
    }
}

    public int VacationDaysAvailable 
    { 
        get => _vacationDaysAvailable; 
        private set 
        {
            if (value < 0 || value > 365)
                throw new ArgumentException("Vacation days: 0-365.");
            _vacationDaysAvailable = value;
        }
    }

    public int VacationDaysUsed 
    { 
        get => _vacationDaysUsed; 
        private set 
        {
            if (value < 0)
                throw new ArgumentException("Used days cannot be negative.");
            _vacationDaysUsed = value;
        }
    }

    public Employee(int id, string firstName, string lastName, Department department, 
                   decimal salary, DateTime hireDate, int initialVacationDays)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Department = department;
        Salary = salary;
        HireDate = hireDate;
        VacationDaysAvailable = initialVacationDays;
        VacationDaysUsed = 0;
    }

    public int GetRemainingVacationDays() => VacationDaysAvailable - VacationDaysUsed;
    public string GetFullName() => $"{FirstName} {LastName}";
    public int GetYearsOfService() => DateTime.Now.Year - HireDate.Year;

    internal void AddVacationDays(int days) => VacationDaysAvailable += days;
    internal bool UseVacationDays(int days)
    {
        if (days > GetRemainingVacationDays()) return false;
        VacationDaysUsed += days;
        return true;
    }
    public override string ToString()
    {
        return $"ID: {Id,-5} Name: {GetFullName(),-20} Dept: {Department,-12} " +
               $"Salary: ${Salary,10:N2} Hire Date: {HireDate:MM/dd/yyyy} " +
               $"Tenure: {GetYearsOfService()}y " +
               $"Vacation: {VacationDaysAvailable}/{VacationDaysUsed}/{GetRemainingVacationDays()}";
    }
}