// ***********************************************************************
// Assembly         : clipboard
// Author           : @tysmithnet
// Created          : 10-14-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-14-2018
// ***********************************************************************
// <copyright file="ListOptions.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using CommandLine;

namespace Tools.Clipboard
{
    /// <summary>
    ///     Class ListOptions.
    /// </summary>
    /// <seealso cref="Tools.Clipboard.Options" />
    [Verb("list", HelpText = "List the contents of the clipboard")]
    public class ListOptions : Options
    {
        /// <summary>
        ///     Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        [Option('s', "source", HelpText = "Which clipboard contents to list", Default = Source.Text)]
        public Source Source { get; set; }
    }
}