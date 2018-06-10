using System.Collections.Generic;

namespace Nappy.Core
{
    public class RouteHelper
    {
        public IEnumerable<Route> GetRoutesFor(ResourceDefinition resource)
        {
            var resourceName = resource.ResourceName.ToLower();
            var idName = resource.ResourceIdentifierName?.ToLower();

            var getAll = new Route(HttpVerbs.Get, $"/{resourceName}");
            var getSingle = new Route(HttpVerbs.Get, $"/{resourceName}/{{{idName}}}");
            var postNew = new Route(HttpVerbs.Post, $"/{resourceName}");
            var putSingle = new Route(HttpVerbs.Put, $"/{resourceName}/{{{idName}}}");
            var patchSingle = new Route(HttpVerbs.Patch, $"/{resourceName}/{{{idName}}}");
            var deleteSingle = new Route(HttpVerbs.Delete, $"/{resourceName}/{{{idName}}}");

            if (resource.ResourceIdentifierName == null)
            {
                return new[]
                {
                    getAll,
                    postNew,
                };
            }

            return new[]
            {
                getAll,
                getSingle,
                postNew, 
                putSingle,
                patchSingle,
                deleteSingle,
            };
        }
    }
}