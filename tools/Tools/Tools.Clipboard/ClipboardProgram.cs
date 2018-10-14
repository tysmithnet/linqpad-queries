// ***********************************************************************
// Assembly         : Tools.Clipboard
// Author           : @tysmithnet
// Created          : 10-13-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-14-2018
// ***********************************************************************
// <copyright file="Program.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CommandLine;
using Serilog;
using Tools.Common;
using MsClipboard = System.Windows.Forms.Clipboard;

namespace Tools.Clipboard
{
    /// <summary>
    ///     Program to interact and do various convenience actions by the clipboard
    /// </summary>
    internal class ClipboardProgram
    {
        /// <summary>
        ///     Initializes static members of the <see cref="ClipboardProgram" /> class.
        /// </summary>
        static ClipboardProgram()
        {
            LoggingFacade.SetupLogger();
            Log = Serilog.Log.ForContext<ClipboardProgram>();
        }

        /// <summary>
        ///     The log
        /// </summary>
        public static ILogger Log;

        /// <summary>
        ///     Gets the clipboard text.
        /// </summary>
        /// <returns>IList&lt;System.String&gt;.</returns>
        public static IList<string> GetClipboardText()
        {
            var text = MsClipboard.GetText();
            return Regex.Split(text, Environment.NewLine).ToArray();
        }

        /// <summary>
        ///     Handles the new text.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="newText">The new text.</param>
        public static void HandleNewText(Options options, string newText)
        {
            if (options.IsOverwite)
            {
                Log.Information("Overwriting clipboard. {Preview}",
                    new string(newText.Take(100).Concat("...").ToArray()));
                MsClipboard.SetText(newText);
            }

            if (options.IsQuiet)
                return;
            Console.WriteLine(newText);
        }

        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>System.Int32.</returns>
        [STAThread]
        public static int Main(string[] args)
        {
            var parser = new Parser(settings =>
            {
                settings.CaseInsensitiveEnumValues = true;
                settings.CaseSensitive = false;
                settings.HelpWriter = Parser.Default.Settings.HelpWriter;
                settings.EnableDashDash = Parser.Default.Settings.EnableDashDash;
                settings.IgnoreUnknownArguments = Parser.Default.Settings.IgnoreUnknownArguments;
                settings.MaximumDisplayWidth = 104;
                settings.ParsingCulture = Parser.Default.Settings.ParsingCulture;
            });
            parser.ParseArguments<SortOptions, ReverseOptions, ZipOptions, ListOptions>(args)
                .MapResult((SortOptions o) =>
                    {
                        SortCommand.Sort(o);
                        return 0;
                    }, (ReverseOptions o) =>
                    {
                        ReverseCommand.Reverse(o);
                        return 0;
                    },
                    (ZipOptions o) =>
                    {
                        ZipCommand.Zip(o);
                        return 0;
                    },
                    (ListOptions o) =>
                    {
                        ListCommand.List(o);
                        return 0;
                    },
                    errs =>
                    {
                        foreach (var error in errs) Log.Fatal("{Error}", error);
                        return 1;
                    });
            return 0;
        }
    }
}