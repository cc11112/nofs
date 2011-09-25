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

namespace biz.ritter.javapi.util
{


    /**
     * {@code Arrays} contains static methods which operate on arrays.
     * 
     * @since 1.2
     */
    [Serializable]
    public class Arrays<T>
    {

    /* Specifies when to switch to insertion sort */
    private const int SIMPLE_LENGTH = 7;

        private Arrays()
        {
            /* empty */
        }

        /**
         * Fills the specified array with the specified element.
         * @param array
         *            the {@code byte} array to fill.
         * @param value
         *            the {@code byte} element.
         */
        public static void fill(byte[] array, byte value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified range in the array with the specified element.
         * 
         * @param array
         *            the {@code byte} array to fill.
         * @param start
         *            the first index to fill.
         * @param end
         *            the last + 1 index to fill.
         * @param value
         *            the {@code byte} element.
         * @throws IllegalArgumentException
         *                if {@code start > end}.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code start < 0} or {@code end > array.length}.
         */
        public static void fill(byte[] array, int start, int end, byte value)
        {
            checkBounds(array.Length, start, end);
            for (int i = start; i < end; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified array with the specified element.
         * 
         * @param array
         *            the {@code short} array to fill.
         * @param value
         *            the {@code short} element.
         */
        public static void fill(short[] array, short value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified range in the array with the specified element.
         * 
         * @param array
         *            the {@code short} array to fill.
         * @param start
         *            the first index to fill.
         * @param end
         *            the last + 1 index to fill.
         * @param value
         *            the {@code short} element.
         * @throws IllegalArgumentException
         *                if {@code start > end}.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code start < 0} or {@code end > array.length}.
         */
        public static void fill(short[] array, int start, int end, short value)
        {
            checkBounds(array.Length, start, end);
            for (int i = start; i < end; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified array with the specified element.
         * 
         * @param array
         *            the {@code char} array to fill.
         * @param value
         *            the {@code char} element.
         */
        public static void fill(char[] array, char value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified range in the array with the specified element.
         * 
         * @param array
         *            the {@code char} array to fill.
         * @param start
         *            the first index to fill.
         * @param end
         *            the last + 1 index to fill.
         * @param value
         *            the {@code char} element.
         * @throws IllegalArgumentException
         *                if {@code start > end}.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code start < 0} or {@code end > array.length}.
         */
        public static void fill(char[] array, int start, int end, char value)
        {
            checkBounds(array.Length, start, end);
            for (int i = start; i < end; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified array with the specified element.
         * 
         * @param array
         *            the {@code int} array to fill.
         * @param value
         *            the {@code int} element.
         */
        public static void fill(int[] array, int value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified range in the array with the specified element.
         * 
         * @param array
         *            the {@code int} array to fill.
         * @param start
         *            the first index to fill.
         * @param end
         *            the last + 1 index to fill.
         * @param value
         *            the {@code int} element.
         * @throws IllegalArgumentException
         *                if {@code start > end}.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code start < 0} or {@code end > array.length}.
         */
        public static void fill(int[] array, int start, int end, int value)
        {
            checkBounds(array.Length, start, end);
            for (int i = start; i < end; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified array with the specified element.
         * 
         * @param array
         *            the {@code long} array to fill.
         * @param value
         *            the {@code long} element.
         */
        public static void fill(long[] array, long value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified range in the array with the specified element.
         * 
         * @param array
         *            the {@code long} array to fill.
         * @param start
         *            the first index to fill.
         * @param end
         *            the last + 1 index to fill.
         * @param value
         *            the {@code long} element.
         * @throws IllegalArgumentException
         *                if {@code start > end}.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code start < 0} or {@code end > array.length}.
         */
        public static void fill(long[] array, int start, int end, long value)
        {
            checkBounds(array.Length, start, end);
            for (int i = start; i < end; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified array with the specified element.
         * 
         * @param array
         *            the {@code float} array to fill.
         * @param value
         *            the {@code float} element.
         */
        public static void fill(float[] array, float value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified range in the array with the specified element.
         * 
         * @param array
         *            the {@code float} array to fill.
         * @param start
         *            the first index to fill.
         * @param end
         *            the last + 1 index to fill.
         * @param value
         *            the {@code float} element.
         * @throws IllegalArgumentException
         *                if {@code start > end}.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code start < 0} or {@code end > array.length}.
         */
        public static void fill(float[] array, int start, int end, float value)
        {
            checkBounds(array.Length, start, end);
            for (int i = start; i < end; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified array with the specified element.
         * 
         * @param array
         *            the {@code double} array to fill.
         * @param value
         *            the {@code double} element.
         */
        public static void fill(double[] array, double value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified range in the array with the specified element.
         * 
         * @param array
         *            the {@code double} array to fill.
         * @param start
         *            the first index to fill.
         * @param end
         *            the last + 1 index to fill.
         * @param value
         *            the {@code double} element.
         * @throws IllegalArgumentException
         *                if {@code start > end}.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code start < 0} or {@code end > array.length}.
         */
        public static void fill(double[] array, int start, int end, double value)
        {
            checkBounds(array.Length, start, end);
            for (int i = start; i < end; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified array with the specified element.
         * 
         * @param array
         *            the {@code boolean} array to fill.
         * @param value
         *            the {@code boolean} element.
         */
        public static void fill(bool[] array, bool value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified range in the array with the specified element.
         * 
         * @param array
         *            the {@code boolean} array to fill.
         * @param start
         *            the first index to fill.
         * @param end
         *            the last + 1 index to fill.
         * @param value
         *            the {@code boolean} element.
         * @throws IllegalArgumentException
         *                if {@code start > end}.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code start < 0} or {@code end > array.length}.
         */
        public static void fill(bool[] array, int start, int end, bool value)
        {
            checkBounds(array.Length, start, end);
            for (int i = start; i < end; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified array with the specified element.
         * 
         * @param array
         *            the {@code Object} array to fill.
         * @param value
         *            the {@code Object} element.
         */
        public static void fill(Object[] array, Object value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        /**
         * Fills the specified range in the array with the specified element.
         * 
         * @param array
         *            the {@code Object} array to fill.
         * @param start
         *            the first index to fill.
         * @param end
         *            the last + 1 index to fill.
         * @param value
         *            the {@code Object} element.
         * @throws IllegalArgumentException
         *                if {@code start > end}.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code start < 0} or {@code end > array.length}.
         */
        public static void fill(Object[] array, int start, int end, Object value)
        {
            checkBounds(array.Length, start, end);
            for (int i = start; i < end; i++)
            {
                array[i] = value;
            }
        }
        /**
         * Fills the specified range in the array with the specified element.
         * 
         * @param array
         *            the {@code Object} array to fill.
         * @param start
         *            the first index to fill.
         * @param end
         *            the last + 1 index to fill.
         * @param value
         *            the {@code Object} element.
         * @throws IllegalArgumentException
         *                if {@code start > end}.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code start < 0} or {@code end > array.length}.
         */
        public static void fill(T[] array, int start, int end, T value)
        {
            checkBounds(array.Length, start, end);
            for (int i = start; i < end; i++)
            {
                array[i] = value;
            }
        }
        private static void checkBounds(int arrLength, int start, int end)
        {
            if (start > end)
            {
                // luni.35=Start index ({0}) is greater than end index ({1})
                throw new java.lang.IllegalArgumentException("Start index ({"+start+"}) is greater than end index ({"+end+"})");
            }
            if (start < 0)
            {
                // luni.36=Array index out of range\: {0}
                throw new java.lang.ArrayIndexOutOfBoundsException("Array index out of range: "+ start); //$NON-NLS-1$
            }
            if (end > arrLength)
            {
                // luni.36=Array index out of range\: {0}
                throw new java.lang.ArrayIndexOutOfBoundsException("Array index out of range: "+ end); //$NON-NLS-1$
            }
        }
        /**
         * Compares the two arrays.
         * 
         * @param array1
         *            the first {@code byte} array.
         * @param array2
         *            the second {@code byte} array.
         * @return {@code true} if both arrays are {@code null} or if the arrays have the
         *         same length and the elements at each index in the two arrays are
         *         equal, {@code false} otherwise.
         */
        public static bool equals(byte[] array1, byte[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }
        /**
         * Compares the two arrays.
         * 
         * @param array1
         *            the first {@code short} array.
         * @param array2
         *            the second {@code short} array.
         * @return {@code true} if both arrays are {@code null} or if the arrays have the
         *         same length and the elements at each index in the two arrays are
         *         equal, {@code false} otherwise.
         */
        public static bool equals(short[] array1, short[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

        /**
         * Compares the two arrays.
         * 
         * @param array1
         *            the first {@code char} array.
         * @param array2
         *            the second {@code char} array.
         * @return {@code true} if both arrays are {@code null} or if the arrays have the
         *         same length and the elements at each index in the two arrays are
         *         equal, {@code false} otherwise.
         */
        public static bool equals(char[] array1, char[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

        /**
         * Compares the two arrays.
         * 
         * @param array1
         *            the first {@code int} array.
         * @param array2
         *            the second {@code int} array.
         * @return {@code true} if both arrays are {@code null} or if the arrays have the
         *         same length and the elements at each index in the two arrays are
         *         equal, {@code false} otherwise.
         */
        public static bool equals(int[] array1, int[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

        /**
         * Compares the two arrays.
         * 
         * @param array1
         *            the first {@code long} array.
         * @param array2
         *            the second {@code long} array.
         * @return {@code true} if both arrays are {@code null} or if the arrays have the
         *         same length and the elements at each index in the two arrays are
         *         equal, {@code false} otherwise.
         */
        public static bool equals(long[] array1, long[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

        /**
         * Compares the two arrays.
         * 
         * @param array1
         *            the first {@code boolean} array.
         * @param array2
         *            the second {@code boolean} array.
         * @return {@code true} if both arrays are {@code null} or if the arrays have the
         *         same length and the elements at each index in the two arrays are
         *         equal, {@code false} otherwise.
         */
        public static bool equals(bool[] array1, bool[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

        /**
         * Compares the two arrays.
         * 
         * @param array1
         *            the first {@code Object} array.
         * @param array2
         *            the second {@code Object} array.
         * @return {@code true} if both arrays are {@code null} or if the arrays have the
         *         same length and the elements at each index in the two arrays are
         *         equal according to {@code equals()}, {@code false} otherwise.
         */
        public static bool equals(Object[] array1, Object[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                Object e1 = array1[i], e2 = array2[i];
                if (!(e1 == null ? e2 == null : e1.equals(e2)))
                {
                    return false;
                }
            }
            return true;
        }

        /**
         * Performs a binary search for the specified element in a part of the
         * specified sorted array.
         * 
         * @param array
         *            the sorted int array to search
         * @param startIndex
         *            the inclusive start index
         * @param endIndex
         *            the exclusive end index
         * @param value
         *            the int element to find
         * @return the non-negative index of the element, or a negative index which
         *         is the -index - 1 where the element would be inserted
         * @throws IllegalArgumentException -
         *             if startIndex is bigger than endIndex
         * @throws ArrayIndexOutOfBoundsException -
         *             if startIndex is smaller than zero or or endIndex is bigger
         *             than length of array
         * @since 1.6
         */
        public static int binarySearch(int[] array, int startIndex, int endIndex,
                int value) {
            checkIndexForBinarySearch(array.Length, startIndex, endIndex);
            int low = startIndex, mid = -1, high = endIndex - 1;
            while (low <= high) {
                mid = java.dotnet.lang.Operator.shiftRightUnsignet ((low + high) , 1);
                if (value > array[mid]) {
                    low = mid + 1;
                } else if (value == array[mid]) {
                    return mid;
                } else {
                    high = mid - 1;
                }
            }
            if (mid < 0) {
                int insertPoint = endIndex;
                for (int index = startIndex; index < endIndex; index++) {
                    if (value < array[index]) {
                        insertPoint = index;
                    }
                }
                return -insertPoint - 1;
            }
            return -mid - (value < array[mid] ? 1 : 2);
        }

        /**
         * Performs a binary search for the specified element in the specified
         * sorted array.
         * 
         * @param array
         *            the sorted int array to search
         * @param value
         *            the int element to find
         * @return the non-negative index of the element, or a negative index which
         *         is the -index - 1 where the element would be inserted
         */
        public static int binarySearch(int[] array, int value)
        {
            return binarySearch(array, 0, array.Length, value);
        }

        /**
         * Fills the array with the given value.
         * 
         * @param length
         *            length of the array
         * @param start
         *            start index
         * @param end
         *            end index
         */
        private static void checkIndexForBinarySearch(int length, int start, int end)
        {
            if (start > end)
            {
                throw new java.lang.IllegalArgumentException();
            }
            if (length < end || 0 > start)
            {
                throw new java.lang.ArrayIndexOutOfBoundsException();
            }
        }

        /**
         * Returns a {@code List} of the objects in the specified array. The size of the
         * {@code List} cannot be modified, i.e. adding and removing are unsupported, but
         * the elements can be set. Setting an element modifies the underlying
         * array.
         * 
         * @param array
         *            the array.
         * @return a {@code List} of the elements of the specified array.
         */
        public static List<T> asList<T> (T[] array) {
            return new ArrayList<T>(array);
        }

    /**
     * Sorts the specified range in the array using the specified {@code Comparator}.
     * All elements must be comparable to each other without a
     * {@code ClassCastException} being thrown.
     *
     * @param array
     *            the {@code Object} array to be sorted.
     * @param start
     *            the start index to sort.
     * @param end
     *            the last + 1 index to sort.
     * @param comparator
     *            the {@code Comparator}.
     * @throws ClassCastException
     *                if elements in the array cannot be compared to each other
     *                using the {@code Comparator}.
     * @throws IllegalArgumentException
     *                if {@code start > end}.
     * @throws ArrayIndexOutOfBoundsException
     *                if {@code start < 0} or {@code end > array.length}.
     */
    public static void sort<T>(T[] array, int start, int end, Comparator<T> comparator) {
        if (array == null) {
            throw new java.lang.NullPointerException();
        }
        checkBounds(array.Length, start, end);
        sort(start, end, array, comparator);
    }
    private static void sort<T>(int start, int end, T[] array, Comparator<T> comparator) {
        int length = end - start;
        Object[] inJ = new Object[end];
        Object[] outJ = new Object[end];
        java.lang.SystemJ.arraycopy(array, start, outJ, start, length);
        java.lang.SystemJ.arraycopy(array, start, inJ, start, length);
        if (comparator == null)
        {
            sort(start, end, inJ);
        } else {
            mergeSort(inJ, outJ, start, end, (Comparator<Object>)comparator);
        }
        for (int i = 0; i < outJ.Length; i++)
        {
            array[i] = (T)outJ[i];
        }
    }


    /**
     * Sorts the specified array using the specified {@code Comparator}. All elements
     * must be comparable to each other without a {@code ClassCastException} being thrown.
     * 
     * @param array
     *            the {@code Object} array to be sorted.
     * @param comparator
     *            the {@code Comparator}.
     * @throws ClassCastException
     *                if elements in the array cannot be compared to each other
     *                using the {@code Comparator}.
     */
    public static void sort<T>(T[] array, Comparator<T> comparator) {
        sort(0, array.Length, array, comparator);
    }
    /**
     * Sorts the specified range in the array in ascending natural order. All
     * elements must implement the {@code Comparable} interface and must be
     * comparable to each other without a {@code ClassCastException} being
     * thrown.
     * 
     * @param array
     *            the {@code Object} array to be sorted.
     * @param start
     *            the start index to sort.
     * @param end
     *            the last + 1 index to sort.
     * @throws ClassCastException
     *                if an element in the array does not implement {@code Comparable}
     *                or some elements cannot be compared to each other.
     * @throws IllegalArgumentException
     *                if {@code start > end}.
     * @throws ArrayIndexOutOfBoundsException
     *                if {@code start < 0} or {@code end > array.length}.
     */
    public static void sort(Object[] array, int start, int end)
    {
        if (array == null)
        {
            throw new java.lang.NullPointerException();
        }
        checkBounds(array.Length, start, end);
        sort(start, end, array);
    }

    private static void sort(int start, int end, Object[] array) {
        int length = end - start;
        if (length <= 0) {
            return;
        }
        if (array is String[]) {
            stableStringSort((String[]) array, start, end);
        } else {
            Object[] outJ = new Object[end];
            java.lang.SystemJ.arraycopy(array, start, outJ, start, length);
            mergeSort(outJ, array, start, end);
        }
    }

    /**
     * Performs a sort on the section of the array between the given indices
     * using a mergesort with exponential search algorithm (in which the merge
     * is performed by exponential search). n*log(n) performance is guaranteed
     * and in the average case it will be faster then any mergesort in which the
     * merge is performed by linear search.
     * 
     * @param in -
     *            the array for sorting.
     * @param out -
     *            the result, sorted array.
     * @param start
     *            the start index
     * @param end
     *            the end index + 1
     */
    private static void mergeSort(Object[] inJ, Object[] outJ, int start,
            int end) {
        int len = end - start;
        // use insertion sort for small arrays
        if (len <= SIMPLE_LENGTH) {
            for (int i = start + 1; i < end; i++) {
                java.lang.Comparable<Object> current = (java.lang.Comparable<Object>) outJ[i];
                Object prev = outJ[i - 1];
                if (current.compareTo(prev) < 0) {
                    int j = i;
                    do {
                        outJ[j--] = prev;
                    } while (j > start
                            && current.compareTo(prev = outJ[j - 1]) < 0);
                    outJ[j] = current;
                }
            }
            return;
        }
        int med = java.dotnet.lang.Operator.shiftRightUnsignet ((end + start) , 1);
        mergeSort(outJ, inJ, start, med);
        mergeSort(outJ, inJ, med, end);

        // merging

        // if arrays are already sorted - no merge
        if (((java.lang.Comparable<Object>) inJ[med - 1]).compareTo(inJ[med]) <= 0) {
            java.lang.SystemJ.arraycopy(inJ, start, outJ, start, len);
            return;
        }
        int r = med, i2 = start;

        // use merging with exponential search
        do {
            java.lang.Comparable<Object> fromVal = (java.lang.Comparable<Object>) inJ[start];
            java.lang.Comparable<Object> rVal = (java.lang.Comparable<Object>) inJ[r];
            if (fromVal.compareTo(rVal) <= 0) {
                int l_1 = find(inJ, rVal, -1, start + 1, med - 1);
                int toCopy = l_1 - start + 1;
                java.lang.SystemJ.arraycopy(inJ, start, outJ, i2, toCopy);
                i2 += toCopy;
                outJ[i2++] = rVal;
                r++;
                start = l_1 + 1;
            } else {
                int r_1 = find(inJ, fromVal, 0, r + 1, end - 1);
                int toCopy = r_1 - r + 1;
                java.lang.SystemJ.arraycopy(inJ, r, outJ, i2, toCopy);
                i2 += toCopy;
                outJ[i2++] = fromVal;
                start++;
                r = r_1 + 1;
            }
        } while ((end - r) > 0 && (med - start) > 0);

        // copy rest of array
        if ((end - r) <= 0) {
            java.lang.SystemJ.arraycopy(inJ, start, outJ, i2, med - start);
        } else {
            java.lang.SystemJ.arraycopy(inJ, r, outJ, i2, end - r);
        }
    }

    /**
     * Performs a sort on the section of the array between the given indices
     * using a mergesort with exponential search algorithm (in which the merge
     * is performed by exponential search). n*log(n) performance is guaranteed
     * and in the average case it will be faster then any mergesort in which the
     * merge is performed by linear search.
     * 
     * @param in -
     *            the array for sorting.
     * @param out -
     *            the result, sorted array.
     * @param start
     *            the start index
     * @param end
     *            the end index + 1
     * @param c -
     *            the comparator to determine the order of the array.
     */
    private static void mergeSort(Object[] inJ, Object[] outJ, int start,
            int end, Comparator<Object> c) {
        int len = end - start;
        // use insertion sort for small arrays
        if (len <= SIMPLE_LENGTH) {
            for (int i = start + 1; i < end; i++) {
                Object current = outJ[i];
                Object prev = outJ[i - 1];
                if (c.compare(prev, current) > 0) {
                    int j = i;
                    do {
                        outJ[j--] = prev;
                    } while (j > start
                            && (c.compare(prev = outJ[j - 1], current) > 0));
                    outJ[j] = current;
                }
            }
            return;
        }
        int med = java.dotnet.lang.Operator.shiftRightUnsignet ((end + start) , 1);
        mergeSort(outJ, inJ, start, med, c);
        mergeSort(outJ, inJ, med, end, c);

        // merging

        // if arrays are already sorted - no merge
        if (c.compare(inJ[med - 1],inJ[med] ) <= 0) {
            java.lang.SystemJ.arraycopy(inJ, start, outJ, start, len);
            return;
        }
        int r = med, i2 = start;

        // use merging with exponential search
        do {
            Object fromVal = inJ[start];
            Object rVal = inJ[r];
            if (c.compare(fromVal, rVal) <= 0) {
                int l_1 = find(inJ, rVal, -1, start + 1, med - 1, c);
                int toCopy = l_1 - start + 1;
                java.lang.SystemJ.arraycopy(inJ, start, outJ, i2, toCopy);
                i2 += toCopy;
                outJ[i2++] = rVal;
                r++;
                start = l_1 + 1;
            } else {
                int r_1 = find(inJ, fromVal, 0, r + 1, end - 1, c);
                int toCopy = r_1 - r + 1;
                java.lang.SystemJ.arraycopy(inJ, r, outJ, i2, toCopy);
                i2 += toCopy;
                outJ[i2++] = fromVal;
                start++;
                r = r_1 + 1;
            }
        } while ((end - r) > 0 && (med - start) > 0);

        // copy rest of array
        if ((end - r) <= 0) {
            java.lang.SystemJ.arraycopy(inJ, start, outJ, i2, med - start);
        } else {
            java.lang.SystemJ.arraycopy(inJ, r, outJ, i2, end - r);
        }
    }

    /**
     * Performs a sort on the given String array. Elements will be re-ordered into
     * ascending order.
     * 
     * @param arr -
     *            the array to sort
     * @param start -
     *            the start index
     * @param end -
     *            the end index + 1
     */
    private static void stableStringSort(String[] arr, int start,
            int end)
    {
        stableStringSort(arr, arr, new String[end], start, end, 0);
    }

    /**
     * Performs a sort on the given String array. Elements will be re-ordered into
     * ascending order. Uses a stable ternary quick sort algorithm.
     * 
     * @param arr -
     *            the array to sort
     * @param src -
     *            auxiliary array
     * @param dst -
     *            auxiliary array
     * @param start -
     *            the start index
     * @param end -
     *            the end index + 1
     * @param chId -
     *            index of char for current sorting
     */
    private static void stableStringSort(String[] arr, String[] src,
            String[] dst, int start, int end, int chId)
    {
        int length = end - start;
        // use insertion sort for small arrays
        if (length < SIMPLE_LENGTH)
        {
            if (src == arr)
            {
                for (int i = start + 1; i < end; i++)
                {
                    String current = arr[i];
                    String prev = arr[i - 1];
                    if (current.compareTo(prev) < 0)
                    {
                        int j = i;
                        do
                        {
                            arr[j--] = prev;
                        } while (j > start
                                && current.compareTo(prev = arr[j - 1]) < 0);
                        arr[j] = current;
                    }
                }
            }
            else
            {
                int actualEnd = end - 1;
                dst[start] = src[actualEnd--];
                for (int i = start + 1; i < end; i++, actualEnd--)
                {
                    String current = src[actualEnd];
                    String prev;
                    int j = i;
                    while (j > start
                            && current.compareTo(prev = dst[j - 1]) < 0)
                    {
                        dst[j--] = prev;
                    }
                    dst[j] = current;
                }
            }
            return;
        }
        // Approximate median
        int s;
        int mid = start + length / 2;
        int lo = start;
        int hi = end - 1;
        if (length > 40)
        {
            s = length / 8;
            lo = medChar(lo, lo + s, lo + s * 2, src, chId);
            mid = medChar(mid - s, mid, mid + s, src, chId);
            hi = medChar(hi, hi - s, hi - s * 2, src, chId);
        }
        mid = medChar(lo, mid, hi, src, chId);
        // median found
        // create 4 pointers <a (in star of src) ,
        // =b(in start of dst), >c (in end of dst)
        // i - current element;
        int midVal = charAt(src[mid], chId);
        int a, b, c;
        a = b = start;
        c = end - 1;
        int cmp;

        for (int i = start; i < end; i++)
        {
            String el = src[i];
            cmp = charAt(el, chId) - midVal;
            if (cmp < 0)
            {
                src[a] = el;
                a++;
            }
            else if (cmp > 0)
            {
                dst[c] = el;
                c--;
            }
            else
            {
                dst[b] = el;
                b++;
            }
        }

        s = b - start;
        if (s > 0)
        {
            if (arr == src)
            {
                java.lang.SystemJ.arraycopy(dst, start, arr, a, s);
            }
            else
            {
                copySwap(dst, start, arr, a, s);
            }

            if (b >= end && midVal == -1)
            {
                return;
            }
            stableStringSort(arr, arr, arr == dst ? src : dst, a, a + s,
                    chId + 1);
        }

        s = a - start;
        if (s > 0)
        {
            stableStringSort(arr, src, dst, start, a, chId);
        }

        c++;
        s = end - c;
        if (s > 0)
        {
            stableStringSort(arr, dst, src, c, end, chId);
        }
    }
    /*
     * returns the median index.
     */
    private static int medChar(int a, int b, int c, String[] arr, int id)
    {
        int ac = charAt(arr[a], id);
        int bc = charAt(arr[b], id);
        int cc = charAt(arr[c], id);
        return ac < bc ? (bc < cc ? b : (ac < cc ? c : a))
                : (bc < cc ? (ac < cc ? a : c) : b);

    }

    /*
     * Returns the char value at the specified index of string or -1 if the
     * index more than the length of this string.
     */
    private static int charAt(String str, int i)
    {
        if (i >= str.length())
        {
            return -1;
        }
        return str.charAt(i);
    }
    /**
     * Copies object from one array to another array with reverse of objects
     * order. Source and destination arrays may be the same.
     * 
     * @param src -
     *            the source array.
     * @param from -
     *            starting position in the source array.
     * @param dst -
     *            the destination array.
     * @param to -
     *            starting position in the destination array.
     * @param len -
     *            the number of array elements to be copied.
     */
    private static void copySwap(Object[] src, int from, Object[] dst, int to,
            int len)
    {
        if (src == dst && from + len > to)
        {
            int new_to = to + len - 1;
            for (; from < to; from++, new_to--, len--)
            {
                dst[new_to] = src[from];
            }
            for (; len > 1; from++, new_to--, len -= 2)
            {
                swap(from, new_to, dst);
            }

        }
        else
        {
            to = to + len - 1;
            for (; len > 0; from++, to--, len--)
            {
                dst[to] = src[from];
            }
        }
    }
    /**
     * Swaps the elements at the given indices in the array.
     * 
     * @param a -
     *            the index of one element to be swapped.
     * @param b -
     *            the index of the other element to be swapped.
     * @param arr -
     *            the array in which to swap elements.
     */
    private static void swap(int a, int b, Object[] arr)
    {
        Object tmp = arr[a];
        arr[a] = arr[b];
        arr[b] = tmp;
    }
    /**
     * Finds the place of specified range of specified sorted array, where the
     * element should be inserted for getting sorted array. Uses exponential
     * search algorithm.
     * 
     * @param arr -
     *            the array with already sorted range
     * @param val -
     *            object to be inserted
     * @param l -
     *            the start index
     * @param r -
     *            the end index
     * @param bnd -
     *            possible values 0,-1. "-1" - val is located at index more then
     *            elements equals to val. "0" - val is located at index less
     *            then elements equals to val.
     * @param c -
     *            the comparator used to compare Objects
     */
    private static int find(Object[] arr, Object val, int bnd, int l, int r,
            Comparator<Object> c) {
        int m = l;
        int d = 1;
        while (m <= r) {
            if (c.compare(val, arr[m]) > bnd) {
                l = m + 1;
            } else {
                r = m - 1;
                break;
            }
            m += d;
            d <<= 1;
        }
        while (l <= r) {
            m = java.dotnet.lang.Operator.shiftRightUnsignet ((l + r) , 1);
            if (c.compare(val, arr[m]) > bnd) {
                l = m + 1;
            } else {
                r = m - 1;
            }
        }
        return l - 1;
    }
    /**
     * Finds the place in the given range of specified sorted array, where the
     * element should be inserted for getting sorted array. Uses exponential
     * search algorithm.
     * 
     * @param arr -
     *            the array with already sorted range
     * 
     * @param val -
     *            object to be inserted
     * 
     * @param l -
     *            the start index
     * 
     * @param r -
     *            the end index
     * 
     * @param bnd -
     *            possible values 0,-1. "-1" - val is located at index more then
     *            elements equals to val. "0" - val is located at index less
     *            then elements equals to val.
     * 
     */
    private static int find(Object[] arr, java.lang.Comparable<Object> val, int bnd, int l, int r) {
        int m = l;
        int d = 1;
        while (m <= r) {
            if (val.compareTo(arr[m]) > bnd) {
                l = m + 1;
            } else {
                r = m - 1;
                break;
            }
            m += d;
            d <<= 1;
        }
        while (l <= r) {
            m = java.dotnet.lang.Operator.shiftRightUnsignet ((l + r) , 1);
            if (val.compareTo(arr[m]) > bnd) {
                l = m + 1;
            } else {
                r = m - 1;
            }
        }
        return l - 1;
    }

    }
}
