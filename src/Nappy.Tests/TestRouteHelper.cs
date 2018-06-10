using Nappy.Core;
using Xunit;

namespace Nappy.Tests
{
    public class TestRouteHelper
    {
        [Theory]
        [InlineData(HttpVerbs.Get, "/foo")]
        [InlineData(HttpVerbs.Get, "/foo/{id}")]
        [InlineData(HttpVerbs.Post, "/foo")]
        [InlineData(HttpVerbs.Put, "/foo/{id}")]
        [InlineData(HttpVerbs.Patch, "/foo/{id}")]
        [InlineData(HttpVerbs.Delete, "/foo/{id}")]
        public void returns_expected_route_for_identifiable_resource(HttpVerbs expectedVerb, string expectedUrl)
        {
            var sut = new RouteHelper();

            var resource = new ResourceDefinition
            {
                ResourceName = "Foo",
                ResourceIdentifierName = "Id"
            };

            var result = sut.GetRoutesFor(resource);

            Assert.Contains(
                expected: new Route(expectedVerb, expectedUrl),
                collection: result
            );
        }

        [Fact]
        public void returns_expected_routes_for_non_identifiable_resource()
        {
            var sut = new RouteHelper();

            var resource = new ResourceDefinition
            {
                ResourceName = "Foo",
            };

            var result = sut.GetRoutesFor(resource);

            var expected = new[]
            {
                new Route(HttpVerbs.Get, "/foo"),
                new Route(HttpVerbs.Post, "/foo"),
            };

            Assert.Equal(expected, result);
        }

    }
}