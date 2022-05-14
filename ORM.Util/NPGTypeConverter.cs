using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ORM.Util;

namespace OZCore.Converters
{
    public class NPGTypeConverter
    {
        public static NpgsqlDbType NPGTypeFromCSType(Type type) {
            if (type == typeof(string)) {
                return NpgsqlDbType.Text;
            }
            if (Reflection.IsNumericType(type))
            {
                return NpgsqlDbType.Integer;
            }

            return NpgsqlDbType.Char;
        }
    }
}
