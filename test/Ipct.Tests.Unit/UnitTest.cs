using System;
using System.Linq;
using System.Net;
using FluentAssertions;
using Ipct.WebApi;
using Ipct.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Ipct.Tests.Unit
{
    public class UnitTest
    {
        private readonly Mock<ILogger<WeatherForecastController>> _mockLogger;
        public UnitTest()
        {
            _mockLogger = new Mock<ILogger<WeatherForecastController>>();
        }
        [Fact]
        public void TestGet()
        {
            var controller = new WeatherForecastController(_mockLogger.Object);
            var results = controller.Get();
            results.Count().Should().Be(5);
        }

        [Fact]
        public void TestPost()
        {
            var controller = new WeatherForecastController(_mockLogger.Object);
            var x = new WeatherForecast
            {
                Date = DateTime.Now,
                Summary = "Foo",
                TemperatureC = 10
            };

            var results = (CreatedResult)controller.Post(x);
            results.StatusCode.Should().Be((int) HttpStatusCode.Created);
            ((WeatherForecast) results.Value).TemperatureC.Should().Be(100);
            ((WeatherForecast)results.Value).Summary.Should().Be("Boiling");
        }
    }
}
