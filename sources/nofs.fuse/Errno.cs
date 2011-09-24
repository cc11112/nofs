
/**
 *   FUSE-J: Java bindings for FUSE (Filesystem in Userspace by Miklos Szeredi (mszeredi@inf.bme.hu))
 *
 *   Copyright (C) 2003 Peter Levart (peter@select-tech.si)
 *
 *   This program can be distributed under the terms of the GNU LGPL.
 *   See the file COPYING.LIB
 */

/**
 * This is an enumeration of error return values
 */

namespace Nofs.Fuse
{
    public sealed class Errno
    {
        //
        // generated from <errno.h>
        // Converted  by Crane (schen2@luc.edu)

        public static int EPERM { get { return 1; } }         /* Operation not permitted */
        public static int ENOENT { get { return 2; } }          /* No such file or directory */
        public static int ESRCH { get { return 3; } }          /* No such process */
        public static int EINTR { get { return 4; } }          /* Interrupted system call */
        public static int EIO { get { return 5; } }          /* I/O error */
        public static int ENXIO { get { return 6; } }          /* No such device or address */
        public static int E2BIG { get { return 7; } }          /* Arg list too long */
        public static int ENOEXEC { get { return 8; } }          /* Exec format error */
        public static int EBADF { get { return 9; } }          /* Bad file number */
        public static int ECHILD { get { return 10; } }          /* No child processes */
        public static int EAGAIN { get { return 11; } }          /* Try again */
        public static int ENOMEM { get { return 12; } }          /* Out of memory */
        public static int EACCES { get { return 13; } }          /* Permission denied */
        public static int EFAULT { get { return 14; } }          /* Bad address */
        public static int ENOTBLK { get { return 15; } }          /* Block device required */
        public static int EBUSY { get { return 16; } }          /* Device or resource busy */
        public static int EEXIST { get { return 17; } }          /* File exists */
        public static int EXDEV { get { return 18; } }          /* Cross-device link */
        public static int ENODEV { get { return 19; } }          /* No such device */
        public static int ENOTDIR { get { return 20; } }          /* Not a directory */
        public static int EISDIR { get { return 21; } }          /* Is a directory */
        public static int EINVAL { get { return 22; } }          /* Invalid argument */
        public static int ENFILE { get { return 23; } }          /* File table overflow */
        public static int EMFILE { get { return 24; } }          /* Too many open files */
        public static int ENOTTY { get { return 25; } }          /* Not a typewriter */
        public static int ETXTBSY { get { return 26; } }          /* Text file busy */
        public static int EFBIG { get { return 27; } }          /* File too large */
        public static int ENOSPC { get { return 28; } }          /* No space left on device */
        public static int ESPIPE { get { return 29; } }          /* Illegal seek */
        public static int EROFS { get { return 30; } }          /* Read-only file system */
        public static int EMLINK { get { return 31; } }          /* Too many links */
        public static int EPIPE { get { return 32; } }          /* Broken pipe */
        public static int EDOM { get { return 33; } }          /* Math argument out of domain of func */
        public static int ERANGE { get { return 34; } }          /* Math result not representable */
        public static int EDEADLK { get { return 35; } }          /* Resource deadlock would occur */
        public static int ENAMETOOLONG { get { return 36; } }          /* File name too long */
        public static int ENOLCK { get { return 37; } }          /* No record locks available */
        public static int ENOSYS { get { return 38; } }          /* Function not implemented */
        public static int ENOTEMPTY { get { return 39; } }          /* Directory not empty */
        public static int ELOOP { get { return 40; } }          /* Too many symbolic links encountered */
        public static int EWOULDBLOCK { get { return EAGAIN; } }          /* Operation would block */
        public static int ENOMSG { get { return 42; } }          /* No message of desired type */
        public static int EIDRM { get { return 43; } }          /* Identifier removed */
        public static int ECHRNG { get { return 44; } }          /* Channel number out of range */
        public static int EL2NSYNC { get { return 45; } }          /* Level 2 not synchronized */
        public static int EL3HLT { get { return 46; } }          /* Level 3 halted */
        public static int EL3RST { get { return 47; } }          /* Level 3 reset */
        public static int ELNRNG { get { return 48; } }          /* Link number out of range */
        public static int EUNATCH { get { return 49; } }          /* Protocol driver not attached */
        public static int ENOCSI { get { return 50; } }          /* No CSI structure available */
        public static int EL2HLT { get { return 51; } }          /* Level 2 halted */
        public static int EBADE { get { return 52; } }          /* Invalid exchange */
        public static int EBADR { get { return 53; } }          /* Invalid request descriptor */
        public static int EXFULL { get { return 54; } }          /* Exchange full */
        public static int ENOANO { get { return 55; } }          /* No anode */
        public static int EBADRQC { get { return 56; } }          /* Invalid request code */
        public static int EBADSLT { get { return 57; } }          /* Invalid slot */
        public static int EDEADLOCK { get { return EDEADLK; } }
        public static int EBFONT { get { return 59; } }          /* Bad font file format */
        public static int ENOSTR { get { return 60; } }          /* Device not a stream */
        public static int ENODATA { get { return 61; } }          /* No data available */
        public static int ETIME { get { return 62; } }          /* Timer expired */
        public static int ENOSR { get { return 63; } }          /* Out of streams resources */
        public static int ENONET { get { return 64; } }          /* Machine is not on the network */
        public static int ENOPKG { get { return 65; } }          /* Package not installed */
        public static int EREMOTE { get { return 66; } }          /* Object is remote */
        public static int ENOLINK { get { return 67; } }          /* Link has been severed */
        public static int EADV { get { return 68; } }          /* Advertise error */
        public static int ESRMNT { get { return 69; } }          /* Srmount error */
        public static int ECOMM { get { return 70; } }          /* Communication error on send */
        public static int EPROTO { get { return 71; } }          /* Protocol error */
        public static int EMULTIHOP { get { return 72; } }          /* Multihop attempted */
        public static int EDOTDOT { get { return 73; } }          /* RFS specific error */
        public static int EBADMSG { get { return 74; } }          /* Not a data message */
        public static int EOVERFLOW { get { return 75; } }          /* Value too large for defined data type */
        public static int ENOTUNIQ { get { return 76; } }          /* Name not unique on network */
        public static int EBADFD { get { return 77; } }          /* File descriptor in bad state */
        public static int EREMCHG { get { return 78; } }          /* Remote address changed */
        public static int ELIBACC { get { return 79; } }          /* Can not access a needed shared library */
        public static int ELIBBAD { get { return 80; } }          /* Accessing a corrupted shared library */
        public static int ELIBSCN { get { return 81; } }          /* .lib section in a.out corrupted */
        public static int ELIBMAX { get { return 82; } }          /* Attempting to link in too many shared libraries */
        public static int ELIBEXEC { get { return 83; } }          /* Cannot exec a shared library directly */
        public static int EILSEQ { get { return 84; } }          /* Illegal byte sequence */
        public static int ERESTART { get { return 85; } }          /* Interrupted system call should be restarted */
        public static int ESTRPIPE { get { return 86; } }          /* Streams pipe error */
        public static int EUSERS { get { return 87; } }          /* Too many users */
        public static int ENOTSOCK { get { return 88; } }          /* Socket operation on non-socket */
        public static int EDESTADDRREQ { get { return 89; } }          /* Destination address required */
        public static int EMSGSIZE { get { return 90; } }          /* Message too long */
        public static int EPROTOTYPE { get { return 91; } }          /* Protocol wrong type for socket */
        public static int ENOPROTOOPT { get { return 92; } }          /* Protocol not available */
        public static int EPROTONOSUPPORT { get { return 93; } }          /* Protocol not supported */
        public static int ESOCKTNOSUPPORT { get { return 94; } }          /* Socket type not supported */
        public static int EOPNOTSUPP { get { return 95; } }          /* Operation not supported on transport endpoint */
        public static int EPFNOSUPPORT { get { return 96; } }          /* Protocol family not supported */
        public static int EAFNOSUPPORT { get { return 97; } }          /* Address family not supported by protocol */
        public static int EADDRINUSE { get { return 98; } }          /* Address already in use */
        public static int EADDRNOTAVAIL { get { return 99; } }          /* Cannot assign requested address */
        public static int ENETDOWN { get { return 100; } }          /* Network is down */
        public static int ENETUNREACH { get { return 101; } }          /* Network is unreachable */
        public static int ENETRESET { get { return 102; } }          /* Network dropped connection because of reset */
        public static int ECONNABORTED { get { return 103; } }          /* Software caused connection abort */
        public static int ECONNRESET { get { return 104; } }          /* Connection reset by peer */
        public static int ENOBUFS { get { return 105; } }          /* No buffer space available */
        public static int EISCONN { get { return 106; } }          /* Transport endpoint is already connected */
        public static int ENOTCONN { get { return 107; } }          /* Transport endpoint is not connected */
        public static int ESHUTDOWN { get { return 108; } }          /* Cannot send after transport endpoint shutdown */
        public static int ETOOMANYREFS { get { return 109; } }          /* Too many references: cannot splice */
        public static int ETIMEDOUT { get { return 110; } }          /* Connection timed out */
        public static int ECONNREFUSED { get { return 111; } }          /* Connection refused */
        public static int EHOSTDOWN { get { return 112; } }          /* Host is down */
        public static int EHOSTUNREACH { get { return 113; } }          /* No route to host */
        public static int EALREADY { get { return 114; } }          /* Operation already in progress */
        public static int EINPROGRESS { get { return 115; } }          /* Operation now in progress */
        public static int ESTALE { get { return 116; } }          /* Stale NFS file handle */
        public static int EUCLEAN { get { return 117; } }          /* Structure needs cleaning */
        public static int ENOTNAM { get { return 118; } }          /* Not a XENIX named type file */
        public static int ENAVAIL { get { return 119; } }          /* No XENIX semaphores available */
        public static int EISNAM { get { return 120; } }          /* Is a named type file */
        public static int EREMOTEIO { get { return 121; } }          /* Remote I/O error */
        public static int EDQUOT { get { return 122; } }          /* Quota exceeded */
        public static int ENOMEDIUM { get { return 123; } }          /* No medium found */
        public static int EMEDIUMTYPE { get { return 124; } }          /* Wrong medium type */

        // extended attributes support needs these...

        public static int ENOATTR { get { return ENODATA; } }  /* No such attribute */
        public static int ENOTSUPP { get { return 524; } }     /* Operation is not supported*/
    }
}
