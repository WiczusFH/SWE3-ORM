using System;

namespace ORM.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ForeignKey : Attribute
    {
        public string column { get; }
        public ForeignKey(string column)
        {
            this.column = column;
        }
    }
}
