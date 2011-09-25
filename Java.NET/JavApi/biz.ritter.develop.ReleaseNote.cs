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
using System.Collections.Generic;
using System.Text;

namespace biz.ritter.develop
{
    [System.AttributeUsage (AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple=true)]
    public class ReleaseNote : System.Attribute
    {
        public string author = "Ritter, Sebastian";
        /// <summary>
        /// Versionsnummer
        /// </summary>
        public double jVersion = 1.0;
        /// <summary>
        /// Umfang der bereits durchgeführten Portierung in %
        /// </summary>
        public float port = 0f;
        public ReleaseNote()
        {
        }
    }
}
