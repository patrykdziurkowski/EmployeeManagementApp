using FastMember;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts OracleDataReader row into a given entity type
        /// </summary>
        /// <typeparam name="T">The type of database entity to be returned</typeparam>
        /// <returns>An instance of T object</returns>
        public static T ConvertToObject<T>(this OracleDataReader reader) where T : class, new()
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var result = new T();

            for (int field = 0; field < reader.FieldCount; field++)
            {
                if (!reader.IsDBNull(field))
                {
                    string fieldName = reader.GetName(field);

                    bool foundMatchForGivenField = members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase));
                    if (foundMatchForGivenField)
                    {
                        accessor[result, fieldName] = reader.GetValue(field);
                    }
                }
            }

            return result;
        }
    }
}
