using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nofs.Net.Utils
{
    [Serializable]
    public sealed class UUID : IComparable, ICloneable
    {
        /// <summary>
        /// A class that represents an immutable universally unique identifier (UUID). A UUID represents a 128-bit value.
        /// </summary>
        /// 
        private Guid uuid;
        public UUID()
        {
            uuid = Guid.NewGuid();
        }

        

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
