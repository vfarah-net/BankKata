using System;

namespace Codurance.Domain
{
    public class Account : IAccount
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly Action<TransactionCreatedEventArgs> transactionCreatedEvent;
        private readonly Action<TransactionsPrintedEventArgs> transactionsPrintedEvent;

        public Account(
            ITransactionRepository transactionRepository,
            Action<TransactionCreatedEventArgs> transactionCreatedEvent = null,
            Action<TransactionsPrintedEventArgs> transactionsPrintedEvent = null)
        {
            this.transactionRepository = transactionRepository;
            this.transactionCreatedEvent = transactionCreatedEvent;
            this.transactionsPrintedEvent = transactionsPrintedEvent;
        }

        public void Deposit(int amount)
        {
            Transaction transaction = transactionRepository.Deposit(amount);
            this.AddTransaction(transaction);
        }

        public void Withdraw(int amount)
        {
            Transaction transaction = transactionRepository.Withdraw(amount);
            this.AddTransaction(transaction);
        }

        public void PrintStatement()
        {
            var transactionsToPrint = transactionRepository.GetTransactionsInReverse;
            OnTransactionsPrint(new TransactionsPrintedEventArgs(transactionsToPrint));
        }

       private void OnTransactionCreated(TransactionCreatedEventArgs arg)
        {
            transactionCreatedEvent?.Invoke(arg);
        }

        private void OnTransactionsPrint(TransactionsPrintedEventArgs arg)
        {
            transactionsPrintedEvent?.Invoke(arg);
        }

        private void AddTransaction(Transaction transaction)
        {
            OnTransactionCreated(new TransactionCreatedEventArgs(transaction));
        }       
    }
}
