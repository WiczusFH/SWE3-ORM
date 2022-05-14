using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Util
{
    public class Reflection
    {
        public static Type getTypeFromMember(MemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                return ((FieldInfo)memberInfo).FieldType;
            }
            if (memberInfo.MemberType == MemberTypes.Property)
            {
                return ((PropertyInfo)memberInfo).PropertyType;
            }
            return null;
        }

        public static void setValue(MemberInfo info, object forObject, object value) {
            Type t = forObject.GetType();
            if (info.MemberType == MemberTypes.Field)
            {
                ((FieldInfo)info).SetValue(forObject,value);
            }
            if (info.MemberType == MemberTypes.Property)
            {
                ((PropertyInfo)info).SetValue(forObject, value);
            }
        }
        public static object GetValue(MemberInfo memberInfo, object forObject)
        {
            if (forObject == null) {
                return null;
            }
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(forObject);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(forObject);
                default:
                    throw new NotImplementedException();
            }
        }
        public static bool isList(MemberInfo memberInfo)
        {
            Type type = getTypeFromMember(memberInfo);
            return type?.GetInterface("IList") != null;

        }


        public static List<Type> factoryTypes = Assembly.GetExecutingAssembly().GetType().Module.Assembly.GetExportedTypes().ToList();

        public static bool isPrimitive(MemberInfo info)
        {
            return (factoryTypes.Find(n => n == Reflection.getTypeFromMember(info)) != null);
        }


        //https://stackoverflow.com/questions/1749966/c-sharp-how-to-determine-whether-a-type-is-a-number
        public static bool IsNumericType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

    }
}
