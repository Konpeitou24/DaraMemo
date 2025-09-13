using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaraMemo.Services.Api {
    public static class RequestUriData {
        public static string BaseAddress { get; } = "http://localhost:5000/";
        public static string StatusCurrentEndpoint { get;} = "/api/status/current";
        public static string StatusSetEndpoint { get; } = "/api/status/set";
        public static string StatusResetEndpoint { get; } = "/api/status/reset";
        public static string StatusKillEndpoint { get; } = "/api/status/kill";
        public static string StatusRecordEndpoint { get; } = "/api/status/record";
        public static Uri BuildUri(string endpoint) {
            var base_uri = new Uri(BaseAddress);
            return new Uri(base_uri, endpoint);
        }
    }
}
