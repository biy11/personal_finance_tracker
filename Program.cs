using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceTracker
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsIncome { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Transaction> transactions = new List<Transaction>();
            while (true)
            {
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

                switch (choice)
                {
                    case "1":
                        AddTransaction(transactions, true);
                        break;
                    case "2":
                        AddTransaction(transactions, false);
                        break;
                    case "3":
                        ViewTransactions(transactions);
                        break;
                    case "4":
                        ViewBalance(transactions);
                        break;
                    case "5":
                        DeleteTransaction(transactions);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        static void AddTransaction(List<Transaction> transactions, bool isIncome)
        {
            Console.Write("Enter description: ");
            var description = Console.ReadLine();

            decimal amount;
            while (true){
                Console.Write("Enter amount: ");
                var input = Console.ReadLine();
                try{
                    amount = decimal.Parse(input);
                    break;
                }catch(FormatException){
                    Console.WriteLine("Ivalid amount. Please enter a valid number.");
                }   
            }
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
    }
}
