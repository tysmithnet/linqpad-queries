// ***********************************************************************
// Assembly         : Tools.Clipboard
// Author           : @tysmithnet
// Created          : 10-13-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-13-2018
// ***********************************************************************
// <copyright file="Program.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CommandLine;
using Humanizer;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Serilog;
using Serilog.Events;
using Tools.Common;
using MsClipboard = System.Windows.Forms.Clipboard;

namespace Tools.Clipboard
{
    /// <summary>
    ///     Program to interact and do various convenience actions by the clipboard
    /// </summary>
    internal class ClipboardProgram
    {
        internal static ILogger Log;
        static ClipboardProgram()
        {
            LoggingSetup.SetupLogger();
            Log = Serilog.Log.ForContext<ClipboardProgram>();
        }
        
        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>System.Int32.</returns>
        [STAThread]
        public static int Main(string[] args)
        {
            Parser.Default.ParseArguments<SortOptions, ReverseOptions, ZipOptions>(args)
                .MapResult((SortOptions o) =>
                    {
                        Sort(o);
                        return 0;
                    }, (ReverseOptions o) =>
                    {
                        Reverse(o);
                        return 0;
                    },
                    (ZipOptions o) =>
                    {
                        Zip(o);
                        return 0;
                    },
                    errs =>
                    {
                        foreach (var error in errs) Log.Fatal("{Error}", error);
                        return 1;
                    });
            return 0;
        }

        /// <summary>
        ///     Adds a file to the zip archive
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="file">The file.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="zip">The zip.</param>
        /// <returns>AddEntryResult.</returns>
        private static AddEntryResult AddEntry(ZipOptions options, string file, string prefix, ZipOutputStream zip)
        {
            var fileInfo = new FileInfo(file);
            var entryFileName = ZipEntry.CleanName(file.Substring(prefix.Length));
            ZipEntry entry;
            try
            {
                entry = new ZipEntry(entryFileName)
                {
                    DateTime = fileInfo.LastWriteTime,
                    Size = fileInfo.Length,
                    ExternalFileAttributes = Convert.ToInt32(fileInfo.Attributes)
                };
            }
            catch (Exception e)
            {
                Log.Error(e, "Problem creating zip entry for {File}", file);
                if (!options.ContinueOnError)
                    throw;
                return new AddEntryResult
                {
                    Success = false,
                    Info = fileInfo
                };
            }

            zip.PutNextEntry(entry);
            var buffer = new byte[4096];
            try
            {
                using (var streamReader = File.OpenRead(file))
                {
                    StreamUtils.Copy(streamReader, zip, buffer);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Problem writing zip entry for {File}", file);
                if (!options.ContinueOnError)
                    throw;
            }

            zip.CloseEntry();
            return new AddEntryResult
            {
                Info = fileInfo,
                Success = true
            };
        }

        /// <summary>
        ///     Calculates the prefix.
        /// </summary>
        /// <param name="allFiles">All files.</param>
        /// <returns>System.String.</returns>
        private static string CalculatePrefix(ISet<string> allFiles)
        {
            var prefix = allFiles.Aggregate((l, r) =>
            {
                var limit = Math.Min(l.Length, r.Length);
                var i = 0;
                for (; i < limit; i++)
                    if (l[i] != r[i])
                        return l.Substring(0, i);

                return l.Substring(0, i);
            });
            return prefix;
        }

        /// <summary>
        ///     Gets the clipboard text.
        /// </summary>
        /// <returns>IList&lt;System.String&gt;.</returns>
        private static IList<string> GetClipboardText()
        {
            var text = MsClipboard.GetText();
            return Regex.Split(text, Environment.NewLine).ToArray();
        }

        /// <summary>
        ///     Gets the files for zip.
        /// </summary>
        /// <returns>ISet&lt;System.String&gt;.</returns>
        private static ISet<string> GetFilesForZip()
        {
            var allFiles = new SortedSet<string>();
            var clipBoardItems = MsClipboard.GetFileDropList();
            foreach (var path in clipBoardItems)
                if (Directory.Exists(path))
                    foreach (var subFile in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                        allFiles.Add(subFile);
                else if (File.Exists(path)) allFiles.Add(path);
            return allFiles;
        }

        /// <summary>
        ///     Handles the new text.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="newText">The new text.</param>
        private static void HandleNewText(Options options, string newText)
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
        ///     Reverses the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        private static void Reverse(ReverseOptions options)
        {
            var lines = GetClipboardText();
            var newText = string.Join(Environment.NewLine, lines.Reverse());
            HandleNewText(options, newText);
        }

        /// <summary>
        ///     Sorts the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        private static void Sort(SortOptions options)
        {
            try
            {
                var lines = GetClipboardText();
                var sorted = options.IsDescending ? lines.OrderByDescending(x => x) : lines.OrderBy(x => x);
                var list = sorted.ToList();
                var newText = string.Join(Environment.NewLine, list);
                HandleNewText(options, newText);
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Unknown problem");
            }
        }

        /// <summary>
        ///     Zips the files that are on the clipboard
        /// </summary>
        /// <param name="options">The options.</param>
        private static void Zip(ZipOptions options)
        {
            Log.Information("Zipping files");
            var allFiles = GetFilesForZip();

            if (allFiles.Count == 0)
            {
                Log.Warning("No files were found on the clipboard");
                return;
            }

            var prefix = CalculatePrefix(allFiles);

            long size = 0;
            var success = 0;
            using (var fs = new FileStream(options.Name, FileMode.OpenOrCreate))
            using (var zip = new ZipOutputStream(fs))
            {
                foreach (var file in allFiles)
                {
                    var res = AddEntry(options, file, prefix, zip);
                    if (res.Success)
                    {
                        Log.Information("Added {File}", res.Info.FullName);
                        success++;
                        size += res.Info.Length;
                    }
                    else
                    {
                        Log.Error("Error adding {File}", res.Info.FullName);
                    }
                }
            }

            var zipFile = new FileInfo(options.Name);
            if (options.IsOverwite)
            {
                var collection = new StringCollection
                {
                    zipFile.FullName
                };
                MsClipboard.SetFileDropList(collection);
            }

            Log.Information("Created: {ZipFile} Files: {FileCount} Size: {Size}", zipFile, success,
                zipFile.Length.Bytes().Humanize());
        }

        /// <summary>
        ///     Class AddEntryResult.
        /// </summary>
        private class AddEntryResult
        {
            /// <summary>
            ///     Gets or sets the information.
            /// </summary>
            /// <value>The information.</value>
            public FileInfo Info { get; set; }

            /// <summary>
            ///     Gets or sets a value indicating whether this <see cref="AddEntryResult" /> is success.
            /// </summary>
            /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
            public bool Success { get; set; }
        }
    }
}