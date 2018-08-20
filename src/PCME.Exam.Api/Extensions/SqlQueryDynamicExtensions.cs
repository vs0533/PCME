using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Exam.Api.Extensions
{
    public static class SqlQueryDynamicExtensions
    {
        public static IEnumerable<dynamic> SqlQueryDynamic(this DbContext db, string Sql, params SqlParameter[] parameters)
        {
            using (var cmd = db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Sql;

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                foreach (var p in parameters)
                {
                    var dbParameter = cmd.CreateParameter();
                    dbParameter.DbType = p.DbType;
                    dbParameter.ParameterName = p.ParameterName;
                    dbParameter.Value = p.Value;
                    cmd.Parameters.Add(dbParameter);
                }

                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var row = new ExpandoObject() as IDictionary<string, object>;
                        for (var fieldCount = 0; fieldCount<dataReader.FieldCount; fieldCount++)
                        {
                            row.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
                        }
                        yield return row;
                    }
                }
            }
        }
    }
}
