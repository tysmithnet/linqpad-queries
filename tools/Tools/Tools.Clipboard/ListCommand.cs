// ***********************************************************************
// Assembly         : clipboard
// Author           : @tysmithnet
// Created          : 10-14-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-14-2018
// ***********************************************************************
// <copyright file="ListCommand.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Tools.Clipboard
{
    /// <summary>
    ///     Class ListCommand.
    /// </summary>
    internal class ListCommand
    {
        /// <summary>
        ///     Lists the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static int List(ListOptions options)
        {
            switch (options.Source)
            {
                case Source.Text:
                    Console.WriteLine(System.Windows.Forms.Clipboard.GetText());
                    return 0;
                case Source.Files:
                    foreach (var file in System.Windows.Forms.Clipboard.GetFileDropList()) Console.WriteLine(file);
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}