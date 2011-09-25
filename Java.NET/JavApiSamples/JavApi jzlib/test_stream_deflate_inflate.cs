/* -*-mode:java; c-basic-offset:2; -*- */
using System;
using java = biz.ritter.javapi;
using com.jcraft.jzlib;

namespace test.jzlib
{
    public class test_stream_deflate_inflate
    {
        public static void main(String[] args)
        {
            try
            {
                java.io.ByteArrayOutputStream outJ = new java.io.ByteArrayOutputStream();
                ZOutputStream zOut = new ZOutputStream(outJ, JZlib.Z_BEST_COMPRESSION);
                String hello = "Hello World!";
                //java.io.ObjectOutputStream objOut = new java.io.ObjectOutputStream(zOut);
                //objOut.writeObject(hello);
                outJ.write(hello.getBytes());
                zOut.close();

                java.io.ByteArrayInputStream inJ = new java.io.ByteArrayInputStream(outJ.toByteArray());
                ZInputStream zIn = new ZInputStream(inJ);
                //java.io.ObjectInputStream objIn=new java.io.ObjectInputStream(zIn);
                byte[] buffer = new byte[hello.length()];
                inJ.read(buffer);
                java.lang.SystemJ.outJ.println(new java.lang.StringJ(buffer).ToString() /*objIn.readObject()*/);
            }
            catch (java.lang.Exception e)
            {
                e.printStackTrace();
            }
        }
    }
}