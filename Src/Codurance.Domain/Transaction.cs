using System;

namespace Codurance.Domain
{
    public class Transaction
    {
        public Transaction(int amount, string createdDate, int balance)
        {
            Amount = amount;
            CreatedDate = createdDate;
            Balance = balance;
        }

        public int Amount { get; }
        public string CreatedDate { get; }
        public int Balance { get; }
    }
}
