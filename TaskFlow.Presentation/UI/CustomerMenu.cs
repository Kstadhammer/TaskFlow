using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskFlow.Core.Interfaces.Services;
using TaskFlow.Core.Models.Requests;
using TaskFlow.Presentation.UI.Helpers;

namespace TaskFlow.Presentation.UI
{
    public class CustomerMenu
    {
        private readonly ICustomerService _customerService;

        public CustomerMenu(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                var options = new Dictionary<string, string>
                {
                    { "1", "View All Customers" },
                    { "2", "Add New Customer" },
                    { "3", "Edit Customer" },
                    { "4", "View Customer Details" },
                    { "0", "Back to Main Menu" },
                };

                var choice = MenuHelper.ShowMenu("Customer Management", options);

                switch (choice)
                {
                    case "1":
                        await ViewAllCustomers();
                        break;
                    case "2":
                        await AddNewCustomer();
                        break;
                    case "3":
                        await EditCustomer();
                        break;
                    case "4":
                        await ViewCustomerDetails();
                        break;
                    case "0":
                        return;
                }
            }
        }

        private async Task ViewAllCustomers()
        {
            var result = await _customerService.GetAllAsync();
            if (result.Success)
            {
                Console.Clear();
                Console.WriteLine("=== All Customers ===\n");
                Console.WriteLine("ID\tName\tEmail");
                Console.WriteLine("----------------------------------------");
                foreach (var customer in result.Data)
                {
                    Console.WriteLine($"{customer.Id}\t{customer.Name}\t{customer.Email}");
                }
            }
            else
            {
                Console.WriteLine($"\nError: {result.Message}");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task AddNewCustomer()
        {
            Console.Clear();
            Console.WriteLine("=== Add New Customer ===\n");

            Console.Write("Customer Name: ");
            var name = Console.ReadLine()?.Trim();

            Console.Write("Email: ");
            var email = Console.ReadLine()?.Trim();

            var request = new CreateCustomerRequest { Name = name, Email = email };

            var result = await _customerService.CreateAsync(request);
            if (result.Success)
            {
                Console.WriteLine($"\nSuccess: Customer created successfully");
            }
            else
            {
                Console.WriteLine($"\nError: {result.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task EditCustomer()
        {
            Console.Clear();
            Console.WriteLine("=== Edit Customer ===\n");

            Console.Write("Enter Customer ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format");
                Console.ReadKey();
                return;
            }

            var customerResult = await _customerService.GetByIdAsync(id);
            if (!customerResult.Success || customerResult.Data is null)
            {
                Console.WriteLine($"\nError: {customerResult.Message}");
                Console.ReadKey();
                return;
            }

            var customer = customerResult.Data;
            Console.WriteLine($"\nEditing Customer: {customer.Name}");

            var name = ConsoleHelper.GetRequiredInput("New Name");
            var email = ConsoleHelper.GetRequiredInput("New Email");
            var phoneNumber = ConsoleHelper.GetRequiredInput("New Phone Number");

            var request = new UpdateCustomerRequest
            {
                Id = id,
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
            };

            var result = await _customerService.UpdateAsync(request);
            if (result.Success)
            {
                Console.WriteLine("\nSuccess: Customer updated successfully");
            }
            else
            {
                Console.WriteLine($"\nError: {result.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task ViewCustomerDetails()
        {
            Console.Clear();
            Console.WriteLine("=== Customer Details ===\n");

            Console.Write("Enter Customer ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format");
                Console.ReadKey();
                return;
            }

            var result = await _customerService.GetByIdAsync(id);
            if (!result.Success || result.Data is null)
            {
                Console.WriteLine($"\nError: {result.Message}");
                Console.ReadKey();
                return;
            }

            var customer = result.Data;
            Console.WriteLine($"\nID: {customer.Id}");
            Console.WriteLine($"Name: {customer.Name}");
            Console.WriteLine($"Email: {customer.Email}");

            if (customer.Projects?.Any() == true)
            {
                Console.WriteLine("\nProjects:");
                foreach (var project in customer.Projects)
                {
                    Console.WriteLine($"- {project.ProjectNumber}: {project.Name}");
                }
            }
            else
            {
                Console.WriteLine("\nNo associated projects");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
