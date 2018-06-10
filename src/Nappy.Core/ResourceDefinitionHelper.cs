using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Nappy.Core
{
    public class ResourceDefinitionHelper
    {
        public ResourceDefinition GetDefinitionFor<T>()
        {
            return GetDefinitionFor(typeof(T));
        }

        public ResourceDefinition GetDefinitionFor(Type type)
        {
            return new ResourceDefinition
            {
                ResourceName = ExtractNameFrom(type),
                ResourceIdentifierName = ExtractIdentifierNameFrom(type)
            };
        }

        private string ExtractNameFrom(Type type)
        {
            var attribute = type.GetCustomAttribute<ResourceAttribute>();

            if (attribute != null && !string.IsNullOrWhiteSpace(attribute.Name))
            {
                return attribute.Name;
            }

            return Regex.Replace(type.Name, @"^(.*?)(Resource)?$", "$1");
        }

        private string ExtractIdentifierNameFrom(Type type)
        {
            var idProperty = type
                .GetProperties()
                .SingleOrDefault(x => x.GetCustomAttribute<IdAttribute>() != null);

            if (idProperty != null)
            {
                var attribute = idProperty.GetCustomAttribute<IdAttribute>();

                if (!string.IsNullOrWhiteSpace(attribute.Name))
                {
                    return attribute.Name;
                }

                return idProperty.Name;
            }

            idProperty = type.GetProperty("Id");
            return idProperty?.Name;
        }
    }
}