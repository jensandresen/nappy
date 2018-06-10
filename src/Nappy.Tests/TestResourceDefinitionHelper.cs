using Nappy.Core;
using Xunit;

namespace Nappy.Tests
{
    public class TestResourceDefinitionHelper
    {
        [Fact]
        public void returns_expected_resource_name()
        {
            var sut = new ResourceDefinitionHelper();
            var result = sut.GetDefinitionFor<FooResource>();

            Assert.Equal("Foo", result.ResourceName);
        }

        [Fact]
        public void returns_expected_resource_name_from_annotated_attribute()
        {
            var sut = new ResourceDefinitionHelper();
            var result = sut.GetDefinitionFor<QuxResource>();

            Assert.Equal("Foo", result.ResourceName);
        }

        [Fact]
        public void returns_expected_id_when_resource_does_not_have_one()
        {
            var sut = new ResourceDefinitionHelper();
            var result = sut.GetDefinitionFor<FooResource>();

            Assert.Null(result.ResourceIdentifierName);
        }

        [Fact]
        public void returns_expected_id()
        {
            var sut = new ResourceDefinitionHelper();
            var result = sut.GetDefinitionFor<BarResource>();

            Assert.Equal("Id", result.ResourceIdentifierName);
        }

        [Fact]
        public void returns_expected_id_when_annotated_with_attribute()
        {
            var sut = new ResourceDefinitionHelper();
            var result = sut.GetDefinitionFor<BazResource>();

            Assert.Equal("Number", result.ResourceIdentifierName);
        }

        [Fact]
        public void returns_expected_id_name_from_annotated_attribute()
        {
            var sut = new ResourceDefinitionHelper();
            var result = sut.GetDefinitionFor<QuxResource>();

            Assert.Equal("Id", result.ResourceIdentifierName);
        }

        private class FooResource { }

        private class BarResource
        {
            public string Id { get; set; }
        }

        private class BazResource
        {
            [Id]
            public string Number { get; set; }
        }

        [Resource(Name = "Foo")]
        private class QuxResource
        {
            [Id(Name = "Id")]
            public string Number { get; set; }
        }
    }
}