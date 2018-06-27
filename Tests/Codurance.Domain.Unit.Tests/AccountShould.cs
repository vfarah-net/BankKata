using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Xunit;

namespace Codurance.Domain.Unit.Tests
{
    public class WhenWorkingWithTheAccount : IDisposable
    {
        private readonly IFixture fixture;
        private readonly Mock<ITransactionRepository> mockTransactionRepository;

        public WhenWorkingWithTheAccount()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            mockTransactionRepository = fixture.Freeze<Mock<ITransactionRepository>>();
        }

        private Transaction CreateTransaction(
            int? amount = null,
            string createdDate = null,
            int? balance = null)
        {
            return new Transaction(
                amount ?? fixture.Create<int>(),
                createdDate ?? fixture.Create<string>(),
                balance ?? fixture.Create<int>());
        }

        public void Dispose()
        {
            mockTransactionRepository?.Reset();
        }

        public class AndDepositingAnyAmount : WhenWorkingWithTheAccount
        {
            private Transaction transaction;

            public AndDepositingAnyAmount()
            {
                SetUpTransactionRepositoryToReturnATransaction();
            }


            private void SetUpTransactionRepositoryToReturnATransaction()
            {
                transaction = CreateTransaction();
                mockTransactionRepository
                    .Setup(x => x.Deposit(It.IsAny<int>()))
                    .Returns(() => transaction);
            }


            [Theory, AutoData]
            public void ShouldUseTheTransactionRepositoryToDepositAnAmount(int amount)
            {
                Account account = new Account(mockTransactionRepository.Object);

                account.Deposit(amount);

                mockTransactionRepository.Verify(x => x.Deposit(It.IsIn(amount)), Times.Once);
            }
        }

        public class AndWithdrawingAnyAmount : WhenWorkingWithTheAccount
        {
            private Transaction transaction;

            public AndWithdrawingAnyAmount()
            {
                SetUpTransactionRepositoryToReturnATransaction();
            }

            private void SetUpTransactionRepositoryToReturnATransaction()
            {
                transaction = CreateTransaction();
                mockTransactionRepository
                    .Setup(x => x.Withdraw(It.IsAny<int>()))
                    .Returns(() => transaction);
            }

            [Theory, AutoData]
            public void ShouldUseTheTransactionRepositoryToWithdrawAnyAmount(int amount)
            {
                Account account = new Account(mockTransactionRepository.Object);

                account.Withdraw(amount);

                mockTransactionRepository.Verify(x => x.Withdraw(It.IsIn(amount)), Times.Once);
            }
        }

        public class AndPrintingStatements : WhenWorkingWithTheAccount
        {
            private List<Transaction> transactions;

            public AndPrintingStatements()
            {
                SetUpTransactionRepositoryToReturnExpectedTransactions();
            }

            private void SetUpTransactionRepositoryToReturnExpectedTransactions()
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

            [Fact]
            public void ShouldUseTheTransactionRepositoryToGetTransactionsInReverse()
            {
                Account account = new Account(mockTransactionRepository.Object);

                account.PrintStatement();

                mockTransactionRepository.Verify(x => x.GetTransactionsInReverse, Times.Once);
            }

            [Fact]
            public void ShouldPrintTheTransactionsAccordingToTheAcceptanceCriteria()
            {
                string acceptanceCriteria =
                    " DATE | AMOUNT | BALANCE " + Environment.NewLine +
                    "10/04/2014 | 500.00 | 1400.00" + Environment.NewLine +
                    "02/04/2014 | -100.00 | 900.00" + Environment.NewLine +
                    "01/04/2014 | 1000.00 | 1000.00" + Environment.NewLine;
                string actualStatement = null;
                Account account = new Account(mockTransactionRepository.Object, null, (args) => {
                    actualStatement = args.Statement;
                });

                account.PrintStatement();

                actualStatement.Should().Be(acceptanceCriteria);
            }
        }
    }
}