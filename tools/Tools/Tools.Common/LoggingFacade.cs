// ***********************************************************************
// Assembly         : Tools.Common
// Author           : @tysmithnet
// Created          : 10-13-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-14-2018
// ***********************************************************************
// <copyright file="LoggingFacade.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Tools.Common
{
    /// <summary>
    ///     Class LoggingFacade.
    /// </summary>
    public static class LoggingFacade
    {
        /// <summary>
        ///     Disables this instance.
        /// </summary>
        public static void Disable()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Debug()
                .CreateLogger();
        }

        /// <summary>
        ///     Setups the logger.
        /// </summary>
        public static void SetupLogger()
        {
            CreateLogger();
            AppDomain.CurrentDomain.DomainUnload += (sender, args) => { Log.CloseAndFlush(); };
        }

        /// <summary>
        ///     Creates the logger.
        /// </summary>
        private static void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Debug()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("localhost:9200")))
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}