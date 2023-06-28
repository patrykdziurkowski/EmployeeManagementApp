using FastMember;
using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

namespace Models
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts OracleDataReader row into a given entity type
        /// </summary>
        /// <typeparam name="T">The type of database entity to be returned</typeparam>
        /// <returns>An instance of T object</returns>
        public static async Task<T> ConvertToObject<T>(this OracleDataReader reader) where T : class, new()
        {
            T result = new();

            TypeAccessor accessor = TypeAccessor.Create(typeof(T));
            MemberSet fields = accessor.GetMembers();

            ReadOnlyCollection<DbColumn> columns = await reader.GetColumnSchemaAsync();

            foreach (Member field in fields)
            {
                foreach (DbColumn column in columns)
                {
                    bool foundDatabaseMatchForField = DoesColumnMatchField(column, field);

                    if (foundDatabaseMatchForField)
                    {
                        bool columnNotNull = !(await reader.IsDBNullAsync(column.ColumnName));
                        if (columnNotNull)
                        {
                            accessor[result, field.Name] = reader.GetValue(column.ColumnName);
                        }

                        break;
                    }
                }
            }

            return result;
        }

        private static bool DoesColumnMatchField(DbColumn column, Member field)
        {
            string columnNameNoUnderscores = column.ColumnName.Replace("_", "");

            bool foundDatabaseMatchForField = string
                .Equals(columnNameNoUnderscores, field.Name, StringComparison.OrdinalIgnoreCase);

            return foundDatabaseMatchForField;
        }

    }
}
