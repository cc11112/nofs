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

namespace biz.ritter.javapi.io
{
    public interface ObjectOutput : DataOutput
    {
		void flush();
		void close();
		void write (byte [] b);
		void write (byte [] b, int startOffset, int length);
		void write (int b);
		void writeObject (Object obj);
    }
}
