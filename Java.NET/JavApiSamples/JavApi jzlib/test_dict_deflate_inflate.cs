/* -*-mode:java; c-basic-offset:2; -*- */
using System;
using java = biz.ritter.javapi;
using com.jcraft.jzlib;

// Test deflate() with preset dictionary
namespace test.jzlib
{
    class test_dict_deflate_inflate
    {

        static readonly byte[] dictionary = "hello ".getBytes();
        static readonly byte[] hello = "hello, hello! ".getBytes();
        static test_dict_deflate_inflate()
        {
            dictionary[dictionary.Length - 1] = 0;
            hello[hello.Length - 1] = 0;
        }

        public static void main(String[] arg)
        {
            int err;
            int comprLen = 40000;
            int uncomprLen = comprLen;
            byte[] uncompr = new byte[uncomprLen];
            byte[] compr = new byte[comprLen];
            long dictId;

            ZStream c_stream = new ZStream();
            err = c_stream.deflateInit(JZlib.Z_BEST_COMPRESSION);
            CHECK_ERR(c_stream, err, "deflateInit");

            err = c_stream.deflateSetDictionary(dictionary, dictionary.Length);
            CHECK_ERR(c_stream, err, "deflateSetDictionary");

            dictId = c_stream.adler;

            c_stream.next_out = compr;
            c_stream.next_out_index = 0;
            c_stream.avail_out = comprLen;

            c_stream.next_in = hello;
            c_stream.next_in_index = 0;
            c_stream.avail_in = hello.Length;

            err = c_stream.deflate(JZlib.Z_FINISH);
            if (err != JZlib.Z_STREAM_END)
            {
                java.lang.SystemJ.outJ.println("deflate should report Z_STREAM_END");
                java.lang.SystemJ.exit(1);
            }
            err = c_stream.deflateEnd();
            CHECK_ERR(c_stream, err, "deflateEnd");

            ZStream d_stream = new ZStream();

            d_stream.next_in = compr;
            d_stream.next_in_index = 0;
            d_stream.avail_in = comprLen;

            err = d_stream.inflateInit();
            CHECK_ERR(d_stream, err, "inflateInit");
            d_stream.next_out = uncompr;
            d_stream.next_out_index = 0;
            d_stream.avail_out = uncomprLen;

            while (true)
            {
                err = d_stream.inflate(JZlib.Z_NO_FLUSH);
                if (err == JZlib.Z_STREAM_END)
                {
                    break;
                }
                if (err == JZlib.Z_NEED_DICT)
                {
                    if ((int)d_stream.adler != (int)dictId)
                    {
                        java.lang.SystemJ.outJ.println("unexpected dictionary");
                        java.lang.SystemJ.exit(1);
                    }
                    err = d_stream.inflateSetDictionary(dictionary, dictionary.Length);
                }
                CHECK_ERR(d_stream, err, "inflate with dict");
            }

            err = d_stream.inflateEnd();
            CHECK_ERR(d_stream, err, "inflateEnd");

            int j = 0;
            for (; j < uncompr.Length; j++) if (uncompr[j] == 0) break;

            java.lang.SystemJ.outJ.println("after inflateSync(): hel" + new java.lang.StringJ(uncompr, 0, j));
        }

        static void CHECK_ERR(ZStream z, int err, String msg)
        {
            if (err != JZlib.Z_OK)
            {
                if (z.msg != null) java.lang.SystemJ.outJ.print(z.msg + " ");
                java.lang.SystemJ.outJ.println(msg + " error: " + err);

                java.lang.SystemJ.exit(1);
            }
        }
    }
}