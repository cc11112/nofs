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
using org.apache.commons.collections.map;

namespace org.apache.commons.collections
{

    /** 
     * Provides utility methods and decorators for
     * {@link Map} and {@link SortedMap} instances.
     * <p>
     * It contains various type safe methods
     * as well as other useful features like deep copying.
     * <p>
     * It also provides the following decorators:
     *
     *  <ul>
     *  <li>{@link #fixedSizeMap(Map)}
     *  <li>{@link #fixedSizeSortedMap(SortedMap)}
     *  <li>{@link #lazyMap(Map,Factory)}
     *  <li>{@link #lazyMap(Map,Transformer)}
     *  <li>{@link #lazySortedMap(SortedMap,Factory)}
     *  <li>{@link #lazySortedMap(SortedMap,Transformer)}
     *  <li>{@link #predicatedMap(Map,Predicate,Predicate)}
     *  <li>{@link #predicatedSortedMap(SortedMap,Predicate,Predicate)}
     *  <li>{@link #transformedMap(Map, Transformer, Transformer)}
     *  <li>{@link #transformedSortedMap(SortedMap, Transformer, Transformer)}
     *  <li>{@link #typedMap(Map, Class, Class)}
     *  <li>{@link #typedSortedMap(SortedMap, Class, Class)}
     *  <li>{@link #multiValueMap( Map )}
     *  <li>{@link #multiValueMap( Map, Class )}
     *  <li>{@link #multiValueMap( Map, Factory )}
     *  </ul>
     *
     * @since Commons Collections 1.0
     * @version $Revision$ $Date$
     * 
     * @author <a href="mailto:jstrachan@apache.org">James Strachan</a>
     * @author <a href="mailto:nissim@nksystems.com">Nissim Karpenstein</a>
     * @author <a href="mailto:knielsen@apache.org">Kasper Nielsen</a>
     * @author Paul Jack
     * @author Stephen Colebourne
     * @author Matthew Hawthorne
     * @author Arun Mammen Thomas
     * @author Janek Bogucki
     * @author Max Rydahl Andersen
     * @author <a href="mailto:equinus100@hotmail.com">Ashwin S</a>
     * @author <a href="mailto:jcarman@apache.org">James Carman</a>
     * @author Neil O'Toole
     */
    public class MapUtils
    {

        /**
         * An empty unmodifiable map.
         * This was not provided in JDK1.2.
         */
        public static readonly java.util.Map<Object, Object> EMPTY_MAP = UnmodifiableMap.decorate(new java.util.HashMap<Object, Object>());
        /**
         * An empty unmodifiable sorted map.
         * This is not provided in the JDK.
         */
        public static readonly java.util.SortedMap<Object, Object> EMPTY_SORTED_MAP = UnmodifiableSortedMap.decorate(new java.util.TreeMap<Object, Object>());
        /**
         * String used to indent the verbose and debug Map prints.
         */
        private static readonly String INDENT_STRING = "    ";

        /**
         * <code>MapUtils</code> should not normally be instantiated.
         */
        public MapUtils()
        {
        }

        // Type safe getters
        //-------------------------------------------------------------------------
        /**
         * Gets from a Map in a null-safe manner.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map, <code>null</code> if null map input
         */
        public static Object getObject(java.util.Map<Object, Object> map, Object key)
        {
            if (map != null)
            {
                return map.get(key);
            }
            return null;
        }

        /**
         * Gets a String from a Map in a null-safe manner.
         * <p>
         * The String is obtained via <code>toString</code>.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as a String, <code>null</code> if null map input
         */
        public static String getString(java.util.Map<Object, Object> map, Object key)
        {
            if (map != null)
            {
                Object answer = map.get(key);
                if (answer != null)
                {
                    return answer.toString();
                }
            }
            return null;
        }

        /**
         * Gets a bool from a Map in a null-safe manner.
         * <p>
         * If the value is a <code>Boolean</code> it is returned directly.
         * If the value is a <code>String</code> and it equals 'true' ignoring case
         * then <code>true</code> is returned, otherwise <code>false</code>.
         * If the value is a <code>Number</code> an integer zero value returns
         * <code>false</code> and non-zero returns <code>true</code>.
         * Otherwise, <code>null</code> is returned.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as a Boolean, <code>null</code> if null map input
         */
        public static java.lang.Boolean getBoolean(java.util.Map<Object, Object> map, Object key)
        {
            if (map != null)
            {
                Object answer = map.get(key);
                if (answer != null)
                {
                    if (answer is java.lang.Boolean)
                    {
                        return (java.lang.Boolean)answer;

                    }
                    else if (answer is String)
                    {
                        return new java.lang.Boolean((String)answer);

                    }
                    else if (answer is java.lang.Number)
                    {
                        java.lang.Number n = (java.lang.Number)answer;
                        return (n.intValue() != 0) ? java.lang.Boolean.TRUE : java.lang.Boolean.FALSE;
                    }
                }
            }
            return null;
        }

        /**
         * Gets a Number from a Map in a null-safe manner.
         * <p>
         * If the value is a <code>Number</code> it is returned directly.
         * If the value is a <code>String</code> it is converted using
         * {@link NumberFormat#parse(String)} on the system default formatter
         * returning <code>null</code> if the conversion fails.
         * Otherwise, <code>null</code> is returned.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as a Number, <code>null</code> if null map input
         */
        public static java.lang.Number getNumber(java.util.Map<Object, Object> map, Object key)
        {
            if (map != null)
            {
                Object answer = map.get(key);
                if (answer != null)
                {
                    if (answer is java.lang.Number)
                    {
                        return (java.lang.Number)answer;

                    }
                    else if (answer is String)
                    {
                        try
                        {
                            String text = (String)answer;
                            

                            return java.text.NumberFormat.getInstance().parse(text);

                        }
                        catch (java.text.ParseException e)
                        {
                            logInfo(e);
                        }
                    }
                }
            }
            return null;
        }

        /**
         * Gets a Byte from a Map in a null-safe manner.
         * <p>
         * The Byte is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as a Byte, <code>null</code> if null map input
         */
        public static java.lang.Byte getByte(java.util.Map<Object, Object> map, Object key)
        {
            java.lang.Number answer = getNumber(map, key);
            if (answer == null)
            {
                return null;
            }
            else if (answer is java.lang.Byte)
            {
                return (java.lang.Byte)answer;
            }
            return new java.lang.Byte(answer.byteValue());
        }

        /**
         * Gets a Short from a Map in a null-safe manner.
         * <p>
         * The Short is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as a Short, <code>null</code> if null map input
         */
        public static java.lang.Short getShort(java.util.Map<Object,Object> map, Object key)
        {
            java.lang.Number answer = getNumber(map, key);
            if (answer == null)
            {
                return null;
            }
            else if (answer is java.lang.Short)
            {
                return (java.lang.Short)answer;
            }
            return new java.lang.Short(answer.shortValue());
        }

        /**
         * Gets a Integer from a Map in a null-safe manner.
         * <p>
         * The Integer is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as a Integer, <code>null</code> if null map input
         */
        public static java.lang.Integer getInteger(java.util.Map<Object, Object> map, Object key)
        {
            java.lang.Number answer = getNumber(map, key);
            if (answer == null)
            {
                return null;
            }
            else if (answer is java.lang.Integer)
            {
                return (java.lang.Integer)answer;
            }
            return new java.lang.Integer(answer.intValue());
        }

        /**
         * Gets a Long from a Map in a null-safe manner.
         * <p>
         * The Long is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as a Long, <code>null</code> if null map input
         */
        public static java.lang.Long getLong(java.util.Map<Object, Object> map, Object key)
        {
            java.lang.Number answer = getNumber(map, key);
            if (answer == null)
            {
                return null;
            }
            else if (answer is java.lang.Long)
            {
                return (java.lang.Long)answer;
            }
            return new java.lang.Long(answer.longValue());
        }

        /**
         * Gets a Float from a Map in a null-safe manner.
         * <p>
         * The Float is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as a Float, <code>null</code> if null map input
         */
        public static java.lang.Float getFloat(java.util.Map<Object, Object> map, Object key)
        {
            java.lang.Number answer = getNumber(map, key);
            if (answer == null)
            {
                return null;
            }
            else if (answer is java.lang.Float)
            {
                return (java.lang.Float)answer;
            }
            return new java.lang.Float(answer.floatValue());
        }

        /**
         * Gets a Double from a Map in a null-safe manner.
         * <p>
         * The Double is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as a Double, <code>null</code> if null map input
         */
        public static java.lang.Double getDouble(java.util.Map<Object, Object> map, Object key)
        {
            java.lang.Number answer = getNumber(map, key);
            if (answer == null)
            {
                return null;
            }
            else if (answer is java.lang.Double)
            {
                return (java.lang.Double)answer;
            }
            return new java.lang.Double(answer.doubleValue());
        }

        /**
         * Gets a Map from a Map in a null-safe manner.
         * <p>
         * If the value returned from the specified map is not a Map then
         * <code>null</code> is returned.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as a Map, <code>null</code> if null map input
         */
        public static java.util.Map<Object, Object> getMap(java.util.Map<Object, Object> map, Object key)
        {
            if (map != null)
            {
                Object answer = map.get(key);
                if (answer != null && answer is java.util.Map<Object, Object>)
                {
                    return (java.util.Map<Object, Object>)answer;
                }
            }
            return null;
        }

        // Type safe getters with default values
        //-------------------------------------------------------------------------
        /**
         *  Looks up the given key in the given map, converting null into the
         *  given default value.
         *
         *  @param map  the map whose value to look up
         *  @param key  the key of the value to look up in that map
         *  @param defaultValue  what to return if the value is null
         *  @return  the value in the map, or defaultValue if the original value
         *    is null or the map is null
         */
        public static Object getObject(java.util.Map<Object, Object> map, Object key, Object defaultValue)
        {
            if (map != null)
            {
                Object answer = map.get(key);
                if (answer != null)
                {
                    return answer;
                }
            }
            return defaultValue;
        }

        /**
         *  Looks up the given key in the given map, converting the result into
         *  a string, using the default value if the the conversion fails.
         *
         *  @param map  the map whose value to look up
         *  @param key  the key of the value to look up in that map
         *  @param defaultValue  what to return if the value is null or if the
         *     conversion fails
         *  @return  the value in the map as a string, or defaultValue if the 
         *    original value is null, the map is null or the string conversion
         *    fails
         */
        public static String getString(java.util.Map<Object, Object> map, Object key, String defaultValue)
        {
            String answer = getString(map, key);
            if (answer == null)
            {
                answer = defaultValue;
            }
            return answer;
        }

        /**
         *  Looks up the given key in the given map, converting the result into
         *  a boolean, using the default value if the the conversion fails.
         *
         *  @param map  the map whose value to look up
         *  @param key  the key of the value to look up in that map
         *  @param defaultValue  what to return if the value is null or if the
         *     conversion fails
         *  @return  the value in the map as a boolean, or defaultValue if the 
         *    original value is null, the map is null or the bool conversion
         *    fails
         */
        public static bool getBoolean(java.util.Map<Object, Object> map, Object key, bool defaultValue)
        {
            java.lang.Boolean answer = getBoolean(map, key);
            if (answer == null)
            {
                answer = new java.lang.Boolean(defaultValue);
            }
            return answer.booleanValue();
        }

        /**
         *  Looks up the given key in the given map, converting the result into
         *  a number, using the default value if the the conversion fails.
         *
         *  @param map  the map whose value to look up
         *  @param key  the key of the value to look up in that map
         *  @param defaultValue  what to return if the value is null or if the
         *     conversion fails
         *  @return  the value in the map as a number, or defaultValue if the 
         *    original value is null, the map is null or the number conversion
         *    fails
         */
        public static java.lang.Number getNumber(java.util.Map<Object, Object> map, Object key, java.lang.Number defaultValue)
        {
            java.lang.Number answer = getNumber(map, key);
            if (answer == null)
            {
                answer = defaultValue;
            }
            return answer;
        }

        /**
         *  Looks up the given key in the given map, converting the result into
         *  a byte, using the default value if the the conversion fails.
         *
         *  @param map  the map whose value to look up
         *  @param key  the key of the value to look up in that map
         *  @param defaultValue  what to return if the value is null or if the
         *     conversion fails
         *  @return  the value in the map as a number, or defaultValue if the 
         *    original value is null, the map is null or the number conversion
         *    fails
         */
        public static java.lang.Byte getByte(java.util.Map<Object, Object> map, Object key, java.lang.Byte defaultValue)
        {
            java.lang.Byte answer = getByte(map, key);
            if (answer == null)
            {
                answer = defaultValue;
            }
            return answer;
        }

        /**
         *  Looks up the given key in the given map, converting the result into
         *  a short, using the default value if the the conversion fails.
         *
         *  @param map  the map whose value to look up
         *  @param key  the key of the value to look up in that map
         *  @param defaultValue  what to return if the value is null or if the
         *     conversion fails
         *  @return  the value in the map as a number, or defaultValue if the 
         *    original value is null, the map is null or the number conversion
         *    fails
         */
        public static java.lang.Short getShort(java.util.Map<Object, Object> map, Object key, java.lang.Short defaultValue)
        {
            java.lang.Short answer = getShort(map, key);
            if (answer == null)
            {
                answer = defaultValue;
            }
            return answer;
        }

        /**
         *  Looks up the given key in the given map, converting the result into
         *  an integer, using the default value if the the conversion fails.
         *
         *  @param map  the map whose value to look up
         *  @param key  the key of the value to look up in that map
         *  @param defaultValue  what to return if the value is null or if the
         *     conversion fails
         *  @return  the value in the map as a number, or defaultValue if the 
         *    original value is null, the map is null or the number conversion
         *    fails
         */
        public static java.lang.Integer getInteger(java.util.Map<Object, Object> map, Object key, java.lang.Integer defaultValue)
        {
            java.lang.Integer answer = getInteger(map, key);
            if (answer == null)
            {
                answer = defaultValue;
            }
            return answer;
        }

        /**
         *  Looks up the given key in the given map, converting the result into
         *  a long, using the default value if the the conversion fails.
         *
         *  @param map  the map whose value to look up
         *  @param key  the key of the value to look up in that map
         *  @param defaultValue  what to return if the value is null or if the
         *     conversion fails
         *  @return  the value in the map as a number, or defaultValue if the 
         *    original value is null, the map is null or the number conversion
         *    fails
         */
        public static java.lang.Long getLong(java.util.Map<Object, Object> map, Object key, java.lang.Long defaultValue)
        {
            java.lang.Long answer = getLong(map, key);
            if (answer == null)
            {
                answer = defaultValue;
            }
            return answer;
        }

        /**
         *  Looks up the given key in the given map, converting the result into
         *  a float, using the default value if the the conversion fails.
         *
         *  @param map  the map whose value to look up
         *  @param key  the key of the value to look up in that map
         *  @param defaultValue  what to return if the value is null or if the
         *     conversion fails
         *  @return  the value in the map as a number, or defaultValue if the 
         *    original value is null, the map is null or the number conversion
         *    fails
         */
        public static java.lang.Float getFloat(java.util.Map<Object, Object> map, Object key, java.lang.Float defaultValue)
        {
            java.lang.Float answer = getFloat(map, key);
            if (answer == null)
            {
                answer = defaultValue;
            }
            return answer;
        }

        /**
         *  Looks up the given key in the given map, converting the result into
         *  a double, using the default value if the the conversion fails.
         *
         *  @param map  the map whose value to look up
         *  @param key  the key of the value to look up in that map
         *  @param defaultValue  what to return if the value is null or if the
         *     conversion fails
         *  @return  the value in the map as a number, or defaultValue if the 
         *    original value is null, the map is null or the number conversion
         *    fails
         */
        public static java.lang.Double getDouble(java.util.Map<Object, Object> map, Object key, java.lang.Double defaultValue)
        {
            java.lang.Double answer = getDouble(map, key);
            if (answer == null)
            {
                answer = defaultValue;
            }
            return answer;
        }

        /**
         *  Looks up the given key in the given map, converting the result into
         *  a map, using the default value if the the conversion fails.
         *
         *  @param map  the map whose value to look up
         *  @param key  the key of the value to look up in that map
         *  @param defaultValue  what to return if the value is null or if the
         *     conversion fails
         *  @return  the value in the map as a number, or defaultValue if the 
         *    original value is null, the map is null or the map conversion
         *    fails
         */
        public static java.util.Map<Object, Object> getMap(java.util.Map<Object, Object> map, Object key, java.util.Map<Object, Object> defaultValue)
        {
            java.util.Map<Object, Object> answer = getMap(map, key);
            if (answer == null)
            {
                answer = defaultValue;
            }
            return answer;
        }


        // Type safe primitive getters
        //-------------------------------------------------------------------------
        /**
         * Gets a bool from a Map in a null-safe manner.
         * <p>
         * If the value is a <code>Boolean</code> its value is returned.
         * If the value is a <code>String</code> and it equals 'true' ignoring case
         * then <code>true</code> is returned, otherwise <code>false</code>.
         * If the value is a <code>Number</code> an integer zero value returns
         * <code>false</code> and non-zero returns <code>true</code>.
         * Otherwise, <code>false</code> is returned.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as a Boolean, <code>false</code> if null map input
         */
        public static bool getBooleanValue(java.util.Map<Object, Object> map, Object key)
        {
            java.lang.Boolean booleanObject = getBoolean(map, key);
            if (booleanObject == null)
            {
                return false;
            }
            return booleanObject.booleanValue();
        }

        /**
         * Gets a byte from a Map in a null-safe manner.
         * <p>
         * The byte is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as a byte, <code>0</code> if null map input
         */
        public static byte getByteValue(java.util.Map<Object, Object> map, Object key)
        {
            java.lang.Byte byteObject = getByte(map, key);
            if (byteObject == null)
            {
                return 0;
            }
            return byteObject.byteValue();
        }

        /**
         * Gets a short from a Map in a null-safe manner.
         * <p>
         * The short is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as a short, <code>0</code> if null map input
         */
        public static short getShortValue(java.util.Map<Object, Object> map, Object key)
        {
            java.lang.Short shortObject = getShort(map, key);
            if (shortObject == null)
            {
                return 0;
            }
            return shortObject.shortValue();
        }

        /**
         * Gets an int from a Map in a null-safe manner.
         * <p>
         * The int is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as an int, <code>0</code> if null map input
         */
        public static int getIntValue(java.util.Map<Object, Object> map, Object key)
        {
            java.lang.Integer integerObject = getInteger(map, key);
            if (integerObject == null)
            {
                return 0;
            }
            return integerObject.intValue();
        }

        /**
         * Gets a long from a Map in a null-safe manner.
         * <p>
         * The long is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as a long, <code>0L</code> if null map input
         */
        public static long getLongValue(java.util.Map<Object, Object> map, Object key)
        {
            java.lang.Long longObject = getLong(map, key);
            if (longObject == null)
            {
                return 0L;
            }
            return longObject.longValue();
        }

        /**
         * Gets a float from a Map in a null-safe manner.
         * <p>
         * The float is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as a float, <code>0.0F</code> if null map input
         */
        public static float getFloatValue(java.util.Map<Object, Object> map, Object key)
        {
            java.lang.Float floatObject = getFloat(map, key);
            if (floatObject == null)
            {
                return 0f;
            }
            return floatObject.floatValue();
        }

        /**
         * Gets a double from a Map in a null-safe manner.
         * <p>
         * The double is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @return the value in the Map as a double, <code>0.0</code> if null map input
         */
        public static double getDoubleValue(java.util.Map<Object, Object> map, Object key)
        {
            java.lang.Double doubleObject = getDouble(map, key);
            if (doubleObject == null)
            {
                return 0d;
            }
            return doubleObject.doubleValue();
        }

        // Type safe primitive getters with default values
        //-------------------------------------------------------------------------
        /**
         * Gets a bool from a Map in a null-safe manner,
         * using the default value if the the conversion fails.
         * <p>
         * If the value is a <code>Boolean</code> its value is returned.
         * If the value is a <code>String</code> and it equals 'true' ignoring case
         * then <code>true</code> is returned, otherwise <code>false</code>.
         * If the value is a <code>Number</code> an integer zero value returns
         * <code>false</code> and non-zero returns <code>true</code>.
         * Otherwise, <code>defaultValue</code> is returned.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @param defaultValue  return if the value is null or if the
         *     conversion fails
         * @return the value in the Map as a Boolean, <code>defaultValue</code> if null map input
         */
        public static bool getBooleanValue(java.util.Map<Object, Object> map, Object key, bool defaultValue)
        {
            java.lang.Boolean booleanObject = getBoolean(map, key);
            if (booleanObject == null)
            {
                return defaultValue;
            }
            return booleanObject.booleanValue();
        }

        /**
         * Gets a byte from a Map in a null-safe manner,
         * using the default value if the the conversion fails.     
         * <p>
         * The byte is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @param defaultValue  return if the value is null or if the
         *     conversion fails
         * @return the value in the Map as a byte, <code>defaultValue</code> if null map input
         */
        public static byte getByteValue(java.util.Map<Object, Object> map, Object key, byte defaultValue)
        {
            java.lang.Byte byteObject = getByte(map, key);
            if (byteObject == null)
            {
                return defaultValue;
            }
            return byteObject.byteValue();
        }

        /**
         * Gets a short from a Map in a null-safe manner,
         * using the default value if the the conversion fails.     
         * <p>
         * The short is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @param defaultValue  return if the value is null or if the
         *     conversion fails
         * @return the value in the Map as a short, <code>defaultValue</code> if null map input
         */
        public static short getShortValue(java.util.Map<Object, Object> map, Object key, short defaultValue)
        {
            java.lang.Short shortObject = getShort(map, key);
            if (shortObject == null)
            {
                return defaultValue;
            }
            return shortObject.shortValue();
        }

        /**
         * Gets an int from a Map in a null-safe manner,
         * using the default value if the the conversion fails.     
         * <p>
         * The int is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @param defaultValue  return if the value is null or if the
         *     conversion fails
         * @return the value in the Map as an int, <code>defaultValue</code> if null map input
         */
        public static int getIntValue(java.util.Map<Object, Object> map, Object key, int defaultValue)
        {
            java.lang.Integer integerObject = getInteger(map, key);
            if (integerObject == null)
            {
                return defaultValue;
            }
            return integerObject.intValue();
        }

        /**
         * Gets a long from a Map in a null-safe manner,
         * using the default value if the the conversion fails.     
         * <p>
         * The long is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @param defaultValue  return if the value is null or if the
         *     conversion fails
         * @return the value in the Map as a long, <code>defaultValue</code> if null map input
         */
        public static long getLongValue(java.util.Map<Object, Object> map, Object key, long defaultValue)
        {
            java.lang.Long longObject = getLong(map, key);
            if (longObject == null)
            {
                return defaultValue;
            }
            return longObject.longValue();
        }

        /**
         * Gets a float from a Map in a null-safe manner,
         * using the default value if the the conversion fails.     
         * <p>
         * The float is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @param defaultValue  return if the value is null or if the
         *     conversion fails
         * @return the value in the Map as a float, <code>defaultValue</code> if null map input
         */
        public static float getFloatValue(java.util.Map<Object, Object> map, Object key, float defaultValue)
        {
            java.lang.Float floatObject = getFloat(map, key);
            if (floatObject == null)
            {
                return defaultValue;
            }
            return floatObject.floatValue();
        }

        /**
         * Gets a double from a Map in a null-safe manner,
         * using the default value if the the conversion fails.     
         * <p>
         * The double is obtained from the results of {@link #getNumber(Map,Object)}.
         *
         * @param map  the map to use
         * @param key  the key to look up
         * @param defaultValue  return if the value is null or if the
         *     conversion fails
         * @return the value in the Map as a double, <code>defaultValue</code> if null map input
         */
        public static double getDoubleValue(java.util.Map<Object, Object> map, Object key, double defaultValue)
        {
            java.lang.Double doubleObject = getDouble(map, key);
            if (doubleObject == null)
            {
                return defaultValue;
            }
            return doubleObject.doubleValue();
        }

        // Conversion methods
        //-------------------------------------------------------------------------
        /**
         * Gets a new Properties object initialised with the values from a Map.
         * A null input will return an empty properties object.
         * 
         * @param map  the map to convert to a Properties object, may not be null
         * @return the properties object
         */
        public static java.util.Properties toProperties(java.util.Map<Object, Object> map)
        {
            java.util.Properties answer = new java.util.Properties();
            if (map != null)
            {
                for (java.util.Iterator<java.util.MapNS.Entry<Object, Object>> iter = map.entrySet().iterator(); iter.hasNext(); )
                {
                    java.util.MapNS.Entry<Object, Object> entry = iter.next();
                    Object key = entry.getKey();
                    Object value = entry.getValue();
                    answer.put(key, value);
                }
            }
            return answer;
        }

        /**
         * Creates a new HashMap using data copied from a ResourceBundle.
         * 
         * @param resourceBundle  the resource bundle to convert, may not be null
         * @return the hashmap containing the data
         * @throws NullPointerException if the bundle is null
         */
        public static java.util.Map<Object, Object> toMap(java.util.ResourceBundle resourceBundle)
        {
            java.util.Enumeration<Object> enumeration = resourceBundle.getKeys();
            java.util.Map<Object, Object> map = new java.util.HashMap<Object,Object>();

            while (enumeration.hasMoreElements())
            {
                String key = (String)enumeration.nextElement();
                Object value = resourceBundle.getObject(key);
                map.put(key, value);
            }

            return map;
        }

        // Printing methods
        //-------------------------------------------------------------------------
        /**
         * Prints the given map with nice line breaks.
         * <p>
         * This method prints a nicely formatted String describing the Map.
         * Each map entry will be printed with key and value.
         * When the value is a Map, recursive behaviour occurs.
         * <p>
         * This method is NOT thread-safe in any special way. You must manually
         * synchronize on either this class or the stream as required.
         *
         * @param out  the stream to print to, must not be null
         * @param label  The label to be used, may be <code>null</code>.
         *  If <code>null</code>, the label is not output.
         *  It typically represents the name of the property in a bean or similar.
         * @param map  The map to print, may be <code>null</code>.
         *  If <code>null</code>, the text 'null' is output.
         * @throws NullPointerException if the stream is <code>null</code>
         */
        public static void verbosePrint(
            java.lang.PrintStream outJ,
            Object label,
            java.util.Map<Object, Object> map)
        {

            verbosePrintInternal(outJ, label, map, new ArrayStack(), false);
        }

        /**
         * Prints the given map with nice line breaks.
         * <p>
         * This method prints a nicely formatted String describing the Map.
         * Each map entry will be printed with key, value and value classname.
         * When the value is a Map, recursive behaviour occurs.
         * <p>
         * This method is NOT thread-safe in any special way. You must manually
         * synchronize on either this class or the stream as required.
         *
         * @param out  the stream to print to, must not be null
         * @param label  The label to be used, may be <code>null</code>.
         *  If <code>null</code>, the label is not output.
         *  It typically represents the name of the property in a bean or similar.
         * @param map  The map to print, may be <code>null</code>.
         *  If <code>null</code>, the text 'null' is output.
         * @throws NullPointerException if the stream is <code>null</code>
         */
        public static void debugPrint(
            java.lang.PrintStream outJ,
            Object label,
            java.util.Map<Object, Object> map)
        {

            verbosePrintInternal(outJ, label, map, new ArrayStack(), true);
        }

        // Implementation methods
        //-------------------------------------------------------------------------
        /**
         * Logs the given exception to <code>System.out</code>.
         * <p>
         * This method exists as Jakarta Collections does not depend on logging.
         *
         * @param ex  the exception to log
         */
        protected static void logInfo(Exception ex)
        {
            java.lang.SystemJ.outJ.println("INFO: Exception: " + ex);
        }

        /**
         * Implementation providing functionality for {@link #debugPrint} and for 
         * {@link #verbosePrint}.  This prints the given map with nice line breaks.
         * If the debug flag is true, it additionally prints the type of the object 
         * value.  If the contents of a map include the map itself, then the text 
         * <em>(this Map)</em> is printed out.  If the contents include a 
         * parent container of the map, the the text <em>(ancestor[i] Map)</em> is 
         * printed, where i actually indicates the number of levels which must be 
         * traversed in the sequential list of ancestors (e.g. father, grandfather, 
         * great-grandfather, etc).  
         *
         * @param out  the stream to print to
         * @param label  the label to be used, may be <code>null</code>.
         *  If <code>null</code>, the label is not output.
         *  It typically represents the name of the property in a bean or similar.
         * @param map  the map to print, may be <code>null</code>.
         *  If <code>null</code>, the text 'null' is output
         * @param lineage  a stack consisting of any maps in which the previous 
         *  argument is contained. This is checked to avoid infinite recursion when
         *  printing the output
         * @param debug  flag indicating whether type names should be output.
         * @throws NullPointerException if the stream is <code>null</code>
         */
        private static void verbosePrintInternal(
            java.lang.PrintStream outJ,
            Object label,
            java.util.Map<Object, Object> map,
            ArrayStack lineage,
            bool debug)
        {

            printIndent(outJ, lineage.size());

            if (map == null)
            {
                if (label != null)
                {
                    outJ.print(label);
                    outJ.print(" = ");
                }
                outJ.println("null");
                return;
            }
            if (label != null)
            {
                outJ.print(label);
                outJ.println(" = ");
            }

            printIndent(outJ, lineage.size());
            outJ.println("{");

            lineage.push(map);

            for (java.util.Iterator<java.util.MapNS.Entry<Object, Object>> it = map.entrySet().iterator(); it.hasNext(); )
            {
                java.util.MapNS.Entry<Object, Object> entry = it.next();
                Object childKey = entry.getKey();
                Object childValue = entry.getValue();
                if (childValue is java.util.Map<Object, Object> && !lineage.contains(childValue))
                {
                    verbosePrintInternal(
                        outJ,
                        (childKey == null ? "null" : childKey),
                        (java.util.Map<Object, Object>)childValue,
                        lineage,
                        debug);
                }
                else
                {
                    printIndent(outJ, lineage.size());
                    outJ.print(childKey);
                    outJ.print(" = ");

                    int lineageIndex = lineage.indexOf(childValue);
                    if (lineageIndex == -1)
                    {
                        outJ.print(childValue);
                    }
                    else if (lineage.size() - 1 == lineageIndex)
                    {
                        outJ.print("(this Map)");
                    }
                    else
                    {
                        outJ.print(
                            "(ancestor["
                                + (lineage.size() - 1 - lineageIndex - 1)
                                + "] Map)");
                    }

                    if (debug && childValue != null)
                    {
                        outJ.print(' ');
                        outJ.println(childValue.getClass().getName());
                    }
                    else
                    {
                        outJ.println();
                    }
                }
            }

            lineage.pop();

            printIndent(outJ, lineage.size());
            outJ.println(debug ? "} " + map.getClass().getName() : "}");
        }

        /**
         * Writes indentation to the given stream.
         *
         * @param out  the stream to indent
         */
        private static void printIndent(java.lang.PrintStream outJ, int indent)
        {
            for (int i = 0; i < indent; i++)
            {
                outJ.print(INDENT_STRING);
            }
        }

        // Misc
        //-----------------------------------------------------------------------
        /**
         * Inverts the supplied map returning a new HashMap such that the keys of
         * the input are swapped with the values.
         * <p>
         * This operation assumes that the inverse mapping is well defined.
         * If the input map had multiple entries with the same value mapped to
         * different keys, the returned map will map one of those keys to the 
         * value, but the exact key which will be mapped is undefined.
         *
         * @param map  the map to invert, may not be null
         * @return a new HashMap containing the inverted data
         * @throws NullPointerException if the map is null
         */
        public static java.util.Map<Object, Object> invertMap(java.util.Map<Object, Object> map)
        {
            java.util.HashMap<Object, Object> outJ = new java.util.HashMap<Object, Object>(map.size());
            for (java.util.Iterator<java.util.MapNS.Entry<Object, Object>> it = map.entrySet().iterator(); it.hasNext(); )
            {
                java.util.MapNS.Entry<Object, Object> entry = it.next();
                outJ.put(entry.getValue(), entry.getKey());
            }
            return outJ;
        }

        //-----------------------------------------------------------------------
        /**
         * Protects against adding null values to a map.
         * <p>
         * This method checks the value being added to the map, and if it is null
         * it is replaced by an empty string.
         * <p>
         * This could be useful if the map does not accept null values, or for
         * receiving data from a source that may provide null or empty string
         * which should be held in the same way in the map.
         * <p>
         * Keys are not validated.
         * 
         * @param map  the map to add to, may not be null
         * @param key  the key
         * @param value  the value, null converted to ""
         * @throws NullPointerException if the map is null
         */
        public static void safeAddToMap(java.util.Map<Object, Object> map, Object key, Object value)
        {//throws NullPointerException {
            if (value == null)
            {
                map.put(key, "");
            }
            else
            {
                map.put(key, value);
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Puts all the keys and values from the specified array into the map.
         * <p>
         * This method is an alternative to the {@link java.util.Map#putAll(java.util.Map)}
         * method and constructors. It allows you to build a map from an object array
         * of various possible styles.
         * <p>
         * If the first entry in the object array implements {@link java.util.Map.Entry}
         * or {@link KeyValue} then the key and value are added from that object.
         * If the first entry in the object array is an object array itself, then
         * it is assumed that index 0 in the sub-array is the key and index 1 is the value.
         * Otherwise, the array is treated as keys and values in alternate indices.
         * <p>
         * For example, to create a color map:
         * <pre>
         * Map colorMap = MapUtils.putAll(new HashMap(), new String[][] {
         *     {"RED", "#FF0000"},
         *     {"GREEN", "#00FF00"},
         *     {"BLUE", "#0000FF"}
         * });
         * </pre>
         * or:
         * <pre>
         * Map colorMap = MapUtils.putAll(new HashMap(), new String[] {
         *     "RED", "#FF0000",
         *     "GREEN", "#00FF00",
         *     "BLUE", "#0000FF"
         * });
         * </pre>
         * or:
         * <pre>
         * Map colorMap = MapUtils.putAll(new HashMap(), new Map.Entry[] {
         *     new DefaultMapEntry("RED", "#FF0000"),
         *     new DefaultMapEntry("GREEN", "#00FF00"),
         *     new DefaultMapEntry("BLUE", "#0000FF")
         * });
         * </pre>
         *
         * @param map  the map to populate, must not be null
         * @param array  an array to populate from, null ignored
         * @return the input map
         * @throws NullPointerException  if map is null
         * @throws IllegalArgumentException  if sub-array or entry matching used and an
         *  entry is invalid
         * @throws ClassCastException if the array contents is mixed
         * @since Commons Collections 3.2
         */
        public static java.util.Map<Object, Object> putAll(java.util.Map<Object, Object> map, Object[] array)
        {
            map.size();  // force NPE
            if (array == null || array.Length == 0)
            {
                return map;
            }
            Object obj = array[0];
            if (obj is java.util.MapNS.Entry<Object, Object>)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)array[i];
                    map.put(entry.getKey(), entry.getValue());
                }
            }
            else if (obj is KeyValue)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    KeyValue keyval = (KeyValue)array[i];
                    map.put(keyval.getKey(), keyval.getValue());
                }
            }
            else if (obj is Object[])
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Object[] sub = (Object[])array[i];
                    if (sub == null || sub.Length < 2)
                    {
                        throw new java.lang.IllegalArgumentException("Invalid array element: " + i);
                    }
                    map.put(sub[0], sub[1]);
                }
            }
            else
            {
                for (int i = 0; i < array.Length - 1; )
                {
                    map.put(array[i++], array[i++]);
                }
            }
            return map;
        }

        //-----------------------------------------------------------------------
        /**
         * Null-safe check if the specified map is empty.
         * <p>
         * Null returns true.
         * 
         * @param map  the map to check, may be null
         * @return true if empty or null
         * @since Commons Collections 3.2
         */
        public static bool isEmpty(java.util.Map<Object, Object> map)
        {
            return (map == null || map.isEmpty());
        }

        /**
         * Null-safe check if the specified map is not empty.
         * <p>
         * Null returns false.
         * 
         * @param map  the map to check, may be null
         * @return true if non-null and non-empty
         * @since Commons Collections 3.2
         */
        public static bool isNotEmpty(java.util.Map<Object, Object> map)
        {
            return !MapUtils.isEmpty(map);
        }

        // Map decorators
        //-----------------------------------------------------------------------
        /**
         * Returns a synchronized map backed by the given map.
         * <p>
         * You must manually synchronize on the returned buffer's iterator to 
         * avoid non-deterministic behavior:
         *  
         * <pre>
         * Map m = MapUtils.synchronizedMap(myMap);
         * Set s = m.keySet();  // outside synchronized block
         * synchronized (m) {  // synchronized on MAP!
         *     Iterator i = s.iterator();
         *     while (i.hasNext()) {
         *         process (i.next());
         *     }
         * }
         * </pre>
         * 
         * This method uses the implementation in {@link java.util.Collections Collections}.
         * 
         * @param map  the map to synchronize, must not be null
         * @return a synchronized map backed by the given map
         * @throws IllegalArgumentException  if the map is null
         */
        public static java.util.Map<Object, Object> synchronizedMap(java.util.Map<Object, Object> map)
        {
            return java.util.Collections.synchronizedMap(map);
        }

        /**
         * Returns an unmodifiable map backed by the given map.
         * <p>
         * This method uses the implementation in the decorators subpackage.
         *
         * @param map  the map to make unmodifiable, must not be null
         * @return an unmodifiable map backed by the given map
         * @throws IllegalArgumentException  if the map is null
         */
        public static java.util.Map<Object, Object> unmodifiableMap(java.util.Map<Object, Object> map)
        {
            return UnmodifiableMap.decorate(map);
        }

        /**
         * Returns a predicated (validating) map backed by the given map.
         * <p>
         * Only objects that pass the tests in the given predicates can be added to the map.
         * Trying to add an invalid object results in an IllegalArgumentException.
         * Keys must pass the key predicate, values must pass the value predicate.
         * It is important not to use the original map after invoking this method,
         * as it is a backdoor for adding invalid objects.
         *
         * @param map  the map to predicate, must not be null
         * @param keyPred  the predicate for keys, null means no check
         * @param valuePred  the predicate for values, null means no check
         * @return a predicated map backed by the given map
         * @throws IllegalArgumentException  if the Map is null
         */
        public static java.util.Map<Object, Object> predicatedMap(java.util.Map<Object, Object> map, Predicate keyPred, Predicate valuePred)
        {
            return PredicatedMap.decorate(map, keyPred, valuePred);
        }

        /**
         * Returns a typed map backed by the given map.
         * <p>
         * Only keys and values of the specified types can be added to the map.
         * 
         * @param map  the map to limit to a specific type, must not be null
         * @param keyType  the type of keys which may be added to the map, must not be null
         * @param valueType  the type of values which may be added to the map, must not be null
         * @return a typed map backed by the specified map
         * @throws IllegalArgumentException  if the Map or Class is null
         */
        public static java.util.Map<Object, Object> typedMap(java.util.Map<Object, Object> map, java.lang.Class keyType, java.lang.Class valueType)
        {
            return TypedMap.decorate(map, keyType, valueType);
        }

        /**
         * Returns a transformed map backed by the given map.
         * <p>
         * This method returns a new map (decorating the specified map) that
         * will transform any new entries added to it.
         * Existing entries in the specified map will not be transformed.
         * If you want that behaviour, see {@link TransformedMap#decorateTransform}.
         * <p>
         * Each object is passed through the transformers as it is added to the
         * Map. It is important not to use the original map after invoking this 
         * method, as it is a backdoor for adding untransformed objects.
         * <p>
         * If there are any elements already in the map being decorated, they
         * are NOT transformed.
         *
         * @param map  the map to transform, must not be null, typically empty
         * @param keyTransformer  the transformer for the map keys, null means no transformation
         * @param valueTransformer  the transformer for the map values, null means no transformation
         * @return a transformed map backed by the given map
         * @throws IllegalArgumentException  if the Map is null
         */
        public static java.util.Map<Object, Object> transformedMap(java.util.Map<Object, Object> map, Transformer keyTransformer, Transformer valueTransformer)
        {
            return TransformedMap.decorate(map, keyTransformer, valueTransformer);
        }

        /**
         * Returns a fixed-sized map backed by the given map.
         * Elements may not be added or removed from the returned map, but 
         * existing elements can be changed (for instance, via the 
         * {@link Map#put(Object,Object)} method).
         *
         * @param map  the map whose size to fix, must not be null
         * @return a fixed-size map backed by that map
         * @throws IllegalArgumentException  if the Map is null
         */
        public static java.util.Map<Object, Object> fixedSizeMap(java.util.Map<Object, Object> map)
        {
            return FixedSizeMap.decorate(map);
        }

        /**
         * Returns a "lazy" map whose values will be created on demand.
         * <p>
         * When the key passed to the returned map's {@link Map#get(Object)}
         * method is not present in the map, then the factory will be used
         * to create a new object and that object will become the value
         * associated with that key.
         * <p>
         * For instance:
         * <pre>
         * Factory factory = new Factory() {
         *     public Object create() {
         *         return new Date();
         *     }
         * }
         * Map lazyMap = MapUtils.lazyMap(new HashMap(), factory);
         * Object obj = lazyMap.get("test");
         * </pre>
         *
         * After the above code is executed, <code>obj</code> will contain
         * a new <code>Date</code> instance.  Furthermore, that <code>Date</code>
         * instance is the value for the <code>"test"</code> key in the map.
         *
         * @param map  the map to make lazy, must not be null
         * @param factory  the factory for creating new objects, must not be null
         * @return a lazy map backed by the given map
         * @throws IllegalArgumentException  if the Map or Factory is null
         */
        public static java.util.Map<Object, Object> lazyMap(java.util.Map<Object, Object> map, Factory factory)
        {
            return LazyMap.decorate(map, factory);
        }

        /**
         * Returns a "lazy" map whose values will be created on demand.
         * <p>
         * When the key passed to the returned map's {@link Map#get(Object)}
         * method is not present in the map, then the factory will be used
         * to create a new object and that object will become the value
         * associated with that key. The factory is a {@link Transformer}
         * that will be passed the key which it must transform into the value.
         * <p>
         * For instance:
         * <pre>
         * Transformer factory = new Transformer() {
         *     public Object transform(Object mapKey) {
         *         return new File(mapKey);
         *     }
         * }
         * Map lazyMap = MapUtils.lazyMap(new HashMap(), factory);
         * Object obj = lazyMap.get("C:/dev");
         * </pre>
         *
         * After the above code is executed, <code>obj</code> will contain
         * a new <code>File</code> instance for the C drive dev directory.
         * Furthermore, that <code>File</code> instance is the value for the
         * <code>"C:/dev"</code> key in the map.
         * <p>
         * If a lazy map is wrapped by a synchronized map, the result is a simple
         * synchronized cache. When an object is not is the cache, the cache itself
         * calls back to the factory Transformer to populate itself, all within the
         * same synchronized block.
         *
         * @param map  the map to make lazy, must not be null
         * @param transformerFactory  the factory for creating new objects, must not be null
         * @return a lazy map backed by the given map
         * @throws IllegalArgumentException  if the Map or Transformer is null
         */
        public static java.util.Map<Object, Object> lazyMap(java.util.Map<Object, Object> map, Transformer transformerFactory)
        {
            return LazyMap.decorate(map, transformerFactory);
        }

        /**
         * Returns a map that maintains the order of keys that are added
         * backed by the given map.
         * <p>
         * If a key is added twice, the order is determined by the first add.
         * The order is observed through the keySet, values and entrySet.
         *
         * @param map  the map to order, must not be null
         * @return an ordered map backed by the given map
         * @throws IllegalArgumentException  if the Map is null
         */
        public static java.util.Map<Object, Object> orderedMap(java.util.Map<Object, Object> map)
        {
            return ListOrderedMap.decorate(map);
        }

        /**
         * Creates a mult-value map backed by the given map which returns
         * collections of type ArrayList.
         *
         * @param map  the map to decorate
         * @return a multi-value map backed by the given map which returns ArrayLists of values.
         * @see MultiValueMap
         * @since Commons Collections 3.2
         */
        public static java.util.Map<Object, Object> multiValueMap(java.util.Map<Object, Object> map)
        {
            return MultiValueMap.decorate(map);
        }

        /**
         * Creates a multi-value map backed by the given map which returns
         * collections of the specified type.
         *
         * @param map  the map to decorate
         * @param collectionClass  the type of collections to return from the map (must contain public no-arg constructor
         *  and extend Collection).
         * @return a multi-value map backed by the given map which returns collections of the specified type
         * @see MultiValueMap
         * @since Commons Collections 3.2
         */
        public static java.util.Map<Object, Object> multiValueMap(java.util.Map<Object, Object> map, java.lang.Class collectionClass)
        {
            return MultiValueMap.decorate(map, collectionClass);
        }

        /**
         * Creates a multi-value map backed by the given map which returns
         * collections created by the specified collection factory.
         *
         * @param map  the map to decorate
         * @param collectionFactory  a factor which creates collection objects
         * @return a multi-value map backed by the given map which returns collections
         * created by the specified collection factory
         * @see MultiValueMap
         * @since Commons Collections 3.2
         */
        public static java.util.Map<Object, Object> multiValueMap(java.util.Map<Object, Object> map, Factory collectionFactory)
        {
            return MultiValueMap.decorate(map, collectionFactory);
        }

        // SortedMap decorators
        //-----------------------------------------------------------------------
        /**
         * Returns a synchronized sorted map backed by the given sorted map.
         * <p>
         * You must manually synchronize on the returned buffer's iterator to 
         * avoid non-deterministic behavior:
         *  
         * <pre>
         * Map m = MapUtils.synchronizedSortedMap(myMap);
         * Set s = m.keySet();  // outside synchronized block
         * synchronized (m) {  // synchronized on MAP!
         *     Iterator i = s.iterator();
         *     while (i.hasNext()) {
         *         process (i.next());
         *     }
         * }
         * </pre>
         * 
         * This method uses the implementation in {@link java.util.Collections Collections}.
         * 
         * @param map  the map to synchronize, must not be null
         * @return a synchronized map backed by the given map
         * @throws IllegalArgumentException  if the map is null
         */
        public static java.util.Map<Object, Object> synchronizedSortedMap(java.util.SortedMap<Object,Object> map)
        {
            return java.util.Collections.synchronizedSortedMap(map);
        }

        /**
         * Returns an unmodifiable sorted map backed by the given sorted map.
         * <p>
         * This method uses the implementation in the decorators subpackage.
         *
         * @param map  the sorted map to make unmodifiable, must not be null
         * @return an unmodifiable map backed by the given map
         * @throws IllegalArgumentException  if the map is null
         */
        public static java.util.Map<Object, Object> unmodifiableSortedMap(java.util.SortedMap<Object,Object> map)
        {
            return UnmodifiableSortedMap.decorate(map);
        }

        /**
         * Returns a predicated (validating) sorted map backed by the given map.
         * <p>
         * Only objects that pass the tests in the given predicates can be added to the map.
         * Trying to add an invalid object results in an IllegalArgumentException.
         * Keys must pass the key predicate, values must pass the value predicate.
         * It is important not to use the original map after invoking this method,
         * as it is a backdoor for adding invalid objects.
         *
         * @param map  the map to predicate, must not be null
         * @param keyPred  the predicate for keys, null means no check
         * @param valuePred  the predicate for values, null means no check
         * @return a predicated map backed by the given map
         * @throws IllegalArgumentException  if the SortedMap is null
         */
        public static java.util.SortedMap<Object, Object> predicatedSortedMap(java.util.SortedMap<Object, Object> map, Predicate keyPred, Predicate valuePred)
        {
            return PredicatedSortedMap.decorate(map, keyPred, valuePred);
        }

        /**
         * Returns a typed sorted map backed by the given map.
         * <p>
         * Only keys and values of the specified types can be added to the map.
         * 
         * @param map  the map to limit to a specific type, must not be null
         * @param keyType  the type of keys which may be added to the map, must not be null
         * @param valueType  the type of values which may be added to the map, must not be null
         * @return a typed map backed by the specified map
         */
        public static java.util.SortedMap<Object, Object> typedSortedMap(java.util.SortedMap<Object, Object> map, java.lang.Class keyType, java.lang.Class valueType)
        {
            return TypedSortedMap.decorate(map, keyType, valueType);
        }

        /**
         * Returns a transformed sorted map backed by the given map.
         * <p>
         * This method returns a new sorted map (decorating the specified map) that
         * will transform any new entries added to it.
         * Existing entries in the specified map will not be transformed.
         * If you want that behaviour, see {@link TransformedSortedMap#decorateTransform}.
         * <p>
         * Each object is passed through the transformers as it is added to the
         * Map. It is important not to use the original map after invoking this 
         * method, as it is a backdoor for adding untransformed objects.
         * <p>
         * If there are any elements already in the map being decorated, they
         * are NOT transformed.
         *
         * @param map  the map to transform, must not be null, typically empty
         * @param keyTransformer  the transformer for the map keys, null means no transformation
         * @param valueTransformer  the transformer for the map values, null means no transformation
         * @return a transformed map backed by the given map
         * @throws IllegalArgumentException  if the SortedMap is null
         */
        public static java.util.SortedMap<Object, Object> transformedSortedMap(java.util.SortedMap<Object, Object> map, Transformer keyTransformer, Transformer valueTransformer)
        {
            return TransformedSortedMap.decorate(map, keyTransformer, valueTransformer);
        }

        /**
         * Returns a fixed-sized sorted map backed by the given sorted map.
         * Elements may not be added or removed from the returned map, but 
         * existing elements can be changed (for instance, via the 
         * {@link Map#put(Object,Object)} method).
         *
         * @param map  the map whose size to fix, must not be null
         * @return a fixed-size map backed by that map
         * @throws IllegalArgumentException  if the SortedMap is null
         */
        public static java.util.SortedMap<Object, Object> fixedSizeSortedMap(java.util.SortedMap<Object, Object> map)
        {
            return FixedSizeSortedMap.decorate(map);
        }

        /**
         * Returns a "lazy" sorted map whose values will be created on demand.
         * <p>
         * When the key passed to the returned map's {@link Map#get(Object)}
         * method is not present in the map, then the factory will be used
         * to create a new object and that object will become the value
         * associated with that key.
         * <p>
         * For instance:
         *
         * <pre>
         * Factory factory = new Factory() {
         *     public Object create() {
         *         return new Date();
         *     }
         * }
         * SortedMap lazy = MapUtils.lazySortedMap(new TreeMap(), factory);
         * Object obj = lazy.get("test");
         * </pre>
         *
         * After the above code is executed, <code>obj</code> will contain
         * a new <code>Date</code> instance.  Furthermore, that <code>Date</code>
         * instance is the value for the <code>"test"</code> key.
         *
         * @param map  the map to make lazy, must not be null
         * @param factory  the factory for creating new objects, must not be null
         * @return a lazy map backed by the given map
         * @throws IllegalArgumentException  if the SortedMap or Factory is null
         */
        public static java.util.SortedMap<Object, Object> lazySortedMap(java.util.SortedMap<Object, Object> map, Factory factory)
        {
            return LazySortedMap.decorate(map, factory);
        }

        /**
         * Returns a "lazy" sorted map whose values will be created on demand.
         * <p>
         * When the key passed to the returned map's {@link Map#get(Object)}
         * method is not present in the map, then the factory will be used
         * to create a new object and that object will become the value
         * associated with that key. The factory is a {@link Transformer}
         * that will be passed the key which it must transform into the value.
         * <p>
         * For instance:
         * <pre>
         * Transformer factory = new Transformer() {
         *     public Object transform(Object mapKey) {
         *         return new File(mapKey);
         *     }
         * }
         * SortedMap lazy = MapUtils.lazySortedMap(new TreeMap(), factory);
         * Object obj = lazy.get("C:/dev");
         * </pre>
         *
         * After the above code is executed, <code>obj</code> will contain
         * a new <code>File</code> instance for the C drive dev directory.
         * Furthermore, that <code>File</code> instance is the value for the
         * <code>"C:/dev"</code> key in the map.
         * <p>
         * If a lazy map is wrapped by a synchronized map, the result is a simple
         * synchronized cache. When an object is not is the cache, the cache itself
         * calls back to the factory Transformer to populate itself, all within the
         * same synchronized block.
         *
         * @param map  the map to make lazy, must not be null
         * @param transformerFactory  the factory for creating new objects, must not be null
         * @return a lazy map backed by the given map
         * @throws IllegalArgumentException  if the Map or Transformer is null
         */
        public static java.util.SortedMap<Object, Object> lazySortedMap(java.util.SortedMap<Object, Object> map, Transformer transformerFactory)
        {
            return LazySortedMap.decorate(map, transformerFactory);
        }

    }
}