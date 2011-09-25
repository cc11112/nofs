/*
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at 
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *  
 */

using System;
using java = biz.ritter.javapi;
using org.apache.commons.collections;
using org.apache.commons.collections.collection;

namespace org.apache.commons.collections.set
{

    /**
     * Decorates a set of other sets to provide a single unified view.
     * <p>
     * Changes made to this set will actually be made on the decorated set.
     * Add operations require the use of a pluggable strategy.
     * If no strategy is provided then add is unsupported.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Brian McCallister
     */
    public class CompositeSet : CompositeCollection, java.util.Set<Object>
    {
        /**
         * Create an empty CompositeSet
         */
        public CompositeSet()
            : base()
        {

        }

        /**
         * Create a CompositeSet with just <code>set</code> composited
         * @param set The initial set in the composite
         */
        public CompositeSet(java.util.Set<Object> set)
            : base(set)
        {
        }

        /**
         * Create a composite set with sets as the initial set of composited Sets
         */
        public CompositeSet(java.util.Set<Object>[] sets)
            : base(sets)
        {
        }

        /**
         * Add a Set to this composite
         *
         * @param c Must implement Set
         * @throws IllegalArgumentException if c does not implement java.util.Set
         *         or if a SetMutator is set, but fails to resolve a collision
         * @throws UnsupportedOperationException if there is no SetMutator set, or
         *         a CollectionMutator is set instead of a SetMutator
         * @see org.apache.commons.collections.collection.CompositeCollection.CollectionMutator
         * @see SetMutator
         */
        public override void addComposited(java.util.Collection<Object> c)
        {
            lock (this)
            {
                if (!(c is java.util.Set<Object>))
                {
                    throw new java.lang.IllegalArgumentException("Collections added must implement java.util.Set");
                }

                for (java.util.Iterator<Object> i = this.getCollections().iterator(); i.hasNext(); )
                {
                    java.util.Set<Object> set = (java.util.Set<Object>)i.next();
                    java.util.Collection<Object> intersects = CollectionUtils.intersection(set, c);
                    if (intersects.size() > 0)
                    {
                        if (this.mutator == null)
                        {
                            throw new java.lang.UnsupportedOperationException(
                                "Collision adding composited collection with no SetMutator set");
                        }
                        else if (!(this.mutator is SetMutator))
                        {
                            throw new java.lang.UnsupportedOperationException(
                                "Collision adding composited collection to a CompositeSet with a CollectionMutator instead of a SetMutator");
                        }
                        ((SetMutator)this.mutator).resolveCollision(this, set, (java.util.Set<Object>)c, intersects);
                        if (CollectionUtils.intersection(set, c).size() > 0)
                        {
                            throw new java.lang.IllegalArgumentException(
                                "Attempt to add illegal entry unresolved by SetMutator.resolveCollision()");
                        }
                    }
                }
                base.addComposited(new java.util.Collection<Object>[] { c });
            }
        }

        /**
         * Add two sets to this composite
         *
         * @throws IllegalArgumentException if c or d does not implement java.util.Set
         */
        public override void addComposited(java.util.Collection<Object> c, java.util.Collection<Object> d)
        {
            lock (this)
            {
                if (!(c is java.util.Set<Object>)) throw new java.lang.IllegalArgumentException("Argument must implement java.util.Set");
                if (!(d is java.util.Set<Object>)) throw new java.lang.IllegalArgumentException("Argument must implement java.util.Set");
                this.addComposited(new java.util.Set<Object>[] { (java.util.Set<Object>)c, (java.util.Set<Object>)d });
            }
        }

        /**
         * Add an array of sets to this composite
         * @param comps
         * @throws IllegalArgumentException if any of the collections in comps do not implement Set
         */
        public override void addComposited(java.util.Collection<Object>[] comps)
        {
            lock (this)
            {
                for (int i = comps.Length - 1; i >= 0; --i)
                {
                    this.addComposited(comps[i]);
                }
            }
        }

        /**
         * This can receive either a <code>CompositeCollection.CollectionMutator</code>
         * or a <code>CompositeSet.SetMutator</code>. If a
         * <code>CompositeCollection.CollectionMutator</code> is used than conflicts when adding
         * composited sets will throw IllegalArgumentException
         * <p>
         */
        public override void setMutator(CollectionMutator mutator)
        {
            base.setMutator(mutator);
        }

        /* Set operations */

        /**
         * If a <code>CollectionMutator</code> is defined for this CompositeSet then this
         * method will be called anyway.
         *
         * @param obj Object to be removed
         * @return true if the object is removed, false otherwise
         */
        public override bool remove(Object obj)
        {
            for (java.util.Iterator<Object> i = this.getCollections().iterator(); i.hasNext(); )
            {
                java.util.Set<Object> set = (java.util.Set<Object>)i.next();
                if (set.contains(obj)) return set.remove(obj);
            }
            return false;
        }


        /**
         * @see Set#equals
         */
        public override bool Equals(Object obj)
        {
            if (obj is java.util.Set<Object>)
            {
                java.util.Set<Object> set = (java.util.Set<Object>)obj;
                if (set.containsAll(this) && set.size() == this.size())
                {
                    return true;
                }
            }
            return false;
        }

        /**
         * @see Set#hashCode
         */
        public override int GetHashCode()
        {
            int code = 0;
            for (java.util.Iterator<Object> i = this.iterator(); i.hasNext(); )
            {
                Object next = i.next();
                code += (next != null ? next.GetHashCode() : 0);
            }
            return code;
        }

        /**
         * Define callbacks for mutation operations.
         * <p>
         * Defining remove() on implementations of SetMutator is pointless
         * as they are never called by CompositeSet.
         */
        public interface SetMutator : CompositeCollection.CollectionMutator
        {
            /**
             * <p>
             * Called when a Set is added to the CompositeSet and there is a
             * collision between existing and added sets.
             * </p>
             * <p>
             * If <code>added</code> and <code>existing</code> still have any intersects
             * after this method returns an IllegalArgumentException will be thrown.
             * </p>
             * @param comp The CompositeSet being modified
             * @param existing The Set already existing in the composite
             * @param added the Set being added to the composite
             * @param intersects the intersection of th existing and added sets
             */
            void resolveCollision(CompositeSet comp, java.util.Set<Object> existing, java.util.Set<Object> added, java.util.Collection<Object> intersects);
        }
    }
}