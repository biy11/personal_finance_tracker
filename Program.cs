/*
 * @(#) program.cs 0.3 2024/06/12.
 * 
 * This is C# Consol Application for a personal finanace tracker.
 *
*/  

/* 
 * @aouthor Bilal [biy1]
 * @version 0.1 - Initial development.
 * @version 0.2 - Added error exeptions for invalid transaction amounts.
 * @version 0.3 - Added comments.
*/ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace PersonalFinanceTracker
{
    // Class to represent a financial transaction. 
    public class Transaction
    {
        public int Id { get; set; } // Unique identifier for the transaction.
        public string Description { get; set; } = string.Empty; // Description of the transaction. Set to empty.
        public decimal Amount { get; set; } // Amount of money involved in the trasaction.
        public DateTime Date { get; set; } // Date and time of the transaction.
        public bool IsIncome { get; set; } // Flag to deteremine weather amount is income or expense.
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Transaction> transactions = LoadTransactions(); // List to store the laoded the transactions.

            // Main loop to display and process user input.
            while (true)
            {
                // Menue for the finance tracker.
                Console.Clear();
                Console.WriteLine("Personal Finance Tracker");
                Console.WriteLine("1. Add Income");
                Console.WriteLine("2. Add Expense");
                Console.WriteLine("3. View Transactions");
                Console.WriteLine("4. View Balance");
                Console.WriteLine("5. Delete Transaction");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                // Switch cases for handleing user choice.
                switch (choice)
                {
                    case "1":
                        AddTransaction(transactions, true); // Add income to transaction.
                        break;
                    case "2":
                        AddTransaction(transactions, false); // Add expense transaction.
                        break;
                    case "3":
                        ViewTransactions(transactions); // View all transactions.
                        break;
                    case "4":
                        ViewBalance(transactions); // View current balance.
                        break;
                    case "5":
                        DeleteTransaction(transactions); // Delete a specific transaction.
                        break;
                    case "0": // Exit the application.
                        SaveTransactions(transactions);
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again."); // Handle invalid input.
                        break;
                }
            }
        }

        // Method to add a transaction (income or expense).
        static void AddTransaction(List<Transaction> transactions, bool isIncome)
        {
            Console.Write("Enter description: ");
            var description = Console.ReadLine() ?? string.Empty;
            decimal amount;
            // Loop to ensure a valid amount is entered.
            while (true)
            {
                Console.Write("Enter amount: ");
                var input = Console.ReadLine();
                try
                {
                    amount = decimal.Parse(input ?? "0");
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid amount. Please enter a valid number.");
                }
            }
            // Create a new transaction and add it to the list.
            var transaction = new Transaction
            {
                Id = transactions.Count + 1,
                Description = description,
                Amount = amount,
                Date = DateTime.Now,
                IsIncome = isIncome
            };
            transactions.Add(transaction);
            Console.WriteLine("Transaction added successfully!");
            Console.ReadKey();
        }

        // Method to view the current balance.
        static void ViewTransactions(List<Transaction> transactions)
        {
            Console.Clear();
            Console.WriteLine("Transactions:");
            foreach (var transaction in transactions)
            {
                Console.WriteLine($"{transaction.Id}. {transaction.Description} - {(transaction.IsIncome ? "Income" : "Expense")}: ${transaction.Amount} on {transaction.Date}");
            }
            Console.ReadKey();
        }

        // Method to view the current balance.
        static void ViewBalance(List<Transaction> transactions)
        {
            var income = transactions.Where(t => t.IsIncome).Sum(t => t.Amount);
            var expenses = transactions.Where(t => !t.IsIncome).Sum(t => t.Amount);
            var balance = income - expenses;
            Console.WriteLine($"Total Income: ${income}");
            Console.WriteLine($"Total Expenses: ${expenses}");
            Console.WriteLine($"Balance: ${balance}");
            Console.ReadKey();
        }

        // Method to delete a transaction.
        static void DeleteTransaction(List<Transaction> transactions)
        {
            Console.Clear();
            Console.WriteLine("Transactions:");
            if( transactions.Count == 0){
                Console.WriteLine("No existing transactions");
                Console.ReadKey();
                return;
            }else{
                foreach (var transaction in transactions)
                {
                    Console.WriteLine($"{transaction.Id}. {transaction.Description} - {(transaction.IsIncome ? "Income" : "Expense")}: ${transaction.Amount} on {transaction.Date}");
                }
            }

            int id;
            // Loop to esnure a valid transaction ID is entered.
            while (true)
            {
                Console.WriteLine("Enter the transaction ID to be deleted");
                var input = Console.ReadLine();
                if (input != null && int.TryParse(input, out id))
                {
                    break;
                }
                Console.WriteLine("Invalid ID! Please enter a valid ID number.");
            }
            // Find and remove the transaction with the specified ID.
            var foundTransaction = transactions.FirstOrDefault(t => t.Id == id);
            if (foundTransaction != null)
            {
                transactions.Remove(foundTransaction);
                Console.WriteLine("Transaction deleted successfully!");
            }
            else
            {
                Console.WriteLine("Transaction not found.");
            }
            Console.ReadKey();
        }

        static void SaveTransactions(List<Transaction> transactions){
            var json = JsonConvert.SerializeObject(transactions, Formatting.Indented);
            File.WriteAllText("transaction.json", json);
        }

        static List<Transaction> LoadTransactions(){
            if(File.Exists("transaction.json")){
                var json = File.ReadAllText("transaction.json");
                return JsonConvert.DeserializeObject<List<Transaction>>(json);
            }
            return new List<Transaction>();
        }
    }
}
