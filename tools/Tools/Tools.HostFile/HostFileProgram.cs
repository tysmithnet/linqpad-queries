// ***********************************************************************
// Assembly         : Tools.HostFile
// Author           : @tysmithnet
// Created          : 10-13-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-14-2018
// ***********************************************************************
// <copyright file="HostFileProgram.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.IO;
using System.Linq;
using CommandLine;
using Serilog;
using Tools.Common;

namespace Tools.HostFile
{
    /// <summary>
    ///     Class HostFileProgram.
    /// </summary>
    public class HostFileProgram
    {
        /// <summary>
        ///     The host file
        /// </summary>
        private const string HOST_FILE = @"c:\windows\system32\drivers\etc\hosts";

        /// <summary>
        ///     Initializes static members of the <see cref="HostFileProgram" /> class.
        /// </summary>
        static HostFileProgram()
        {
            LoggingFacade.SetupLogger();
            Log = Serilog.Log.ForContext<HostFileProgram>();
        }

        /// <summary>
        ///     The log
        /// </summary>
        private static readonly ILogger Log;

        /// <summary>
        ///     Blocks the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        private static void Block(BlockOptions options)
        {
            var lines = File.ReadAllLines(HOST_FILE);
            options.Domains = options.Domains.OrderBy(x => x).Distinct();
            var newGuys = options.IncludeWwwSubdomain
                ? options.Domains.SelectMany(x => new[] {x, $"www.{x}"})
                : options.Domains;
            File.WriteAllLines(HOST_FILE, lines.Concat(newGuys.Select(x => $"127.0.0.1 {x}")));
            DnsFlusher.Flush();
        }

        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<BlockOptions, UnblockOptions, SetOptions>(args)
                .MapResult((BlockOptions o) =>
                {
                    Block(o);
                    return 0;
                }, (UnblockOptions o) =>
                {
                    Unblock(o);
                    return 0;
                }, (SetOptions o) =>
                {
                    Set(o);
                    return 0;
                }, errs =>
                {
                    foreach (var error in errs) Log.Fatal("{Error}", error);
                    return 1;
                });
        }

        /// <summary>
        ///     Sets the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        private static void Set(SetOptions options)
        {
            var lines = File.ReadAllLines(HOST_FILE);
            var linesForHost = lines.Where(x => x.Trim().EndsWith($" {options.Domain}"));
            var newLines = lines.Except(linesForHost).ToList();
            if (options.IncludeWwwSubdomain)
            {
                var wwws = newLines.Where(x => x.Trim().EndsWith($"www.{options.Domain}"));
                newLines = newLines.Except(wwws).ToList();
            }

            newLines.Add($"{options.IpAddress} {options.Domain}");
            File.WriteAllLines(HOST_FILE, newLines);
            DnsFlusher.Flush();
        }

        /// <summary>
        ///     Unblocks the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        private static void Unblock(UnblockOptions options)
        {
            var lines = File.ReadAllLines(HOST_FILE);
            options.Domains = options.Domains.OrderBy(x => x).Distinct();
            var newGuys = options.IncludeWwwSubdomain
                ? options.Domains.SelectMany(x => new[] {x, $"www.{x}"})
                : options.Domains;
            lines = lines.Except(newGuys.Select(x => $"127.0.0.1 {x}")).Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();
            File.WriteAllLines(HOST_FILE, lines);
            DnsFlusher.Flush();
        }
    }
}