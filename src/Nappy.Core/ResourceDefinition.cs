using System;

namespace Nappy.Core
{
    public class ResourceDefinition
    {
        [Obsolete]
        public ResourceDefinition()
        {
            
        }

        public ResourceDefinition(string resourceName, string resourceIdentifierName)
        {
            ResourceName = resourceName;
            ResourceIdentifierName = resourceIdentifierName;
        }

        public string ResourceName { get; set; }
        public string ResourceIdentifierName { get; set; }
    }
}