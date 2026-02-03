
## 🌟 Key Features

### 👤 EmployeeInformation
- **Complete CRUD**: Full lifecycle management for employee records.
- **Server-Side Pagination**: Uses **DataTables.net** with backend paging for lightning-fast performance even with millions of records.
- **Robust Searching**: Employee Information search functionality across Names, Emails, Designations.
- **One-Click Export**: Export data instantly to **Excel** or **PDF**.
- **Auto-Gender Detection**: Intelligent UI that automatically detects gender based on the selected Title (Mr. → Male, Mrs. → Female).
- **Salary Highlighting**: 
    - 🟢 Rows with the **Highest Basic Salary** are highlighted in green.
    - 🔴 Rows with the **Lowest Basic Salary** are highlighted in red.
- **Net Salary Calculation**: Live calculation of `Basic Salary + Allowances - Deductions`.

### 🔐 Security & Access Control
- **Authentication**: Secure Login and Registration system using **ASP.NET Core Identity**.
- **Authorization**: Protected management pages ensuring only logged-in users can view or modify data.

## 🏗️ Technical Architecture

This project follows a **Clean Architecture** approach with strict separation of concerns:

- **Repository Pattern**: Abstraction over data access for flexibility.
- **Service Layer Pattern**: Contains the core business logic, isolated from the UI and Data layers.
- **Unit of Work**: Ensures transactional integrity across multiple repository operations.
- **Genetic Repository**: A reusable base repository for all standard entity operations.
- **OOP Principles**:
    - **Inheritance**: Models use a `BaseEntity` for automatic tracking of `CreatedOn` and `UpdatedOn` timestamps.
    - **Abstraction**: Extensive use of Interfaces to ensure the code is testable and maintainable.

## 🛠️ Technology Stack
- **Backend**: .NET 10 (ASP.NET Core MVC)
- **Database**: MSSQL (SQL Server)
- **Frontend**: jQuery, DataTables.net, Bootstrap 5, Bootstrap Icons
- **Tools**: Entity Framework Core (Code First)


## 📖 User Manual

### 1. Account Setup
- **Register**: Create a new account via the Registration page.
- **Login**: Use your credentials to access the dashboard.
- **Logout**: Securely end your session via the profile menu.

### 2. Managing Data
- **Add Record**: Click "Add New" in the Employee, Designation, or Salary pages.
- **Edit/Delete**: Use the action buttons provided in each table row. Deletions require a secure confirmation modal.
- **Clear Form**: Use the "Clear" button in entry forms to quickly reset all inputs and validation errors.

## 🚀 Getting Started
1. Update your connection string in `appsettings.json`.
2. Run dotnet ef migrations add InitialCreate
3. Run dotnet ef database update
4. Run `dotnet run` to start the application.

