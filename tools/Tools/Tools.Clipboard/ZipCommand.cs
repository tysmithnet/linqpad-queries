// ***********************************************************************
// Assembly         : clipboard
// Author           : @tysmithnet
// Created          : 10-14-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-14-2018
// ***********************************************************************
// <copyright file="ZipCommand.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Humanizer;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace Tools.Clipboard
{
    /// <summary>
    ///     Class ZipCommand.
    /// </summary>
    internal class ZipCommand
    {
        /// <summary>
        ///     Zips the files that are on the clipboard
        /// </summary>
        /// <param name="options">The options.</param>
        public static void Zip(ZipOptions options)
        {
            ClipboardProgram.Log.Information("Zipping files");
            var allFiles = GetFilesForZip();

            if (allFiles.Count == 0)
            {
                ClipboardProgram.Log.Warning("No files were found on the clipboard");
                return;
            }

            var prefix = CalculatePrefix(allFiles);

            long size = 0;
            var success = 0;
            using (var fs = new FileStream(options.FileName, FileMode.OpenOrCreate))
            using (var zip = new ZipOutputStream(fs))
            {
                foreach (var file in allFiles)
                {
                    var res = AddEntry(options, file, prefix, zip);
                    if (res.Success)
                    {
                        ClipboardProgram.Log.Information("Added {File}", res.Info.FullName);
                        success++;
                        size += res.Info.Length;
                    }
                    else
                    {
                        ClipboardProgram.Log.Error("Error adding {File}", res.Info.FullName);
                    }
                }
            }

            var zipFile = new FileInfo(options.FileName);
            if (options.IsOverwite)
            {
                var collection = new StringCollection
                {
                    zipFile.FullName
                };
                System.Windows.Forms.Clipboard.SetFileDropList(collection);
            }

            ClipboardProgram.Log.Information("Created: {ZipFile} Files: {FileCount} Size: {Size}", zipFile, success,
                zipFile.Length.Bytes().Humanize());
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
                ClipboardProgram.Log.Error(e, "Problem creating zip entry for {File}", file);
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
                ClipboardProgram.Log.Error(e, "Problem writing zip entry for {File}", file);
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
        ///     Gets the files for zip.
        /// </summary>
        /// <returns>ISet&lt;System.String&gt;.</returns>
        private static ISet<string> GetFilesForZip()
        {
            var allFiles = new SortedSet<string>();
            var clipBoardItems = System.Windows.Forms.Clipboard.GetFileDropList();
            foreach (var path in clipBoardItems)
                if (Directory.Exists(path))
                    foreach (var subFile in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                        allFiles.Add(subFile);
                else if (File.Exists(path)) allFiles.Add(path);
            return allFiles;
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