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
using org.apache.commons.collections.iterators;

namespace org.apache.commons.collections
{


    /**
     * Provides static utility methods and decorators for {@link Iterator} 
     * instances. The implementations are provided in the iterators subpackage.
     * <p>
     * WARNING: Due to human error certain binary incompatabilities were introduced
     * between Commons Collections 2.1 and 3.0. The class remained source and test
     * compatible, so if you can recompile all your classes and dependencies
     * everything is OK. Those methods which are binary incompatible are marked as
     * such, together with alternate solutions that are binary compatible
     * against versions 2.1.1 and 3.1.
     *
     * @since Commons Collections 2.1
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     * @author Phil Steitz
     */
    public class IteratorUtils
    {
        // validation is done in this class in certain cases because the
        // public classes allow invalid states

        /**
         * An iterator over no elements.
         * <p>
         * WARNING: This constant is binary incompatible with Commons Collections 2.1 and 2.1.1.
         * Use <code>EmptyIterator.INSTANCE</code> for compatability with Commons Collections 2.1.1.
         */
        public static readonly ResettableIterator EMPTY_ITERATOR = EmptyIterator.RESETTABLE_INSTANCE;
        /**
         * A list iterator over no elements.
         * <p>
         * WARNING: This constant is binary incompatible with Commons Collections 2.1 and 2.1.1.
         * Use <code>EmptyListIterator.INSTANCE</code> for compatability with Commons Collections 2.1.1.
         */
        public static readonly ResettableListIterator EMPTY_LIST_ITERATOR = EmptyListIterator.RESETTABLE_INSTANCE;
        /**
         * An ordered iterator over no elements.
         */
        public static readonly OrderedIterator EMPTY_ORDERED_ITERATOR = EmptyOrderedIterator.INSTANCE;
        /**
         * A map iterator over no elements.
         */
        public static readonly MapIterator EMPTY_MAP_ITERATOR = EmptyMapIterator.INSTANCE;
        /**
         * An ordered map iterator over no elements.
         */
        public static readonly OrderedMapIterator EMPTY_ORDERED_MAP_ITERATOR = EmptyOrderedMapIterator.INSTANCE;

        /**
         * IteratorUtils is not normally instantiated.
         */
        public IteratorUtils()
        {
        }

        // Empty
        //-----------------------------------------------------------------------
        /**
         * Gets an empty iterator.
         * <p>
         * This iterator is a valid iterator object that will iterate over
         * nothing.
         * <p>
         * WARNING: This method is binary incompatible with Commons Collections 2.1 and 2.1.1.
         * Use <code>EmptyIterator.INSTANCE</code> for compatability with Commons Collections 2.1.1.
         *
         * @return  an iterator over nothing
         */
        public static ResettableIterator emptyIterator()
        {
            return EMPTY_ITERATOR;
        }

        /**
         * Gets an empty list iterator.
         * <p>
         * This iterator is a valid list iterator object that will iterate 
         * over nothing.
         * <p>
         * WARNING: This method is binary incompatible with Commons Collections 2.1 and 2.1.1.
         * Use <code>EmptyListIterator.INSTANCE</code> for compatability with Commons Collections 2.1.1.
         *
         * @return  a list iterator over nothing
         */
        public static ResettableListIterator emptyListIterator()
        {
            return EMPTY_LIST_ITERATOR;
        }

        /**
         * Gets an empty ordered iterator.
         * <p>
         * This iterator is a valid iterator object that will iterate 
         * over nothing.
         *
         * @return  an ordered iterator over nothing
         */
        public static OrderedIterator emptyOrderedIterator()
        {
            return EMPTY_ORDERED_ITERATOR;
        }

        /**
         * Gets an empty map iterator.
         * <p>
         * This iterator is a valid map iterator object that will iterate 
         * over nothing.
         *
         * @return  a map iterator over nothing
         */
        public static MapIterator emptyMapIterator()
        {
            return EMPTY_MAP_ITERATOR;
        }

        /**
         * Gets an empty ordered map iterator.
         * <p>
         * This iterator is a valid map iterator object that will iterate 
         * over nothing.
         *
         * @return  a map iterator over nothing
         */
        public static OrderedMapIterator emptyOrderedMapIterator()
        {
            return EMPTY_ORDERED_MAP_ITERATOR;
        }

        // Singleton
        //-----------------------------------------------------------------------
        /**
         * Gets a singleton iterator.
         * <p>
         * This iterator is a valid iterator object that will iterate over
         * the specified object.
         * <p>
         * WARNING: This method is binary incompatible with Commons Collections 2.1 and 2.1.1.
         * Use <code>new SingletonIterator(object)</code> for compatability.
         *
         * @param object  the single object over which to iterate
         * @return  a singleton iterator over the object
         */
        public static ResettableIterator singletonIterator(Object obj)
        {
            return new SingletonIterator(obj);
        }

        /**
         * Gets a singleton list iterator.
         * <p>
         * This iterator is a valid list iterator object that will iterate over
         * the specified object.
         *
         * @param object  the single object over which to iterate
         * @return  a singleton list iterator over the object
         */
        public static java.util.ListIterator<Object> singletonListIterator(Object obj)
        {
            return new SingletonListIterator(obj);
        }

        // Arrays
        //-----------------------------------------------------------------------
        /**
         * Gets an iterator over an object array.
         * <p>
         * WARNING: This method is binary incompatible with Commons Collections 2.1 and 2.1.1.
         * Use <code>new ArrayIterator(array)</code> for compatability.
         *
         * @param array  the array over which to iterate
         * @return  an iterator over the array
         * @throws NullPointerException if array is null
         */
        public static ResettableIterator arrayIterator(Object[] array)
        {
            return new ObjectArrayIterator(array);
        }

        /**
         * Gets an iterator over an object or primitive array.
         * <p>
         * This method will handle primitive arrays as well as object arrays.
         * The primitives will be wrapped in the appropriate wrapper class.
         *
         * @param array  the array over which to iterate
         * @return  an iterator over the array
         * @throws IllegalArgumentException if the array is not an array
         * @throws NullPointerException if array is null
         */
        public static ResettableIterator arrayIterator(Object array)
        {
            return new ArrayIterator(array);
        }

        /**
         * Gets an iterator over the end part of an object array.
         * <p>
         * WARNING: This method is binary incompatible with Commons Collections 2.1 and 2.1.1.
         * Use <code>new ArrayIterator(array,start)</code> for compatability.
         *
         * @param array  the array over which to iterate
         * @param start  the index to start iterating at
         * @return an iterator over part of the array
         * @throws IndexOutOfBoundsException if start is less than zero or greater
         *  than the length of the array
         * @throws NullPointerException if array is null
         */
        public static ResettableIterator arrayIterator(Object[] array, int start)
        {
            return new ObjectArrayIterator(array, start);
        }

        /**
         * Gets an iterator over the end part of an object or primitive array.
         * <p>
         * This method will handle primitive arrays as well as object arrays.
         * The primitives will be wrapped in the appropriate wrapper class.
         *
         * @param array  the array over which to iterate
         * @param start  the index to start iterating at
         * @return an iterator over part of the array
         * @throws IllegalArgumentException if the array is not an array
         * @throws IndexOutOfBoundsException if start is less than zero or greater
         *  than the length of the array
         * @throws NullPointerException if array is null
         */
        public static ResettableIterator arrayIterator(Object array, int start)
        {
            return new ArrayIterator(array, start);
        }

        /**
         * Gets an iterator over part of an object array.
         * <p>
         * WARNING: This method is binary incompatible with Commons Collections 2.1 and 2.1.1.
         * Use <code>new ArrayIterator(array,start,end)</code> for compatability.
         *
         * @param array  the array over which to iterate
         * @param start  the index to start iterating at
         * @param end  the index to finish iterating at
         * @return an iterator over part of the array
         * @throws IndexOutOfBoundsException if array bounds are invalid
         * @throws IllegalArgumentException if end is before start
         * @throws NullPointerException if array is null
         */
        public static ResettableIterator arrayIterator(Object[] array, int start, int end)
        {
            return new ObjectArrayIterator(array, start, end);
        }

        /**
         * Gets an iterator over part of an object or primitive array.
         * <p>
         * This method will handle primitive arrays as well as object arrays.
         * The primitives will be wrapped in the appropriate wrapper class.
         *
         * @param array  the array over which to iterate
         * @param start  the index to start iterating at
         * @param end  the index to finish iterating at
         * @return an iterator over part of the array
         * @throws IllegalArgumentException if the array is not an array
         * @throws IndexOutOfBoundsException if array bounds are invalid
         * @throws IllegalArgumentException if end is before start
         * @throws NullPointerException if array is null
         */
        public static ResettableIterator arrayIterator(Object array, int start, int end)
        {
            return new ArrayIterator(array, start, end);
        }

        //-----------------------------------------------------------------------
        /**
         * Gets a list iterator over an object array.
         *
         * @param array  the array over which to iterate
         * @return  a list iterator over the array
         * @throws NullPointerException if array is null
         */
        public static ResettableListIterator arrayListIterator(Object[] array)
        {
            return new ObjectArrayListIterator(array);
        }

        /**
         * Gets a list iterator over an object or primitive array.
         * <p>
         * This method will handle primitive arrays as well as object arrays.
         * The primitives will be wrapped in the appropriate wrapper class.
         *
         * @param array  the array over which to iterate
         * @return  a list iterator over the array
         * @throws IllegalArgumentException if the array is not an array
         * @throws NullPointerException if array is null
         */
        public static ResettableListIterator arrayListIterator(Object array)
        {
            return new ArrayListIterator(array);
        }

        /**
         * Gets a list iterator over the end part of an object array.
         *
         * @param array  the array over which to iterate
         * @param start  the index to start iterating at
         * @return a list iterator over part of the array
         * @throws IndexOutOfBoundsException if start is less than zero
         * @throws NullPointerException if array is null
         */
        public static ResettableListIterator arrayListIterator(Object[] array, int start)
        {
            return new ObjectArrayListIterator(array, start);
        }

        /**
         * Gets a list iterator over the end part of an object or primitive array.
         * <p>
         * This method will handle primitive arrays as well as object arrays.
         * The primitives will be wrapped in the appropriate wrapper class.
         *
         * @param array  the array over which to iterate
         * @param start  the index to start iterating at
         * @return a list iterator over part of the array
         * @throws IllegalArgumentException if the array is not an array
         * @throws IndexOutOfBoundsException if start is less than zero
         * @throws NullPointerException if array is null
         */
        public static ResettableListIterator arrayListIterator(Object array, int start)
        {
            return new ArrayListIterator(array, start);
        }

        /**
         * Gets a list iterator over part of an object array.
         *
         * @param array  the array over which to iterate
         * @param start  the index to start iterating at
         * @param end  the index to finish iterating at
         * @return a list iterator over part of the array
         * @throws IndexOutOfBoundsException if array bounds are invalid
         * @throws IllegalArgumentException if end is before start
         * @throws NullPointerException if array is null
         */
        public static ResettableListIterator arrayListIterator(Object[] array, int start, int end)
        {
            return new ObjectArrayListIterator(array, start, end);
        }

        /**
         * Gets a list iterator over part of an object or primitive array.
         * <p>
         * This method will handle primitive arrays as well as object arrays.
         * The primitives will be wrapped in the appropriate wrapper class.
         *
         * @param array  the array over which to iterate
         * @param start  the index to start iterating at
         * @param end  the index to finish iterating at
         * @return a list iterator over part of the array
         * @throws IllegalArgumentException if the array is not an array
         * @throws IndexOutOfBoundsException if array bounds are invalid
         * @throws IllegalArgumentException if end is before start
         * @throws NullPointerException if array is null
         */
        public static ResettableListIterator arrayListIterator(Object array, int start, int end)
        {
            return new ArrayListIterator(array, start, end);
        }

        // Unmodifiable
        //-----------------------------------------------------------------------
        /**
         * Gets an immutable version of an {@link Iterator}. The returned object
         * will always throw an {@link UnsupportedOperationException} for
         * the {@link Iterator#remove} method.
         *
         * @param iterator  the iterator to make immutable
         * @return an immutable version of the iterator
         */
        public static java.util.Iterator<Object> unmodifiableIterator(java.util.Iterator<Object> iterator)
        {
            return UnmodifiableIterator.decorate(iterator);
        }

        /**
         * Gets an immutable version of a {@link ListIterator}. The returned object
         * will always throw an {@link UnsupportedOperationException} for
         * the {@link Iterator#remove}, {@link ListIterator#add} and
         * {@link ListIterator#set} methods.
         *
         * @param listIterator  the iterator to make immutable
         * @return an immutable version of the iterator
         */
        public static java.util.ListIterator<Object> unmodifiableListIterator(java.util.ListIterator<Object> listIterator)
        {
            return UnmodifiableListIterator.decorate(listIterator);
        }

        /**
         * Gets an immutable version of a {@link MapIterator}. The returned object
         * will always throw an {@link UnsupportedOperationException} for
         * the {@link Iterator#remove}, {@link MapIterator#setValue(Object)} methods.
         *
         * @param mapIterator  the iterator to make immutable
         * @return an immutable version of the iterator
         */
        public static MapIterator unmodifiableMapIterator(MapIterator mapIterator)
        {
            return UnmodifiableMapIterator.decorate(mapIterator);
        }

        // Chained
        //-----------------------------------------------------------------------
        /**
         * Gets an iterator that iterates through two {@link Iterator}s 
         * one after another.
         *
         * @param iterator1  the first iterators to use, not null
         * @param iterator2  the first iterators to use, not null
         * @return a combination iterator over the iterators
         * @throws NullPointerException if either iterator is null
         */
        public static java.util.Iterator<Object> chainedIterator(java.util.Iterator<Object> iterator1, java.util.Iterator<Object> iterator2)
        {
            return new IteratorChain(iterator1, iterator2);
        }

        /**
         * Gets an iterator that iterates through an array of {@link Iterator}s 
         * one after another.
         *
         * @param iterators  the iterators to use, not null or empty or contain nulls
         * @return a combination iterator over the iterators
         * @throws NullPointerException if iterators array is null or contains a null
         */
        public static java.util.Iterator<Object> chainedIterator(java.util.Iterator<Object>[] iterators)
        {
            return new IteratorChain(iterators);
        }

        /**
         * Gets an iterator that iterates through a collections of {@link Iterator}s 
         * one after another.
         *
         * @param iterators  the iterators to use, not null or empty or contain nulls
         * @return a combination iterator over the iterators
         * @throws NullPointerException if iterators collection is null or contains a null
         * @throws ClassCastException if the iterators collection contains the wrong object type
         */
        public static java.util.Iterator<Object> chainedIterator(java.util.Collection<Object> iterators)
        {
            return new IteratorChain(iterators);
        }

        // Collated
        //-----------------------------------------------------------------------
        /**
         * Gets an iterator that provides an ordered iteration over the elements
         * contained in a collection of ordered {@link Iterator}s.
         * <p>
         * Given two ordered {@link Iterator}s <code>A</code> and <code>B</code>,
         * the {@link Iterator#next()} method will return the lesser of 
         * <code>A.next()</code> and <code>B.next()</code>.
         * <p>
         * The comparator is optional. If null is specified then natural order is used.
         *
         * @param comparator  the comparator to use, may be null for natural order
         * @param iterator1  the first iterators to use, not null
         * @param iterator2  the first iterators to use, not null
         * @return a combination iterator over the iterators
         * @throws NullPointerException if either iterator is null
         */
        public static java.util.Iterator<Object> collatedIterator(java.util.Comparator<Object> comparator, java.util.Iterator<Object> iterator1, java.util.Iterator<Object> iterator2)
        {
            return new CollatingIterator(comparator, iterator1, iterator2);
        }

        /**
         * Gets an iterator that provides an ordered iteration over the elements
         * contained in an array of {@link Iterator}s.
         * <p>
         * Given two ordered {@link Iterator}s <code>A</code> and <code>B</code>,
         * the {@link Iterator#next()} method will return the lesser of 
         * <code>A.next()</code> and <code>B.next()</code> and so on.
         * <p>
         * The comparator is optional. If null is specified then natural order is used.
         *
         * @param comparator  the comparator to use, may be null for natural order
         * @param iterators  the iterators to use, not null or empty or contain nulls
         * @return a combination iterator over the iterators
         * @throws NullPointerException if iterators array is null or contains a null
         */
        public static java.util.Iterator<Object> collatedIterator(java.util.Comparator<Object> comparator, java.util.Iterator<Object>[] iterators)
        {
            return new CollatingIterator(comparator, iterators);
        }

        /**
         * Gets an iterator that provides an ordered iteration over the elements
         * contained in a collection of {@link Iterator}s.
         * <p>
         * Given two ordered {@link Iterator}s <code>A</code> and <code>B</code>,
         * the {@link Iterator#next()} method will return the lesser of 
         * <code>A.next()</code> and <code>B.next()</code> and so on.
         * <p>
         * The comparator is optional. If null is specified then natural order is used.
         *
         * @param comparator  the comparator to use, may be null for natural order
         * @param iterators  the iterators to use, not null or empty or contain nulls
         * @return a combination iterator over the iterators
         * @throws NullPointerException if iterators collection is null or contains a null
         * @throws ClassCastException if the iterators collection contains the wrong object type
         */
        public static java.util.Iterator<Object> collatedIterator(java.util.Comparator<Object> comparator, java.util.Collection<Object> iterators)
        {
            return new CollatingIterator(comparator, iterators);
        }

        // Object Graph
        //-----------------------------------------------------------------------
        /**
         * Gets an iterator that operates over an object graph.
         * <p>
         * This iterator can extract multiple objects from a complex tree-like object graph.
         * The iteration starts from a single root object.
         * It uses a <code>Transformer</code> to extract the iterators and elements.
         * Its main benefit is that no intermediate <code>List</code> is created.
         * <p>
         * For example, consider an object graph:
         * <pre>
         *                 |- Branch -- Leaf
         *                 |         \- Leaf
         *         |- Tree |         /- Leaf
         *         |       |- Branch -- Leaf
         *  Forest |                 \- Leaf
         *         |       |- Branch -- Leaf
         *         |       |         \- Leaf
         *         |- Tree |         /- Leaf
         *                 |- Branch -- Leaf
         *                 |- Branch -- Leaf</pre>
         * The following <code>Transformer</code>, used in this class, will extract all
         * the Leaf objects without creating a combined intermediate list:
         * <pre>
         * public Object transform(Object input) {
         *   if (input is Forest) {
         *     return ((Forest) input).treeIterator();
         *   }
         *   if (input is Tree) {
         *     return ((Tree) input).branchIterator();
         *   }
         *   if (input is Branch) {
         *     return ((Branch) input).leafIterator();
         *   }
         *   if (input is Leaf) {
         *     return input;
         *   }
         *   throw new ClassCastException();
         * }</pre>
         * <p>
         * Internally, iteration starts from the root object. When next is called,
         * the transformer is called to examine the object. The transformer will return
         * either an iterator or an object. If the object is an Iterator, the next element
         * from that iterator is obtained and the process repeats. If the element is an object
         * it is returned.
         * <p>
         * Under many circumstances, linking Iterators together in this manner is
         * more efficient (and convenient) than using nested for loops to extract a list.
         * 
         * @param root  the root object to start iterating from, null results in an empty iterator
         * @param transformer  the transformer to use, see above, null uses no effect transformer
         * @return a new object graph iterator
         * @since Commons Collections 3.1
         */
        public static java.util.Iterator<Object> objectGraphIterator(Object root, Transformer transformer)
        {
            return new ObjectGraphIterator(root, transformer);
        }

        // Transformed
        //-----------------------------------------------------------------------
        /**
         * Gets an iterator that transforms the elements of another iterator.
         * <p>
         * The transformation occurs during the next() method and the underlying
         * iterator is unaffected by the transformation.
         *
         * @param iterator  the iterator to use, not null
         * @param transform  the transform to use, not null
         * @return a new transforming iterator
         * @throws NullPointerException if either parameter is null
         */
        public static java.util.Iterator<Object> transformedIterator(java.util.Iterator<Object> iterator, Transformer transform)
        {
            if (iterator == null)
            {
                throw new java.lang.NullPointerException("Iterator must not be null");
            }
            if (transform == null)
            {
                throw new java.lang.NullPointerException("Transformer must not be null");
            }
            return new TransformIterator(iterator, transform);
        }

        // Filtered
        //-----------------------------------------------------------------------
        /**
         * Gets an iterator that filters another iterator.
         * <p>
         * The returned iterator will only return objects that match the specified
         * filtering predicate.
         *
         * @param iterator  the iterator to use, not null
         * @param predicate  the predicate to use as a filter, not null
         * @return a new filtered iterator
         * @throws NullPointerException if either parameter is null
         */
        public static java.util.Iterator<Object> filteredIterator(java.util.Iterator<Object> iterator, Predicate predicate)
        {
            if (iterator == null)
            {
                throw new java.lang.NullPointerException("Iterator must not be null");
            }
            if (predicate == null)
            {
                throw new java.lang.NullPointerException("Predicate must not be null");
            }
            return new FilterIterator(iterator, predicate);
        }

        /**
         * Gets a list iterator that filters another list iterator.
         * <p>
         * The returned iterator will only return objects that match the specified
         * filtering predicate.
         *
         * @param listIterator  the list iterator to use, not null
         * @param predicate  the predicate to use as a filter, not null
         * @return a new filtered iterator
         * @throws NullPointerException if either parameter is null
         */
        public static java.util.ListIterator<Object> filteredListIterator(java.util.ListIterator<Object> listIterator, Predicate predicate)
        {
            if (listIterator == null)
            {
                throw new java.lang.NullPointerException("ListIterator must not be null");
            }
            if (predicate == null)
            {
                throw new java.lang.NullPointerException("Predicate must not be null");
            }
            return new FilterListIterator(listIterator, predicate);
        }

        // Looping
        //-----------------------------------------------------------------------
        /**
         * Gets an iterator that loops continuously over the supplied collection.
         * <p>
         * The iterator will only stop looping if the remove method is called
         * enough times to empty the collection, or if the collection is empty
         * to start with.
         *
         * @param coll  the collection to iterate over, not null
         * @return a new looping iterator
         * @throws NullPointerException if the collection is null
         */
        public static ResettableIterator loopingIterator(java.util.Collection<Object> coll)
        {
            if (coll == null)
            {
                throw new java.lang.NullPointerException("Collection must not be null");
            }
            return new LoopingIterator(coll);
        }

        /**
         * Gets an iterator that loops continuously over the supplied list.
         * <p>
         * The iterator will only stop looping if the remove method is called
         * enough times to empty the list, or if the list is empty to start with.
         *
         * @param list  the list to iterate over, not null
         * @return a new looping iterator
         * @throws NullPointerException if the list is null
         * @since Commons Collections 3.2
         */
        public static ResettableListIterator loopingListIterator(java.util.List<Object> list)
        {
            if (list == null)
            {
                throw new java.lang.NullPointerException("List must not be null");
            }
            return new LoopingListIterator(list);
        }

        // Views
        //-----------------------------------------------------------------------
        /**
         * Gets an iterator that provides an iterator view of the given enumeration.
         *
         * @param enumeration  the enumeration to use
         * @return a new iterator
         */
        public static java.util.Iterator<Object> asIterator(java.util.Enumeration<Object> enumeration)
        {
            if (enumeration == null)
            {
                throw new java.lang.NullPointerException("Enumeration must not be null");
            }
            return new EnumerationIterator(enumeration);
        }

        /**
         * Gets an iterator that provides an iterator view of the given enumeration 
         * that will remove elements from the specified collection.
         *
         * @param enumeration  the enumeration to use
         * @param removeCollection  the collection to remove elements from
         * @return a new iterator
         */
        public static java.util.Iterator<Object> asIterator(java.util.Enumeration<Object> enumeration, java.util.Collection<Object> removeCollection)
        {
            if (enumeration == null)
            {
                throw new java.lang.NullPointerException("Enumeration must not be null");
            }
            if (removeCollection == null)
            {
                throw new java.lang.NullPointerException("Collection must not be null");
            }
            return new EnumerationIterator(enumeration, removeCollection);
        }

        /**
         * Gets an enumeration that wraps an iterator.
         *
         * @param iterator  the iterator to use, not null
         * @return a new enumeration
         * @throws NullPointerException if iterator is null
         */
        public static java.util.Enumeration<Object> asEnumeration(java.util.Iterator<Object> iterator)
        {
            if (iterator == null)
            {
                throw new java.lang.NullPointerException("Iterator must not be null");
            }
            return new IteratorEnumeration(iterator);
        }

        /**
         * Gets a list iterator based on a simple iterator.
         * <p>
         * As the wrapped Iterator is traversed, a LinkedList of its values is
         * cached, permitting all required operations of ListIterator.
         *
         * @param iterator  the iterator to use, not null
         * @return a new iterator
         * @throws NullPointerException if iterator parameter is null
         */
        public static java.util.ListIterator<Object> toListIterator(java.util.Iterator<Object> iterator)
        {
            if (iterator == null)
            {
                throw new java.lang.NullPointerException("Iterator must not be null");
            }
            return new ListIteratorWrapper(iterator);
        }

        /**
         * Gets an array based on an iterator.
         * <p>
         * As the wrapped Iterator is traversed, an ArrayList of its values is
         * created. At the end, this is converted to an array.
         *
         * @param iterator  the iterator to use, not null
         * @return an array of the iterator contents
         * @throws NullPointerException if iterator parameter is null
         */
        public static Object[] toArray(java.util.Iterator<Object> iterator)
        {
            if (iterator == null)
            {
                throw new java.lang.NullPointerException("Iterator must not be null");
            }
            java.util.List<Object> list = toList(iterator, 100);
            return list.toArray();
        }

        /**
         * Gets an array based on an iterator.
         * <p>
         * As the wrapped Iterator is traversed, an ArrayList of its values is
         * created. At the end, this is converted to an array.
         *
         * @param iterator  the iterator to use, not null
         * @param arrayClass  the class of array to create
         * @return an array of the iterator contents
         * @throws NullPointerException if iterator parameter is null
         * @throws NullPointerException if arrayClass is null
         * @throws ClassCastException if the arrayClass is invalid
         */
        public static Object[] toArray(java.util.Iterator<Object> iterator, Type arrayClass)
        {
            if (iterator == null)
            {
                throw new java.lang.NullPointerException("Iterator must not be null");
            }
            if (arrayClass == null)
            {
                throw new java.lang.NullPointerException("Array class must not be null");
            }
            java.util.List<Object> list = toList(iterator, 100);
            return list.toArray((Object[])java.lang.reflect.Array.newInstance(arrayClass, list.size()));
        }

        /**
         * Gets a list based on an iterator.
         * <p>
         * As the wrapped Iterator is traversed, an ArrayList of its values is
         * created. At the end, the list is returned.
         *
         * @param iterator  the iterator to use, not null
         * @return a list of the iterator contents
         * @throws NullPointerException if iterator parameter is null
         */
        public static java.util.List<Object> toList(java.util.Iterator<Object> iterator)
        {
            return toList(iterator, 10);
        }

        /**
         * Gets a list based on an iterator.
         * <p>
         * As the wrapped Iterator is traversed, an ArrayList of its values is
         * created. At the end, the list is returned.
         *
         * @param iterator  the iterator to use, not null
         * @param estimatedSize  the initial size of the ArrayList
         * @return a list of the iterator contents
         * @throws NullPointerException if iterator parameter is null
         * @throws IllegalArgumentException if the size is less than 1
         */
        public static java.util.List<Object> toList(java.util.Iterator<Object> iterator, int estimatedSize)
        {
            if (iterator == null)
            {
                throw new java.lang.NullPointerException("Iterator must not be null");
            }
            if (estimatedSize < 1)
            {
                throw new java.lang.IllegalArgumentException("Estimated size must be greater than 0");
            }
            java.util.List<Object> list = new java.util.ArrayList<Object>(estimatedSize);
            while (iterator.hasNext())
            {
                list.add(iterator.next());
            }
            return list;
        }

        /** 
         * Gets a suitable Iterator for the given object.
         * <p>
         * This method can handles objects as follows
         * <ul>
         * <li>null - empty iterator
         * <li>Iterator - returned directly
         * <li>Enumeration - wrapped
         * <li>Collection - iterator from collection returned
         * <li>Map - values iterator returned
         * <li>Dictionary - values (elements) enumeration returned as iterator
         * <li>array - iterator over array returned
         * <li>object with iterator() public method accessed by reflection
         * <li>object - singleton iterator
         * </ul>
         * 
         * @param obj  the object to convert to an iterator
         * @return a suitable iterator, never null
         */
        public static java.util.Iterator<Object> getIterator(Object obj)
        {
            if (obj == null)
            {
                return emptyIterator();

            }
            else if (obj is java.util.Iterator<Object>)
            {
                return (java.util.Iterator<Object>)obj;

            }
            else if (obj is java.util.Collection<Object>)
            {
                return ((java.util.Collection<Object>)obj).iterator();

            }
            else if (obj is Object[])
            {
                return new ObjectArrayIterator((Object[])obj);

            }
            else if (obj is java.util.Enumeration<Object>)
            {
                return new EnumerationIterator((java.util.Enumeration<Object>)obj);

            }
            else if (obj is java.util.Map<Object, Object>)
            {
                return ((java.util.Map<Object, Object>)obj).values().iterator();

            }
            else if (obj is java.util.Dictionary<Object, Object>)
            {
                return new EnumerationIterator(((java.util.Dictionary<Object, Object>)obj).elements());

            }
            else if (obj != null && obj.GetType().IsArray)
            {
                return new ArrayIterator(obj);

            }
            else
            {
                try
                {
                    //Method method = obj.getClass().getMethod("iterator", (Class[]) null);
                    System.Reflection.MethodInfo mi = obj.GetType().GetMethod("iterator");
                    //if (Iterator.class.isAssignableFrom(method.getReturnType())) {
                    if (typeof(java.util.Iterator<Object>).IsAssignableFrom(mi.ReturnType))
                    {
                        //java.util.Iterator<Object> it = (java.util.Iterator<Object>) method.invoke(obj, (Object[]) null);
                        java.util.Iterator<Object> it = (java.util.Iterator<Object>)mi.Invoke(obj, (Object[])null);
                        if (it != null)
                        {
                            return it;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ignore
                }
                return singletonIterator(obj);
            }
        }

    }
}
