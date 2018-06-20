using Nappy.Core;
using Xunit;

namespace Nappy.Tests
{
    public class TestResourceRequest
    {
        [Theory]
        [InlineData(HttpVerbs.Get)]
        [InlineData(HttpVerbs.Post)]
        public void returns_expected_verb_when_initialized(HttpVerbs expectedVerb)
        {
            var sut = new ResourceRequest(expectedVerb, "dummy-url");
            Assert.Equal(expectedVerb, sut.HttpVerb);
        }

        [Theory]
        [InlineData("foo")]
        [InlineData("bar")]
        public void returns_expected_url_when_initialized(string expectedUrl)
        {
            var dummyVerb = HttpVerbs.Get;
            var sut = new ResourceRequest(dummyVerb, expectedUrl);

            Assert.Equal(expectedUrl, sut.Url);
        }

        [Theory]
        [InlineData("/foo", "foo")]
        [InlineData("/bar", "bar")]
        public void returns_expected_resource_name(string url, string expectedName)
        {
            var dummyVerb = HttpVerbs.Get;
            var sut = new ResourceRequest(dummyVerb, url);

            Assert.Equal(expectedName, sut.ResourceName);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void returns_expected_resource_id(string expectedId)
        {
            var dummyVerb = HttpVerbs.Get;
            var sut = new ResourceRequest(dummyVerb, $"/foo/{expectedId}");

            Assert.Equal(expectedId, sut.ResourceId);
        }

        [Fact]
        public void returns_expected_resource_id_when_not_part_of_url()
        {
            var dummyVerb = HttpVerbs.Get;
            var sut = new ResourceRequest(dummyVerb, "/foo");

            Assert.Null(sut.ResourceId);
        }

    }
}