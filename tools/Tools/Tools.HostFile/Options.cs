// ***********************************************************************
// Assembly         : Tools.HostFile
// Author           : @tysmithnet
// Created          : 10-13-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-13-2018
// ***********************************************************************
// <copyright file="Options.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using CommandLine;

namespace Tools.HostFile
{
    /// <summary>
    ///     Class Options.
    /// </summary>
    public class Options
    {
        /// <summary>
        ///     Gets or sets a value indicating whether [include WWW subdomain].
        /// </summary>
        /// <value><c>true</c> if [include WWW subdomain]; otherwise, <c>false</c>.</value>
        [Option('w', "www", HelpText = "Include www. subdomain automatically", Default = true)]
        public bool IncludeWwwSubdomain { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is logging enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is logging enabled; otherwise, <c>false</c>.</value>
        [Option('l', "log", HelpText = "Specify if logging is enabled")]
        public bool IsLoggingEnabled { get; set; }
    }
}