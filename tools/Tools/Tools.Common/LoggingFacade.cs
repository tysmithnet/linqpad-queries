using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Tools.Common
{
    public static class LoggingFacade
    {
        public static void SetupLogger()
        {
            CreateLogger();
            AppDomain.CurrentDomain.DomainUnload += (sender, args) => { Log.CloseAndFlush(); };
        }

        private static void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Debug()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("localhost:9200")))
                .WriteTo.Console()
                .CreateLogger();
        }

        public static void Disable()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Debug()
                .CreateLogger();
        }
    }
}
