using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace GnZY.Performance.Tests
{
    public class ByteConverterTests
    {
        [Test]
        public void ToStringSimple()
        {
            var random = new Random();
            var buffer = new byte[0xFF];

            random.NextBytes(buffer);

            ByteConverter.ToString(buffer).Should().Be(BitConverter.ToString(buffer).Replace("-", string.Empty));
        }

        [Test]
        public void ToStringIndex()
        {
            var random = new Random();
            var buffer = new byte[0xFF];

            random.NextBytes(buffer);

            ByteConverter.ToString(buffer, 0x0F).Should().Be(BitConverter.ToString(buffer, 0x0F).Replace("-", string.Empty));
        }

        [Test]
        public void ToStringIndexLength()
        {
            var random = new Random();
            var buffer = new byte[0xFF];

            random.NextBytes(buffer);

            ByteConverter.ToString(buffer, 0x0F, 0xCC).Should().Be(BitConverter.ToString(buffer, 0x0F, 0xCC).Replace("-", string.Empty));
        }

        [Test]
        public void FromString()
        {
            var random = new Random();
            var buffer = new byte[0xFF];

            random.NextBytes(buffer);

            var hex = ByteConverter.ToString(buffer);
            var target =
                Enumerable.Range(0, hex.Length)
                    .Where(_ => _ % 2 == 0)
                    .Select(_ => Convert.ToByte(hex.Substring(_, 2), 16));

            ByteConverter.FromHex(hex).Should().Equal(target);
        }
    }
}
