using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Codurance.Domain.Unit.Tests
{
    [TestFixture]
    public class WhenWorkingWithTheAccount
    {
        private IFixture fixture;
        private Mock<ITransactionRepository> mockTransactionRepository;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            mockTransactionRepository = fixture.Freeze<Mock<ITransactionRepository>>();
        }

        [TearDown]
        public void TearDown()
        {
            mockTransactionRepository.Reset();
        }

        private Transaction CreateTransaction(
            int? amount = null, 
            string createdDate = null, 
            int? balance = null)
        {
            return new Transaction(
                    amount??fixture.Create<int>(),
                    createdDate??fixture.Create<string>(),
                    balance??fixture.Create<int>());
        }

        [TestFixture]
        public class AndDepositingAnyAmount : WhenWorkingWithTheAccount
        {
            private Transaction transaction;

            [SetUp]
            public void SetUpTransactionRepositoryToReturnATransaction()
            {
                transaction = CreateTransaction();
                mockTransactionRepository
                    .Setup(x => x.Deposit(It.IsAny<int>()))
                    .Returns(() => transaction);
            }

            [Test, AutoData]
            public void ShouldUseTheTransactionRepositoryToDepositAnAmount(int amount)
            {
                Account subject = new Account(mockTransactionRepository.Object);

                subject.Deposit(amount);

                mockTransactionRepository.Verify(x => x.Deposit(It.IsIn(amount)), Times.Once);
            }
        }

        [TestFixture]
        public class AndWithdrawingAnyAmount : WhenWorkingWithTheAccount
        {
            private Transaction transaction;

            [SetUp]
            public void SetUpTransactionRepositoryToReturnATransaction()
            {
                transaction = CreateTransaction();
                mockTransactionRepository
                    .Setup(x => x.Withdraw(It.IsAny<int>()))
                    .Returns(() => transaction);
            }

            [Test, AutoData]
            public void ShouldUseTheTransactionRepositoryToWithdrawAnyAmount(int amount)
            {
                Account subject = new Account(mockTransactionRepository.Object);

                subject.Withdraw(amount);

                mockTransactionRepository.Verify(x => x.Withdraw(It.IsIn(amount)), Times.Once);
            }
        }

        [TestFixture]
        public class AndPrintingStatements : WhenWorkingWithTheAccount
        {
            private List<Transaction> transactions;

            [SetUp]
            public void SetUpTransactionRepositoryToReturnExpectedTransactions()
            {
                // Expected transactions
                //> 10/04/2014 | 500.00  | 1400.00
                //> 02/04/2014 | -100.00 | 900.00
                //> 01/04/2014 | 1000.00 | 1000.00
                transactions = new List<Transaction>
                {
                    CreateTransaction(1000, "01/04/2014", 1000),
                    CreateTransaction(-100, "02/04/2014", 900),
                    CreateTransaction(500, "10/04/2014", 1400)
                };

                mockTransactionRepository
                    .Setup(x => x.GetTransactions)
                    .Returns(() => transactions);

                mockTransactionRepository
                    .Setup(x => x.GetTransactionsInReverse)
                    .Returns(() => transactions?.Reverse<Transaction>().ToList());
            }

            [Test]
            public void ShouldUseTheTransactionRepositoryToGetTransactionsInReverse()
            {
                Account subject = new Account(mockTransactionRepository.Object);

                subject.PrintStatement();

                mockTransactionRepository.Verify(x => x.GetTransactionsInReverse, Times.Once);
            }

            [Test]
            public void ShouldPrintTheTransactionsAccordingToTheAcceptanceCriteria()
            {
                string acceptanceCriteria = 
                    " DATE | AMOUNT | BALANCE " + Environment.NewLine +
                    "10/04/2014 | 500.00 | 1400.00" + Environment.NewLine +
                    "02/04/2014 | -100.00 | 900.00" + Environment.NewLine +
                    "01/04/2014 | 1000.00 | 1000.00" + Environment.NewLine;
                string actualStatement = null;
                Account subject = new Account(mockTransactionRepository.Object, null, (args) => {
                    actualStatement = args.Statement;
                });

                subject.PrintStatement();

                actualStatement.Should().Be(acceptanceCriteria);
            }
        }
    }
}

