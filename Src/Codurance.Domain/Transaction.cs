using System;

namespace Codurance.Domain
{
    public class Transaction
    {
        public Transaction(int amount, string createdDate, int balance)
        {
            Amount = amount;            
            Balance = balance;
            CreatedDate = createdDate;
        }

        public int Amount { get; }        
        public int Balance { get; }
        public string CreatedDate { get; }
    }
}
