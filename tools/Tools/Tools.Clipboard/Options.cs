// ***********************************************************************
// Assembly         : Tools.Clipboard
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

namespace Tools.Clipboard
{
    /// <summary>
    ///     Class Options.
    /// </summary>
    public class Options
    {
        /// <summary>
        ///     Gets or sets a value indicating whether [automatic accept].
        /// </summary>
        /// <value><c>true</c> if [automatic accept]; otherwise, <c>false</c>.</value>
        [Option('a', "auto-accept", HelpText = "Always take the default option if being prompted")]
        public bool AutoAccept { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is logging enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is logging enabled; otherwise, <c>false</c>.</value>
        [Option('l', "log", HelpText = "Enable logging", Default = false)]
        public bool IsLoggingEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is overwite.
        /// </summary>
        /// <value><c>true</c> if this instance is overwite; otherwise, <c>false</c>.</value>
        [Option('o', "overwrite", HelpText = "Overwrite current clipboard content with result")]
        public bool IsOverwite { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is quiet.
        /// </summary>
        /// <value><c>true</c> if this instance is quiet; otherwise, <c>false</c>.</value>
        [Option('q', "quiet", HelpText = "Surpress output")]
        public bool IsQuiet { get; set; }
    }
}