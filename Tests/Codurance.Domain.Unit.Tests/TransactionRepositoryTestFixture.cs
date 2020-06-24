using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Codurance.Domain.Unit.Tests
{
    [TestFixture]
    public class WhenWorkingWithTheTransactionRepository
    {
        private string date;
        private IFixture fixture;
        private Mock<IClock> mockClock;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            mockClock = fixture.Freeze<Mock<IClock>>();
        }

        [TearDown]
        public void TearDown()
        {
            mockClock.Reset();
        }

        [SetUp]
        public void SetUpTheClockToAlwaysReturnATestableDate()
        {
            date = "01/04/2014";
            this.mockClock
                .Setup(x => x.Now)
                .Returns(() => date);
        }

        // Should be able to deposit amounts
        [TestFixture]
        public class AndDepositingAnAmount: WhenWorkingWithTheTransactionRepository
        {            
            [Test, AutoData]
            public void ShouldCreateATransactionWithTheCorrectInformation(int amount)
            {
                Transaction actualTransaction = null;
                ITransactionRepository subject = fixture.Create<TransactionRepository>();

                actualTransaction = subject.Deposit(amount);

                actualTransaction.Should().NotBeNull();
                actualTransaction.Amount.Should().Be(amount);
                actualTransaction.Balance.Should().Be(amount);
                actualTransaction.CreatedDate.Should().Be(date);
            }

            [Test]
            public void ShouldKeepARunningBalance()
            {
                const int expectedBalance = 200;
                ITransactionRepository subject = fixture.Create<TransactionRepository>();

                subject.Deposit(100);
                Transaction finalTransaction = subject.Deposit(100);

                finalTransaction.Balance.Should().Be(expectedBalance);
            }
        }

        [TestFixture]
        public class AndWithdrawingAnAmount : WhenWorkingWithTheTransactionRepository
        {
            [Test, AutoData]
            public void ShouldCreateATransactionWithTheCorrectInformation(int amount)
            {
                Transaction actualTransaction = null;
                ITransactionRepository subject = fixture.Create<TransactionRepository>();

                actualTransaction = subject.Withdraw(amount);

                actualTransaction.Should().NotBeNull();
                actualTransaction.Amount.Should().Be(-amount);
                actualTransaction.Balance.Should().Be(-amount);
                actualTransaction.CreatedDate.Should().Be(date);
            }
        }

        [TestFixture]
        public class AndGettingMultipleWithDrawsAndDeposits : WhenWorkingWithTheTransactionRepository
        {
            [Test]
            public void ShouldGetTransactionsThatHaveOccured()
            {
                List<Transaction> expectedTransactions = new List<Transaction>();
                ITransactionRepository subject = fixture.Create<TransactionRepository>();
                Transaction firstDeposit = subject.Deposit(1000);
                Transaction firstWithdraw = subject.Withdraw(100);
                Transaction secondDeposit = subject.Deposit(500);

                var actualTransactions = subject.GetTransactions;

                actualTransactions.Should().NotBeNullOrEmpty();
                actualTransactions.Should().Contain(firstDeposit);
                actualTransactions.Should().Contain(firstWithdraw);
                actualTransactions.Should().Contain(secondDeposit);
            }

            [Test]
            public void ShouldGetTransactionsThatHaveOccuredInReverse()
            {
                List<Transaction> expectedTransactions = new List<Transaction>();
                ITransactionRepository subject = fixture.Create<TransactionRepository>();
                Transaction firstTransaction = subject.Deposit(1000);
                Transaction secondTransaction = subject.Withdraw(100);
                Transaction thirdTransaction = subject.Deposit(500);

                var actualTransactions = subject.GetTransactionsInReverse;

                actualTransactions[0].Should().Be(thirdTransaction);
                actualTransactions[1].Should().Be(secondTransaction);
                actualTransactions[2].Should().Be(firstTransaction);
            }
        }
    }
}
