using System;

public class BankAccount
{
    private string accountHolder;
    private decimal balance; // Private field for data hiding

    // Constructor
    public BankAccount(string accountHolder, decimal initialBalance)
    {
        this.accountHolder = accountHolder;
        this.balance = initialBalance;
    }

    // Method to deposit money with validation
    public void Deposit(decimal amount)
    {
        if (amount > 0)
        {
            balance += amount;
        }
        else
        {
            Console.WriteLine("Deposit amount must be positive.");
        }
    }

    // Method to retrieve the balance (controlled read-only access)
    public decimal GetBalance()
    {
        return balance;
    }
}