using System;
using System.Text;
using Nofs.Fuse.Util;

namespace Nofs.Fuse
{

    public class FuseStatfs : Struct, FuseStatfsSetter
    {
        public int blockSize;
        public int blocks;
        public int blocksFree;
        public int blocksAvail = -1; // by default (if not overwriten) it is computed as: blocksFree * 20 / 19
        public int files;
        public int filesFree;
        public int namelen;


        /**
         * FuseStatfsSetter implementation
         */
        public void set(int blockSize, int blocks, int blocksFree, int blocksAvail, int files, int filesFree, int namelen)
        {
            this.blockSize = blockSize;
            this.blocks = blocks;
            this.blocksFree = blocksFree;
            this.blocksAvail = (blocksAvail >= 0) ? blocksAvail : (int)((long)blocksFree * 20L / 19L);
            this.files = files;
            this.filesFree = filesFree;
            this.namelen = namelen;
        }


        protected override Boolean appendAttributes(StringBuilder buff, Boolean isPrefixed)
        {
            buff.Append(base.appendAttributes(buff, isPrefixed) ? ", " : " ");

            buff.Append("blockSize=").Append(blockSize)
               .Append(", blocks=").Append(blocks)
               .Append(", blocksFree=").Append(blocksFree)
               .Append(", blocksAvail=").Append(blocksAvail)
               .Append(", files=").Append(files)
               .Append(", filesFree=").Append(filesFree)
               .Append(", namelen=").Append(namelen);

            return true;
        }
    }
}
