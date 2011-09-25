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
 */
using System;

namespace biz.ritter.javapi.nio.charset
{
    /**
     * Used to indicate what kind of actions to take in case of encoding/decoding
     * errors. Currently three actions are defined: {@code IGNORE}, {@code REPLACE}
     * and {@code REPORT}.
     */
    public class CodingErrorAction {

        /**
         * Denotes the action to ignore any errors.
         */
        public static readonly CodingErrorAction IGNORE = new CodingErrorAction(
                "IGNORE"); //$NON-NLS-1$

        /**
         * Denotes the action to fill in the output with a replacement character
         * when malformed input or an unmappable character is encountered.
         */
        public static readonly CodingErrorAction REPLACE = new CodingErrorAction(
                "REPLACE"); //$NON-NLS-1$

        /**
         * Denotes the action to report the encountered error in an appropriate
         * manner, for example to throw an exception or return an informative
         * result.
         */
        public static readonly CodingErrorAction REPORT = new CodingErrorAction(
                "REPORT"); //$NON-NLS-1$

        // The name of this action
        private String action;

        /*
         * Can't instantiate outside.
         */
        private CodingErrorAction(String action) {
            this.action = action;
        }

        /**
         * Returns a text description of this action indication.
         * 
         * @return a text description of this action indication.
         */
        public override String ToString() {
            return "Action: " + this.action; //$NON-NLS-1$
        }
    }
}
