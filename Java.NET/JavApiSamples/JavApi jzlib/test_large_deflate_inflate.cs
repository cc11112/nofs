/* -*-mode:java; c-basic-offset:2; -*- */
using System;
using java = biz.ritter.javapi;
using com.jcraft.jzlib;

namespace test.jzlib
{

    // Test deflate() with large buffers and dynamic change of compression level
    class test_large_deflate_inflate
    {

        static readonly byte[] hello = "hello, hello! ".getBytes();
        static test_large_deflate_inflate()
        {
            hello[hello.Length - 1] = 0;
        }

        public static void main(String[] arg)
        {
            int err;
            int comprLen = 40000;
            int uncomprLen = comprLen;
            byte[] compr = new byte[comprLen];
            byte[] uncompr = new byte[uncomprLen];

            ZStream c_stream = new ZStream();

            err = c_stream.deflateInit(JZlib.Z_BEST_SPEED);
            CHECK_ERR(c_stream, err, "deflateInit");

            c_stream.next_out = compr;
            c_stream.next_out_index = 0;
            c_stream.avail_out = comprLen;

            // At this point, uncompr is still mostly zeroes, so it should compress
            // very well:
            c_stream.next_in = uncompr;
            c_stream.avail_in = uncomprLen;
            err = c_stream.deflate(JZlib.Z_NO_FLUSH);
            CHECK_ERR(c_stream, err, "deflate");
            if (c_stream.avail_in != 0)
            {
                java.lang.SystemJ.outJ.println("deflate not greedy");
                java.lang.SystemJ.exit(1);
            }

            // Feed in already compressed data and switch to no compression:
            c_stream.deflateParams(JZlib.Z_NO_COMPRESSION, JZlib.Z_DEFAULT_STRATEGY);
            c_stream.next_in = compr;
            c_stream.next_in_index = 0;
            c_stream.avail_in = comprLen / 2;
            err = c_stream.deflate(JZlib.Z_NO_FLUSH);
            CHECK_ERR(c_stream, err, "deflate");

            // Switch back to compressing mode:
            c_stream.deflateParams(JZlib.Z_BEST_COMPRESSION, JZlib.Z_FILTERED);
            c_stream.next_in = uncompr;
            c_stream.next_in_index = 0;
            c_stream.avail_in = uncomprLen;
            err = c_stream.deflate(JZlib.Z_NO_FLUSH);
            CHECK_ERR(c_stream, err, "deflate");

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

            while (true)
            {
                d_stream.next_out = uncompr;
                d_stream.next_out_index = 0;
                d_stream.avail_out = uncomprLen;
                err = d_stream.inflate(JZlib.Z_NO_FLUSH);
                if (err == JZlib.Z_STREAM_END) break;
                CHECK_ERR(d_stream, err, "inflate large");
            }

            err = d_stream.inflateEnd();
            CHECK_ERR(d_stream, err, "inflateEnd");

            if (d_stream.total_out != 2 * uncomprLen + comprLen / 2)
            {
                java.lang.SystemJ.outJ.println("bad large inflate: " + d_stream.total_out);
                java.lang.SystemJ.exit(1);
            }
            else
            {
                java.lang.SystemJ.outJ.println("large_inflate(): OK");
            }
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