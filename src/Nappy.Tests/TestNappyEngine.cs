using System;
using System.Collections.Generic;
using System.Linq;
using Nappy.Core;
using Xunit;

namespace Nappy.Tests
{
    public class TestNappyEngine
    {
        #region get

        [Fact]
        public void get_returns_expected_response_when_nothing_has_been_registered()
        {
            var sut = new EngineBuilder().Build();

            var response = sut.GetResponseFor(ResourceRequest.Get("/foo"));

            Assert.Equal(
                expected: ResourceResponse.NotFound(),
                actual: response,
                comparer: new StatusCodeComparer()
            );
        }

        [Fact]
        public void get_returns_expected_response()
        {
            var expectedBody = "foo-body";

            var sut = new EngineBuilder().Build();

            sut.RegisterHandler(new HandlerDefinition
            {
                HttpVerb = HttpVerbs.Get,
                ResourceName = "foo",
                Handler = request => HandlerResult.FromResult(expectedBody)
            });

            var response = sut.GetResponseFor(ResourceRequest.Get("/foo"));

            Assert.Equal(
                expected: ResourceResponse.Ok(expectedBody),
                actual: response,
                comparer: new StatusCodeAndBodyComparer()
            );
        }

        [Fact]
        public void get_returns_expected_response_when_nothing_was_found()
        {
            var sut = new EngineBuilder().Build();

            sut.RegisterHandler(new HandlerDefinition
            {
                HttpVerb = HttpVerbs.Get,
                ResourceName = "foo",
                Handler = request => HandlerResult.Nothing()
            });

            var response = sut.GetResponseFor(ResourceRequest.Get("/foo"));

            Assert.Equal(
                expected: ResourceResponse.NotFound(),
                actual: response,
                comparer: new StatusCodeComparer()
            );
        }

        [Fact]
        public void get_single_returns_expected_response()
        {
            var expectedBody = "foo-body";

            var sut = new EngineBuilder().Build();

            sut.RegisterHandler(new HandlerDefinition
            {
                HttpVerb = HttpVerbs.Get,
                ResourceName = "foo",
                Handler = request => HandlerResult.FromResult(expectedBody)
            });

            var response = sut.GetResponseFor(ResourceRequest.Get("/foo/1"));

            Assert.Equal(
                expected: ResourceResponse.Ok(expectedBody),
                actual: response,
                comparer: new StatusCodeAndBodyComparer()
            );
        }

        [Fact]
        public void get_single_returns_expected_response_when_nothing_was_found()
        {
            var sut = new EngineBuilder().Build();

            sut.RegisterHandler(new HandlerDefinition
            {
                HttpVerb = HttpVerbs.Get,
                ResourceName = "foo",
                Handler = request => HandlerResult.Nothing()
            });

            var response = sut.GetResponseFor(ResourceRequest.Get("/foo/1"));

            Assert.Equal(
                expected: ResourceResponse.NotFound(),
                actual: response,
                comparer: new StatusCodeComparer()
            );
        }

        [Fact]
        public void get_returns_expected_response_when_exceptions_occure()
        {
            var sut = new EngineBuilder().Build();

            sut.RegisterHandler(new HandlerDefinition
            {
                HttpVerb = HttpVerbs.Get,
                ResourceName = "foo",
                Handler = request => throw new Exception()
            });

            var response = sut.GetResponseFor(ResourceRequest.Get("/foo"));

            Assert.Equal(
                expected: ResourceResponse.InternalServerError(),
                actual: response,
                comparer: new StatusCodeComparer()
            );
        }
        #endregion
    }

    public class EngineBuilder
    {
        public Engine Build()
        {
            return new Engine();
        }
    }

    public class ResourceHandlerBuilder<TResource>
    {
        public ResourceHandlerBuilder<TResource> OnGet(Func<object> getAll)
        {
            return this;
        }

        public object Build()
        {
            return this;
        }
    }

    public class StatusCodeAndBodyComparer : IEqualityComparer<ResourceResponse>
    {
        public bool Equals(ResourceResponse left, ResourceResponse right)
        {
            return new[]
            {
                left.StatusCode == right.StatusCode,
                left.Body == right.Body
            }.All(x => x);
        }

        public int GetHashCode(ResourceResponse obj)
        {
            throw new System.NotImplementedException();
        }
    }

    public class StatusCodeComparer : IEqualityComparer<ResourceResponse>
    {
        public bool Equals(ResourceResponse left, ResourceResponse right)
        {
            return new[]
            {
                left.StatusCode == right.StatusCode,
            }.All(x => x);
        }

        public int GetHashCode(ResourceResponse obj)
        {
            throw new System.NotImplementedException();
        }
    }

    public enum HttpStatusCodes
    {
        Unknown = 0,
        NotFound,
        Ok,
        InternalServerError
    }

    public class ResourceRequest
    {
        public ResourceRequest(HttpVerbs httpVerb, string url)
        {
            HttpVerb = httpVerb;
            Url = url;

            var (name, id) = ExtractResourceInfoFrom(url);

            ResourceName = name;
            ResourceId = id;
        }

        private static (string Name, string Id) ExtractResourceInfoFrom(string url)
        {
            var segments = url.Split('/', StringSplitOptions.RemoveEmptyEntries);

            var name = segments.Length > 0 ? segments[0] : null;
            var id = segments.Length > 1 ? segments[1] : null;

            return ValueTuple.Create(name, id);
        }

        public HttpVerbs HttpVerb { get; }
        public string Url { get; }
        public string ResourceName { get; }
        public string ResourceId { get; }

        public static ResourceRequest Get(string url) => new ResourceRequest(HttpVerbs.Get, url);
    }

    public class ResourceResponse
    {
        public ResourceResponse(HttpStatusCodes statusCode, string body)
        {
            StatusCode = statusCode;
            Body = body;
        }

        public HttpStatusCodes StatusCode { get; }
        public string Body { get; }

        public static ResourceResponse NotFound() => new ResourceResponse(HttpStatusCodes.NotFound, "");
        public static ResourceResponse Ok(string body = "") => new ResourceResponse(HttpStatusCodes.Ok, body);
        public static ResourceResponse InternalServerError() => new ResourceResponse(HttpStatusCodes.InternalServerError, "Internal Server Error");
    }

    public class Engine
    {
        private readonly List<HandlerDefinition> _handlers = new List<HandlerDefinition>();

        public void RegisterHandler(HandlerDefinition handlerDefinition)
        {
            _handlers.Add(handlerDefinition);
        }

        public ResourceResponse GetResponseFor(ResourceRequest request)
        {
            var handler = _handlers
                .Where(x => x.HttpVerb == request.HttpVerb)
                .Where(x => x.ResourceName == request.ResourceName)
                .SingleOrDefault();

            return handler != null
                ? CreateResponse(request, handler)
                : ResourceResponse.NotFound();
        }

        private static ResourceResponse CreateResponse(ResourceRequest request, HandlerDefinition handler)
        {
            try
            {
                var result = handler.Handler(request);

                if (!result.HasResult)
                {
                    return ResourceResponse.NotFound();
                }

                var resultBody = result
                    .Result
                    .ToString();

                return ResourceResponse.Ok(resultBody);
            }
            catch
            {
                return ResourceResponse.InternalServerError();
            }
        }
    }

    public class HandlerDefinition
    {
        public HttpVerbs HttpVerb { get; set; }
        public string ResourceName { get; set; }
        public Func<ResourceRequest, HandlerResult> Handler { get; set; }
    }

    public class HandlerResult
    {
        private HandlerResult(bool hasResult, object result)
        {
            HasResult = hasResult;
            Result = result;
        }

        public bool HasResult { get; }
        public object Result { get; }

        public static HandlerResult Nothing() => new HandlerResult(false, null);
        public static HandlerResult FromResult(object result) => new HandlerResult(true, result);
    }
}