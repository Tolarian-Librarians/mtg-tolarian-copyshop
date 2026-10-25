using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tests.ScryfallApiChangeTests
{
    [TestClass]
    public class ScryfallModelGuardTests
    {
        /// <summary>
        /// Newtonsoft fails the whole response when Scryfall sends a value an enum does not know, which crashed the app several times.
        /// Scryfall models therefore keep such values as strings, and the mappers translate the known ones into enums.
        /// </summary>
        [TestMethod]
        public void ScryfallModels_DoNotDeserializeIntoEnums()
        {
            IEnumerable<Type> scryfallModels = typeof(SfCard).Assembly.GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.StartsWith("Tolarian.Copyshop.Business.Models.Sf", StringComparison.Ordinal));

            List<string> violations = scryfallModels
                .SelectMany(t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(p => (Type: t, Property: p)))
                .Where(x => ContainsEnum(x.Property.PropertyType))
                .Select(x => $"{x.Type.Name}.{x.Property.Name}")
                .ToList();

            Assert.IsEmpty(violations, "Scryfall models must not use enums: " + string.Join(", ", violations));
        }

        private static bool ContainsEnum(Type type)
        {
            Type underlying = Nullable.GetUnderlyingType(type) ?? type;

            if (underlying.IsEnum)
            {
                return true;
            }

            if (underlying.IsArray)
            {
                return ContainsEnum(underlying.GetElementType());
            }

            return underlying.IsGenericType && underlying.GetGenericArguments().Any(ContainsEnum);
        }
    }
}
