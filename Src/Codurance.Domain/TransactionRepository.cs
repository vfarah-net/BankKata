using System;
using System.Collections.Generic;
using System.Linq;

namespace Codurance.Domain
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IClock clock;
        private readonly List<Transaction> transactions = new List<Transaction>();

        public TransactionRepository(IClock clock)
        {
            this.clock = clock;
        }

        public Transaction Deposit(int amount)
        {
            return AddTransaction(amount);
        }

        public Transaction Withdraw(int amount)
        {
            return AddTransaction(-Math.Abs(amount));
        }

        public IReadOnlyList<Transaction> GetTransactions
        {
            get { return transactions; }
        }

        public IReadOnlyList<Transaction> GetTransactionsInReverse
        {
            get { return transactions.Reverse<Transaction>().ToList(); }
        }

        private Transaction AddTransaction(int amount)
        {
            Transaction transaction = new Transaction(
                amount,
                clock.Now,
                GetCurrentBalance() + amount);
            transactions.Add(transaction);
            return transaction;
        }

        private int GetCurrentBalance()
        {
            return transactions.Sum(each => each.Amount);
        }
    }
}
