using System;

namespace GnZY.Performance
{
    public class ByteConverter
    {
        const char AlphaValue = (char)('A' - 10);

        public static string ToString(byte[] input)
        {
            return ToString(input, 0, input.Length);
        }

        public static string ToString(byte[] input, int index)
        {
            return ToString(input, index, input.Length - index);
        }

        public static string ToString(byte[] input, int index, int length)
        {
#if UNSAFE
            var output = new String('\0', length << 1);

            unsafe
            {
                char* outputSeeker;
                byte* inputSeeker;

                fixed (char* seeker = output) outputSeeker = seeker;
                fixed (byte* seeker = input) inputSeeker = seeker;

                inputSeeker += index;

                for (var i = 0; i < length; i++)
                {
                    var left = *inputSeeker >> 4;
                    var right = *inputSeeker++ & 0x0F;

                    left = left < 10 ? left | 0x30 : left + AlphaValue;
                    right = right < 10 ? right | 0x30 : right + AlphaValue;

                    *outputSeeker++ = (char)left;
                    *outputSeeker++ = (char)right;
                }
            }

            return output;
#else
            var output = new char[length << 1];
            var outputIndex = 0;

            for (var i = 0; i < length; i++)
            {
                var value = input[index++];
                var left = value >> 4;
                var right = value & 0x0F;

                left = left < 10 ? left | 0x30 : left + AlphaValue;
                right = right < 10 ? right | 0x30 : right + AlphaValue;

                output[outputIndex++] = (char)left;
                output[outputIndex++] = (char)right;
            }

            return new String(output);
#endif
        }

        public static byte[] FromHex(string input)
        {
            var length = input.Length;
            var output = new byte[length >> 1];

#if UNSAFE
            unsafe
            {
                char* inputSeeker;
                byte* outputSeeker;

                fixed (char* seeker = input) inputSeeker = seeker;
                fixed (byte* seeker = output) outputSeeker = seeker;

                for (var i = 0; i < length; i += 2)
                {
                    var left = inputSeeker[i];
                    var right = inputSeeker[i + 1];

                    *outputSeeker++ = (byte)(
                        (((left & 0x40) == 0 ? left & 0x0F : (left & 0xDF) - AlphaValue) << 4) |
                        ((right & 0x40) == 0 ? right & 0x0F : (right & 0xDF) - AlphaValue)
                    );
                }
            }
#else
            for (var i = 0; i < length; i += 2)
            {
                var left = input[i];
                var right = input[i + 1];

                output[i >> 1] = (byte)(
                    (((left & 0x40) == 0 ? left & 0x0F : (left & 0xDF) - AlphaValue) << 4) |
                    ((right & 0x40) == 0 ? right & 0x0F : (right & 0xDF) - AlphaValue)
                );
            }
#endif

            return output;
        }
    }
}
