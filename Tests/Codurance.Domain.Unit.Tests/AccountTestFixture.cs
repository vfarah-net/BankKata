using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Codurance.Domain.Unit.Tests
{
    [TestFixture]
    public class WhenWorkingWithTheAccount
    {
        [TestFixture]
        public class AndDepositingAnAmountOfOneThousand : WhenWorkingWithTheAccount
        {
            [Test]
            public void ShouldCreateATransactionWithTheCorrectInformation()
            {
                Transaction expectedTransaction = null;
                Account subject = new Account((arg) => expectedTransaction = arg.Transaction);
                Regex expectedDateFormat = new Regex(@"^(\d{1,2})\/(\d{1,2})\/(\d{4})$");

                subject.Deposit(1000);

                Assert.That(expectedTransaction, Is.Not.Null);
                Assert.That(expectedDateFormat.IsMatch(expectedTransaction.CreatedDate), Is.True);
                Assert.That(expectedTransaction.Amount, Is.EqualTo(1000));
                Assert.That(expectedTransaction.Balance, Is.EqualTo(1000));
            }

            [Test]
            public void ShouldDepositAnAmountOfOneThousandAndHaveABalanceOfAThousand()
            {
                Transaction expectedTransaction = null;
                Account subject = new Account((arg) => expectedTransaction = arg.Transaction);

                subject.Deposit(1000);

                Assert.That(expectedTransaction.Amount, Is.EqualTo(1000));
                Assert.That(expectedTransaction.Balance, Is.EqualTo(1000));
            }
        }

        [TestFixture]
        public class AndWithdrawingAnAmountOfOneHundred : WhenWorkingWithTheAccount
        {
            [Test]
            public void ShouldWithdrawMinusOneHundredAndLeaveABalanceOfNineHundred()
            {
                IList<Transaction> expectedTransactions = new List<Transaction>();
                Account subject = new Account((arg) => expectedTransactions.Add(arg.Transaction));

                subject.Deposit(1000);
                subject.Withdraw(-100);

                Assert.That(expectedTransactions.Count, Is.EqualTo(2));
                Assert.That(expectedTransactions.Last().Balance, Is.EqualTo(900));
            }

            [Test]
            public void ShouldWithdrawPositiveOneHundredAndLeaveABalanceOfNineHundred()
            {
                IList<Transaction> expectedTransactions = new List<Transaction>();
                Account subject = new Account((arg) => expectedTransactions.Add(arg.Transaction));

                subject.Deposit(1000);
                subject.Withdraw(100);

                Assert.That(expectedTransactions.Count, Is.EqualTo(2));
                Assert.That(expectedTransactions.Last().Balance, Is.EqualTo(900));
            }
        }

        [TestFixture]
        public class AndFinallyDepositingFiveHundred : WhenWorkingWithTheAccount
        {
            [Test]
            public void ShouldDepositFiveHundredAndLeaveABalanceOfOneThousandFourHundred()
            {
                IList<Transaction> expectedTransactions = new List<Transaction>();
                Account subject = new Account((arg) => expectedTransactions.Add(arg.Transaction));

                subject.Deposit(1000);
                subject.Withdraw(100);
                subject.Deposit(500);

                Assert.That(expectedTransactions.Count, Is.EqualTo(3));
                Assert.That(expectedTransactions.Last().Balance, Is.EqualTo(1400));
            }
        }

        [TestFixture]
        public class AndPrintingAllTransactions : WhenWorkingWithTheAccount
        {
            [Test]
            public void ShouldPrintAllTransactionsInDescendingOrder()
            {
                List<Transaction> expectedTransactions = new List<Transaction>();
                Account subject = new Account(null, (arg) => expectedTransactions.AddRange(arg.Transactions));
                subject.Deposit(1000);
                subject.Withdraw(100);
                subject.Deposit(500);

                subject.PrintStatement();

                CollectionAssert.IsNotEmpty(expectedTransactions);
                Assert.That(expectedTransactions.Count, Is.EqualTo(3));
                Assert.That(expectedTransactions[0].Amount, Is.EqualTo(500));
                Assert.That(expectedTransactions[0].Balance, Is.EqualTo(1400));

                Assert.That(expectedTransactions[1].Amount, Is.EqualTo(-100));
                Assert.That(expectedTransactions[1].Balance, Is.EqualTo(900));

                Assert.That(expectedTransactions[2].Amount, Is.EqualTo(1000));
                Assert.That(expectedTransactions[2].Balance, Is.EqualTo(1000));
            }

            [Test]
            public void ShouldPrintAStatementWithHeadersAndExpectedNumberFormatting()
            {
                string expectedHeader = " DATE | AMOUNT | BALANCE ";
                string expectedFiveHundredDepositNumberFormat = "500.00";
                string actualStatement = null;
                Account subject = new Account(null, (arg) =>
                {
                    actualStatement = arg.Statement;
                });
                subject.Deposit(1000);
                subject.Withdraw(100);
                subject.Deposit(500);

                subject.PrintStatement();

                Assert.That(actualStatement, Is.Not.Empty);
                Assert.That(actualStatement.Contains(expectedHeader), Is.True);
                Assert.That(actualStatement.Contains(expectedFiveHundredDepositNumberFormat), Is.True);
            }
        }
    }
}

