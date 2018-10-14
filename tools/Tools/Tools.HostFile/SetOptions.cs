// ***********************************************************************
// Assembly         : Tools.HostFile
// Author           : @tysmithnet
// Created          : 10-13-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-13-2018
// ***********************************************************************
// <copyright file="SetOptions.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using CommandLine;

namespace Tools.HostFile
{
    /// <summary>
    ///     Class SetOptions.
    /// </summary>
    /// <seealso cref="Tools.HostFile.Options" />
    [Verb("set", HelpText = "Set the ip address resolution for a domain")]
    public class SetOptions : Options
    {
        /// <summary>
        ///     Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        [Option('d', "domain", HelpText = "Domain name to use in the mapping", Required = true)]
        public string Domain { get; set; }

        /// <summary>
        ///     Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        [Option('a', "address", HelpText = "IP address to use in the mapping", Required = true)]
        public string IpAddress { get; set; }
    }
}