// ***********************************************************************
// Assembly         : clipboard
// Author           : @tysmithnet
// Created          : 10-14-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-14-2018
// ***********************************************************************
// <copyright file="ReverseCommand.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Linq;

namespace Tools.Clipboard
{
    /// <summary>
    ///     Class ReverseCommand.
    /// </summary>
    internal class ReverseCommand
    {
        /// <summary>
        ///     Reverses the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public static void Reverse(ReverseOptions options)
        {
            var lines = ClipboardProgram.GetClipboardText();
            var newText = string.Join(Environment.NewLine, lines.Reverse());
            ClipboardProgram.HandleNewText(options, newText);
        }
    }
}