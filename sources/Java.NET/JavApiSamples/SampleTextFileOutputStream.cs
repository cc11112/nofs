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
using System.Linq;
using System.Text;

using java = biz.ritter.javapi;
using biz.ritter.javapi.dotnet;

namespace javapi.sample
{
    class SampleTextFileOutputStream
    {
        static void Main()
        {
            String[] pangramm = {
                "Welch fieser Katzentyp quält da süße Vögel bloß zum Jux?", // de
                "The quick brown fox jumps over the lazy dog",              // en
                "Portez ce vieux whisky au juge blond qui fume",            // fr
                "Эй, жлоб! Где туз? Прячь юных съёмщиц в шкаф",             // ru
                "Chwyć małżonkę, strój bądź pleśń z fugi.",                 // pl
                "V kožuščku hudobnega fanta stopiclja mizar in kliče.",     // sl
                "以呂波耳本へ止 千利奴流乎和加 餘多連曽津祢那 良牟有為能於久 耶万計不己衣天 阿佐伎喩女美之 恵比毛勢須", // jp
                                 };
            java.io.FileOutputStream fos = new java.io.FileOutputStream("c:/temp/sample.txt");
            foreach (String p in pangramm) {
                fos.write(p.getBytes());
            }
            fos.flush();
            fos.close();

        }
    }
}
