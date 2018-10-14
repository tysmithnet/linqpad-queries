// ***********************************************************************
// Assembly         : Tools.Clipboard
// Author           : @tysmithnet
// Created          : 10-13-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-14-2018
// ***********************************************************************
// <copyright file="ReverseOptions.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using CommandLine;

namespace Tools.Clipboard
{
    /// <summary>
    ///     Class ReverseOptions.
    /// </summary>
    /// <seealso cref="Tools.Clipboard.Options" />
    [Verb("rev", HelpText = "Reverse clipboard contents")]
    public class ReverseOptions : Options
    {
    }
}