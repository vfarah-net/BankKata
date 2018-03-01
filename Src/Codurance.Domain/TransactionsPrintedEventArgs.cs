using System;
using System.Collections.Generic;
using System.Text;

namespace Codurance.Domain
{
    public class TransactionsPrintedEventArgs : EventArgs
    {
        public TransactionsPrintedEventArgs(IReadOnlyList<Transaction> transactions)
        {
            Transactions = transactions;
            GenerateStatement();
        }

        private void GenerateStatement()
        {
            StringBuilder statementBuilder = new StringBuilder();
            statementBuilder.AppendLine(" DATE | AMOUNT | BALANCE ");
            if (Transactions != null)
            {
                foreach (var transaction in Transactions)
                {
                    statementBuilder.AppendLine(
                        $"{transaction.CreatedDate} | {FormatAmount(transaction.Amount)} | {FormatAmount(transaction.Balance)}"
                    );                    
                }
            }
            Statement = statementBuilder.ToString();
        }

        public IReadOnlyList<Transaction> Transactions { get; private set; }
        public string Statement { get; private set; }

        private string FormatAmount(int amount)
        {
            return string.Format("{0:0.00}", amount);
        }
    }
}
