using System;
using System.Collections.Generic;
using FluentAssertions;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.NUnit3;

namespace Codurance.Domain.Integration.Tests
{
    [FeatureDescription("As a user I want an Account to make deposits, withdrawels or print statements")]
    public partial class AccountFeature
    {
        [Scenario]
        [Label("Print Account Statement")]
        public void PrintStatement() 
        {
            Runner.RunScenario(
                _ => Given_A_Client_Makes_A_Deposit(1000),
                _ => Given_A_Client_Makes_A_Withdrawal(100),
                _ => Given_A_Client_Makes_A_Deposit(500),
                _ => When_A_Client_Prints_A_Statement(),
                _ => Then_They_Should_See_Three_Transactions_In_Reverse_Order(),
                _ => Then_They_Should_See_A_Formatted_Statement());
        }
    }

    public partial class AccountFeature : FeatureFixture
    {
        private readonly Clock clock;
        private readonly TransactionRepository transactionRepository;
        private readonly Account subject;
        private readonly List<TransactionCreatedEventArgs> transactionsCreatedList = new List<TransactionCreatedEventArgs>();
        private TransactionsPrintedEventArgs transactionsPrinted;

        public AccountFeature()
        {
            clock = new Clock();
            transactionRepository = new TransactionRepository(clock);
            subject = new Account(transactionRepository, 
                OnTransactionCreated,
                OnTransactionPrinted);
        }
      
        private void Given_A_Client_Makes_A_Deposit(int amount)
        {
            subject.Deposit(amount);
        }

        private void Given_A_Client_Makes_A_Withdrawal(int amount)
        {
            subject.Withdraw(amount);
        }

        private void When_A_Client_Prints_A_Statement()
        {
            subject.PrintStatement();
        }

        private void Then_They_Should_See_Three_Transactions_In_Reverse_Order()
        {
            transactionsPrinted.Should().NotBeNull();
            transactionsPrinted.Transactions.Count.Should().Be(3);

            transactionsPrinted.Transactions[0].Amount.Should().Be(500);
            transactionsPrinted.Transactions[0].Balance.Should().Be(1400);
            transactionsPrinted.Transactions[0].CreatedDate.Should().Be(clock.Now);

            transactionsPrinted.Transactions[1].Amount.Should().Be(-100);
            transactionsPrinted.Transactions[1].Balance.Should().Be(900);
            transactionsPrinted.Transactions[1].CreatedDate.Should().Be(clock.Now);

            transactionsPrinted.Transactions[2].Amount.Should().Be(1000);
            transactionsPrinted.Transactions[2].Balance.Should().Be(1000);
            transactionsPrinted.Transactions[2].CreatedDate.Should().Be(clock.Now);
        }

        private void Then_They_Should_See_A_Formatted_Statement()
        {
            string expectedStatement =
                " DATE | AMOUNT | BALANCE " + Environment.NewLine +
                $"{clock.Now} | 500.00 | 1400.00" + Environment.NewLine +
                $"{clock.Now} | -100.00 | 900.00" + Environment.NewLine +
                $"{clock.Now} | 1000.00 | 1000.00" + Environment.NewLine;
            transactionsPrinted.Should().NotBeNull();
            transactionsPrinted.Statement.Should().Be(expectedStatement);
        }

        private void OnTransactionPrinted(TransactionsPrintedEventArgs transactionsPrinted)
        {
            this.transactionsPrinted = transactionsPrinted;
        }

        private void OnTransactionCreated(TransactionCreatedEventArgs transactionsCreated)
        {
            transactionsCreatedList.Add(transactionsCreated);
        }
    }
}
