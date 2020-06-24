using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using NUnit.Framework;

namespace Codurance.Domain.Unit.Tests
{
    [TestFixture]
    public class WhenWorkingWithTheClock
    {
        [TestFixture]
        public class AndGettingNowAsADateFormattedString : WhenWorkingWithTheClock
        {
            [Test]
            public void ShouldCreateValueInTheExpectedDateFormat()
            {
                Regex expectedDateFormat = new Regex(@"^(\d{1,2})\/(\d{1,2})\/(\d{4})$");
                IClock subject = new Clock();

                string actual = subject.Now;

                expectedDateFormat.IsMatch(actual).Should().BeTrue();
            }        
        }
    }
}
