using System;
using System.Net;

namespace Perihelion.Types.Extensions
{
    /// <summary> Helper methods for working with IP addresses. </summary>
    public static class IPAddressExtensions
    {
        /// <summary> Generate a new random IPv6 address. </summary>
        /// <param name="random"> A reference to the random number generator. </param>
        /// <returns> The constructed data object. </returns>
        public static IPAddress GenerateIPv6Address(Random random)
        {
            Byte[] bytes = new Byte[16];
            random.NextBytes(bytes);
            return new IPAddress(bytes);
        }
    }
}
