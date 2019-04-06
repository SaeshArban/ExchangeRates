using API.Entities;
using API.Entities.Comparers;
using FluentAssertions;
using System;
using Xunit;

namespace Tests.Entities.Comparers
{
    // Пример теста красный-зелёный
    public class ExchangeRateComparerTests
    {
        [Fact]
        public void Equals_EqualByRate_False()
        {
            // Arrange
            var x = new ExchangeRate
            {
                Code = "Test",
                Date = new DateTime(2017, 1, 1),
                Rate = 1
            };
            var y = new ExchangeRate
            {
                Code = "Test2",
                Date = x.Date.AddDays(1),
                Rate = x.Rate
            };

            var comparer = new ExchangeRateComparer();

            // Act
            var isEqual = comparer.Equals(x, y);

            // Assert
            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_EqualByCode_False()
        {
            // Arrange
            var x = new ExchangeRate
            {
                Code = "Test",
                Date = new DateTime(2017, 1, 1)
            };
            var y = new ExchangeRate
            {
                Code = x.Code,
                Date = x.Date.AddDays(1)
            };

            var comparer = new ExchangeRateComparer();

            // Act
            var isEqual = comparer.Equals(x, y);

            // Assert
            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_EqualByDate_False()
        {
            // Arrange
            var x = new ExchangeRate
            {
                Code = "Test",
                Date = new DateTime(2017, 1, 1)
            };
            var y = new ExchangeRate
            {
                Code = "Test2",
                Date = x.Date
            };

            var comparer = new ExchangeRateComparer();

            // Act
            var isEqual = comparer.Equals(x, y);

            // Assert
            isEqual.Should().BeFalse();
        }

        [Fact]
        public void Equals_EqualByDateAndCode_True()
        {
            // Arrange
            var x = new ExchangeRate
            {
                Code = "Test",
                Date = new DateTime(2017, 1, 1)
            };
            var y = new ExchangeRate
            {
                Code = x.Code,
                Date = x.Date
            };

            var comparer = new ExchangeRateComparer();

            // Act
            var isEqual = comparer.Equals(x, y);

            // Assert
            isEqual.Should().BeTrue();
        }
    }
}
