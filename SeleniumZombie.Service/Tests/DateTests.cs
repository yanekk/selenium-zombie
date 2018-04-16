using System;
using NUnit.Framework;
using SeleniumZombie.Service.Configuration;

namespace SeleniumZombie.Service.Tests
{
    [TestFixture]
    public class DateTests
    {
        private void IsServiceUp(TimeSpan startTime, TimeSpan endTime, TimeSpan now, bool expectedResult)
        {
            var service = new SeleniumZombieService(new ConfigurationModel
            {
                StartTime = startTime,
                EndTime = endTime
            });
            var nowDate = DateTime.Today.AddMilliseconds(now.TotalMilliseconds);
            var actualResult = service.IsDateWithinBoundaries(nowDate);
            Assert.AreEqual(expectedResult, actualResult,
                $"StartTime: {startTime}, EndTime: {endTime}, now: {now}");
        }

        [Test]
        public void IsTimWithinBoundaries()
        {
            
            IsServiceUp(new TimeSpan(0, 30, 0), new TimeSpan(6, 30, 0), new TimeSpan(0, 29, 0), false);
            IsServiceUp(new TimeSpan(0, 30, 0), new TimeSpan(6, 30, 0), new TimeSpan(0, 31, 0), true);
            IsServiceUp(new TimeSpan(0, 30, 0), new TimeSpan(6, 30, 0), new TimeSpan(6, 29, 0), true);
            IsServiceUp(new TimeSpan(0, 30, 0), new TimeSpan(6, 30, 0), new TimeSpan(6, 31, 0), false);
            
            IsServiceUp(new TimeSpan(21, 0, 0), new TimeSpan(06, 30, 0), new TimeSpan(20, 59, 0), false);
            IsServiceUp(new TimeSpan(21, 0, 0), new TimeSpan(06, 30, 0), new TimeSpan(21, 01, 0), true);
            IsServiceUp(new TimeSpan(21, 0, 0), new TimeSpan(06, 30, 0), new TimeSpan(6, 29, 0), true);
            IsServiceUp(new TimeSpan(21, 0, 0), new TimeSpan(06, 30, 0), new TimeSpan(6, 31, 0), false);
        }
    }
}
