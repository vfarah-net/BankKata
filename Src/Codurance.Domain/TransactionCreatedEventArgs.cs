using System;

namespace Codurance.Domain
{
    public class TransactionCreatedEventArgs: EventArgs
    {
        public TransactionCreatedEventArgs(Transaction transaction)
        {
            this.Transaction = transaction;
        }
        public Transaction Transaction{ get; private set; }
    }
}
