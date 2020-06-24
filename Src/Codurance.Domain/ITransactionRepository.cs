using System.Collections.Generic;

namespace Codurance.Domain
{
    public interface ITransactionRepository
    {
        IReadOnlyList<Transaction> GetTransactions { get; }
        IReadOnlyList<Transaction> GetTransactionsInReverse { get; }

        Transaction Deposit(int amount);
        Transaction Withdraw(int amount);
    }
}