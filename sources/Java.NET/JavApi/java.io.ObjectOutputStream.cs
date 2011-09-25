using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.io
{
    public class ObjectOutputStream : OutputStream, DataOutput, ObjectStreamConstants, Flushable, Closeable
    {
        private DataOutputStream delegateInstance;
            
        protected ObjectOutputStream() {}
        public ObjectOutputStream (OutputStream oStream)
        {
            DataOutputStream dos = (oStream is DataOutputStream) ? (DataOutputStream)oStream : new DataOutputStream (oStream);
            this.delegateInstance = dos;
        }

        public void defaultWriteObject()
        {
            //Empty implementation
        }
        public void writeObject (Object obj) // Template method pattern
        {
            this.writeObjectOverride (obj);
        }
        
        protected void writeObjectOverride (Object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter ();
            MemoryStream inMemoryStream = new MemoryStream ();
            formatter.Serialize (inMemoryStream, obj);
            this.write (inMemoryStream.ToArray ());
            inMemoryStream.Close();
        }

        #region DataOutput implementation
        public override void write (byte[] buffer)
        {
            delegateInstance.write (buffer);
        }

        public override void write (byte[] buffer, int offset, int count)
        {
            delegateInstance.write (buffer, offset, count);
        }

        public override void write (int oneByte)
        {
            delegateInstance.write (oneByte);
        }

        public void writeBoolean (bool val)
        {
            delegateInstance.writeBoolean (val);
        }

        public void writeByte (int val)
        {
            delegateInstance.writeInt (val);
        }

        public void writeBytes (string str)
        {
            delegateInstance.write (str.getBytes());
        }

        public void writeChar (int val)
        {
            delegateInstance.write (val);
        }

        public void writeChars (string str)
        {
            delegateInstance.writeChars (str);
        }

        public void writeDouble (double val)
        {
            delegateInstance.writeDouble (val);
        }

        public void writeFloat (float val)
        {
            delegateInstance.writeFloat (val);
        }

        public void writeInt (int val)
        {
            delegateInstance.writeInt (val);
        }

        public void writeLong (long val)
        {
            delegateInstance.writeLong (val);
        }

        public void writeShort (int val)
        {
            delegateInstance.writeShort (val);
        }

        public void writeUTF (string str)
        {
            delegateInstance.writeUTF (str);
        }
        #endregion
    }
}

