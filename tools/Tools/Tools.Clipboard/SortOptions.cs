// ***********************************************************************
// Assembly         : Tools.Clipboard
// Author           : @tysmithnet
// Created          : 10-13-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-13-2018
// ***********************************************************************
// <copyright file="SortOptions.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using CommandLine;

namespace Tools.Clipboard
{
    /// <summary>
    ///     Class SortOptions.
    /// </summary>
    /// <seealso cref="Tools.Clipboard.Options" />
    [Verb("sort", HelpText = "Sort clipboard contents")]
    public class SortOptions : Options
    {
        /// <summary>
        ///     Gets or sets a value indicating whether this instance is descending.
        /// </summary>
        /// <value><c>true</c> if this instance is descending; otherwise, <c>false</c>.</value>
        [Option('d', "desc", HelpText = "Sort in descending order")]
        public bool IsDescending { get; set; }
    }
}