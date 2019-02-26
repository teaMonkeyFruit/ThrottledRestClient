using System.Net;
using FluentAssertions;
using Xunit;

namespace TeaMonkeyThrottledRestClient.UnitTests
{
    public class Basic
    {

        private readonly string _baseUrl = "https://jsonplaceholder.typicode.com";
        
        [Fact]
        public void GetSucceeds()
        {
            var client = new ThrottledRestClient();

            var result = client.Get($"{_baseUrl}/Posts");

            result.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public void PostSucceeds()
        {
            var client = new ThrottledRestClient();

            var result = client.Post($"{_baseUrl}/Posts", new {test = "test"});

            result.Response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        
        [Fact]
        public void PatchSucceeds()
        {
            var client = new ThrottledRestClient();

            var result = client.Patch($"{_baseUrl}/Posts/1", new {test = "test"});

            result.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}