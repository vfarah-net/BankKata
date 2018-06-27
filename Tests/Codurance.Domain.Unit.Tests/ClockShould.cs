
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Codurance.Domain.Unit.Tests
{
    public class WhenWorkingWithTheClock
    { 
        public class AndGettingNowAsADateFormattedString : WhenWorkingWithTheClock
        {
            [Fact]
            public void ShouldCreateValueInTheExpectedDateFormat()
            {
                Regex expectedDateFormat = new Regex(@"^(\d{1,2})\/(\d{1,2})\/(\d{4})$");
                IClock clock = new Clock();

                string actual = clock.Now;

                expectedDateFormat.IsMatch(actual).Should().BeTrue();
            }
        }
    }
}
