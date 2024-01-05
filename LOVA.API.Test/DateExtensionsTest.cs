using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LOVA.API.Test
{
    public class DateExtensionsTest
    {
        [Fact]
        public void VerifyReturnDates()
        {
            // Arrange
            DateTime now = DateTime.Now;



            // act
            var resultSameHour = LOVA.API.Extensions.DateExtensions.TotalHours(now, now.AddMinutes(-2));
            var resultMultipleHours = LOVA.API.Extensions.DateExtensions.TotalHours(now, now.AddHours(-1));
            var resultMultiple2Hours = LOVA.API.Extensions.DateExtensions.TotalHours(now, now.AddHours(-2));
            var resultMultiple3Hours = LOVA.API.Extensions.DateExtensions.TotalHours(now, now.AddHours(-3));

            // Assert
            Assert.Single(resultSameHour);
            Assert.Single(resultMultipleHours);
            Assert.Equal(2, resultMultiple2Hours.Count());
            Assert.Equal(3, resultMultiple3Hours.Count());
        }

        [Fact]
        public void VerifyNewHour()
        {
            // Arrange
            DateTime now = DateTime.Now;


            // act
            var resultSameHour = LOVA.API.Extensions.DateExtensions.NewHour(now, now);
            var resultMultipleHours = LOVA.API.Extensions.DateExtensions.NewHour(now, now.AddHours(-1));


            // Assert
            Assert.False(resultSameHour);
            Assert.True(resultMultipleHours);

        }

        [Fact]
        public void VerifyNewDay()
        {
            // Arrange
            DateTime now = DateTime.Now;


            // act
            var resultSameHour = LOVA.API.Extensions.DateExtensions.IsNewDay(now, now);
            var resultMultipleHours = LOVA.API.Extensions.DateExtensions.IsNewDay(now, now.AddDays(-2));


            // Assert
            Assert.False(resultSameHour);
            Assert.True(resultMultipleHours);

        }
    }
}
