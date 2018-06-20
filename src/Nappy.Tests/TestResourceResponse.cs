using Xunit;

namespace Nappy.Tests
{
    public class TestResourceResponse
    {
        [Theory]
        [InlineData(HttpStatusCodes.Ok)]
        [InlineData(HttpStatusCodes.NotFound)]
        public void returns_expected_status_code_when_initialized(HttpStatusCodes expected)
        {
            var sut = new ResourceResponse(expected, "dummy-body");
            Assert.Equal(expected, sut.StatusCode);
        }

        [Theory]
        [InlineData("foo")]
        [InlineData("bar")]
        public void returns_expected_body_when_initialized(string expected)
        {
            var dummyStatusCode = HttpStatusCodes.Ok;
            var sut = new ResourceResponse(dummyStatusCode, expected);
            Assert.Equal(expected, sut.Body);
        }


    }
}