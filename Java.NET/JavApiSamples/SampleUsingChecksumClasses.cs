using System;
using java = biz.ritter.javapi;


namespace javapi.sample
{
    class SampleUsingChecksumClasses
    {
        static void Main()
        {
            java.util.zip.CRC32 crc = new java.util.zip.CRC32();
            java.util.zip.Adler32 a32 = new java.util.zip.Adler32();
            java.lang.SystemJ.outJ.println(crc.getValue()+"\t"+a32.getValue());
            crc.update("Bastie".getBytes());
            a32.update("Bastie".getBytes());
            java.lang.SystemJ.outJ.println(crc.getValue()+"\t"+a32.getValue());
            crc = new java.util.zip.CRC32();
            a32 = new java.util.zip.Adler32();
            java.lang.SystemJ.outJ.println(crc.getValue()+"\t"+a32.getValue());
            crc.update("Bas".getBytes());
            a32.update("Bas".getBytes());
            java.lang.SystemJ.outJ.println(crc.getValue()+"\t"+a32.getValue());
            crc.update("tie".getBytes());
            a32.update("tie".getBytes());
            java.lang.SystemJ.outJ.println(crc.getValue()+"\t"+a32.getValue());
             
        }
    }
}
