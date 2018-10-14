// ***********************************************************************
// Assembly         : clipboard
// Author           : @tysmithnet
// Created          : 10-14-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-14-2018
// ***********************************************************************
// <copyright file="SortCommand.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Linq;

namespace Tools.Clipboard
{
    /// <summary>
    ///     Class SortCommand.
    /// </summary>
    internal class SortCommand
    {
        /// <summary>
        ///     Sorts the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public static void Sort(SortOptions options)
        {
            try
            {
                var lines = ClipboardProgram.GetClipboardText();
                var sorted = options.IsDescending ? lines.OrderByDescending(x => x) : lines.OrderBy(x => x);
                var list = sorted.ToList();
                var newText = string.Join(Environment.NewLine, list);
                ClipboardProgram.HandleNewText(options, newText);
            }
            catch (Exception e)
            {
                ClipboardProgram.Log.Fatal(e, "Unknown problem");
            }
        }
    }
}