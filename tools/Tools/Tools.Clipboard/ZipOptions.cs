// ***********************************************************************
// Assembly         : Tools.Clipboard
// Author           : @tysmithnet
// Created          : 10-13-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-13-2018
// ***********************************************************************
// <copyright file="ZipOptions.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using CommandLine;

namespace Tools.Clipboard
{
    /// <summary>
    ///     Class ZipOptions.
    /// </summary>
    /// <seealso cref="Tools.Clipboard.Options" />
    [Verb("zip", HelpText = "Add all files currently on the clipboard to a zip archive")]
    public class ZipOptions : Options
    {
        [Option('c', "continue", HelpText = "Continue trying to create the zip file even if an error occurs",
            Default = false)]
        public bool ContinueOnError { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Option('n', "name", HelpText = "Name of the zip file", Required = true)]
        public string Name { get; set; }
    }
}