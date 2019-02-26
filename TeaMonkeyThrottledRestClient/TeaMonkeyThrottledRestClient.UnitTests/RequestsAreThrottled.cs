using System.Diagnostics;
using System.Net;
using FluentAssertions;
using Xunit;

namespace TeaMonkeyThrottledRestClient.UnitTests
{
    public class RequestsAreThrottled
    {
        private readonly string _baseUrl = "https://jsonplaceholder.typicode.com";

        [Fact]
        public void FiveRequestsPerSecond()
        {
            var client = new ThrottledRestClient();
            client.MaxRequestsPerSecond = 5;
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            for (int i = 0; i < 6; i++)
            {
                var result = client.Get($"{_baseUrl}/Posts/1");
            }
            stopwatch.Stop();

            stopwatch.Elapsed.TotalSeconds.Should().BeGreaterThan(1);
        }
        
        [Fact]
        public void TwoRequestsPerSecond()
        {
            var client = new ThrottledRestClient();
            client.MaxRequestsPerSecond = 2;
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            for (int i = 0; i < 6; i++)
            {
                var result = client.Get($"{_baseUrl}/Posts/1");
            }
            stopwatch.Stop();

            stopwatch.Elapsed.TotalSeconds.Should().BeGreaterThan(2);
        }
        
        [Fact]
        public void OneRequestPerSecond()
        {
            var client = new ThrottledRestClient();
            client.MaxRequestsPerSecond = 1;
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            for (int i = 0; i < 6; i++)
            {
                var result = client.Get($"{_baseUrl}/Posts/1");
            }
            stopwatch.Stop();

            stopwatch.Elapsed.TotalSeconds.Should().BeGreaterThan(5);
        }
    }
}