using System;
using System.Collections.Generic;
using System.Linq;

namespace Codurance.Domain
{
    public class Account 
    {
        private readonly List<Transaction> transactions = new List<Transaction>();
        private readonly Action<TransactionCreatedEventArgs> onTransactionCreatedEvent;
        private readonly Action<TransactionsPrintedEventArgs> onTransactionsPrintedEvent;

        public Account(
            Action<TransactionCreatedEventArgs> onTransactionCreatedEvent = null,
            Action<TransactionsPrintedEventArgs> onTransactionsPrintedEvent = null)
        {
            this.onTransactionCreatedEvent = onTransactionCreatedEvent;
            this.onTransactionsPrintedEvent = onTransactionsPrintedEvent;
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
            onTransactionCreatedEvent?.Invoke(arg);
        }

        protected void OnTransactionsPrint(TransactionsPrintedEventArgs arg)
        {
            onTransactionsPrintedEvent?.Invoke(arg);
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
