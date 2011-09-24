using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nofs.Fuse.Util;

namespace Nofs.Fuse
{
    public class FuseSize : Struct, FuseSizeSetter
    {
        public int size;

        //
        // FuseSizeSetter implementation

        public void setSize(int size)
        {
            this.size = size;
        }


        protected override Boolean appendAttributes(StringBuilder buff, Boolean isPrefixed)
        {
            buff.Append(base.appendAttributes(buff, isPrefixed) ? ", " : " ");

            buff.Append("size=").Append(size);

            return true;
        }
    }

}
