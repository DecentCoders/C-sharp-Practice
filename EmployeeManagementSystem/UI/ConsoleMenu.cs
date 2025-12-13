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
        Console.Title = "WANG & HRIDOY TECH - EMPLOYEE MANAGEMENT ";
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
    // Title (Magenta)
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("==================== WANG & HRIDOY TECH - EMPLOYEE MANAGEMENT  ====================");
    Console.ResetColor();

    // Menu options (Numbers = DarkYellow, Text = Blue)
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("1. ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Add New Employee");
    
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("2. ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Update Employee");
    
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("3. ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Delete Employee");
    
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("4. ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Search Employee (ID/Name)");
    
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("5. ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Show All Employees");
    
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("6. ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Payroll Menu");
    
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("7. ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Vacation Menu");
    
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("8. ");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Exit");

    // Footer (Magenta)
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("====================================================================");
    Console.ResetColor();

    // Prompt (White with Blue highlight)
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("Enter your choice: ");
    Console.ResetColor();
}

    #region Employee Core Operations
 private void AddEmployee()
{
    // Colorful title (Magenta) - Fixed: Added line break for spacing
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine("\n=== ADD NEW EMPLOYEE ===");
    Console.ResetColor();

    try
    {
        var id = _repo.GetNextAvailableId();

        // First Name (LightGray prompt + fixed validation for empty/only spaces)
        Console.ForegroundColor = ConsoleColor.Gray;
        var firstName = GetValidString(
            prompt: "First Name (2-50 chars): ", 
            validator: s => s.Length is >= 2 and <= 50
        );
        Console.ResetColor(); // Ensure color reset immediately after input

        // Last Name (LightGray prompt + same validation fix)
        Console.ForegroundColor = ConsoleColor.Gray;
        var lastName = GetValidString(
            prompt: "Last Name (2-50 chars): ", 
            validator: s => s.Length is >= 2 and <= 50
        );
        Console.ResetColor();

        // === Colorful Number-Based Department Selection (0 to Exit) ===
        Console.WriteLine("\nAvailable Departments:");
        var departmentOptions = Enum.GetValues<Department>().ToList();
        
        // Fixed: Ensure loop uses valid enum values (no nulls)
        for (int i = 0; i < departmentOptions.Count; i++)
        {
            Console.ForegroundColor = ConsoleColor.Yellow; // Number color
            Console.Write($"{i + 1}. ");
            Console.ForegroundColor = ConsoleColor.Green; // Department name color
            Console.WriteLine(departmentOptions[i]);
            Console.ResetColor(); // Reset after each line to avoid leaks
        }

        // Exit option (Red number + White text) - Fixed: Better spacing
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("0. ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Exit (cancel adding employee)");
        Console.ResetColor();

        int selectedDeptNumber;
        while (true)
        {
            // Colored prompt (Blue) - Fixed: No extra spaces
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Enter the number for your choice: ");
            Console.ResetColor();

            // Fixed: Handle null input safely (trim + null coalescing)
            var input = Console.ReadLine()?.Trim() ?? string.Empty;
            if (int.TryParse(input, out selectedDeptNumber))
            {
                // Option 0: Cancel and exit
                if (selectedDeptNumber == 0)
                {
                    ShowInfo("Exiting employee addition. No employee was added.");
                    return; // Stop the add process (safe exit)
                }

                // Valid number (1 to total departments)
                if (selectedDeptNumber >= 1 && selectedDeptNumber <= departmentOptions.Count)
                {
                    break; // Exit loop - valid selection
                }
            }

            // Fixed: Clear error message (no duplicate ❌)
            ShowError($" Invalid input. Enter a number between 0 and {departmentOptions.Count}");
        }

        // Map number to department (fixed: index safety)
        var dept = departmentOptions[selectedDeptNumber - 1];

            // Salary (LightGray prompt + fixed validation message)
            Console.ForegroundColor = ConsoleColor.Gray;
       
        
        var salary = GetValidDecimal(
            prompt: "Annual Salary (10000-1000000, no $ or 'k'): ",
            validator: d => d is >= 10000 and <= 1000000,
            error: "Salary must be between $10,000 and $1,000,000 (enter full number, e.g., 80000)"
        );
        Console.ResetColor();

        // Hire Date (LightGray prompt + 2020 rule + clean error)
        Console.ForegroundColor = ConsoleColor.Gray;
        var hireDate = GetValidDate(
            prompt: "Hire Date (MM/dd/yyyy) : ",
            error: "Invalid date format. Use MM/dd/yyyy (e.g., 10/20/2020)."
        );
        Console.ResetColor();

        // Vacation Days (LightGray prompt + fixed validation)
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        var vacationDays = GetValidInt(
            prompt: "Initial Vacation Days (0-365): ",
            validator: i => i is >= 0 and <= 365
        );
        Console.ResetColor();

        // Fixed: Ensure employee creation uses valid values
        var employee = new Employee(
            id: id,
            firstName: firstName,
            lastName: lastName,
            department: dept,
            salary: salary,
            hireDate: hireDate,
            initialVacationDays: vacationDays
        );

        _repo.AddEmployee(employee);
        ShowSuccess($"Employee added! ID: {id} | Name: {employee.GetFullName()}");
    }
    catch (ArgumentNullException ex)
    {
        // Specific error for null inputs (safe handling)
        ShowError($"Add failed: Missing required input - {ex.Message}");
    }
    catch (ArgumentException ex)
    {
        // Specific error for invalid values (e.g., salary too low)
        ShowError($"Add failed: Invalid input - {ex.Message}");
    }
    catch (Exception ex)
    {
        // General error (catch-all for safety)
        ShowError($"Add failed: Unexpected error - {ex.Message}");
    }
}    private void UpdateEmployee()
    {
                Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n=== UPDATE EMPLOYEE ===");
                Console.ResetColor();

        try
        {
            var id = GetValidInt("Enter Employee ID: ", _repo.EmployeeExists, "Employee not found.");
            var existing = _repo.GetEmployeeById(id)!;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Current Details:\n{existing}\n");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("Leave field empty to keep current value.");
            Console.ResetColor();

            var firstName = GetOptionalString("New First Name: ", s => s.Length is >=2 and <=50, existing.FirstName);
            var lastName = GetOptionalString("New Last Name: ", s => s.Length is >=2 and <=50, existing.LastName);
            var dept = GetOptionalDepartment(existing.Department);
   
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
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("\n=== DELETE EMPLOYEE ===");
        Console.ResetColor();
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
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("\n=== SEARCH EMPLOYEE ===");
        Console.ResetColor();
        Console.WriteLine("1. Search by ID");
        Console.WriteLine("2. Search by Name (partial match)");
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        var searchType = GetValidInt("Search type (1/2): ", t => t is 1 or 2, "Enter 1 or 2.");
        Console.ResetColor();

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
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("\n=== ALL EMPLOYEES ===");
        Console.ResetColor();
        var employees = _repo.GetAllEmployees();

        if (employees.Any())
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            foreach (var e in employees) Console.WriteLine(e);
        Console.ResetColor();
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
      Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\n=== PAYROLL MENU ===");
    Console.ResetColor();

    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("1. ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Generate Report for Single Employee");
    
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("2. ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Generate Report for All Employees");
    Console.ResetColor();

    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write("Choice (1/2): ");
    Console.ResetColor();

    var choice = GetValidInt("", c => c is 1 or 2, "Enter 1 or 2.");

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
       Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine("\n=== VACATION MENU ===");
    Console.ResetColor();

    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("1. ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Add Vacation Days");
    
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("2. ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Use Vacation Days");
    
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("3. ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Single Employee Report");
    
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("4. ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("All Employees Report");
    Console.ResetColor();

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write("Choice (1-4): ");
    Console.ResetColor();

    var choice = GetValidInt("", c => c is 1 or 2 or 3 or 4, "Enter 1-4.");

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

        // NEW Check 1: Block empty/only-space names
        if (string.IsNullOrEmpty(input))
        {
            ShowError("Name cannot be empty or consist of only spaces.");
            continue;
        }

        // Existing Check 2: Block names with only numbers
        if (input.All(char.IsDigit))
        {
            ShowWarning("Warning: Name cannot contain only numbers! Please include letters (e.g., Hridoy123 is allowed).");
            continue;
        }

        // Existing Check 3: Length/other validation
        if (validator(input))
            return input;
        
        ShowError(error);
    }
}

    private string GetOptionalString(string prompt, Func<string, bool> validator, string defaultValue)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim() ?? string.Empty;
        return string.IsNullOrEmpty(input) ? defaultValue : validator(input) ? input : throw new ArgumentException(" Invalid input.");
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

  private DateTime GetValidDate(string prompt, string error = "Use MM/dd/yyyy (e.g., 10/20/2020). Hire date cannot be before 2020 or in the future.")
{
    // Company establishment year: 2020
    var companyStartDate = new DateTime(2020, 1, 1);
    
    while (true)
    {
        Console.Write(prompt);
        if (DateTime.TryParse(Console.ReadLine()?.Trim(), out var date))
        {
            // Validate date rules
            if (date > DateTime.Now)
            {
                ShowError(" Hire date cannot be in the future.");
            }
            else if (date < companyStartDate)
            {
                ShowError(" our company was established in 2020. Hire date cannot be before 2020.");
            }
            else
            {
                return date; // Valid date
            }
        }
        else
        {
            ShowError(error); // Invalid date format
        }
    }
}

private DateTime GetOptionalDate(string prompt, DateTime defaultValue)
{
    var companyStartDate = new DateTime(2020, 1, 1);
    Console.Write(prompt);
    var input = Console.ReadLine()?.Trim() ?? string.Empty;
    
    if (string.IsNullOrEmpty(input))
        return defaultValue;
    
    if (DateTime.TryParse(input, out var date))
    {
        if (date > DateTime.Now)
            throw new ArgumentException("Hire date cannot be in the future.");
        if (date < companyStartDate)
            throw new ArgumentException("our company was established in 2020. Hire date cannot be before 2020.");
        return date;
    }
    
    throw new ArgumentException("Invalid date. Use MM/dd/yyyy (e.g., 10/20/2020).");
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
    Console.WriteLine($"❌{message}");
    Console.ResetColor();
}

private void ShowInfo(string message)
{
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine($"ℹ️ {message}");
    Console.ResetColor();
}

// NEW: Yellow warning for non-critical issues (e.g., all-numeric names)
private void ShowWarning(string message)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"⚠️ {message}");
    Console.ResetColor();
}
#endregion
}