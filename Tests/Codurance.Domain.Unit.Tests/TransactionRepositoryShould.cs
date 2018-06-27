using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Xunit;

namespace Codurance.Domain.Unit.Tests
{
    public class WhenWorkingWithTheTransactionRepository : IDisposable
    {
        private string date;
        private readonly IFixture fixture;
        private readonly Mock<IClock> mockClock;

        public WhenWorkingWithTheTransactionRepository()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            mockClock = fixture.Freeze<Mock<IClock>>();
            SetUpTheClockToAlwaysReturnATestableDate();
        }

        public void Dispose()
        {
            mockClock?.Reset();
        }

        private void SetUpTheClockToAlwaysReturnATestableDate()
        {
            date = "01/04/2014";
            this.mockClock
                .Setup(x => x.Now)
                .Returns(() => date);
        }

        public class AndDepositingAnAmount : WhenWorkingWithTheTransactionRepository
        {
            [Theory, AutoData]
            public void ShouldCreateATransactionWithTheCorrectInformation(int amount)
            {
                Transaction actualTransaction = null;
                ITransactionRepository transactionRepository = fixture.Create<TransactionRepository>();

                actualTransaction = transactionRepository.Deposit(amount);

                actualTransaction.Should().NotBeNull();
                actualTransaction.Amount.Should().Be(amount);
                actualTransaction.Balance.Should().Be(amount);
                actualTransaction.CreatedDate.Should().Be(date);
            }

            [Fact]
            public void ShouldKeepARunningBalance()
            {
                const int expectedBalance = 200;
                ITransactionRepository transactionRepository = fixture.Create<TransactionRepository>();

                transactionRepository.Deposit(100);
                Transaction finalTransaction = transactionRepository.Deposit(100);

                finalTransaction.Balance.Should().Be(expectedBalance);
            }
        }

        public class AndWithdrawingAnAmount : WhenWorkingWithTheTransactionRepository
        {
            [Theory, AutoData]
            public void ShouldCreateATransactionWithTheCorrectInformation(int amount)
            {
                Transaction actualTransaction = null;
                ITransactionRepository transactionRepository = fixture.Create<TransactionRepository>();

                actualTransaction = transactionRepository.Withdraw(amount);

                actualTransaction.Should().NotBeNull();
                actualTransaction.Amount.Should().Be(-amount);
                actualTransaction.Balance.Should().Be(-amount);
                actualTransaction.CreatedDate.Should().Be(date);
            }
        }

        public class AndGettingMultipleWithdrawsAndDeposits : WhenWorkingWithTheTransactionRepository
        {
            [Fact]
            public void ShouldGetTransactionsThatHaveOccured()
            {
                List<Transaction> expectedTransactions = new List<Transaction>();
                ITransactionRepository transactionRepository = fixture.Create<TransactionRepository>();
                Transaction firstDeposit = transactionRepository.Deposit(1000);
                Transaction firstWithdraw = transactionRepository.Withdraw(100);
                Transaction secondDeposit = transactionRepository.Deposit(500);

                var actualTransactions = transactionRepository.GetTransactions;

                actualTransactions.Should().NotBeNullOrEmpty();
                actualTransactions.Should().Contain(firstDeposit);
                actualTransactions.Should().Contain(firstWithdraw);
                actualTransactions.Should().Contain(secondDeposit);
            }

            [Fact]
            public void ShouldGetTransactionsThatHaveOccuredInReverse()
            {
                List<Transaction> expectedTransactions = new List<Transaction>();
                ITransactionRepository transactionRepository = fixture.Create<TransactionRepository>();
                Transaction firstTransaction = transactionRepository.Deposit(1000);
                Transaction secondTransaction = transactionRepository.Withdraw(100);
                Transaction thirdTransaction = transactionRepository.Deposit(500);

                var actualTransactions = transactionRepository.GetTransactionsInReverse;

                actualTransactions[0].Should().Be(thirdTransaction);
                actualTransactions[1].Should().Be(secondTransaction);
                actualTransactions[2].Should().Be(firstTransaction);
            }
        }
    }
}
