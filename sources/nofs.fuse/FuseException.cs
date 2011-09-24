using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nofs.Fuse
{
    public class FuseException : System.Exception
    {
        private const long serialVersionUID = 0;

        private int errno;

        public FuseException()
        {
        }

        public FuseException(String message)
            : base(message)
        {
        }


        public FuseException(String message, System.Exception cause)
            : base(message, cause)
        {

        }

        //
        public Errno ErrorNumber
        {
            get;
            set;
        }

        public FuseException initErrno(int errno)
        {
            this.errno = errno;

            return this;
        }

        public int getErrno()
        {
            return errno;
        }
    }
}
