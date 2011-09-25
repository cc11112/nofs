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
 *  Copyright © 2011 Sebastian Ritter
 */
using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.lang
{
    /// <summary>
    /// Simple Implementation of <code>Deprecated</code> annotation, but
    /// we cannot extends the .net <code>System.ObsoleteAttribute</code> and
    /// so use [Obsolete] are better for IDE...
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Field, Inherited=false, AllowMultiple=true)]
    public class Deprecated : java.lang.annotation.AbstractAnnotation
    {
        // Sorry, can't extend System.ObsoleteAttribute
    }
}
