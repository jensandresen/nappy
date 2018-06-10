using System;
using Xunit;

namespace Nappy.Tests
{
    public class TestRouter
    {
        [Theory]
        [InlineData("foo", "1")]
        [InlineData("foo", "2")]
        [InlineData("bar", "1")]
        [InlineData("bar", "2")]
        public void returns_expected_when_extracting_resource_name_and_identifier_from_url(string expectedResourceName, string expectedResourceIdentifier)
        {
            var sut = new Router();

            var result = sut.ConvertToResourceUrl($"/{expectedResourceName}/{expectedResourceIdentifier}");

            Assert.Equal(expectedResourceName, result.Name);
            Assert.Equal(expectedResourceIdentifier, result.Id);
        }

        [Theory]
        [InlineData("foo")]
        [InlineData("bar")]
        public void returns_expected_when_extracting_resource_name_and_identifier_from_url_without_identifier(string expectedName)
        {
            var sut = new Router();
            var result = sut.ConvertToResourceUrl($"/{expectedName}");

            Assert.Equal(expectedName, result.Name);
            Assert.Null(result.Id);
        }
    }

    public class Router
    {
        public ResourceUrl ConvertToResourceUrl(string url)
        {
            var segments = url.Split('/', StringSplitOptions.RemoveEmptyEntries);

            return new ResourceUrl
            {
                Name = segments.Length > 0 ? segments[0] : null,
                Id = segments.Length > 1 ? segments[1] : null
            };
        }
    }

    public class ResourceUrl
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}