using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nofs.Fuse
{
    /**
 * This is a byte level API directory entry
 */

    public class FuseFSDirEnt : FuseFtype
    {
        public byte[] name;

        public long inode;


        protected override Boolean appendAttributes(StringBuilder buff, Boolean isPrefixed)
        {
            buff.Append(isPrefixed ? ", " : " ")
                .Append("name='")
                .Append(name)
                .Append("'")
                .Append("inode='")
                .Append(inode)
                .Append("'");

            return base.appendAttributes(buff, true);
        }
    }
}
