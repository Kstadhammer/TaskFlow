# TaskFlow

TaskFlow is a console-based project management system built with .NET 9.0. It helps organizations manage their projects, customers, project managers, and services efficiently.

## Features

- Project Management
  - Create and manage projects with unique project numbers
  - Track project status, timelines, and budgets
  - Associate projects with customers, project managers, and services

- Customer Management
  - Maintain customer records
  - Track customer projects
  - Manage customer contact information

- Project Manager Management
  - Manage project manager profiles
  - Track assigned projects
  - Organize by departments

- Service Management
  - Define available services
  - Set hourly rates
  - Track service usage in projects

- Status Management
  - Create custom project statuses
  - Track project progress
  - Flexible status definitions

## Prerequisites

- .NET 9.0 SDK
- SQLite (included in the project)

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/TaskFlow.git
   cd TaskFlow
   ```

2. Build the project:
   ```bash
   dotnet build
   ```

3. Run the application:
   ```bash
   cd TaskFlow.Presentation
   dotnet run
   ```

## Project Structure

- **TaskFlow.Core**: Contains entities, interfaces, and models
- **TaskFlow.Infrastructure**: Implements data access and services
- **TaskFlow.Presentation**: Console application UI

## Navigation

- Use arrow keys (↑↓) to navigate menus
- Press Enter to select an option
- Follow on-screen prompts for data input

## Database

The application uses SQLite for data storage. The database file (`taskflow.db`) is automatically created in the application directory when you first run the application.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details. 