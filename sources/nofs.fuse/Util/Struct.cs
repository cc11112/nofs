using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nofs.Fuse.Util
{
    public abstract class Struct :ICloneable
    {
        public Object Clone()
        {
           return this.MemberwiseClone();      // call clone method
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder(GetType().Name);

            return sb
               .Append("[ ")
               .Append(appendAttributes(sb, false) ? ", " : "")
               .Append("hashCode=").Append(GetHashCode())
               .Append(" ]")
               .ToString();
        }

        protected virtual Boolean appendAttributes(StringBuilder buff, Boolean isPrefixed)
        {
            return isPrefixed;
        }
    }
}
