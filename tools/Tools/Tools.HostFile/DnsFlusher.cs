// ***********************************************************************
// Assembly         : Tools.HostFile
// Author           : @tysmithnet
// Created          : 10-14-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-14-2018
// ***********************************************************************
// <copyright file="DnsFlusher.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Runtime.InteropServices;

namespace Tools.HostFile
{
    /// <summary>
    ///     Class DnsFlusher.
    /// </summary>
    public static class DnsFlusher
    {
        /// <summary>
        ///     Flushes this instance.
        /// </summary>
        public static void Flush()
        {
            var result = DnsFlushResolverCache();
        }

        /// <summary>
        ///     DNSs the flush resolver cache.
        /// </summary>
        /// <returns>UInt32.</returns>
        [DllImport("dnsapi.dll", EntryPoint = "DnsFlushResolverCache")]
        private static extern uint DnsFlushResolverCache();
    }
}