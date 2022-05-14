using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Solver
{
    /// <summary>
    /// Halves the type into Base Class and Child Class
    /// </summary>
    public class InheritanceSolver
    {
        public List<MemberInfo> childInfos = new List<MemberInfo>();
        public Type baseClass;
        public void solve(Type type)
        {
            baseClass = type.BaseType;
            foreach (MemberInfo info in type.GetMembers()) {
                if (info.MemberType == MemberTypes.Property || info.MemberType == MemberTypes.Field)
                {
                    if (baseClass != null)
                    {
                        if (baseClass.GetMember(info.Name).Length == 0)
                        {
                            childInfos.Add(info);
                        }
                    }
                }
            }

        }

    }
}
