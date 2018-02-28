using System;
using System.Collections.Generic;
using System.Linq;

namespace Codurance.Domain
{
    public class Account 
    {
        private readonly List<Transaction> transactions = new List<Transaction>();
        private readonly Action<TransactionCreatedEventArgs> transactionCreatedEvent;
        private readonly Action<TransactionsPrintedEventArgs> transactionsPrintedEvent;

        public Account(
            Action<TransactionCreatedEventArgs> transactionCreatedEvent = null,
            Action<TransactionsPrintedEventArgs> transactionsPrintedEvent = null)
        {
            this.transactionCreatedEvent = transactionCreatedEvent;
            this.transactionsPrintedEvent = transactionsPrintedEvent;
        }

        public void Deposit(int amount)
        {
            Transaction transaction = new Transaction(
                amount,
                DateTime.Now.ToString("dd/mm/yyyy"),                
                GetCurrentBalance() + amount);
            this.AddTransaction(transaction);
        }

        public void Withdraw(int amount)
        {
            this.Deposit(-Math.Abs(amount));
        }

        public void PrintStatement()
        {
            var transactionsToPrint = transactions.Reverse<Transaction>().ToList();
            OnTransactionsPrint(new TransactionsPrintedEventArgs(transactionsToPrint));
        }

       protected void OnTransactionCreated(TransactionCreatedEventArgs arg)
        {
            transactionCreatedEvent?.Invoke(arg);
        }

        protected void OnTransactionsPrint(TransactionsPrintedEventArgs arg)
        {
            transactionsPrintedEvent?.Invoke(arg);
        }

        private int GetCurrentBalance()
        {
            return transactions.Sum(each => each.Amount);
        }

        private void AddTransaction(Transaction transaction)
        {
            transactions.Add(transaction);
            OnTransactionCreated(new TransactionCreatedEventArgs(transaction));
        }       
    }
}
