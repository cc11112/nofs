using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nofs.Fuse.Util;

namespace Nofs.Fuse
{
    public class FuseFtype : Struct
    {
        public int mode;

        public FuseFtypeConstants FtypeConstants
        {
            get;
            set;
        }

        protected override Boolean appendAttributes(StringBuilder buff, Boolean isPrefixed)
        {
            buff.Append(base.appendAttributes(buff, isPrefixed) ? ", " : " ")
                .Append("mode=").
                Append(Convert.ToString(mode, 8))
                .Append("(OCT)");

            return true;
        }
    }
}
