using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util
{
    /// <summary>
    /// <strong>In change to Java this class extends java.util.AbstractMap and implements
    /// java.util.Dictionary.</strong>
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class Hashtable<K,V> : AbstractMap<K,V>, Dictionary<K,V>
    {
        public Hashtable() : base()
        {
        }

        public override void clear()
        {
            lock (this)
            {
                base.clear();
            }
        }
        public override bool containsKey(object key)
        {
            lock (this)
            {
                return base.containsKey(key);
            }
        }
        public override bool containsValue(object value)
        {
            lock (this)
            {
                return base.containsValue(value);
            }
        }
        public override Set<MapNS.Entry<K, V>> entrySet()
        {
            lock (this)
            {
                return base.entrySet();
            }
        }
        public override V get(object key)
        {
            lock (this)
            {
                return base.get(key);
            }
        }
        public override bool isEmpty()
        {
            lock (this)
            {
                return base.isEmpty();
            }
        }
        public override Set<K> keySet()
        {
            lock (this)
            {
                return base.keySet();
            }
        }
        public override V put(K key, V value)
        {
            lock (this)
            {
                return base.put(key, value);
            }
        }
        public override void putAll(Map<K, V> map)
        {
            lock (this)
            {
                base.putAll(map);
            }
        }
        public override V remove(object key)
        {
            lock (this)
            {
                return base.remove(key);
            }
        }
        public override int size()
        {
            lock (this)
            {
                return base.size();
            }
        }
        public override Collection<V> values()
        {
            lock (this)
            {
                return base.values();
            }
        }
        public virtual Enumeration<K> keys()
        {
            lock (this)
            {
                return new dotnet.util.wrapper.EnumeratorWrapper<K>(this.Keys.AsEnumerable().GetEnumerator());
            }
        }
        public virtual Enumeration<V> elements()
        {
            lock (this)
            {
                return new dotnet.util.wrapper.EnumeratorWrapper<V>(this.Values.AsEnumerable());
            }
        }
    }
}
