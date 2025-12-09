using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.Enums;

namespace EmployeeManagementSystem.UI;

public class ConsoleMenu
{
    private readonly IEmployeeRepository _repo;
    private readonly IPayrollService _payroll;
    private readonly IVacationService _vacation;

    // Inject dependencies
    public ConsoleMenu(IEmployeeRepository repo, IPayrollService payroll, IVacationService vacation)
    {
        _repo = repo;
        _payroll = payroll;
        _vacation = vacation;
    }

    public void Run()
    {
        Console.Title = "Employee Management System";
        bool isRunning = true;

        while (isRunning)
        {
            DisplayMainMenu();
            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1": AddEmployee(); break;
                case "2": UpdateEmployee(); break;
                case "3": DeleteEmployee(); break;
                case "4": SearchEmployee(); break;
                case "5": ShowAllEmployees(); break;
                case "6": PayrollMenu(); break;
                case "7": VacationMenu(); break;
                case "8": isRunning = false; Console.WriteLine("Exiting... Thank you!"); break;
                default: ShowError("Invalid choice. Enter 1-8."); break;
            }

            if (isRunning)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    private void DisplayMainMenu()
    {
        Console.WriteLine("==================== EMPLOYEE MANAGEMENT SYSTEM ====================");
        Console.WriteLine("1. Add New Employee");
        Console.WriteLine("2. Update Employee");
        Console.WriteLine("3. Delete Employee");
        Console.WriteLine("4. Search Employee (ID/Name)");
        Console.WriteLine("5. Show All Employees");
        Console.WriteLine("6. Payroll Menu");
        Console.WriteLine("7. Vacation Menu");
        Console.WriteLine("8. Exit");
        Console.WriteLine("====================================================================");
        Console.Write("Enter your choice: ");
    }

    #region Employee Core Operations
    private void AddEmployee()
    {
        Console.WriteLine("\n=== ADD NEW EMPLOYEE ===");
        try
        {
            var id = _repo.GetNextAvailableId();
            var firstName = GetValidString("First Name (2-50 chars): ", s => s.Length is >=2 and <=50);
            var lastName = GetValidString("Last Name (2-50 chars): ", s => s.Length is >=2 and <=50);
            var dept = GetValidDepartment();
            // In the AddEmployee() method, replace the OLD salary line with this NEW one:
var salary = GetValidDecimal(
    prompt: "Annual Salary ($10,000 - $1,000,000): ", // No $ in prompt (avoids confusion)
    validator: d => d is >= 10000 and <= 1000000,
    error: "❌ Salary must be a number between 10000 and 1000000 (e.g., 80000)",
    isCurrency: true // Enables $ stripping if user accidentally enters it
);
            var hireDate = GetValidDate("Hire Date (MM/dd/yyyy): ");
            var vacationDays = GetValidInt("Initial Vacation Days (0-365): ", i => i is >=0 and <=365);

            var employee = new Employee(id, firstName, lastName, dept, salary, hireDate, vacationDays);
            _repo.AddEmployee(employee);
            ShowSuccess($"Employee added! ID: {id} | Name: {employee.GetFullName()}");
        }
        catch (Exception ex)
        {
            ShowError($"Add failed: {ex.Message}");
        }
    }

    private void UpdateEmployee()
    {
        Console.WriteLine("\n=== UPDATE EMPLOYEE ===");
        try
        {
            var id = GetValidInt("Enter Employee ID: ", _repo.EmployeeExists, "Employee not found.");
            var existing = _repo.GetEmployeeById(id)!;

            Console.WriteLine($"Current Details:\n{existing}\n");
            Console.WriteLine("Leave field empty to keep current value.");

            var firstName = GetOptionalString("New First Name: ", s => s.Length is >=2 and <=50, existing.FirstName);
            var lastName = GetOptionalString("New Last Name: ", s => s.Length is >=2 and <=50, existing.LastName);
            var dept = GetOptionalDepartment(existing.Department);
          // In the UpdateEmployee() method, replace the OLD salary line with this NEW one:
var salary = GetOptionalDecimal(
    prompt: "New Annual Salary ($10,000 - $1,000,000): ",
    validator: d => d is >= 10000 and <= 1000000,
    defaultValue: existing.Salary
);
            var hireDate = GetOptionalDate("New Hire Date: ", existing.HireDate);

            var updated = new Employee(id, firstName, lastName, dept, salary, hireDate, existing.VacationDaysAvailable);
            var success = _repo.UpdateEmployee(updated);

            if (success) ShowSuccess("Employee updated!");
            else ShowError("Update failed.");
        }
        catch (Exception ex)
        {
            ShowError($"Update failed: {ex.Message}");
        }
    }

    private void DeleteEmployee()
    {
        Console.WriteLine("\n=== DELETE EMPLOYEE ===");
        try
        {
            var id = GetValidInt("Enter Employee ID: ", _repo.EmployeeExists, "Employee not found.");
            var employee = _repo.GetEmployeeById(id)!;

            Console.Write($"Delete {employee.GetFullName()} (ID: {id})? (Y/N): ");
            if (Console.ReadLine()?.Trim().ToUpper() != "Y")
            {
                ShowInfo("Deletion cancelled.");
                return;
            }

            var success = _repo.DeleteEmployee(id);
            if (success) ShowSuccess("Employee deleted!");
            else ShowError("Delete failed.");
        }
        catch (Exception ex)
        {
            ShowError($"Delete failed: {ex.Message}");
        }
    }

    private void SearchEmployee()
    {
        Console.WriteLine("\n=== SEARCH EMPLOYEE ===");
        Console.WriteLine("1. Search by ID");
        Console.WriteLine("2. Search by Name (partial match)");
        var searchType = GetValidInt("Search type (1/2): ", t => t is 1 or 2, "Enter 1 or 2.");

        try
        {
            IEnumerable<Employee> results = Enumerable.Empty<Employee>();

            switch (searchType)
            {
                case 1:
                    var id = GetValidInt("Enter ID: ", i => i > 0, "ID must be positive.");
                    var emp = _repo.GetEmployeeById(id);
                    if (emp != null) results = new List<Employee> { emp };
                    break;
                case 2:
                    var name = GetValidString("Enter Name: ", s => !string.IsNullOrWhiteSpace(s), "Name cannot be empty.");
                    results = _repo.SearchEmployeesByName(name);
                    break;
            }

            if (results.Any())
            {
                Console.WriteLine($"\nFound {results.Count()} employee(s):");
                foreach (var e in results) Console.WriteLine(e);
            }
            else
            {
                ShowInfo("No matches found.");
            }
        }
        catch (Exception ex)
        {
            ShowError($"Search failed: {ex.Message}");
        }
    }

    private void ShowAllEmployees()
    {
        Console.WriteLine("\n=== ALL EMPLOYEES ===");
        var employees = _repo.GetAllEmployees();

        if (employees.Any())
        {
            foreach (var e in employees) Console.WriteLine(e);
        }
        else
        {
            ShowInfo("No employees in the system.");
        }
    }
    #endregion

    #region Payroll Operations
    private void PayrollMenu()
    {
        Console.WriteLine("\n=== PAYROLL MENU ===");
        Console.WriteLine("1. Generate Report for Single Employee");
        Console.WriteLine("2. Generate Report for All Employees");
        var choice = GetValidInt("Choice (1/2): ", c => c is 1 or 2, "Enter 1 or 2.");

        try
        {
            switch (choice)
            {
                case 1:
                    var id = GetValidInt("Employee ID: ", _repo.EmployeeExists, "Employee not found.");
                    var emp = _repo.GetEmployeeById(id)!;
                    Console.WriteLine(_payroll.GeneratePayrollReport(emp));
                    break;
                case 2:
                    var reports = _payroll.GenerateAllEmployeesPayrollReport();
                    foreach (var report in reports) Console.WriteLine(report + "\n");
                    break;
            }
        }
        catch (Exception ex)
        {
            ShowError($"Payroll failed: {ex.Message}");
        }
    }
    #endregion

    #region Vacation Operations
    private void VacationMenu()
    {
        Console.WriteLine("\n=== VACATION MENU ===");
        Console.WriteLine("1. Add Vacation Days");
        Console.WriteLine("2. Use Vacation Days");
        Console.WriteLine("3. Single Employee Report");
        Console.WriteLine("4. All Employees Report");
        var choice = GetValidInt("Choice (1-4): ", c => c is 1 or 2 or 3 or 4, "Enter 1-4.");

        try
        {
            switch (choice)
            {
                case 1: AddVacationDays(); break;
                case 2: UseVacationDays(); break;
                case 3: SingleVacationReport(); break;
                case 4: AllVacationReports(); break;
            }
        }
        catch (Exception ex)
        {
            ShowError($"Vacation operation failed: {ex.Message}");
        }
    }

    private void AddVacationDays()
    {
        var id = GetValidInt("Employee ID: ", _repo.EmployeeExists, "Employee not found.");
        var days = GetValidInt("Days to Add (0-365): ", d => d is >=0 and <=365, "Days: 0-365.");

        var success = _vacation.AddVacationDays(id, days);
        if (success) ShowSuccess($"{days} days added!");
        else ShowError("Failed to add days.");
    }

    private void UseVacationDays()
    {
        var id = GetValidInt("Employee ID: ", _repo.EmployeeExists, "Employee not found.");
        var remaining = _vacation.GetRemainingVacationDays(id);
        Console.WriteLine($"Remaining Days: {remaining}");

        var days = GetValidInt("Days to Use: ", d => d >=0, "Days cannot be negative.");
        var success = _vacation.UseVacationDays(id, days);

        if (success) ShowSuccess($"{days} days used! Remaining: {remaining - days}");
        else ShowError($"Insufficient days. Available: {remaining}");
    }

    private void SingleVacationReport()
    {
        var id = GetValidInt("Employee ID: ", _repo.EmployeeExists, "Employee not found.");
        Console.WriteLine(_vacation.GenerateVacationReport(id));
    }

    private void AllVacationReports()
    {
        var reports = _vacation.GenerateAllEmployeesVacationReport();
        foreach (var report in reports) Console.WriteLine(report + "\n");
    }
    #endregion

    #region Input Helpers (No More Errors!)
    private string GetValidString(string prompt, Func<string, bool> validator, string error = "Invalid input.")
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim() ?? string.Empty;
            if (validator(input)) return input;
            ShowError(error);
        }
    }

    private string GetOptionalString(string prompt, Func<string, bool> validator, string defaultValue)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim() ?? string.Empty;
        return string.IsNullOrEmpty(input) ? defaultValue : validator(input) ? input : throw new ArgumentException("Invalid input.");
    }

    private int GetValidInt(string prompt, Func<int, bool> validator, string error = "Invalid number.")
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine()?.Trim(), out var value) && validator(value))
                return value;
            ShowError(error);
        }
    }

  // Replace the OLD GetValidDecimal method with this NEW one:
private decimal GetValidDecimal(string prompt, Func<decimal, bool> validator, string error = "Invalid number.", bool isCurrency = false)
{
    while (true)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim() ?? string.Empty;

        // Auto-remove $ symbol if input is currency (e.g., "$80000" → "80000")
        if (isCurrency)
        {
            input = input.Replace("$", "").Trim();
        }

        if (decimal.TryParse(input, out var value) && validator(value))
            return value;
        ShowError(error);
    }
}

    // Add this below the GetValidDecimal method (in Input Helpers):
private decimal GetOptionalDecimal(string prompt, Func<decimal, bool> validator, decimal defaultValue, bool isCurrency = false)
{
    Console.Write(prompt);
    var input = Console.ReadLine()?.Trim() ?? string.Empty;
    if (string.IsNullOrEmpty(input))
        return defaultValue;

    // Auto-remove $ symbol for currency
    if (isCurrency)
    {
        input = input.Replace("$", "").Trim();
    }

    if (decimal.TryParse(input, out var value) && validator(value))
        return value;
    
    throw new ArgumentException("❌ Invalid salary. Enter a number between 10000 and 1000000.");
}

    private DateTime GetValidDate(string prompt, string error = "Use MM/dd/yyyy (e.g., 01/15/2020).")
    {
        while (true)
        {
            Console.Write(prompt);
            if (DateTime.TryParse(Console.ReadLine()?.Trim(), out var date) && 
                date <= DateTime.Now && date >= new DateTime(1900, 1, 1))
                return date;
            ShowError(error);
        }
    }

    private DateTime GetOptionalDate(string prompt, DateTime defaultValue)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(input)) return defaultValue;
        if (DateTime.TryParse(input, out var date) && date <= DateTime.Now && date >= new DateTime(1900, 1, 1))
            return date;
        throw new ArgumentException("Invalid date. Use MM/dd/yyyy.");
    }

 private Department GetValidDepartment()
{
    Console.WriteLine("\nAvailable Departments:");
    // Rename foreach variable to "deptOption" (no conflict)
    foreach (var deptOption in Enum.GetValues<Department>()) 
        Console.WriteLine($"- {deptOption}");

    while (true)
    {
        Console.Write("Select Department: ");
        // Use unique name "selectedDept"
        if (Enum.TryParse<Department>(Console.ReadLine()?.Trim(), ignoreCase: true, out var selectedDept))
            return selectedDept;
        ShowError("Invalid department. Choose from the list.");
    }
}

  private Department GetOptionalDepartment(Department defaultValue)
{
    Console.WriteLine($"\nCurrent Department: {defaultValue}");
    Console.WriteLine("Available Departments:");
    // Rename foreach variable to "deptOption"
    foreach (var deptOption in Enum.GetValues<Department>()) 
        Console.WriteLine($"- {deptOption}");

    Console.Write("New Department (leave empty to keep): ");
    var input = Console.ReadLine()?.Trim() ?? string.Empty;
    if (string.IsNullOrEmpty(input)) 
        return defaultValue;
    
    // Use unique name "selectedDept"
    if (Enum.TryParse<Department>(input, ignoreCase: true, out var selectedDept))
        return selectedDept;
    
    throw new ArgumentException("Invalid department.");
}
    #endregion

    #region UI Helpers
    private void ShowSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✅ {message}");
        Console.ResetColor();
    }

    private void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ {message}");
        Console.ResetColor();
    }

    private void ShowInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"ℹ️ {message}");
        Console.ResetColor();
    }
    #endregion
}