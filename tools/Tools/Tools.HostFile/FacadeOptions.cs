// ***********************************************************************
// Assembly         : Tools.HostFile
// Author           : @tysmithnet
// Created          : 10-13-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-13-2018
// ***********************************************************************
// <copyright file="FacadeOptions.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using CommandLine;

namespace Tools.HostFile
{
    /// <summary>
    ///     Class FacadeOptions.
    /// </summary>
    /// <seealso cref="Tools.HostFile.Options" />
    public class FacadeOptions : Options
    {
        /// <summary>
        ///     Gets or sets the domains.
        /// </summary>
        /// <value>The domains.</value>
        [Option('d', "domains", HelpText = "Domains to modify")]
        public IEnumerable<string> Domains { get; set; }
    }
}