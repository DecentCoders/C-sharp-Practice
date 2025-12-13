# Wang & Hridoy Employee Management System  
Tried to use Opp and A bit colorful, console-based employee management system built with .NET 10, designed to meet your Instructions only

## 👥 Project Authors  
- Hridoy Hawladar (Student)  
- Wang Haochen (Instructor/Advisor)  

## 📋 Project Overview  
This console application simplifies employee record management with these key features:  
1. Add/update/delete/view employees (preloaded with default entries: Wang Haochen & Hridoy Hawladar)  
2. Payroll Management: Calculate gross/net pay, generate color-coded payroll reports  
3. Vacation Management: Add/use vacation days, generate vacation status reports  
4. Colorful UI: Green titles, dark yellow menu options, red exit buttons for improved UX  
5. Proper Input validation: ESC shortcut to cancel input, range checks for salary/vacation days  

## 📁 Project Structure
The project follows a clean, modular structure (separation of concerns) for scalability and readability:

EmployeeManagementSystem/
├── Models/
│   ├── Employee.cs                      # Domain model (encapsulates state/behaviour)
│   └── Enums/
│       └── Department.cs                #  department enum
├── Interfaces/
│   ├── IEmployeeRepository.cs           # Data access contract
│   ├── IPayrollService.cs               # Payroll  logic contract
│   └── IVacationService.cs              # Vacation  logic contract
├── Repositories/
│   └── InMemoryEmployeeRepository.cs    # In-memory data store 
├── Services/
│   ├── PayrollService.cs                # Implements payroll logic
│   └── VacationService.cs               # Implements vacation logic
├── UI/
│   └── ConsoleMenu.cs                   # Handles user input/output 
└── Program.cs                           # Entry point


## 🛠️ Tech Stack  
- .NET 10 (Console Application)  
- C# (Object-Oriented Programming, Interfaces, In-Memory Data Storage)  
- Console Customization: ANSI escape codes for colors,  

