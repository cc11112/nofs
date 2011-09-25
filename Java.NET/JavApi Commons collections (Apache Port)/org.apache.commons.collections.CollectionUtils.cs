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
using org.apache.commons.collections.collection;

namespace org.apache.commons.collections
{

    /**
     * Provides utility methods and decorators for {@link Collection} instances.
     *
     * @since Commons Collections 1.0
     * @version $Revision$ $Date$
     * 
     * @author Rodney Waldhoff
     * @author Paul Jack
     * @author Stephen Colebourne
     * @author Steve Downey
     * @author Herve Quiroz
     * @author Peter KoBek
     * @author Matthew Hawthorne
     * @author Janek Bogucki
     * @author Phil Steitz
     * @author Steven Melzer
     * @author Jon Schewe
     * @author Neil O'Toole
     * @author Stephen Smith
     */
    public class CollectionUtils
    {

        /** Constant to avoid repeated object creation */
        private static java.lang.Integer INTEGER_ONE = new java.lang.Integer(1);

        /**
         * An empty unmodifiable collection.
         * The JDK provides empty Set and List implementations which could be used for
         * this purpose. However they could be cast to Set or List which might be
         * undesirable. This implementation only implements Collection.
         */
        public static readonly java.util.Collection<Object> EMPTY_COLLECTION = UnmodifiableCollection.decorate(new java.util.ArrayList<Object>());

        /**
         * <code>CollectionUtils</code> should not normally be instantiated.
         */
        public CollectionUtils()
        {
        }

        /**
         * Returns a {@link Collection} containing the union
         * of the given {@link Collection}s.
         * <p>
         * The cardinality of each element in the returned {@link Collection}
         * will be equal to the maximum of the cardinality of that element
         * in the two given {@link Collection}s.
         *
         * @param a  the first collection, must not be null
         * @param b  the second collection, must not be null
         * @return  the union of the two collections
         * @see Collection#addAll
         */
        public static java.util.Collection<Object> union(java.util.Collection<Object> a, java.util.Collection<Object> b)
        {
            java.util.ArrayList<Object> list = new java.util.ArrayList<Object>();
            java.util.Map<Object, Object> mapa = getCardinalityMap(a);
            java.util.Map<Object, Object> mapb = getCardinalityMap(b);
            java.util.Set<Object> elts = new java.util.HashSet<Object>(a);
            elts.addAll(b);
            java.util.Iterator<Object> it = elts.iterator();
            while (it.hasNext())
            {
                Object obj = it.next();
                for (int i = 0, m = java.lang.Math.max(getFreq(obj, mapa), getFreq(obj, mapb)); i < m; i++)
                {
                    list.add(obj);
                }
            }
            return list;
        }

        /**
         * Returns a {@link Collection} containing the intersection
         * of the given {@link Collection}s.
         * <p>
         * The cardinality of each element in the returned {@link Collection}
         * will be equal to the minimum of the cardinality of that element
         * in the two given {@link Collection}s.
         *
         * @param a  the first collection, must not be null
         * @param b  the second collection, must not be null
         * @return the intersection of the two collections
         * @see Collection#retainAll
         * @see #containsAny
         */
        public static java.util.Collection<Object> intersection(java.util.Collection<Object> a, java.util.Collection<Object> b)
        {
            java.util.ArrayList<Object> list = new java.util.ArrayList<Object>();
            java.util.Map<Object, Object> mapa = getCardinalityMap(a);
            java.util.Map<Object, Object> mapb = getCardinalityMap(b);
            java.util.Set<Object> elts = new java.util.HashSet<Object>(a);
            elts.addAll(b);
            java.util.Iterator<Object> it = elts.iterator();
            while (it.hasNext())
            {
                Object obj = it.next();
                for (int i = 0, m = java.lang.Math.min(getFreq(obj, mapa), getFreq(obj, mapb)); i < m; i++)
                {
                    list.add(obj);
                }
            }
            return list;
        }

        /**
         * Returns a {@link Collection} containing the exclusive disjunction
         * (symmetric difference) of the given {@link Collection}s.
         * <p>
         * The cardinality of each element <i>e</i> in the returned {@link Collection}
         * will be equal to
         * <tt>max(cardinality(<i>e</i>,<i>a</i>),cardinality(<i>e</i>,<i>b</i>)) - min(cardinality(<i>e</i>,<i>a</i>),cardinality(<i>e</i>,<i>b</i>))</tt>.
         * <p>
         * This is equivalent to
         * <tt>{@link #subtract subtract}({@link #union union(a,b)},{@link #intersection intersection(a,b)})</tt>
         * or
         * <tt>{@link #union union}({@link #subtract subtract(a,b)},{@link #subtract subtract(b,a)})</tt>.
         *
         * @param a  the first collection, must not be null
         * @param b  the second collection, must not be null
         * @return the symmetric difference of the two collections
         */
        public static java.util.Collection<Object> disjunction(java.util.Collection<Object> a, java.util.Collection<Object> b)
        {
            java.util.ArrayList<Object> list = new java.util.ArrayList<Object>();
            java.util.Map<Object, Object> mapa = getCardinalityMap(a);
            java.util.Map<Object, Object> mapb = getCardinalityMap(b);
            java.util.Set<Object> elts = new java.util.HashSet<Object>(a);
            elts.addAll(b);
            java.util.Iterator<Object> it = elts.iterator();
            while (it.hasNext())
            {
                Object obj = it.next();
                for (int i = 0, m = ((java.lang.Math.max(getFreq(obj, mapa), getFreq(obj, mapb))) - (java.lang.Math.min(getFreq(obj, mapa), getFreq(obj, mapb)))); i < m; i++)
                {
                    list.add(obj);
                }
            }
            return list;
        }

        /**
         * Returns a new {@link Collection} containing <tt><i>a</i> - <i>b</i></tt>.
         * The cardinality of each element <i>e</i> in the returned {@link Collection}
         * will be the cardinality of <i>e</i> in <i>a</i> minus the cardinality
         * of <i>e</i> in <i>b</i>, or zero, whichever is greater.
         *
         * @param a  the collection to subtract from, must not be null
         * @param b  the collection to subtract, must not be null
         * @return a new collection with the results
         * @see Collection#removeAll
         */
        public static java.util.Collection<Object> subtract(java.util.Collection<Object> a, java.util.Collection<Object> b)
        {
            java.util.ArrayList<Object> list = new java.util.ArrayList<Object>(a);
            for (java.util.Iterator<Object> it = b.iterator(); it.hasNext(); )
            {
                list.remove(it.next());
            }
            return list;
        }

        /**
         * Returns <code>true</code> iff at least one element is in both collections.
         * <p>
         * In other words, this method returns <code>true</code> iff the
         * {@link #intersection} of <i>coll1</i> and <i>coll2</i> is not empty.
         * 
         * @param coll1  the first collection, must not be null
         * @param coll2  the first collection, must not be null
         * @return <code>true</code> iff the intersection of the collections is non-empty
         * @since 2.1
         * @see #intersection
         */
        public static bool containsAny(java.util.Collection<Object> coll1, java.util.Collection<Object> coll2)
        {
            if (coll1.size() < coll2.size())
            {
                for (java.util.Iterator<Object> it = coll1.iterator(); it.hasNext(); )
                {
                    if (coll2.contains(it.next()))
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (java.util.Iterator<Object> it = coll2.iterator(); it.hasNext(); )
                {
                    if (coll1.contains(it.next()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /**
         * Returns a {@link Map} mapping each unique element in the given
         * {@link Collection} to an {@link Integer} representing the number
         * of occurrences of that element in the {@link Collection}.
         * <p>
         * Only those elements present in the collection will appear as
         * keys in the map.
         * 
         * @param coll  the collection to get the cardinality map for, must not be null
         * @return the populated cardinality map
         */
        public static java.util.Map<Object, Object> getCardinalityMap(java.util.Collection<Object> coll)
        {
            java.util.Map<Object, Object> count = new java.util.HashMap<Object, Object>();
            for (java.util.Iterator<Object> it = coll.iterator(); it.hasNext(); )
            {
                Object obj = it.next();
                java.lang.Integer c = (java.lang.Integer)(count.get(obj));
                if (c == null)
                {
                    count.put(obj, INTEGER_ONE);
                }
                else
                {
                    count.put(obj, new java.lang.Integer(c.intValue() + 1));
                }
            }
            return count;
        }

        /**
         * Returns <tt>true</tt> iff <i>a</i> is a sub-collection of <i>b</i>,
         * that is, iff the cardinality of <i>e</i> in <i>a</i> is less
         * than or equal to the cardinality of <i>e</i> in <i>b</i>,
         * for each element <i>e</i> in <i>a</i>.
         *
         * @param a  the first (sub?) collection, must not be null
         * @param b  the second (super?) collection, must not be null
         * @return <code>true</code> iff <i>a</i> is a sub-collection of <i>b</i>
         * @see #isProperSubCollection
         * @see Collection#containsAll
         */
        public static bool isSubCollection(java.util.Collection<Object> a, java.util.Collection<Object> b)
        {
            java.util.Map<Object, Object> mapa = getCardinalityMap(a);
            java.util.Map<Object, Object> mapb = getCardinalityMap(b);
            java.util.Iterator<Object> it = a.iterator();
            while (it.hasNext())
            {
                Object obj = it.next();
                if (getFreq(obj, mapa) > getFreq(obj, mapb))
                {
                    return false;
                }
            }
            return true;
        }

        /**
         * Returns <tt>true</tt> iff <i>a</i> is a <i>proper</i> sub-collection of <i>b</i>,
         * that is, iff the cardinality of <i>e</i> in <i>a</i> is less
         * than or equal to the cardinality of <i>e</i> in <i>b</i>,
         * for each element <i>e</i> in <i>a</i>, and there is at least one
         * element <i>f</i> such that the cardinality of <i>f</i> in <i>b</i>
         * is strictly greater than the cardinality of <i>f</i> in <i>a</i>.
         * <p>
         * The implementation assumes
         * <ul>
         *    <li><code>a.size()</code> and <code>b.size()</code> represent the 
         *    total cardinality of <i>a</i> and <i>b</i>, resp. </li>
         *    <li><code>a.size() < Integer.MAXVALUE</code></li>
         * </ul>
         *
         * @param a  the first (sub?) collection, must not be null
         * @param b  the second (super?) collection, must not be null
         * @return <code>true</code> iff <i>a</i> is a <i>proper</i> sub-collection of <i>b</i>
         * @see #isSubCollection
         * @see Collection#containsAll
         */
        public static bool isProperSubCollection(java.util.Collection<Object> a, java.util.Collection<Object> b)
        {
            return (a.size() < b.size()) && CollectionUtils.isSubCollection(a, b);
        }

        /**
         * Returns <tt>true</tt> iff the given {@link Collection}s contain
         * exactly the same elements with exactly the same cardinalities.
         * <p>
         * That is, iff the cardinality of <i>e</i> in <i>a</i> is
         * equal to the cardinality of <i>e</i> in <i>b</i>,
         * for each element <i>e</i> in <i>a</i> or <i>b</i>.
         *
         * @param a  the first collection, must not be null
         * @param b  the second collection, must not be null
         * @return <code>true</code> iff the collections contain the same elements with the same cardinalities.
         */
        public static bool isEqualCollection(java.util.Collection<Object> a, java.util.Collection<Object> b)
        {
            if (a.size() != b.size())
            {
                return false;
            }
            else
            {
                java.util.Map<Object, Object> mapa = getCardinalityMap(a);
                java.util.Map<Object, Object> mapb = getCardinalityMap(b);
                if (mapa.size() != mapb.size())
                {
                    return false;
                }
                else
                {
                    java.util.Iterator<Object> it = mapa.keySet().iterator();
                    while (it.hasNext())
                    {
                        Object obj = it.next();
                        if (getFreq(obj, mapa) != getFreq(obj, mapb))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }

        /**
         * Returns the number of occurrences of <i>obj</i> in <i>coll</i>.
         *
         * @param obj  the object to find the cardinality of
         * @param coll  the collection to search
         * @return the the number of occurrences of obj in coll
         */
        public static int cardinality(Object obj, java.util.Collection<Object> coll)
        {
            if (coll is java.util.Set<Object>)
            {
                return (coll.contains(obj) ? 1 : 0);
            }
            if (coll is Bag)
            {
                return ((Bag)coll).getCount(obj);
            }
            int count = 0;
            if (obj == null)
            {
                for (java.util.Iterator<Object> it = coll.iterator(); it.hasNext(); )
                {
                    if (it.next() == null)
                    {
                        count++;
                    }
                }
            }
            else
            {
                for (java.util.Iterator<Object> it = coll.iterator(); it.hasNext(); )
                {
                    if (obj.equals(it.next()))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /** 
         * Finds the first element in the given collection which matches the given predicate.
         * <p>
         * If the input collection or predicate is null, or no element of the collection 
         * matches the predicate, null is returned.
         *
         * @param collection  the collection to search, may be null
         * @param predicate  the predicate to use, may be null
         * @return the first element of the collection which matches the predicate or null if none could be found
         */
        public static Object find(java.util.Collection<Object> collection, Predicate predicate)
        {
            if (collection != null && predicate != null)
            {
                for (java.util.Iterator<Object> iter = collection.iterator(); iter.hasNext(); )
                {
                    Object item = iter.next();
                    if (predicate.evaluate(item))
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        /** 
         * Executes the given closure on each element in the collection.
         * <p>
         * If the input collection or closure is null, there is no change made.
         * 
         * @param collection  the collection to get the input from, may be null
         * @param closure  the closure to perform, may be null
         */
        public static void forAllDo(java.util.Collection<Object> collection, Closure closure)
        {
            if (collection != null && closure != null)
            {
                for (java.util.Iterator<Object> it = collection.iterator(); it.hasNext(); )
                {
                    closure.execute(it.next());
                }
            }
        }

        /** 
         * Filter the collection by applying a Predicate to each element. If the
         * predicate returns false, remove the element.
         * <p>
         * If the input collection or predicate is null, there is no change made.
         * 
         * @param collection  the collection to get the input from, may be null
         * @param predicate  the predicate to use as a filter, may be null
         */
        public static void filter(java.util.Collection<Object> collection, Predicate predicate)
        {
            if (collection != null && predicate != null)
            {
                for (java.util.Iterator<Object> it = collection.iterator(); it.hasNext(); )
                {
                    if (predicate.evaluate(it.next()) == false)
                    {
                        it.remove();
                    }
                }
            }
        }

        /** 
         * Transform the collection by applying a Transformer to each element.
         * <p>
         * If the input collection or transformer is null, there is no change made.
         * <p>
         * This routine is best for Lists, for which set() is used to do the 
         * transformations "in place."  For other Collections, clear() and addAll()
         * are used to replace elements.  
         * <p>
         * If the input collection controls its input, such as a Set, and the
         * Transformer creates duplicates (or are otherwise invalid), the 
         * collection may reduce in size due to calling this method.
         * 
         * @param collection  the collection to get the input from, may be null
         * @param transformer  the transformer to perform, may be null
         */
        public static void transform(java.util.Collection<Object> collection, Transformer transformer)
        {
            if (collection != null && transformer != null)
            {
                if (collection is java.util.List<Object>)
                {
                    java.util.List<Object> list = (java.util.List<Object>)collection;
                    for (java.util.ListIterator<Object> it = list.listIterator(); it.hasNext(); )
                    {
                        it.set(transformer.transform(it.next()));
                    }
                }
                else
                {
                    java.util.Collection<Object> resultCollection = collect(collection, transformer);
                    collection.clear();
                    collection.addAll(resultCollection);
                }
            }
        }

        /** 
         * Counts the number of elements in the input collection that match the predicate.
         * <p>
         * A <code>null</code> collection or predicate matches no elements.
         * 
         * @param inputCollection  the collection to get the input from, may be null
         * @param predicate  the predicate to use, may be null
         * @return the number of matches for the predicate in the collection
         */
        public static int countMatches(java.util.Collection<Object> inputCollection, Predicate predicate)
        {
            int count = 0;
            if (inputCollection != null && predicate != null)
            {
                for (java.util.Iterator<Object> it = inputCollection.iterator(); it.hasNext(); )
                {
                    if (predicate.evaluate(it.next()))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /** 
         * Answers true if a predicate is true for at least one element of a collection.
         * <p>
         * A <code>null</code> collection or predicate returns false.
         * 
         * @param collection the collection to get the input from, may be null
         * @param predicate the predicate to use, may be null
         * @return true if at least one element of the collection matches the predicate
         */
        public static bool exists(java.util.Collection<Object> collection, Predicate predicate)
        {
            if (collection != null && predicate != null)
            {
                for (java.util.Iterator<Object> it = collection.iterator(); it.hasNext(); )
                {
                    if (predicate.evaluate(it.next()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /** 
         * Selects all elements from input collection which match the given predicate
         * into an output collection.
         * <p>
         * A <code>null</code> predicate matches no elements.
         * 
         * @param inputCollection  the collection to get the input from, may not be null
         * @param predicate  the predicate to use, may be null
         * @return the elements matching the predicate (new list)
         * @throws NullPointerException if the input collection is null
         */
        public static java.util.Collection<Object> select(java.util.Collection<Object> inputCollection, Predicate predicate)
        {
            java.util.ArrayList<Object> answer = new java.util.ArrayList<Object>(inputCollection.size());
            select(inputCollection, predicate, answer);
            return answer;
        }

        /** 
         * Selects all elements from input collection which match the given predicate
         * and adds them to outputCollection.
         * <p>
         * If the input collection or predicate is null, there is no change to the 
         * output collection.
         * 
         * @param inputCollection  the collection to get the input from, may be null
         * @param predicate  the predicate to use, may be null
         * @param outputCollection  the collection to output into, may not be null
         */
        public static void select(java.util.Collection<Object> inputCollection, Predicate predicate, java.util.Collection<Object> outputCollection)
        {
            if (inputCollection != null && predicate != null)
            {
                for (java.util.Iterator<Object> iter = inputCollection.iterator(); iter.hasNext(); )
                {
                    Object item = iter.next();
                    if (predicate.evaluate(item))
                    {
                        outputCollection.add(item);
                    }
                }
            }
        }

        /**
         * Selects all elements from inputCollection which don't match the given predicate
         * into an output collection.
         * <p>
         * If the input predicate is <code>null</code>, the result is an empty list.
         * 
         * @param inputCollection  the collection to get the input from, may not be null
         * @param predicate  the predicate to use, may be null
         * @return the elements <b>not</b> matching the predicate (new list)
         * @throws NullPointerException if the input collection is null
         */
        public static java.util.Collection<Object> selectRejected(java.util.Collection<Object> inputCollection, Predicate predicate)
        {
            java.util.ArrayList<Object> answer = new java.util.ArrayList<Object>(inputCollection.size());
            selectRejected(inputCollection, predicate, answer);
            return answer;
        }

        /** 
         * Selects all elements from inputCollection which don't match the given predicate
         * and adds them to outputCollection.
         * <p>
         * If the input predicate is <code>null</code>, no elements are added to <code>outputCollection</code>.
         * 
         * @param inputCollection  the collection to get the input from, may be null
         * @param predicate  the predicate to use, may be null
         * @param outputCollection  the collection to output into, may not be null
         */
        public static void selectRejected(java.util.Collection<Object> inputCollection, Predicate predicate, java.util.Collection<Object> outputCollection)
        {
            if (inputCollection != null && predicate != null)
            {
                for (java.util.Iterator<Object> iter = inputCollection.iterator(); iter.hasNext(); )
                {
                    Object item = iter.next();
                    if (predicate.evaluate(item) == false)
                    {
                        outputCollection.add(item);
                    }
                }
            }
        }

        /** 
         * Returns a new Collection consisting of the elements of inputCollection transformed
         * by the given transformer.
         * <p>
         * If the input transformer is null, the result is an empty list.
         * 
         * @param inputCollection  the collection to get the input from, may not be null
         * @param transformer  the transformer to use, may be null
         * @return the transformed result (new list)
         * @throws NullPointerException if the input collection is null
         */
        public static java.util.Collection<Object> collect(java.util.Collection<Object> inputCollection, Transformer transformer)
        {
            java.util.ArrayList<Object> answer = new java.util.ArrayList<Object>(inputCollection.size());
            collect(inputCollection, transformer, answer);
            return answer;
        }

        /** 
         * Transforms all elements from the inputIterator with the given transformer 
         * and adds them to the outputCollection.
         * <p>
         * If the input iterator or transformer is null, the result is an empty list.
         * 
         * @param inputIterator  the iterator to get the input from, may be null
         * @param transformer  the transformer to use, may be null
         * @return the transformed result (new list)
         */
        public static java.util.Collection<Object> collect(java.util.Iterator<Object> inputIterator, Transformer transformer)
        {
            java.util.ArrayList<Object> answer = new java.util.ArrayList<Object>();
            collect(inputIterator, transformer, answer);
            return answer;
        }

        /** 
         * Transforms all elements from inputCollection with the given transformer 
         * and adds them to the outputCollection.
         * <p>
         * If the input collection or transformer is null, there is no change to the 
         * output collection.
         *
         * @param inputCollection  the collection to get the input from, may be null
         * @param transformer  the transformer to use, may be null
         * @param outputCollection  the collection to output into, may not be null
         * @return the outputCollection with the transformed input added
         * @throws NullPointerException if the output collection is null
         */
        public static java.util.Collection<Object> collect(java.util.Collection<Object> inputCollection, Transformer transformer, java.util.Collection<Object> outputCollection)
        {
            if (inputCollection != null)
            {
                return collect(inputCollection.iterator(), transformer, outputCollection);
            }
            return outputCollection;
        }

        /** 
         * Transforms all elements from the inputIterator with the given transformer 
         * and adds them to the outputCollection.
         * <p>
         * If the input iterator or transformer is null, there is no change to the 
         * output collection.
         *
         * @param inputIterator  the iterator to get the input from, may be null
         * @param transformer  the transformer to use, may be null
         * @param outputCollection  the collection to output into, may not be null
         * @return the outputCollection with the transformed input added
         * @throws NullPointerException if the output collection is null
         */
        public static java.util.Collection<Object> collect(java.util.Iterator<Object> inputIterator, Transformer transformer, java.util.Collection<Object> outputCollection)
        {
            if (inputIterator != null && transformer != null)
            {
                while (inputIterator.hasNext())
                {
                    Object item = inputIterator.next();
                    Object value = transformer.transform(item);
                    outputCollection.add(value);
                }
            }
            return outputCollection;
        }

        //-----------------------------------------------------------------------
        /**
         * Adds an element to the collection unless the element is null.
         * 
         * @param collection  the collection to add to, must not be null
         * @param object  the object to add, if null it will not be added
         * @return true if the collection changed
         * @throws NullPointerException if the collection is null
         * @since Commons Collections 3.2
         */
        public static bool addIgnoreNull(java.util.Collection<Object> collection, Object obj)
        {
            return (obj == null ? false : collection.add(obj));
        }

        /**
         * Adds all elements in the iteration to the given collection.
         * 
         * @param collection  the collection to add to, must not be null
         * @param iterator  the iterator of elements to add, must not be null
         * @throws NullPointerException if the collection or iterator is null
         */
        public static void addAll(java.util.Collection<Object> collection, java.util.Iterator<Object> iterator)
        {
            while (iterator.hasNext())
            {
                collection.add(iterator.next());
            }
        }

        /**
         * Adds all elements in the enumeration to the given collection.
         * 
         * @param collection  the collection to add to, must not be null
         * @param enumeration  the enumeration of elements to add, must not be null
         * @throws NullPointerException if the collection or enumeration is null
         */
        public static void addAll(java.util.Collection<Object> collection, java.util.Enumeration<Object> enumeration)
        {
            while (enumeration.hasMoreElements())
            {
                collection.add(enumeration.nextElement());
            }
        }

        /** 
         * Adds all elements in the array to the given collection.
         * 
         * @param collection  the collection to add to, must not be null
         * @param elements  the array of elements to add, must not be null
         * @throws NullPointerException if the collection or array is null
         */
        public static void addAll(java.util.Collection<Object> collection, Object[] elements)
        {
            for (int i = 0, size = elements.Length; i < size; i++)
            {
                collection.add(elements[i]);
            }
        }

        /**
         * Given an Object, and an index, returns the nth value in the
         * object.
         * <ul>
         * <li>If obj is a Map, returns the nth value from the <b>keySet</b> iterator, unless 
         *     the Map contains an Integer key with integer value = idx, in which case the
         *     corresponding map entry value is returned.  If idx exceeds the number of entries in
         *     the map, an empty Iterator is returned.
         * <li>If obj is a List or an array, returns the nth value, throwing IndexOutOfBoundsException,
         *     ArrayIndexOutOfBoundsException, resp. if the nth value does not exist.
         * <li>If obj is an iterator, enumeration or Collection, returns the nth value from the iterator,
         *     returning an empty Iterator (resp. Enumeration) if the nth value does not exist.
         * <li>Returns the original obj if it is null or not a Collection or Iterator.
         * </ul>
         * 
         * @param obj  the object to get an index of, may be null
         * @param idx  the index to get
         * @throws IndexOutOfBoundsException
         * @throws ArrayIndexOutOfBoundsException
         *
         * @deprecated use {@link #get(Object, int)} instead. Will be removed in v4.0
         */
        public static Object index(Object obj, int idx)
        {
            return index(obj, new java.lang.Integer(idx));
        }

        /**
         * Given an Object, and a key (index), returns the value associated with
         * that key in the Object. The following checks are made:
         * <ul>
         * <li>If obj is a Map, use the index as a key to get a value. If no match continue.
         * <li>Check key is an Integer. If not, return the object passed in.
         * <li>If obj is a Map, get the nth value from the <b>keySet</b> iterator.
         *     If the Map has fewer than n entries, return an empty Iterator.
         * <li>If obj is a List or an array, get the nth value, throwing IndexOutOfBoundsException,
         *     ArrayIndexOutOfBoundsException, resp. if the nth value does not exist.
         * <li>If obj is an iterator, enumeration or Collection, get the nth value from the iterator,
         *     returning an empty Iterator (resp. Enumeration) if the nth value does not exist.
         * <li>Return the original obj.
         * </ul>
         * 
         * @param obj  the object to get an index of
         * @param index  the index to get
         * @return the object at the specified index
         * @throws IndexOutOfBoundsException
         * @throws ArrayIndexOutOfBoundsException
         *
         * @deprecated use {@link #get(Object, int)} instead. Will be removed in v4.0
         */
        public static Object index(Object obj, Object indexJ)
        {
            if (obj is java.util.Map<Object, Object>)
            {
                java.util.Map<Object, Object> map = (java.util.Map<Object, Object>)obj;
                if (map.containsKey(indexJ))
                {
                    return map.get(indexJ);
                }
            }
            int idx = -1;
            if (indexJ is java.lang.Integer)
            {
                idx = ((java.lang.Integer)indexJ).intValue();
            }
            if (idx < 0)
            {
                return obj;
            }
            else if (obj is java.util.Map<Object, Object>)
            {
                java.util.Map<Object, Object> map = (java.util.Map<Object, Object>)obj;
                java.util.Iterator<Object> iterator = map.keySet().iterator();
                return index(iterator, idx);
            }
            else if (obj is java.util.List<Object>)
            {
                return ((java.util.List<Object>)obj).get(idx);
            }
            else if (obj is Object[])
            {
                return ((Object[])obj)[idx];
            }
            else if (obj is java.util.Enumeration<Object>)
            {
                java.util.Enumeration<Object> it = (java.util.Enumeration<Object>)obj;
                while (it.hasMoreElements())
                {
                    idx--;
                    if (idx == -1)
                    {
                        return it.nextElement();
                    }
                    else
                    {
                        it.nextElement();
                    }
                }
            }
            else if (obj is java.util.Iterator<Object>)
            {
                return index((java.util.Iterator<Object>)obj, idx);
            }
            else if (obj is java.util.Collection<Object>)
            {
                java.util.Iterator<Object> iterator = ((java.util.Collection<Object>)obj).iterator();
                return index(iterator, idx);
            }
            return obj;
        }

        private static Object index(java.util.Iterator<Object> iterator, int idx)
        {
            while (iterator.hasNext())
            {
                idx--;
                if (idx == -1)
                {
                    return iterator.next();
                }
                else
                {
                    iterator.next();
                }
            }
            return iterator;
        }

        /**
         * Returns the <code>index</code>-th value in <code>object</code>, throwing
         * <code>IndexOutOfBoundsException</code> if there is no such element or 
         * <code>IllegalArgumentException</code> if <code>object</code> is not an 
         * instance of one of the supported types.
         * <p>
         * The supported types, and associated semantics are:
         * <ul>
         * <li> Map -- the value returned is the <code>Map.Entry</code> in position 
         *      <code>index</code> in the map's <code>entrySet</code> iterator, 
         *      if there is such an entry.</li>
         * <li> List -- this method is equivalent to the list's get method.</li>
         * <li> Array -- the <code>index</code>-th array entry is returned, 
         *      if there is such an entry; otherwise an <code>IndexOutOfBoundsException</code>
         *      is thrown.</li>
         * <li> Collection -- the value returned is the <code>index</code>-th object 
         *      returned by the collection's default iterator, if there is such an element.</li>
         * <li> Iterator or Enumeration -- the value returned is the
         *      <code>index</code>-th object in the Iterator/Enumeration, if there
         *      is such an element.  The Iterator/Enumeration is advanced to 
         *      <code>index</code> (or to the end, if <code>index</code> exceeds the 
         *      number of entries) as a side effect of this method.</li>
         * </ul>
         * 
         * @param object  the object to get a value from
         * @param index  the index to get
         * @return the object at the specified index
         * @throws IndexOutOfBoundsException if the index is invalid
         * @throws IllegalArgumentException if the object type is invalid
         */
        public static Object get(Object obj, int index)
        {
            if (index < 0)
            {
                throw new java.lang.IndexOutOfBoundsException("Index cannot be negative: " + index);
            }
            if (obj is java.util.Map<Object, Object>)
            {
                java.util.Map<Object, Object> map = (java.util.Map<Object, Object>)obj;
                java.util.Iterator<Object> iterator = (java.util.Iterator<Object>)map.entrySet().iterator();
                return get(iterator, index);
            }
            else if (obj is java.util.List<Object>)
            {
                return ((java.util.List<Object>)obj).get(index);
            }
            else if (obj is Object[])
            {
                return ((Object[])obj)[index];
            }
            else if (obj is java.util.Iterator<Object>)
            {
                java.util.Iterator<Object> it = (java.util.Iterator<Object>)obj;
                while (it.hasNext())
                {
                    index--;
                    if (index == -1)
                    {
                        return it.next();
                    }
                    else
                    {
                        it.next();
                    }
                }
                throw new java.lang.IndexOutOfBoundsException("Entry does not exist: " + index);
            }
            else if (obj is java.util.Collection<Object>)
            {
                java.util.Iterator<Object> iterator = ((java.util.Collection<Object>)obj).iterator();
                return get(iterator, index);
            }
            else if (obj is java.util.Enumeration<Object>)
            {
                java.util.Enumeration<Object> it = (java.util.Enumeration<Object>)obj;
                while (it.hasMoreElements())
                {
                    index--;
                    if (index == -1)
                    {
                        return it.nextElement();
                    }
                    else
                    {
                        it.nextElement();
                    }
                }
                throw new java.lang.IndexOutOfBoundsException("Entry does not exist: " + index);
            }
            else if (obj == null)
            {
                throw new java.lang.IllegalArgumentException("Unsupported object type: null");
            }
            else
            {
                try
                {
                    return java.lang.reflect.Array.get(obj, index);
                }
                catch (java.lang.IllegalArgumentException ex)
                {
                    throw new java.lang.IllegalArgumentException("Unsupported object type: " + obj.getClass().getName());
                }
            }
        }

        /** 
         * Gets the size of the collection/iterator specified.
         * <p>
         * This method can handles objects as follows
         * <ul>
         * <li>Collection - the collection size
         * <li>Map - the map size
         * <li>Array - the array size
         * <li>Iterator - the number of elements remaining in the iterator
         * <li>Enumeration - the number of elements remaining in the enumeration
         * </ul>
         * 
         * @param object  the object to get the size of
         * @return the size of the specified collection
         * @throws IllegalArgumentException thrown if object is not recognised or null
         * @since Commons Collections 3.1
         */
        public static int size(Object obj)
        {
            int total = 0;
            if (obj is java.util.Map<Object, Object>)
            {
                total = ((java.util.Map<Object, Object>)obj).size();
            }
            else if (obj is java.util.Collection<Object>)
            {
                total = ((java.util.Collection<Object>)obj).size();
            }
            else if (obj is Object[])
            {
                total = ((Object[])obj).Length;
            }
            else if (obj is java.util.Iterator<Object>)
            {
                java.util.Iterator<Object> it = (java.util.Iterator<Object>)obj;
                while (it.hasNext())
                {
                    total++;
                    it.next();
                }
            }
            else if (obj is java.util.Enumeration<Object>)
            {
                java.util.Enumeration<Object> it = (java.util.Enumeration<Object>)obj;
                while (it.hasMoreElements())
                {
                    total++;
                    it.nextElement();
                }
            }
            else if (obj == null)
            {
                throw new java.lang.IllegalArgumentException("Unsupported object type: null");
            }
            else
            {
                try
                {
                    total = java.lang.reflect.Array.getLength(obj);
                }
                catch (java.lang.IllegalArgumentException ex)
                {
                    throw new java.lang.IllegalArgumentException("Unsupported object type: " + obj.getClass().getName());
                }
            }
            return total;
        }

        /**
         * Checks if the specified collection/array/iterator is empty.
         * <p>
         * This method can handles objects as follows
         * <ul>
         * <li>Collection - via collection isEmpty
         * <li>Map - via map isEmpty
         * <li>Array - using array size
         * <li>Iterator - via hasNext
         * <li>Enumeration - via hasMoreElements
         * </ul>
         * <p>
         * Note: This method is named to avoid clashing with
         * {@link #isEmpty(Collection)}.
         * 
         * @param object  the object to get the size of, not null
         * @return true if empty
         * @throws IllegalArgumentException thrown if object is not recognised or null
         * @since Commons Collections 3.2
         */
        public static bool sizeIsEmpty(Object obj)
        {
            if (obj is java.util.Collection<Object>)
            {
                return ((java.util.Collection<Object>)obj).isEmpty();
            }
            else if (obj is java.util.Map<Object, Object>)
            {
                return ((java.util.Map<Object, Object>)obj).isEmpty();
            }
            else if (obj is Object[])
            {
                return ((Object[])obj).Length == 0;
            }
            else if (obj is java.util.Iterator<Object>)
            {
                return ((java.util.Iterator<Object>)obj).hasNext() == false;
            }
            else if (obj is java.util.Enumeration<Object>)
            {
                return ((java.util.Enumeration<Object>)obj).hasMoreElements() == false;
            }
            else if (obj == null)
            {
                throw new java.lang.IllegalArgumentException("Unsupported object type: null");
            }
            else
            {
                try
                {
                    return java.lang.reflect.Array.getLength(obj) == 0;
                }
                catch (java.lang.IllegalArgumentException ex)
                {
                    throw new java.lang.IllegalArgumentException("Unsupported object type: " + obj.getClass().getName());
                }
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Null-safe check if the specified collection is empty.
         * <p>
         * Null returns true.
         * 
         * @param coll  the collection to check, may be null
         * @return true if empty or null
         * @since Commons Collections 3.2
         */
        public static bool isEmpty(java.util.Collection<Object> coll)
        {
            return (coll == null || coll.isEmpty());
        }

        /**
         * Null-safe check if the specified collection is not empty.
         * <p>
         * Null returns false.
         * 
         * @param coll  the collection to check, may be null
         * @return true if non-null and non-empty
         * @since Commons Collections 3.2
         */
        public static bool isNotEmpty(java.util.Collection<Object> coll)
        {
            return !CollectionUtils.isEmpty(coll);
        }

        //-----------------------------------------------------------------------
        /**
         * Reverses the order of the given array.
         * 
         * @param array  the array to reverse
         */
        public static void reverseArray(Object[] array)
        {
            int i = 0;
            int j = array.Length - 1;
            Object tmp;

            while (j > i)
            {
                tmp = array[j];
                array[j] = array[i];
                array[i] = tmp;
                j--;
                i++;
            }
        }

        private static int getFreq(Object obj, java.util.Map<Object, Object> freqMap)
        {
            java.lang.Integer count = (java.lang.Integer)freqMap.get(obj);
            if (count != null)
            {
                return count.intValue();
            }
            return 0;
        }

        /**
         * Returns true if no more elements can be added to the Collection.
         * <p>
         * This method uses the {@link BoundedCollection} interface to determine the
         * full status. If the collection does not implement this interface then
         * false is returned.
         * <p>
         * The collection does not have to implement this interface directly.
         * If the collection has been decorated using the decorators subpackage
         * then these will be removed to access the BoundedCollection.
         *
         * @param coll  the collection to check
         * @return true if the BoundedCollection is full
         * @throws NullPointerException if the collection is null
         */
        public static bool isFull(java.util.Collection<Object> coll)
        {
            if (coll == null)
            {
                throw new java.lang.NullPointerException("The collection must not be null");
            }
            if (coll is BoundedCollection)
            {
                return ((BoundedCollection)coll).isFull();
            }
            try
            {
                BoundedCollection bcoll = UnmodifiableBoundedCollection.decorateUsing(coll);
                return bcoll.isFull();

            }
            catch (java.lang.IllegalArgumentException ex)
            {
                return false;
            }
        }

        /**
         * Get the maximum number of elements that the Collection can contain.
         * <p>
         * This method uses the {@link BoundedCollection} interface to determine the
         * maximum size. If the collection does not implement this interface then
         * -1 is returned.
         * <p>
         * The collection does not have to implement this interface directly.
         * If the collection has been decorated using the decorators subpackage
         * then these will be removed to access the BoundedCollection.
         *
         * @param coll  the collection to check
         * @return the maximum size of the BoundedCollection, -1 if no maximum size
         * @throws NullPointerException if the collection is null
         */
        public static int maxSize(java.util.Collection<Object> coll)
        {
            if (coll == null)
            {
                throw new java.lang.NullPointerException("The collection must not be null");
            }
            if (coll is BoundedCollection)
            {
                return ((BoundedCollection)coll).maxSize();
            }
            try
            {
                BoundedCollection bcoll = UnmodifiableBoundedCollection.decorateUsing(coll);
                return bcoll.maxSize();

            }
            catch (java.lang.IllegalArgumentException ex)
            {
                return -1;
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Returns a collection containing all the elements in <code>collection</code>
         * that are also in <code>retain</code>. The cardinality of an element <code>e</code>
         * in the returned collection is the same as the cardinality of <code>e</code>
         * in <code>collection</code> unless <code>retain</code> does not contain <code>e</code>, in which
         * case the cardinality is zero. This method is useful if you do not wish to modify
         * the collection <code>c</code> and thus cannot call <code>c.retainAll(retain);</code>.
         * 
         * @param collection  the collection whose contents are the target of the #retailAll operation
         * @param retain  the collection containing the elements to be retained in the returned collection
         * @return a <code>Collection</code> containing all the elements of <code>collection</code>
         * that occur at least once in <code>retain</code>.
         * @throws NullPointerException if either parameter is null
         * @since Commons Collections 3.2
         */
        public static java.util.Collection<Object> retainAll(java.util.Collection<Object> collection, java.util.Collection<Object> retain)
        {
            return ListUtils.retainAll(collection, retain);
        }

        /**
         * Removes the elements in <code>remove</code> from <code>collection</code>. That is, this
         * method returns a collection containing all the elements in <code>c</code>
         * that are not in <code>remove</code>. The cardinality of an element <code>e</code>
         * in the returned collection is the same as the cardinality of <code>e</code>
         * in <code>collection</code> unless <code>remove</code> contains <code>e</code>, in which
         * case the cardinality is zero. This method is useful if you do not wish to modify
         * the collection <code>c</code> and thus cannot call <code>collection.removeAll(remove);</code>.
         * 
         * @param collection  the collection from which items are removed (in the returned collection)
         * @param remove  the items to be removed from the returned <code>collection</code>
         * @return a <code>Collection</code> containing all the elements of <code>collection</code> except
         * any elements that also occur in <code>remove</code>.
         * @throws NullPointerException if either parameter is null
         * @since Commons Collections 3.2
         */
        public static java.util.Collection<Object> removeAll(java.util.Collection<Object> collection, java.util.Collection<Object> remove)
        {
            return ListUtils.retainAll(collection, remove);
        }

        //-----------------------------------------------------------------------
        /**
         * Returns a synchronized collection backed by the given collection.
         * <p>
         * You must manually synchronize on the returned buffer's iterator to 
         * avoid non-deterministic behavior:
         *  
         * <pre>
         * Collection c = CollectionUtils.synchronizedCollection(myCollection);
         * synchronized (c) {
         *     Iterator i = c.iterator();
         *     while (i.hasNext()) {
         *         process (i.next());
         *     }
         * }
         * </pre>
         * 
         * This method uses the implementation in the decorators subpackage.
         * 
         * @param collection  the collection to synchronize, must not be null
         * @return a synchronized collection backed by the given collection
         * @throws IllegalArgumentException  if the collection is null
         */
        public static java.util.Collection<Object> synchronizedCollection(java.util.Collection<Object> collection)
        {
            return SynchronizedCollection.decorate(collection);
        }

        /**
         * Returns an unmodifiable collection backed by the given collection.
         * <p>
         * This method uses the implementation in the decorators subpackage.
         *
         * @param collection  the collection to make unmodifiable, must not be null
         * @return an unmodifiable collection backed by the given collection
         * @throws IllegalArgumentException  if the collection is null
         */
        public static java.util.Collection<Object> unmodifiableCollection(java.util.Collection<Object> collection)
        {
            return UnmodifiableCollection.decorate(collection);
        }

        /**
         * Returns a predicated (validating) collection backed by the given collection.
         * <p>
         * Only objects that pass the test in the given predicate can be added to the collection.
         * Trying to add an invalid object results in an IllegalArgumentException.
         * It is important not to use the original collection after invoking this method,
         * as it is a backdoor for adding invalid objects.
         *
         * @param collection  the collection to predicate, must not be null
         * @param predicate  the predicate for the collection, must not be null
         * @return a predicated collection backed by the given collection
         * @throws IllegalArgumentException  if the Collection is null
         */
        public static java.util.Collection<Object> predicatedCollection(java.util.Collection<Object> collection, Predicate predicate)
        {
            return PredicatedCollection.decorate(collection, predicate);
        }

        /**
         * Returns a typed collection backed by the given collection.
         * <p>
         * Only objects of the specified type can be added to the collection.
         * 
         * @param collection  the collection to limit to a specific type, must not be null
         * @param type  the type of objects which may be added to the collection
         * @return a typed collection backed by the specified collection
         */
        public static java.util.Collection<Object> typedCollection(java.util.Collection<Object> collection, java.lang.Class type)
        {
            return TypedCollection.decorate(collection, type);
        }

        /**
         * Returns a transformed bag backed by the given collection.
         * <p>
         * Each object is passed through the transformer as it is added to the
         * Collection. It is important not to use the original collection after invoking this 
         * method, as it is a backdoor for adding untransformed objects.
         *
         * @param collection  the collection to predicate, must not be null
         * @param transformer  the transformer for the collection, must not be null
         * @return a transformed collection backed by the given collection
         * @throws IllegalArgumentException  if the Collection or Transformer is null
         */
        public static java.util.Collection<Object> transformedCollection(java.util.Collection<Object> collection, Transformer transformer)
        {
            return TransformedCollection.decorate(collection, transformer);
        }

    }
}