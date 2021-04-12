using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Ipct.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Ipct.Tests.InProcess
{
    /// <summary>
    /// This class differs from UnitTest in that it derives from Xunit's IClassFixture{T}
    /// where WebApplicationFactory{T} is a factory that is injected in the ctor.
    /// </summary>
    public class IntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public IntegrationTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// GET to http://localhost:5000/WeatherForecast
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestGet()
        {
            // Arrange
            var httpClient = _factory.CreateDefaultClient();

            // Act
            var result = await httpClient.GetAsync("http://localhost:5000/WeatherForecast");

            // Assert
            result.EnsureSuccessStatusCode();
            result.StatusCode.Should().Be(200);
        }

        /// <summary>
        /// POST of application/json to http://localhost:5000/WeatherForecast
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestPost()
        {
            // Arrange
            var httpClient = _factory.CreateDefaultClient();
            var x = new WeatherForecast
            {
                Date = DateTime.Now,
                Summary = "Foo",
                TemperatureC = 10
            };
            var json = JsonConvert.SerializeObject(x);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var result = await httpClient.PostAsync("http://localhost:5000/WeatherForecast", content);

            // Assert
            result.EnsureSuccessStatusCode();
            result.StatusCode.Should().Be(201);
        }

        /// <summary>
        /// POST of INCORRECT application/json to http://localhost:5000/WeatherForecast
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestPostWithBadData()
        {
            // Arrange
            var httpClient = _factory.CreateDefaultClient();
            var x = new WeatherForecast
            {
                Date = DateTime.Now,
                Summary = "Foo5",
                TemperatureC = -300
            };
            var json = JsonConvert.SerializeObject(x);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var result = await httpClient.PostAsync("http://localhost:5000/WeatherForecast", content);
            var json2 = await result.Content.ReadAsStringAsync();
            var problemDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(json2);

            // Assert
            result.StatusCode.Should().Be(400);
            problemDetails.Errors["TemperatureC.celsius"]
                .Should()
                .StartWith("Can't be below absolute zero: -300");
        }
    }
}
